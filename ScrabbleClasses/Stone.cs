using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    [Serializable]
    public class Stone
    {
        public int maxCount;
        public int value;
        public char letter;

        public Stone(int value, char letter)
        {
            this.value = value;
            this.letter = letter;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Stone))
            {
                Stone s = (Stone)obj;
                if (s.letter == this.letter && s.value == this.value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
