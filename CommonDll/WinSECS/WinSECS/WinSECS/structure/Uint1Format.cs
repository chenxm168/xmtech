using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class Uint1Format : Format
    {
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 0x29;

        public override int encoding(int startPos, byte[] bs)
        {
            string[] splits = this.Value.Split(new char[] { ' ' });
            int num = this.getLowerLoopCountBetweenLengthAndSplits(splits);
            this.Length = num;
            startPos = base.encodingHeader(startPos, bs);
            byte[] sourceArray = new byte[this.Length * this.DefaultByteLength];
            for (int i = 0; i < num; i++)
            {
                sourceArray[i] = (byte)int.Parse(splits[i]);
            }
            Array.Copy(sourceArray, 0, bs, startPos, sourceArray.Length);
            return (startPos += sourceArray.Length);
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            byte[] destinationArray = new byte[this.Length];
            Array.Copy(bs, pos, destinationArray, 0, this.Length);
            this.Value = ByteToObject.byte2Uint1(destinationArray);
            return (pos += this.Length);
        }

        public override string LogType
        {
            get
            {
                return "U1";
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
