using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    [Serializable]
    public class Move
    {
        private int type; //0=nothing, 1=PlaceWord, 2=ChangeStones
        private String word;
        private int[] position;
        private bool horizontal;

        public Move(int type,String word, int[]position, bool horizontal){
            this.type = type;
            this.word = word;
            this.position = position;
            this.horizontal = horizontal;
        }

        public int getType()
        {
            return type;
        }

        public int getLength()
        {
            return word.Length;
        }

        public String getWord()
        {
            return word;
        }

        public bool isHorizontal()
        {
            return horizontal;
        }

        public int getX()
        {
            return position[1];
        }

        public int getY()
        {
            return position[0];
        }


    }
}
