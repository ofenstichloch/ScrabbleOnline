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
            Console.In.ReadLine();
            NetMessage<String> mess2 = new NetMessage<String>(NetCommand.c_Player_NameChange, 0, "Alex");
            mess2.serializeTo(client.GetStream());
            Console.Out.WriteLine("Changed Name");
            NetMessage<int> mess = new NetMessage<int>(NetCommand.c_Game_Start, 0, 1);
            mess.serializeTo(client.GetStream());
            Console.In.ReadLine();

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
                }
            }
        }

        private static void processMessage<T>(NetMessage<T> netMessage)
        {
            if (typeof(NetMessage<int>) == netMessage.GetType())
            {

            }
            else if (typeof(NetMessage<String>) == netMessage.GetType())
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
            else if (typeof(NetMessage<Move>) == netMessage.GetType())
            {

            }
        }

    }
}
