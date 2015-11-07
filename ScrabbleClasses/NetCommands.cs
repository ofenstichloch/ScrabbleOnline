using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    public class NetCommand
    {
        //Server Messages
        public static int s_Player_Connected = 100;
        public static int s_Player_NameChange = 101;
        public static int s_Player_Hand = 102;
        public static int s_Player_List = 103;
        public static int s_Player_Turn = 104;
        public static int s_Game_Start = 10;
        public static int s_Game_Board = 11;
        
        //Client Messages
        public static int c_Game_Start = 1000;
        public static int c_Player_NameChange = 1001;
        public static int c_DrawStones = 1002;
        public static int c_Move = 1003;

        //Error Messages
        public static int s_Error_BoardError = 2001;
        public static int s_Error_HandError = 2002;
        public static int s_Error_NotYourTurn = 2003;
        public static int s_Error_AlreadyStarted = 2004;

    }
}
