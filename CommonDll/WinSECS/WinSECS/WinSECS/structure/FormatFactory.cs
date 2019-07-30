using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class FormatFactory
    {

        public static IFormat newInstance(byte type)
        {
            return newInstance(type, 0);
        }

        public static IFormat newInstance(int type)
        {
            int lengthbyte = type & 3;
            return newInstance((byte)((type & 0xff) >> 2), lengthbyte);
        }

        public static IFormat newInstance(string type)
        {
            if (type.Equals("A"))
            {
                return new AsciiFormat();
            }
            if (type.Equals("L"))
            {
                return new ListFormat();
            }
            if (type.Equals("BOOLEAN"))
            {
                return new BooleanFormat();
            }
            if (type.Equals("B"))
            {
                return new BinaryFormat();
            }
            if (type.Equals("I1"))
            {
                return new Int1Format();
            }
            if (type.Equals("I2"))
            {
                return new Int2Format();
            }
            if (type.Equals("I4"))
            {
                return new Int4Format();
            }
            if (type.Equals("I8"))
            {
                return new Int8Format();
            }
            if (type.Equals("U1"))
            {
                return new Uint1Format();
            }
            if (type.Equals("U2"))
            {
                return new Uint2Format();
            }
            if (type.Equals("U4"))
            {
                return new Uint4Format();
            }
            if (type.Equals("U8"))
            {
                return new Uint8Format();
            }
            if (type.Equals("F4"))
            {
                return new Float4Format();
            }
            if (type.Equals("F8"))
            {
                return new Float8Format();
            }
            if (type.Equals("X"))
            {
                return new XFormat();
            }
            if (type.Equals("J"))
            {
                return new JISFormat();
            }
            return new SECSTransaction();
        }

        public static IFormat newInstance(byte type, int lengthbyte)
        {
            switch (type)
            {
                case 8:
                    return new BinaryFormat();

                case 9:
                    return new BooleanFormat();

                case 0:
                    return new ListFormat();

                case 0x10:
                    return new AsciiFormat();

                case 0x11:
                    return new JISFormat();

                case 0x18:
                    return new Int8Format();

                case 0x19:
                    return new Int1Format();

                case 0x1a:
                    return new Int2Format();

                case 0x1c:
                    return new Int4Format();

                case 0x20:
                    return new Float8Format();

                case 0x24:
                    return new Float4Format();

                case 40:
                    return new Uint8Format();

                case 0x29:
                    return new Uint1Format();

                case 0x2a:
                    return new Uint2Format();

                case 0x2c:
                    return new Uint4Format();

                case 0x7f:
                    return new XFormat();
            }
            return new SECSTransaction();
        }


    }
}
