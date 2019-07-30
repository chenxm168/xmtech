using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class ByteStream : MemoryStream
    {
        public ByteStream()
        {
        }

        public ByteStream(byte[] buffer)
            : base(buffer)
        {
        }

        public ByteStream(int capacity)
            : base(capacity)
        {
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = (byte)this.ReadByte();
            }
            return buffer;
        }

        public void Write(byte[] buffer)
        {
            this.Write(buffer, 0, buffer.Length);
        }
    }
}
