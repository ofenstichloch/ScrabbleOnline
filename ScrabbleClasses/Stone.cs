using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    [Serializable]
    public class Stone
    {
        public int maxCount;
        public int value;
        public char letter;
        private static Dictionary<char, int> values = new Dictionary<char, int>(){
            {'A', 1},
            {'B', 3},
            {'C', 4},
            {'D', 1},
            {'E', 1},
            {'F', 4},
            {'G', 2},
            {'H', 2},
            {'I', 1},
            {'J', 6},
            {'K', 4},
            {'L', 2},
            {'M', 3},
            {'N', 1},
            {'O', 2},
            {'P', 4},
            {'Q', 10},
            {'R', 1},
            {'S', 1},
            {'T', 1},
            {'U', 1},
            {'V', 6},
            {'W', 3},
            {'X', 8},
            {'Y', 10},
            {'Z', 3},
            {'Ä', 6},
            {'Ü', 6},
            {'Ö', 8},
            {' ', 0}
        };


        public Stone(char letter)
        {
            this.letter = letter;
            this.value = values[letter];
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
