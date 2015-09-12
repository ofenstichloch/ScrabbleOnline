using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Scrabble
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.In.ReadLine();
            TcpClient client = new TcpClient();
            client.Connect("localhost", 42420);
            NetMessage<int> mess = new NetMessage<int>(101,0,42);
            mess.serializeTo(client.GetStream());
            Console.In.ReadLine();
            NetMessage<String> mess2 = new NetMessage<String>(101, 0, "Start");
            mess2.serializeTo(client.GetStream());
            Console.In.ReadLine();

        }
    }
}
