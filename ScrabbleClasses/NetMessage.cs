using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Scrabble
{
    [Serializable]
    public class NetMessage<T>
    {
        [NonSerialized] private static BinaryFormatter b = new BinaryFormatter();
        public int commandType;
        public int origin;
        public T payload;

        public NetMessage(int type,int origin,T data){
            this.commandType = type;
            this.origin = origin;
            if (!typeof(T).IsSerializable) {
                throw new NotSupportedException();
            }
            this.payload = data;
        }

        public void serializeTo(Stream s)
        {
            b.Serialize(s, this);
        }
    }
}
