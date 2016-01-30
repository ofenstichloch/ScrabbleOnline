using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    [Serializable]
    public class Board
    {
        private static int[] tripleWord = {0,7,14,105,119,210,217,224};
        private static int[] doubleWord = {16,28,32,42,48,56,64,70,154,160,168,176,182,192,196,208};
        private static int[] tripleLetter = {20,24,76,80,84,88,136,140,144,148,200,204};
        private static int[] doubleLetter = {3,11,36,38,45,52,59,92,96,98,102,108,116,122,126,128,132,165,172,179,186,188,213,221};

        private Field[,] board;


        public Board()
        {
            board = new Field[15,15];
            for (int i=0; i <15; i++) 
            {
                for (int j = 0; j < 15; j++)
                {
                    if (tripleWord.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.TripleWord, i, j);
                    }
                    else if (doubleWord.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.DoubleWord, i, j);
                    }
                    else if (tripleLetter.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.TripleLetter, i, j);
                    }
                    else if (doubleLetter.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.DoubleLetter, i, j);
                    }
                    else
                    {
                        board[i, j] = new Field(Field.Types.Normal, i, j);
                    }
                }   
            }
        }

        public Stone placeStone(int x, int y, Stone s)
        {
            return board[x, y].placeStone(s);
        }

        public String checkWord(int length, int x, int y, bool horizontal, string s)
        {
            String chars = "";
            for (int i = 0; i < length; i++)
            {
                if (horizontal)
                {
                    if (board[x + i, y].getStone() != null && board[x + i, y].getStone().letter != ' ' && board[x+i,y].getStone().letter != s[i])
                    {
                        return null;
                    }
                    else if (board[x + i, y].getStone() == null)
                    {
                        chars += s[i];
                    }
                }
                else
                {
                    if (board[x, y + i].getStone() != null && board[x, y + i].getStone().letter != ' ' && board[x, y + i].getStone().letter != s[i])
                    {
                        return null;
                    }
                    else if (board[x, y + i].getStone() == null)
                    {
                        chars += s[i];
                    }
                }
            }
            if (chars.Length == 0 && s.Length > 1)
            {
                return null;
            }
            return chars;
        }
        public NetMessage<Board> getMessage()
        {
            NetMessage<Board> mess = new NetMessage<Board>(NetCommand.s_Game_Board, 0, this);
            return mess;
        }

        public void print()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Stone s = (board[i, j].getStone());
                    if(s == null){
                        Console.Out.Write(". ");
                    }
                    else{
                        Console.Out.Write(s.letter+" ");
                    }
                    
                }
                Console.Out.Write("\r\n");
            }
        }

        public int getWidth()
        {
            return board.GetLength(0);
        }

        public int getHeight()
        {
            return board.GetLength(1);
        }

        public Field getField(int x, int y)
        {
      
            return board[x,y];
        }

        public Stone removeStone(int x, int y)
        {
            return board[x, y].removeStone();
        }

    }
}
