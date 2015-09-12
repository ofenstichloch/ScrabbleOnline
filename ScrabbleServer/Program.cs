using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Scrabble
{
    /*
     * Main class
     * Starts listener for new players
     * Every second player starts a new match
     * 
     **/
    class Program
    {
        public static int clientCount = 0;
        public static int gameCount = 0;
        static void Main(string[] args)
        {
            TcpListener loginServer = new TcpListener(System.Net.IPAddress.Any, 42420);
            loginServer.Start();
            while (true)
            {
                //ToDo: Implement Client Queue
                TcpClient client = loginServer.AcceptTcpClient();
                Program.clientCount++;
                Console.Out.WriteLine("Client connected ("+Program.clientCount+")");
                Game g = Game.waitingForPlayers;
                if (g == null)
                {
                    g = new Game();
                }
                g.connectPlayer(client);
            }
            
        }
    }
}
