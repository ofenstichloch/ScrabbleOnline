using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ScrabbleServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Client c = new Client();

            /* 
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
                     parts[5] = parts[5].Replace('_', ' ');
                     NetMessage<Move> move = new NetMessage<Move>(NetCommand.c_Move, 0, new Move(int.Parse(parts[1]), parts[5], xy, bool.Parse(parts[4])));
                     move.serializeTo(client.GetStream());
                 }
             }
             * */
        }

    }
}
