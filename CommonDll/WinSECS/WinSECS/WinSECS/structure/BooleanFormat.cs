using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class BooleanFormat : Format
    {
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 9;

        public override int encoding(int startPos, byte[] bs)
        {
            string[] splits = this.Value.Split(new char[] { ' ' });
            int num = this.getLowerLoopCountBetweenLengthAndSplits(splits);
            this.Length = num;
            bs[startPos] = this.EncodingFormatByte;
            startPos++;
            byte[] sourceArray = this.getEncodingLengthBytes(1);
            Array.Copy(sourceArray, 0, bs, startPos, sourceArray.Length);
            startPos += this.getEncodingLengthForLengthByte();
            byte[] buffer2 = new byte[this.Length];
            for (int i = 0; i < num; i++)
            {
                string str;
                if (splits[i].ToUpper().Contains("T"))
                {
                    str = "1";
                }
                else if (splits[i].ToUpper().Contains("F"))
                {
                    str = "0";
                }
                else
                {
                    str = splits[i];
                }
                buffer2[i] = (byte)int.Parse(str);
            }
            Array.Copy(buffer2, 0, bs, startPos, buffer2.Length);
            return (startPos += buffer2.Length);
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            byte[] destinationArray = new byte[this.Length];
            Array.Copy(bs, pos, destinationArray, 0, this.Length);
            this.Value = ByteToObject.byte2int1(destinationArray);
            return (pos += this.Length);
        }

        public override string LogType
        {
            get
            {
                return "BOOLEAN";
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
