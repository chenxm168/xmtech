using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.Utility
{

[ComVisible(false)]
    public class StringUtils
    {
        public static readonly string[] hexs = new string[] { 
            "00 ", "01 ", "02 ", "03 ", "04 ", "05 ", "06 ", "07 ", "08 ", "09 ", "0A ", "0B ", "0C ", "0D ", "0E ", "0F ", 
            "10 ", "11 ", "12 ", "13 ", "14 ", "15 ", "16 ", "17 ", "18 ", "19 ", "1A ", "1B ", "1C ", "1D ", "1E ", "1F ", 
            "20 ", "21 ", "22 ", "23 ", "24 ", "25 ", "26 ", "27 ", "28 ", "29 ", "2A ", "2B ", "2C ", "2D ", "2E ", "2F ", 
            "30 ", "31 ", "32 ", "33 ", "34 ", "35 ", "36 ", "37 ", "38 ", "39 ", "3A ", "3B ", "3C ", "3D ", "3E ", "3F ", 
            "40 ", "41 ", "42 ", "43 ", "44 ", "45 ", "46 ", "47 ", "48 ", "49 ", "4A ", "4B ", "4C ", "4D ", "4E ", "4F ", 
            "50 ", "51 ", "52 ", "53 ", "54 ", "55 ", "56 ", "57 ", "58 ", "59 ", "5A ", "5B ", "5C ", "5D ", "5E ", "5F ", 
            "60 ", "61 ", "62 ", "63 ", "64 ", "65 ", "66 ", "67 ", "68 ", "69 ", "6A ", "6B ", "6C ", "6D ", "6E ", "6F ", 
            "70 ", "71 ", "72 ", "73 ", "74 ", "75 ", "76 ", "77 ", "78 ", "79 ", "7A ", "7B ", "7C ", "7D ", "7E ", "7F ", 
            "80 ", "81 ", "82 ", "83 ", "84 ", "85 ", "86 ", "87 ", "88 ", "89 ", "8A ", "8B ", "8C ", "8D ", "8E ", "8F ", 
            "90 ", "91 ", "92 ", "93 ", "94 ", "95 ", "96 ", "97 ", "98 ", "99 ", "9A ", "9B ", "9C ", "9D ", "9E ", "9F ", 
            "A0 ", "A1 ", "A2 ", "A3 ", "A4 ", "A5 ", "A6 ", "A7 ", "A8 ", "A9 ", "AA ", "AB ", "AC ", "AD ", "AE ", "AF ", 
            "B0 ", "B1 ", "B2 ", "B3 ", "B4 ", "B5 ", "B6 ", "B7 ", "B8 ", "B9 ", "BA ", "BB ", "BC ", "BD ", "BE ", "BF ", 
            "C0 ", "C1 ", "C2 ", "C3 ", "C4 ", "C5 ", "C6 ", "C7 ", "C8 ", "C9 ", "CA ", "CB ", "CC ", "CD ", "CE ", "CF ", 
            "D0 ", "D1 ", "D2 ", "D3 ", "D4 ", "D5 ", "D6 ", "D7 ", "D8 ", "D9 ", "DA ", "DB ", "DC ", "DD ", "DE ", "DF ", 
            "E0 ", "E1 ", "E2 ", "E3 ", "E4 ", "E5 ", "E6 ", "E7 ", "E8 ", "E9 ", "EA ", "EB ", "EC ", "ED ", "EE ", "EF ", 
            "F0 ", "F1 ", "F2 ", "F3 ", "F4 ", "F5 ", "F6 ", "F7 ", "F8 ", "F9 ", "FA ", "FB ", "FC ", "FD ", "FE ", "FF "
         };
        private const int PAD_LIMIT = 0x2000;
        private static readonly string[] PADDING = new string[0xffff];

        static StringUtils()
        {
            PADDING[0x20] = "                                                                ";
        }

        private StringUtils()
        {
        }

        public static string format(string[] aData)
        {
            string str = "";
            for (int i = 0; i < aData.Length; i++)
            {
                str = str + aData[i];
            }
            return str;
        }

        public static string formatLong(string[] aData)
        {
            StringBuilder builder = new StringBuilder(0x100);
            for (int i = 0; i < aData.Length; i++)
            {
                builder.Append(aData[i]);
            }
            return builder.ToString();
        }

        public static string leftPad(string str, int size)
        {
            return leftPad(str, size, ' ');
        }

        public static string leftPad(string str, int size, char padChar)
        {
            if (str == null)
            {
                return null;
            }
            int repeat = size - str.Length;
            if (repeat <= 0)
            {
                return str;
            }
            if (repeat > 0x2000)
            {
                return leftPad(str, size, Convert.ToString(padChar));
            }
            return (padding(repeat, padChar) + str);
        }

        public static string leftPad(string str, int size, string padStr)
        {
            if (str == null)
            {
                return null;
            }
            if ((padStr == null) || (padStr.Length == 0))
            {
                padStr = " ";
            }
            int length = padStr.Length;
            int num2 = str.Length;
            int num3 = size - num2;
            if (num3 <= 0)
            {
                return str;
            }
            if ((length == 1) && (num3 <= 0x2000))
            {
                return leftPad(str, size, padStr[0]);
            }
            if (num3 == length)
            {
                return (padStr + str);
            }
            if (num3 < length)
            {
                return (padStr.Substring(0, num3) + str);
            }
            char[] chArray = new char[num3];
            char[] chArray2 = padStr.ToCharArray();
            for (int i = 0; i < num3; i++)
            {
                chArray[i] = chArray2[i % length];
            }
            return (new string(chArray) + str);
        }

        private static string padding(int repeat, char padChar)
        {
            string str = PADDING[padChar];
            if (str == null)
            {
                str = Convert.ToString(padChar);
            }
            while (str.Length < repeat)
            {
                str = str + str;
            }
            PADDING[padChar] = str;
            return str.Substring(0, repeat);
        }

        public static string padLeft(string aData, int aLength)
        {
            return leftPad(aData, aLength);
        }

        public static string padRight(string aData, int aLength)
        {
            return rightPad(aData, aLength);
        }

        public static string rightPad(string str, int size)
        {
            return rightPad(str, size, ' ');
        }

        public static string rightPad(string str, int size, char padChar)
        {
            if (str == null)
            {
                return null;
            }
            int repeat = size - str.Length;
            if (repeat <= 0)
            {
                return str;
            }
            if (repeat > 0x2000)
            {
                return rightPad(str, size, Convert.ToString(padChar));
            }
            return (str + padding(repeat, padChar));
        }

        public static string rightPad(string str, int size, string padStr)
        {
            if (str == null)
            {
                return null;
            }
            if ((padStr == null) || (padStr.Length == 0))
            {
                padStr = " ";
            }
            int length = padStr.Length;
            int num2 = str.Length;
            int num3 = size - num2;
            if (num3 <= 0)
            {
                return str;
            }
            if ((length == 1) && (num3 <= 0x2000))
            {
                return rightPad(str, size, padStr[0]);
            }
            if (num3 == length)
            {
                return (str + padStr);
            }
            if (num3 < length)
            {
                return (str + padStr.Substring(0, num3));
            }
            char[] chArray = new char[num3];
            char[] chArray2 = padStr.ToCharArray();
            for (int i = 0; i < num3; i++)
            {
                chArray[i] = chArray2[i % length];
            }
            return (str + new string(chArray));
        }

        public static string toHex2String(byte value_Renamed)
        {
            return hexs[value_Renamed & 0xff];
        }
    }
}
