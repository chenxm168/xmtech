
namespace EQPIO.MNetProtocol
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MNetDev
    {
        public const int DevR = 0x16;
        public const int DevB = 0x17;
        public const int DevW = 0x18;
        public const int DevER = 0x55f0;
        private int m_type;
        private int m_addr;
        public int Type
        {
            get
            {
                return this.m_type;
            }
        }
        public int Addr
        {
            get
            {
                return this.m_addr;
            }
            set
            {
                this.m_addr = value;
            }
        }
        public MNetDev(int t, int n)
        {
            this.m_type = t;
            this.m_addr = n;
        }

        public MNetDev(string sDev)
        {
            string str;
            this.m_type = 0;
            this.m_addr = 0;
            if ((sDev != null) && (sDev.Length != 0))
            {
                sDev = sDev.ToUpper();
                str = sDev.Substring(0, 1);
                if (str == null)
                {
                    goto Label_00CC;
                }
                if (!(str == "B"))
                {
                    if (str == "W")
                    {
                        this.m_type = 0x18;
                        this.m_addr = int.Parse(sDev.Substring(1), NumberStyles.AllowHexSpecifier);
                        return;
                    }
                    if (str == "R")
                    {
                        this.m_type = 0x16;
                        this.m_addr = int.Parse(sDev.Substring(1));
                        return;
                    }
                    goto Label_00CC;
                }
                this.m_type = 0x17;
                this.m_addr = int.Parse(sDev.Substring(1), NumberStyles.AllowHexSpecifier);
            }
            return;
        Label_00CC:
            str = sDev.Substring(0, 2);
            if ((str != null) && (str == "ZR"))
            {
                int num = int.Parse(sDev.Substring(2)) / 0x8000;
                this.m_type = 0x55f0 + num;
                this.m_addr = int.Parse(sDev.Substring(2)) % 0x8000;
            }
        }

        public static MNetDev operator +(MNetDev dev, int n)
        {
            dev.m_addr += n;
            return dev;
        }

        public static MNetDev operator -(MNetDev dev, int n)
        {
            dev.m_addr -= n;
            return dev;
        }

        public static MNetDev operator ++(MNetDev dev)
        {
            dev.m_addr++;
            return dev;
        }

        public static MNetDev operator --(MNetDev dev)
        {
            dev.m_addr--;
            return dev;
        }

        public MNetDev this[int idx]
        {
            get
            {
                return new MNetDev(this.m_type, this.m_addr + idx);
            }
        }
        public override string ToString()
        {
            if (this.m_type == 0x17)
            {
                return string.Format("B{0:X4}", this.m_addr);
            }
            if (this.m_type == 0x18)
            {
                return string.Format("W{0:X4}", this.m_addr);
            }
            if (this.m_type == 0x16)
            {
                return string.Format("R{0}", this.m_addr);
            }
            if (this.m_type >= 0x55f0)
            {
                int num = 0x55f0 - this.m_type;
                return string.Format("ZR{0}", this.m_addr + (0x8000 * num));
            }
            return string.Empty;
        }
    }
}
