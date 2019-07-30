using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.Utility
{


        [ComVisible(false)]
        public class BigEndianBitConverter
        {
            public static string[] hexs = new string[] { 
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", 
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", 
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", 
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", 
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", 
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", 
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", 
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F", 
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", 
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F", 
            "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF", 
            "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF", 
            "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF", 
            "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF", 
            "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF", 
            "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
         };

            public static byte[] GetBytes(bool value)
            {
                return new byte[] { (value ? ((byte)1) : ((byte)0)) };
            }

            public static byte[] GetBytes(double value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                return bytes;
            }

            public static byte[] GetBytes(short value)
            {
                return new byte[] { ((byte)(value >> 8)), ((byte)value) };
            }

            public static byte[] GetBytes(int value)
            {
                return new byte[] { ((byte)(value >> 0x18)), ((byte)(value >> 0x10)), ((byte)(value >> 8)), ((byte)value) };
            }

            public static byte[] GetBytes(long value)
            {
                return new byte[] { ((byte)(value >> 0x38)), ((byte)(value >> 0x30)), ((byte)(value >> 40)), ((byte)(value >> 0x20)), ((byte)(value >> 0x18)), ((byte)(value >> 0x10)), ((byte)(value >> 8)), ((byte)value) };
            }

            public static byte[] GetBytes(float value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                return bytes;
            }

            public static byte[] GetBytes(ushort value)
            {
                return new byte[] { ((byte)(value >> 8)), ((byte)value) };
            }

            public static byte[] GetBytes(uint value)
            {
                return new byte[] { ((byte)(value >> 0x18)), ((byte)(value >> 0x10)), ((byte)(value >> 8)), ((byte)value) };
            }

            public static byte[] GetBytes(ulong value)
            {
                return new byte[] { ((byte)(value >> 0x38)), ((byte)(value >> 0x30)), ((byte)(value >> 40)), ((byte)(value >> 0x20)), ((byte)(value >> 0x18)), ((byte)(value >> 0x10)), ((byte)(value >> 8)), ((byte)value) };
            }

            public static bool ToBoolean(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= value.Length))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (value[startIndex] != 0);
            }

            public static double ToDouble(byte[] value, int startIndex)
            {
                byte[] destinationArray = new byte[8];
                Array.Copy(value, startIndex, destinationArray, 0, 8);
                Array.Reverse(destinationArray);
                return BitConverter.ToDouble(destinationArray, 0);
            }

            public static short ToInt16(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 1)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (short)(value[startIndex + 1] | (value[startIndex] << 8));
            }

            public static int ToInt32(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 3)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (((value[startIndex + 3] | (value[startIndex + 2] << 8)) | (value[startIndex + 1] << 0x10)) | (value[startIndex] << 0x18));
            }

            public static long ToInt64(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 7)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (long)(((((((value[startIndex + 7] | (value[startIndex + 6] << 8)) | (value[startIndex + 5] << 0x10)) | (value[startIndex + 4] << 0x18)) | (value[startIndex + 3] << 0x20)) | (value[startIndex + 2] << 40)) | (value[startIndex + 1] << 0x30)) | (value[startIndex] << 0x38));
            }

            public static short ToInt8(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentException("value");
                }
                return (sbyte)value[startIndex];
            }

            public static float ToSingle(byte[] value, int startIndex)
            {
                byte[] destinationArray = new byte[4];
                Array.Copy(value, startIndex, destinationArray, 0, 4);
                Array.Reverse(destinationArray);
                return BitConverter.ToSingle(destinationArray, 0);
            }

            public static string ToString(byte[] value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                StringBuilder builder = new StringBuilder(value.Length * 3);
                foreach (byte num in value)
                {
                    builder.Append(hexs[num & 0xff]);
                    builder.Append(" ");
                }
                return builder.ToString().TrimEnd(new char[0]);
            }

            public static string ToString(byte[] value, int startIndex, int length)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                StringBuilder builder = new StringBuilder(length * 3);
                for (int i = 0; i < length; i++)
                {
                    builder.Append(hexs[value[i + startIndex] & 0xff]);
                    builder.Append(" ");
                }
                return builder.ToString().TrimEnd(new char[0]);
            }

            public static ushort ToUInt16(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 1)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (ushort)(value[startIndex + 1] | (value[startIndex] << 8));
            }

            public static uint ToUInt32(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 3)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (uint)(((value[startIndex + 3] | (value[startIndex + 2] << 8)) | (value[startIndex + 1] << 0x10)) | (value[startIndex] << 0x18));
            }

            public static ulong ToUInt64(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= (value.Length - 7)))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (ulong)(((((((value[startIndex + 7] | (value[startIndex + 6] << 8)) | (value[startIndex + 5] << 0x10)) | (value[startIndex + 4] << 0x18)) | (value[startIndex + 3] << 0x20)) | (value[startIndex + 2] << 40)) | (value[startIndex + 1] << 0x30)) | (value[startIndex] << 0x38));
            }

            public static ushort ToUInt8(byte[] value, int startIndex)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if ((startIndex < 0) || (startIndex >= value.Length))
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                return (ushort)(value[startIndex] & 0xff);
            }
        }
    }

