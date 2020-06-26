using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowPakTool
{
    public class Item
    {
        public string PathName;
        public uint DataOffset;
        public uint DataSize;

        public Item(string pathName, uint dataOffset, uint dataSize)
        {
            PathName = pathName;
            DataOffset = dataOffset;
            DataSize = dataSize;
        }

    }
}
