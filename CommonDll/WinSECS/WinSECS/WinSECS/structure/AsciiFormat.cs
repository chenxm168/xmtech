using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class AsciiFormat : Format
    {
        private int length;
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 0x10;

        public override int encoding(int startPos, byte[] bs)
        {
            int num;
            byte[] bytes = new byte[0];
            bytes = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(this.Value);
            byte[] buffer2 = new byte[bytes.Length];
            for (num = 0; num < bytes.Length; num++)
            {
                buffer2[num] = bytes[num];
            }
            buffer2 = this.fixLengthAndByteArrays(buffer2);
            this.Length = buffer2.Length;
            startPos = base.encodingHeader(startPos, bs);
            for (num = 0; num < buffer2.Length; num++)
            {
                if (buffer2[num] == 0)
                {
                    buffer2[num] = 0x20;
                }
            }
            Array.Copy(buffer2, 0, bs, startPos, buffer2.Length);
            return (startPos += buffer2.Length);
        }

        private byte[] fixLengthAndByteArrays(byte[] bs)
        {
            if (bs.Length == this.Length)
            {
                return bs;
            }
            byte[] destinationArray = new byte[this.Length];
            int length = (bs.Length < destinationArray.Length) ? bs.Length : destinationArray.Length;
            Array.Copy(bs, 0, destinationArray, 0, length);
            return destinationArray;
        }

        protected internal override int getLowerLoopCountBetweenLengthAndSplits(string[] splits)
        {
            if ((splits.Length == 1) && splits[0].Equals(""))
            {
                return 0;
            }
            return ((this.Length > splits.Length) ? splits.Length : this.Length);
        }

        public override string GetRegexInput()
        {
            return string.Format("{0}<{1}>", this.getformatTypeRegex(), this.length);
        }

        public override string GetRegexPattern()
        {
            if (this.Variable)
            {
                return string.Format(@"{0}<\d*>", this.getformatTypeRegex());
            }
            return string.Format("{0}<{1}>", this.getformatTypeRegex(), this.length);
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            byte[] destinationArray = new byte[this.Length];
            Array.Copy(bs, pos, destinationArray, 0, this.Length);
            for (int i = 0; i < this.Length; i++)
            {
                if (destinationArray[i] == 0)
                {
                    destinationArray[i] = 0x20;
                }
            }
            this.Value = Encoding.GetEncoding("ks_c_5601-1987").GetString(destinationArray);
            return (pos += this.Length);
        }

        public override int Length
        {
            get
            {
                if (this.length < 0)
                {
                    return Encoding.GetEncoding("ks_c_5601-1987").GetBytes(this.Value).Length;
                }
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        public override string LogType
        {
            get
            {
                return "A";
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
