using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinSECS.global;
using WinSECS.Utility;
using System.Xml;

namespace WinSECS.structure
{
    [ComVisible(false)]
    public class Visitor
    {
        public static SECSException decoding(byte[] bs, SECSTransaction message)
        {
            lock (typeof(Visitor))
            {
                int index = 0;
                IFormat format = message;
                while (index < bs.Length)
                {
                    int type = bs[index];
                    IFormat format2 = FormatFactory.newInstance(type);
                    if (format2.Type == 0x3f)
                    {
                        return new SECSException("INVALID SECS FORMAT");
                    }
                    index = ((Format)format2).decoding(bs, index);
                    format.add(format2);
                }
            }
            return null;
        }

        public static SECSException decoding(byte[] bs, SECSTransaction message, int rawByteLimit)
        {
            lock (typeof(Visitor))
            {
                int index = 0;
                IFormat format = message;
                while (index < bs.Length)
                {
                    int type = bs[index];
                    IFormat format2 = FormatFactory.newInstance(type);
                    if (format2.Type == 0x3f)
                    {
                        return new SECSException("INVALID SECS FORMAT");
                    }
                    if (format2.Type == 8)
                    {
                        index = ((BinaryFormat)format2).decoding(bs, index, rawByteLimit);
                    }
                    else
                    {
                        index = ((Format)format2).decoding(bs, index);
                    }
                    format.add(format2);
                }
            }
            return null;
        }

        public static byte[] encoding(IFormatCollection formats)
        {
            byte[] bs = null;
            byte[] destinationArray = null;
            lock (typeof(Visitor))
            {
                bs = new byte[getByteLength(formats)];
                int startPos = 0;
                foreach (IFormat format in formats)
                {
                    startPos = ((Format)format).encoding(startPos, bs);
                }
                destinationArray = new byte[startPos];
                Array.Copy(bs, 0, destinationArray, 0, startPos);
            }
            return destinationArray;
        }

        public static void getBodyElement(XmlElement header, IFormatCollection formats)
        {
            lock (typeof(Visitor))
            {
                foreach (IFormat format in formats)
                {
                    header.AppendChild(format.toElement());
                }
            }
        }

        public static string getBodyLogTree(int length, IFormatCollection formats)
        {
            StringBuilder builder = new StringBuilder(length * 10);
            foreach (IFormat format in formats)
            {
                builder.Append(format.LogFormat);
            }
            return builder.ToString();
        }

        public static int getByteLength(IFormatCollection formats)
        {
            int num = 0;
            lock (typeof(Visitor))
            {
                foreach (IFormat format in formats)
                {
                    Format format2 = (Format)format;
                    num += format2.ByteLength;
                }
            }
            return num;
        }

        public static void setLevel(IFormatCollection formats)
        {
            int num = 0;
            foreach (IFormat format in formats)
            {
                format.Level = num;
            }
        }
    }
}
