using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class BinaryFormat : Format
    {
        private byte[] rawByte;
        private int rawByteLimit = -1;
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 8;
        private bool useRawBytes = false;
        private string value_Renamed;

        internal override int decoding(byte[] bs, int pos)
        {
            int num = bs[pos] & 0xff;
            int length = num & 3;
            pos++;
            this.Length = this.getLength4Type(length, bs, pos) / this.DefaultByteLength;
            pos += length;
            pos = this.valueCopy(bs, pos);
            return pos;
        }

        internal int decoding(byte[] bs, int pos, int rawByteLimit)
        {
            this.rawByteLimit = rawByteLimit;
            int num = bs[pos] & 0xff;
            int length = num & 3;
            pos++;
            this.Length = this.getLength4Type(length, bs, pos) / this.DefaultByteLength;
            pos += length;
            if (this.Length > rawByteLimit)
            {
                pos = this.valueCopyOnlyByte(bs, pos);
                return pos;
            }
            pos = this.valueCopy(bs, pos);
            return pos;
        }

        public override int encoding(int startPos, byte[] bs)
        {
            string[] splits = this.Value.Split(new char[] { ' ' });
            int num = this.getLowerLoopCountBetweenLengthAndSplits(splits);
            this.Length = num;
            startPos = base.encodingHeader(startPos, bs);
            if (this.useRawBytes)
            {
                Array.Copy(this.rawByte, 0, bs, startPos, this.rawByte.Length);
                return (startPos += this.rawByte.Length);
            }
            byte[] sourceArray = new byte[this.Length];
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
            this.Value = ByteToObject.byte2Binary(destinationArray);
            return (pos += this.Length);
        }

        public virtual int valueCopyOnlyByte(byte[] bs, int pos)
        {
            this.useRawBytes = true;
            this.rawByte = new byte[this.Length];
            Array.Copy(bs, pos, this.rawByte, 0, this.Length);
            return (pos += this.Length);
        }

        public override int Length
        {
            get
            {
                if (base.length < 0)
                {
                    if (this.rawByteLimit >= 0)
                    {
                        base.length = this.rawByte.Length;
                    }
                    else
                    {
                        string[] strArray = this.Value.Split(new char[] { ' ' });
                        base.length = strArray.Length;
                    }
                }
                return base.length;
            }
            set
            {
                base.length = value;
            }
        }

        public override string LogType
        {
            get
            {
                return "B";
            }
        }

        public virtual byte[] RawByte
        {
            get
            {
                return this.rawByte;
            }
            set
            {
                this.useRawBytes = true;
                base.length = value.Length;
                this.rawByte = value;
            }
        }

        public override byte Type
        {
            get
            {
                return TYPE;
            }
        }

        public virtual bool UseRawBytes
        {
            get
            {
                return this.useRawBytes;
            }
            set
            {
                this.useRawBytes = value;
            }
        }

        public override string Value
        {
            get
            {
                if (this.value_Renamed == null)
                {
                    if (this.useRawBytes)
                    {
                        this.value_Renamed = ByteToObject.byte2Binary(this.rawByte);
                    }
                    else
                    {
                        this.value_Renamed = "";
                    }
                }
                return this.value_Renamed;
            }
            set
            {
                this.value_Renamed = value;
            }
        }
    }
}
