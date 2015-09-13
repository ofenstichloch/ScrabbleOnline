using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class Bucket
    {
        private Stack<Stone> content;

        public Bucket()
        {
            content = new Stack<Stone>();
            //Fill bag
            String letters = "AAAAAÄBBCCDDDDEEEEEEEEEEEEEEEFFGGGHHHHIIIIIIJKKLLLMMMMNNNNNNNNNOOOÖPQRRRRRRSSSSSSSTTTTTTUUUUUUÜVWXYZ  ";
            Dictionary<char, int> values = new Dictionary<char, int>();
            values.Add('A', 1);
            values.Add('B', 3);
            values.Add('C', 4);
            values.Add('D', 1);
            values.Add('E', 1);
            values.Add('F', 4);
            values.Add('G', 2);
            values.Add('H', 2);
            values.Add('I', 1);
            values.Add('J', 6);
            values.Add('K', 4);
            values.Add('L', 2);
            values.Add('M', 3);
            values.Add('N', 1);
            values.Add('O', 2);
            values.Add('P', 4);
            values.Add('Q', 10);
            values.Add('R', 1);
            values.Add('S', 1);
            values.Add('T', 1);
            values.Add('U', 1);
            values.Add('V', 6);
            values.Add('W', 3);
            values.Add('X', 8);
            values.Add('Y', 10);
            values.Add('Z', 3);
            values.Add('Ä', 6);
            values.Add('Ü', 6);
            values.Add('Ö', 8);
            values.Add(' ', 0);
            foreach (char c in letters)
            {
                content.Push(new Stone(values[c], c));
            }
            shuffle();
        }

        private void shuffle()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            Stone[] array = content.OrderBy(x => rnd.Next()).ToArray();
            content.Clear();
            foreach (Stone s in array)
            {
                content.Push(s);
            }
            
        }

        public Stone[] drawStones(int count){
            Stone[] ret = new Stone[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = content.Pop();
            }
            return ret;
        }


    }
}
