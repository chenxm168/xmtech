using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.structure
{
    [ComVisible(false)]
    public class ObjectToByte
    {
        protected internal const char SPACE = ' ';

        public static byte[] int2Byte(int data)
        {
            return new byte[] { ((byte)(data >> 0x18)), ((byte)(data >> 0x10)), ((byte)(data >> 8)), ((byte)data) };
        }

        public static byte[] long2Byte(long data)
        {
            return new byte[] { ((byte)(data >> 0x38)), ((byte)(data >> 0x30)), ((byte)(data >> 40)), ((byte)(data >> 0x20)), ((byte)(data >> 0x18)), ((byte)(data >> 0x10)), ((byte)(data >> 8)), ((byte)data) };
        }

        public static byte[] short2Byte(int data)
        {
            return new byte[] { ((byte)(data >> 8)), ((byte)data) };
        }

        public static byte[] uint4ToByte(string data)
        {
            string[] strArray = data.Split(new char[] { ' ' });
            byte[] destinationArray = new byte[strArray.Length * 4];
            for (int i = 0; i < strArray.Length; i++)
            {
                Array.Copy(long2Byte(long.Parse(strArray[i])), 4, destinationArray, i * 4, 4);
            }
            return destinationArray;
        }
    }
}
