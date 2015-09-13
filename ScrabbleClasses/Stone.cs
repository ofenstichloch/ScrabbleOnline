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
    }
}
