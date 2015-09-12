using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Scrabble
{
    
    class Game
    {
        public static Game waitingForPlayers;

        private Player[] players;
        private int playerCount;

        private Board board;
        private Bucket bucket;
        private Thread thread;

        Semaphore blockStart;
        private bool isStarted = false;

        public Game()
        {
            players = new Player[4];
            playerCount = 0;
            Program.gameCount++;
            Console.Out.WriteLine("Game created (" + Program.gameCount + ")");
            waitingForPlayers = this;
            blockStart = new Semaphore(0, 1);
            Thread t = new Thread(this.gameLoop);
            this.thread = t;
            t.Start();
        }

        internal void connectPlayer(System.Net.Sockets.TcpClient client)
        {
            Player newPlayer = new Player(client, playerCount, this);
            players[playerCount] = newPlayer;
            playerCount++;
            Thread t = new Thread(newPlayer.listenForCommands);
            t.Start();
        }

        internal void gameLoop()
        {
            Console.Out.WriteLine("Waiting for players");
            blockStart.WaitOne();
            Console.Out.WriteLine("Game started");
        }

        internal void start()
        {
            if (!isStarted)
            {
                this.board = new Board();
                this.bucket = new Bucket();
                waitingForPlayers = null;
                isStarted = true;
                blockStart.Release();
            }
        }

        #region Network Methods
        public void informPlayers(NetMessage<String> message)
        {
            foreach (Player p in players)
            {
                p.informPlayer(message);
            }
        }

        public void sendBoard()
        {

        }
        #endregion


    }
}
