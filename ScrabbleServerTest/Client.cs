using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ScrabbleClasses;

namespace ScrabbleServerTest
{
    public class Client
    {
        private Form_Board form;
        private NetworkStream stream;
        private bool connected = false;
        private int id;
        public Client()
        {
            form= Form_Board.Create(this);
            Console.Out.WriteLine("Form initialized");
        }

         private void listenerLoop()
        {
            BinaryFormatter b = new BinaryFormatter();
            while (true)
            {
                Object obj = b.Deserialize(stream);
                if (typeof(NetMessage<int>) == obj.GetType())
                {
                    processMessage<int>((NetMessage<int>)obj);
                }
                else if (typeof(NetMessage<String>) == obj.GetType())
                {
                    processMessage<String>((NetMessage<String>)obj);
                }
                else if (typeof(NetMessage<Move>) == obj.GetType())
                {
                    processMessage<Move>((NetMessage<Move>)obj);
                }else if(typeof(NetMessage<String[]>)== obj.GetType()){
                    processMessage<String[]>((NetMessage<String[]>)obj);
                }
                else if (typeof(NetMessage<Board>) == obj.GetType())
                {
                    processMessage<Board>((NetMessage<Board>)obj);
                }
                else if (typeof(NetMessage<Hand>) == obj.GetType())
                {
                    processMessage<Hand>((NetMessage<Hand>)obj);
                }
            }
        }

        private void processMessage<T>(NetMessage<T> netMessage)
        {
            if (typeof(T) == typeof(int))
            {
                if (netMessage.commandType == NetCommand.s_Player_id)
                {
                    Object o = netMessage.payload;
                    this.id = (int) o;
                }
            }
            else if (typeof(String) == typeof(T))
            {
                if (netMessage.commandType == NetCommand.s_Player_Connected)
                {
                    
                    form.log("Player " + netMessage.payload + " joined your game!");
                }
                else if (netMessage.commandType == NetCommand.s_Player_NameChange)
                {
                    Console.Out.WriteLine("Player " + netMessage.payload + " changed the name");
                    form.log("Player " + netMessage.payload + " changed the name!");
                }
            }
            else if (typeof(Move) == typeof(T))
            {

            }
            else if (typeof(Hand)==typeof(T))
            {
                Console.Out.WriteLine("Received Hand");
                Object obj = netMessage.payload;
                Hand h = (Hand)obj;
                form.refreshHand(h);
               // h.print();
            }
            else if (typeof(String[]) == typeof(T))
            {
                Object obj = netMessage.payload;
                String[] s = (String[])obj;
                foreach (String st in s)
                {
                    Console.Out.Write(st + " ");
                }
                Console.Out.WriteLine();
            }
            else if (typeof(Board) == typeof(T))
            {
                Object obj = netMessage.payload;
                Board b = (Board)obj;
                form.refreshBoard(b);
                //b.print();
            }
        }

        public bool connect()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("localhost", 42420);
                if (client.Connected) {
                    stream = client.GetStream();
                    Thread t = new Thread(listenerLoop);
                    t.Start();
                    this.connected = true;
                    Console.Out.WriteLine("Connected");
               }
                return client.Connected;
            }
            catch (System.Net.Sockets.SocketException se)
            {
                Console.Out.WriteLine("Error connecting.");
                this.connected = false;
                return false;
            }
        }

        public void startGame()
        {
            new NetMessage<int>(NetCommand.c_Game_Start, 0, 0).serializeTo(stream);
        }

        public bool isConnected()
        {
            return connected;
        }
    }
    }
