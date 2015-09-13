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
            content.Push(new Stone(1, 'a'));
            content.Push(new Stone(2, 'b'));
            content.Push(new Stone(3, 'c'));
            content.Push(new Stone(4, 'd'));
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
