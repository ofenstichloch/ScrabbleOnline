using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    [Serializable]
    public class Move
    {
        private Stone[] word;
        private int[] position;
        private bool horizontal;

        public Move(Stone[] word, int[]position, bool horizontal){
            this.word = word;
            this.position = position;
            this.horizontal = horizontal;
        }

    }
}
