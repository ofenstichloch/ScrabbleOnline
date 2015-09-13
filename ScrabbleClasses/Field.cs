using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    [Serializable]
    class Field
    {
        public enum Types { Normal, DoubleLetter, TripleLetter, DoubleWord, TripleWord };
        private Types type;

        public Field(Types type)
        {
            this.type = type;
        }

        public Stone getStone()
        {
            return new Stone(0, '0');
        }

        public Types getType()
        {
            return type;
        }
    }

}
