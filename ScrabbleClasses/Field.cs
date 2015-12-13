using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    [Serializable]
    public class Field
    {
        public enum Types { Normal, DoubleLetter, TripleLetter, DoubleWord, TripleWord };
        private Types type;
        private Stone stone;

        public Field(Types type)
        {
            this.type = type;
            stone = null;
        }

        public Stone getStone()
        {
            return stone;
        }

        public Types getType()
        {
            return type;
        }

        public Stone placeStone(Stone s)
        {
            if (stone == null)
            {
                stone = s;
                return null;
            }
            else if (stone.letter == ' ')
            {
                Stone joker = stone;
                stone = s;
                return joker;
            }
            else
            {
                return null;
            }
        }

        public Stone removeStone()
        {
            Stone s = stone;
            stone = null;
            return s;
        }
    }

}
