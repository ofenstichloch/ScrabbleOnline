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
            NetMessage<String> mess = new NetMessage<string>(NetCommand.s_Player_Connected,0,newPlayer.name);
            informPlayers<String>(mess);
            if (playerCount == 4)
            {
                Game.waitingForPlayers = null;
                start();
            }
        }

        internal void gameLoop()
        {
            Console.Out.WriteLine("Waiting for players");
            blockStart.WaitOne();
            Console.Out.WriteLine("Game started with players: "+players[0].name);
        }

        internal void start()
        {
            if (!isStarted)
            {
                this.board = new Board();
                this.bucket = new Bucket();
                board.print();
                waitingForPlayers = null;
                isStarted = true;
                foreach (Player p in players)
                {
                    if (p != null)
                    {
                        //Send playerlist
                        //Draw stones
                        p.sendBoard();
                        p.sendHand();
                    }
                }

                NetMessage<int> mess = new NetMessage<int>(NetCommand.s_Game_Start, 0, 1);
                informPlayers<int>(mess);
                blockStart.Release();
                //Send whos turn it is
            }
        }

        #region Network Methods
        public void informPlayers<T>(NetMessage<T> message)
        {
            foreach (Player p in players)
            {
                if (p != null)
                {
                    p.informPlayer<T>(message);
                }
                
            }
        }

        public void sendBoard()
        {

        }
        #endregion


    }
}
