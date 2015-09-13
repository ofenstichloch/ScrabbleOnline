using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    [Serializable]
    public class Hand
    {
        private List<Stone> hand = new List<Stone>();

        public void addStones(Stone[] stones)
        {
            if (hand.Count + stones.Length < 8)
            {
                foreach (Stone s in stones)
                {
                    hand.Add(s);
                }
            }
        }

        public Stone removeStone(char c)
        {
            Stone ret = hand.Find(delegate(Stone s){return s.letter == c;});
            hand.Remove(ret);
            return ret;
        }

        public void exchangeStones(string w, Bucket bucket)
        {
            foreach (char c in w)
            {
                Stone ret = hand.Find(delegate(Stone s) { return s.letter == c; });
                hand.Remove(ret);
                bucket.addStone(ret);
            }
      
        }

        public bool hasWord(String s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if(s.Count(x => x == s[i]) > hand.Count(x => x.letter == s[i])){
                    Log.log("hand","false word",4);
                    return false;
                }
            }
                Log.log("hand","ok word",4);
                return true;
        }

        public int getLength()
        {
            return hand.Count();
        }

        public void print()
        {
            foreach (Stone s in hand)
            {
                Console.Out.Write(s.letter + ", ");
             
            }
            Console.Out.WriteLine();
        }
    }
}
