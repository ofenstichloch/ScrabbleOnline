using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int id;
        public Thread thread;

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
            thread = new Thread(listenForCommands);
            thread.Start();
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
                try
                {
                    Object obj = b.Deserialize(stream);
                    if (typeof(NetMessage<int>) == obj.GetType())
                    {
                        processIntMessage((NetMessage<int>)obj);
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
                catch (IOException e)
                {
                    game.disconnectPlayer(this);
                    Console.Out.WriteLine("Client " + id + " disconnected");
                    break;
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
                game.waitForMove.Release();
            }
            else if (m.getType() == 2)
            {
                if (m.getLength() > 2)
                {
                    Log.log("Player " + this.id, "Cannot exchange more than 2 stones", 3);
                    return;
                }
                hand.exchangeStones(m.getWord(), game.bucket);
                Log.log("Player " + this.id, "Processed empty Move, releasing game", 4);
                drawStones(m.getLength());
                return;
            }
            else
            {

                // TODO: Rewrite so that it accepts words with already placed stones
                //1. let the board check the string. Get a string with the letters that need to be placed as return
                //2. check this string in hand

                //Check Word
                String word = m.getWord();
                bool horizontal = m.isHorizontal();
                int h = horizontal ? 1 : 0;
                int v = horizontal ? 0 : 1;
                int x = m.getX();
                int y = m.getY();
                String neededChars = board.checkWord(m.getLength(), x, y,horizontal, word);
                if (neededChars != null && hand.hasWord(neededChars))
                {
                    Log.log("Player "+this.id, "Board and Hand acceppted Move",4);
                    //If every stone is a new one:
                    if (neededChars.Equals(word))
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            board.placeStone(x+h*i,y+v*i,hand.removeStone(word[i]));
                        }
                    }
                    // If there are only stones to be replaced
                    else if (neededChars.Length == 0 && word.Length == 1)
                    {
                        hand.addStone(board.placeStone(x, y, hand.removeStone(word[0])));
                    }
                    // If some stones do not have to be placed
                    else
                    {
                        int j = 0;
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (word[i] == neededChars[i-j])
                            {
                                Stone ret = board.placeStone(x + h * i, y + v * i, hand.removeStone(word[i]));
                            }
                            else
                            {
                                j++;
                            }
                            
                        }
                    }
                }
                else
                {
                    informPlayer<String>(new NetMessage<String>(NetCommand.s_Error_MoveError, this.id, "False Move, Bord or Hand rejected."));
                    return;
                }
                sendHand();
                //Draw Stones and finish move
                drawStones(7 - hand.getLength());
                Log.log("Player " + this.id, "Processed Move, releasing game", 4);
                game.waitForMove.Release();
            
              
               
            }
            
        }
        #endregion
    }
}
