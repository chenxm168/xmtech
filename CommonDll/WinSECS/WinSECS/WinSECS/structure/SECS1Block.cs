using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using WinSECS.Utility;

namespace WinSECS.structure
{
    [ComVisible(false)]
    public class SECS1Block
    {
        private byte[] checksum;
        private byte[] header;
        private byte[] text;

        public SECS1Block()
        {
            this.header = null;
            this.text = null;
            this.checksum = null;
        }

        public SECS1Block(byte[] packet, byte[] checksum)
        {
            this.header = null;
            this.text = null;
            this.checksum = null;
            this.header = new byte[10];
            Array.Copy(packet, this.header, 10);
            this.text = new byte[packet.Length - 10];
            Array.Copy(packet, 10, this.text, 0, this.text.Length);
            this.checksum = checksum;
        }

        public bool IsValidCheckSum()
        {
            return (this.MakeCheckSum() == BigEndianBitConverter.ToUInt16(this.CheckSum, 0));
        }

        public ushort MakeCheckSum()
        {
            int num2;
            ushort num = 0;
            for (num2 = 0; num2 < this.Header.Length; num2++)
            {
                num = (ushort)(num + this.Header[num2]);
            }
            for (num2 = 0; num2 < this.Text.Length; num2++)
            {
                num = (ushort)(num + this.Text[num2]);
            }
            return num;
        }

        public string ToSECS1LogString()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Header != null)
            {
                builder.Append(BigEndianBitConverter.ToString(this.Header));
            }
            builder.Append(' ', 9);
            int num = (this.Header == null) ? 0 : this.Header.Length;
            num += (this.Text == null) ? 0 : this.Text.Length;
            builder.Append(string.Format("Length = {0}", num).PadRight(0x15));
            builder.Append("(");
            builder.Append(string.Format("S{0}F{1}{2}", this.Stream, this.Function, this.IsWait ? "W" : "").PadRight(12));
            builder.Append(")");
            builder.Append(string.Format(" [SB={0}, ", this.SystemByte));
            builder.Append(string.Format("CS=0x{0}{1}, ", this.CheckSum[0].ToString("X2"), this.CheckSum[1].ToString("X2")));
            builder.Append(string.Format("BN=0x{0}{1}]", this.Header[4].ToString("X2"), this.Header[5].ToString("X2")));
            if ((this.Text != null) && (this.Text.Length > 0))
            {
                for (int i = 0; i < this.Text.Length; i += 20)
                {
                    builder.AppendLine();
                    builder.Append(' ', 0x20);
                    if ((i + 20) < this.Text.Length)
                    {
                        builder.Append(BigEndianBitConverter.ToString(this.Text, i, 20));
                    }
                    else
                    {
                        builder.Append(BigEndianBitConverter.ToString(this.Text, i, this.Text.Length - i));
                    }
                }
            }
            return builder.ToString();
        }

        public ushort BlockNumber
        {
            get
            {
                return BigEndianBitConverter.ToUInt16(new byte[] { (byte)(this.Header[4] & 0x7f), this.Header[5] }, 0);
            }
        }

        public byte[] CheckSum
        {
            get
            {
                return this.checksum;
            }
            set
            {
                this.checksum = value;
            }
        }

        public ushort Function
        {
            get
            {
                return this.Header[3];
            }
        }

        public byte[] Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
            }
        }

        public bool IsLastBlock
        {
            get
            {
                return (this.Header[4] >= 0x80);
            }
        }

        public bool IsWait
        {
            get
            {
                return (this.Header[2] >= 0x80);
            }
        }

        public ushort Stream
        {
            get
            {
                return (ushort)(this.Header[2] & 0x7f);
            }
        }

        public uint SystemByte
        {
            get
            {
                return BigEndianBitConverter.ToUInt32(this.Header, 6);
            }
        }

        public byte[] Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}
