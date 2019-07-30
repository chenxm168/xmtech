using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class JISFormat : Format
    {
        public static readonly byte TYPE = 0x11;

        public override int encoding(int startPos, byte[] bs)
        {
            return 0;
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            return 0;
        }

        public override string LogType
        {
            get
            {
                return "J";
            }
        }

        public override byte Type
        {
            get
            {
                return TYPE;
            }
        }
    }
}
