using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleClasses
{
    public class Log
    {
        public int logLevel;

        public static void log(string issuer, string message,int lvl){
            Console.Out.WriteLine(message);
        }
    }
}
