using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ScrabbleClasses;

namespace ScrabbleServer
{
    
    class Game
    {
        public static Game waitingForPlayers;

        public Player[] players;
        public int playerCount;

        public Board board;
        public Bucket bucket;
        private Thread thread;
        private int id;
        public int currentPlayer;

        //GameFlow
        private Semaphore blockStart;
        public Semaphore waitForMove;
        private bool isStarted = false;
        private bool isFinished = false;
        public int emptyMovesCount = 0;

        public Game(int id)
        {
            players = new Player[4];
            playerCount = 0;
            Program.gameCount++;
            this.id = id;
            Log.log("Game"+this.id.ToString(),"Game created (" + Program.gameCount + ")",3);
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
            }
        }

        internal void gameLoop()
        {
            Log.log("Game" + id, "Waiting for Players", 4);
            blockStart.WaitOne();
            Log.log("Game" + id, "Game started with "+playerCount+" players", 4);

            while (!isFinished)
            {
                informPlayers<int>(new NetMessage<int>(NetCommand.s_Player_Turn, 0, currentPlayer));
                //Wait for the move to be submitted
                Log.log("Game" + id, "Waiting for Move " + currentPlayer, 4);
                waitForMove.WaitOne();
                sendBoard();
                currentPlayer = (currentPlayer + 1) % playerCount;
                //TODO Emptymves
                if (emptyMovesCount == playerCount)
                {
                    isFinished = true;
                }

            }

        }

        internal void start()
        {
            if (!isStarted)
            {
                this.board = new Board();
                this.bucket = new Bucket();
                waitingForPlayers = null;
                isStarted = true;
                String[] playerNames = new String[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    playerNames[i] = players[i].name;
                }
                
                foreach (Player p in players)
                {
                    if (p != null)
                    {
                        NetMessage<String[]> list = new NetMessage<string[]>(NetCommand.s_Player_List, 0, playerNames);
                        p.informPlayer<String[]>(list);
                        p.drawStones(7);
                    }
                }
                sendBoard();
                NetMessage<int> mess = new NetMessage<int>(NetCommand.s_Game_Start, 0, 1);
                informPlayers<int>(mess);
                currentPlayer = 0;
                waitForMove = new Semaphore(0, 1);
                blockStart.Release();
 
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
            foreach (Player p in players)
            {
                if (p != null)
                {
                    p.sendBoard();
                }
            }
        }
        #endregion


    }
}
