using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scrabble
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

        public Player(System.Net.Sockets.TcpClient socket, int id, Game game)
        {
            this.game = game;
            this.socket = socket;
            this.id = id;
            this.stream = socket.GetStream();
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            this.name = "Player" + id;
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

        }
        #endregion
    }
}
