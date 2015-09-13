using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Scrabble
{
    class Program
    {
        private static NetworkStream stream;
        static void Main(string[] args)
        {
            Console.In.ReadLine();
            TcpClient client = new TcpClient();
            client.Connect("localhost", 42420);
            Console.Out.WriteLine("Connected");
            stream = client.GetStream();
            Thread t = new Thread(listenerLoop);
            t.Start();
            while (true)
            {
                Console.Out.WriteLine("Enter Command");
                String command = Console.In.ReadLine();
                String[] parts = command.Split(' ');
                if (parts[0] == "Name")
                {
                    NetMessage<String> mess = new NetMessage<String>(NetCommand.c_Player_NameChange, 0, parts[1]);
                    mess.serializeTo(client.GetStream());
                }
                else if (parts[0] == "Start")
                {
                    NetMessage<int> mess = new NetMessage<int>(NetCommand.c_Game_Start, 0, 1);
                    mess.serializeTo(client.GetStream());
                }
                else if (parts[0] == "Move")
                {
                    int[] xy = new int[2];
                    xy[0] = int.Parse(parts[2]);
                    xy[1] = int.Parse(parts[3]);

                    NetMessage<Move> move = new NetMessage<Move>(NetCommand.c_Move, 0, new Move(int.Parse(parts[1]), parts[5], xy, bool.Parse(parts[4])));
                    move.serializeTo(client.GetStream());
                }
            }
        }

        private static void listenerLoop()
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

        private static void processMessage<T>(NetMessage<T> netMessage)
        {
            if (typeof(T) == typeof(int))
            {

            }
            else if (typeof(String) == typeof(T))
            {
                if (netMessage.commandType == NetCommand.s_Player_Connected)
                {
                    Console.Out.WriteLine("Player " + netMessage.payload + " joined your game!");
                }
                else if (netMessage.commandType == NetCommand.s_Player_NameChange)
                {
                    Console.Out.WriteLine("Player " + netMessage.payload + " changed the name");
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
                h.print();
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
                b.print();
            }
        }

    }
}
