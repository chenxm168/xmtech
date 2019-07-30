
namespace EQPIO.MNetProtocol
{
    using System;

    internal class MProtocolUtils
    {
        public static string CharRevcrse(string data)
        {
            char[] array = data.ToCharArray();
            Array.Reverse(array);
            string str = string.Empty;
            foreach (char ch in array)
            {
                str = str + ch.ToString();
            }
            return str;
        }

        public static ushort HiWord(uint dw)
        {
            return (ushort)((dw >> 0x10) & 0xffff);
        }

        public static ushort LoWord(uint dw)
        {
            return (ushort)(dw & 0xffff);
        }

        public static uint MakeDWord(ushort hi, ushort lo)
        {
            return (uint)(((hi << 0x10) & -65536) | (lo & 0xffff));
        }

        public static string PadLeft(string str, int length, char padChar)
        {
            string str2 = string.Empty;
            str2 = str.PadLeft(length, padChar);
            if (str2.Length > length)
            {
                str2 = str2.Substring(str2.Length - length, str2.Length - (str2.Length - length));
            }
            return str2;
        }

        public static string StringToHex(string data)
        {
            string str = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                str = str + string.Format("{0:X}", Convert.ToInt32(data[i]));
            }
            return str;
        }

        public static void WriteBitStringToMem(ushort[] buf, int start, int len, string bitstr)
        {
            if (!string.IsNullOrEmpty(bitstr))
            {
                Array.Clear(buf, start, len);
                int length = bitstr.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i >= (len * 0x10))
                    {
                        break;
                    }
                    if (bitstr[i] == '1')
                    {
                        int num = i / 0x10;
                        buf[start + num] = (ushort)(buf[start + num] | (((int)1) << (i % 0x10)));
                    }
                }
            }
        }
    }
}
