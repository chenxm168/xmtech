using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class XFormat : Format
    {
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 0x7f;

        public override int encoding(int startPos, byte[] bs)
        {
            IFormat format = FormatFactory.newInstance((byte)0x10);
            format.Name = this.Name;
            format.Length = this.Length;
            format.Variable = this.Variable;
            format.Value = this.Value;
            return ((AsciiFormat)format).encoding(startPos, bs);
        }

        public override int getMaxPossibleByteLength()
        {
            return 0x7fffffff;
        }

        public override string GetRegexPattern()
        {
            return string.Format("(.*)*", new object[0]);
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            byte[] destinationArray = new byte[this.Length];
            Array.Copy(bs, pos, destinationArray, 0, this.Length);
            this.Value = Encoding.GetEncoding("ks_c_5601-1987").GetString(destinationArray);
            return (pos += this.Length);
        }

        public override string LogType
        {
            get
            {
                return "X";
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
