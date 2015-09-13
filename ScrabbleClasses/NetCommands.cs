using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class NetCommand
    {
        public static int s_Player_Connected = 100;
        public static int s_Player_NameChange = 101;
        public static int s_Player_Hand = 102;
        public static int s_Game_Start = 10;

        public static int c_Player_NameChange = 1001;
        public static int c_Game_Start = 1000;
    }
}
