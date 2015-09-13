using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
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
                        board[i, j] = new Field(Field.Types.TripleWord);
                    }
                    else if (doubleWord.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.DoubleWord);
                    }
                    else if (tripleLetter.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.TripleLetter);
                    }
                    else if (doubleLetter.Contains(i * 15 + j))
                    {
                        board[i, j] = new Field(Field.Types.DoubleLetter);
                    }
                    else
                    {
                        board[i, j] = new Field(Field.Types.Normal);
                    }
                }   
            }
        }

        public void print()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.Out.Write(board[i, j].getType().ToString().Substring(0, 1));
                }
                Console.Out.Write("\r\n");
            }
        }
    }
}
