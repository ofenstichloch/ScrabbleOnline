using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ScrabbleClasses;

namespace ScrabbleServer
{
    class Player
    {
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Game game;
        private int id;

        public String name;
        private Hand hand;
        private Board board;

        public Player(System.Net.Sockets.TcpClient socket, int id, Game game)
        {
            this.game = game;
            this.socket = socket;
            this.id = id;
            this.stream = socket.GetStream();
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            this.name = "Player" + id;
            new NetMessage<int>(NetCommand.s_Player_id, id, id).serializeTo(stream);
            hand = new Hand();
        }

        #region Network Methods
        internal void informPlayer<T>(NetMessage<T> message){
            message.serializeTo(stream);
        }


        internal void listenForCommands()
        {
            BinaryFormatter b = new BinaryFormatter();
            while (true)
            {
                Object obj = b.Deserialize(stream);
                if (typeof(NetMessage<int>) == obj.GetType())
                {
                    processIntMessage((NetMessage<int>) obj);
                }
                else if (typeof(NetMessage<String>) == obj.GetType())
                {
                    processStringMessage((NetMessage<String>)obj);
                }
                else if (typeof(NetMessage<Move>) == obj.GetType())
                {
                    Log.log("Player " + this.id, "Received Move", 4);
                    processMoveMessage((NetMessage<Move>)obj);
                }
            }
        }

        internal void sendHand()
        {
            NetMessage<Hand> mess = new NetMessage<Hand>(NetCommand.s_Player_Hand, this.id, hand);
            mess.serializeTo(stream);
        }

        internal void sendBoard()
        {
            this.board = game.board;
            game.board.getMessage().serializeTo(stream);
        }
        #endregion

        #region Local Methods
        private void changeName(String newName){
            this.name = newName;
            NetMessage<String> message = new NetMessage<String>(NetCommand.s_Player_NameChange, this.id, newName);
            game.informPlayers<String>(message);
        }

        public void drawStones(int count)
        {
            hand.addStones(game.bucket.drawStones(count));
            sendHand();
        }

        public bool checkHand(String s)
        {
            return hand.hasWord(s);
        }

#endregion

        #region Process Messages
        private void processStringMessage(NetMessage<String> mess)
        {
            if (mess.commandType == NetCommand.c_Player_NameChange)
            {
                changeName(mess.payload);
            }
        }

        private void processIntMessage(NetMessage<int> mess)
        {
            if (mess.commandType == NetCommand.c_Game_Start)
            {
                game.start();
            }
            else if (mess.commandType == NetCommand.c_DrawStones)
            {
                drawStones(mess.payload);
            }
        }

        private void processMoveMessage(NetMessage<Move> mess)
        {
            if (game.currentPlayer != this.id)
            {
                return;
            }
            Log.log("Player " + this.id, "Processing Move", 4);
            //get Move
            Move m = mess.payload;
            if (m.getType() == 0)
            {
                game.emptyMovesCount++;
            }
            else if (m.getType() == 2)
            {
                //TODO exchange Stone
                if (m.getLength() > 2)
                {
                    Log.log("Player " + this.id, "Cannot exchange more than 2 stones", 3);
                    return;
                }
                hand.exchangeStones(m.getWord(), game.bucket);
                drawStones(m.getLength());
            }
            else
            {
                //Apply move in game
                if (hand.hasWord(m.getWord()))
                {
                    String word = m.getWord();

                    if (!board.checkWord(m.getLength(), m.getX(), m.getY(), m.isHorizontal()))
                    {
                        Log.log("Player " + this.id, "Board blocked Move", 3);
                        //TODO Reply error to client
                        return;
                    }
                    for (int i = 0; i < m.getLength(); i++)
                    {
                        if (m.isHorizontal())
                        {
                            Stone ret = board.placeStone(m.getX() , m.getY()+i, hand.removeStone(word[i]));
                            if (ret != null)
                            {
                                Stone[] giveBack = new Stone[1];
                                giveBack[0] = ret;
                                hand.addStones(giveBack);
                            }
                        }
                        else
                        {
                            Stone ret = board.placeStone(m.getX()+i, m.getY(), hand.removeStone(word[i]));
                            if (ret != null)
                            {
                                Stone[] giveBack = new Stone[1];
                                giveBack[0] = ret;
                                hand.addStones(giveBack);
                            }
                        }
                    }
                    drawStones(7-hand.getLength());
                    Log.log("Player " + this.id, "Processed Move, releasing game", 4);
                    game.waitForMove.Release();
                }
            }
            
        }
        #endregion
    }
}
