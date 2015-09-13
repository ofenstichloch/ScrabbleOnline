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
            foreach (char c in letters)
            {
                content.Push(new Stone(c));
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
