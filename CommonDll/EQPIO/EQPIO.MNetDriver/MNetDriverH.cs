
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class MNetDriverH : IMNetDriver
    {
        private ILog logger = LogManager.GetLogger(typeof(MNetDriverH));
        private int m_nPath = 0;
        private ushort m_nStation = 0xff;
        private static object m_objBitLock = new object();
        private static object m_objReceiveByteLock = new object();
        private static object m_objReciveLock = new object();
        private static object m_objSendLock = new object();
        private const string MNetDllName = "MDFUNC32.DLL";
        private static List<int> mPathList = new List<int>();

        public event ErrorMessage OnErrorMessage;

        public override void Close()
        {
            if (this.m_nPath != 0)
            {
                if (mPathList.Contains(this.m_nPath))
                {
                    short num = mdClose(this.m_nPath);
                    if (num != 0)
                    {
                        this.logger.Error(string.Format("MNetDriverH : Close ErrCode - {0}", num));
                    }
                    mPathList.Remove(this.m_nPath);
                }
                this.m_nPath = 0;
            }
        }

        ~MNetDriverH()
        {
            this.Close();
        }

        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdLedRead(int path, short[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdModRead(int path, ref short mode);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdModSet(int path, short mode);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdRst(int path);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdSwRead(int path, short[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdBdVerRead(int path, short[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdClose(int path);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdControl(int path, ushort station_no, short code);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdDevRst(int path, ushort station_no, short devtype, short devno);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdDevSet(int path, ushort station_no, short devtype, short devno);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdInit(int path);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdOpen(short channel, short mode, ref int path);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdRandR(int path, ushort station_no, short[] dev, ushort[] buf, short bufsize);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdRandW(int path, ushort station_no, short[] dev, ushort[] buf, short bufsize);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdReceive(int path, ushort station_no, short devtype, short devno, ref short bufsize, short[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdReceive(int path, ushort station_no, short devtype, short devno, ref short bufsize, ushort[] buf);
        [DllImport("MDFUNC32.DLL", EntryPoint = "mdReceive")]
        private static extern short mdReceiveByte(int path, ushort station_no, short devtype, short devno, ref short bufsize, byte[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdSend(int path, ushort station_no, short devtype, short devno, ref short bufsize, short[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdSend(int path, ushort station_no, short devtype, short devno, ref short bufsize, ushort[] buf);
        [DllImport("MDFUNC32.DLL", EntryPoint = "mdSend")]
        private static extern short mdSendByte(int path, ushort station_no, short devtype, short devno, ref short bufsize, byte[] buf);
        [DllImport("MDFUNC32.DLL")]
        private static extern short mdTypeRead(int path, ushort station_no, ref short code);
        public override bool Open(int channel)
        {
            if (this.m_nPath != 0)
            {
                this.logger.Error("MNetDriverH : Already open");
                return true;
            }
            if (mPathList.Contains(channel))
            {
                this.m_nPath = channel;
            }
            else
            {
                short num = mdOpen((short)channel, 0, ref this.m_nPath);
                if ((num != 0) && (num != 0x42))
                {
                    this.logger.Error(string.Format("MNetDriverH : OpenErrCode - {0} , Channel - {1} ", num, channel));
                    this.m_nPath = 0;
                    return false;
                }
                mPathList.Add(this.m_nPath);
            }
            return true;
        }

        public override string ReadBit(MNetDev dev)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadBit : MNet is not open");
                    return string.Empty;
                }
                short bufsize = 2;
                ushort[] buf = new ushort[1];
                int num2 = dev.Addr % 0x10;
                dev.Addr = (dev.Addr / 0x10) * 0x10;
                lock (m_objReciveLock)
                {
                    short num3 = mdReceive(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num3 != 0)
                    {
                        this.logger.Error(string.Format("ReadBit : ErrCode({0}), Address({1})", num3, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadBit : ErrCode({0}), Address({1})", num3, dev.ToString()));
                        return string.Empty;
                    }
                }
                return (((buf[0] & (((int)1) << num2)) != 0) ? "1" : "0");
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        public override string ReadBitString(MNetDev dev, int nLen)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadWordBlock : MNet is not open");
                    return null;
                }
                ushort[] numArray = this.ReadWordBlock(dev, nLen);
                if (numArray == null)
                {
                    this.logger.Error("ReadBitString : Empty");
                    return string.Empty;
                }
                string str = "";
                foreach (short num in numArray)
                {
                    for (int i = 0; i < 0x10; i++)
                    {
                        str = str + (((num & (((int)1) << i)) == 0) ? '0' : '1');
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        public override string ReadBlock(MNetDev dev, short nSize)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadBlock : MNet is not open");
                    return string.Empty;
                }
                string str = string.Empty;
                short bufsize = nSize;
                ushort[] buf = new ushort[1];
                lock (m_objReciveLock)
                {
                    short num2 = mdReceive(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("ReadBlock(string) : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadBlock(string) : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return string.Empty;
                    }
                }
                foreach (short num3 in buf)
                {
                    for (int i = 0; i < 0x10; i++)
                    {
                        str = str + (((num3 & (((int)1) << i)) == 0) ? '0' : '1');
                    }
                }
                return str.PadRight(nSize, '0');
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        public override byte[] ReadBlock(MNetDev dev, int nLen)
        {
            if (this.m_nPath == 0)
            {
                this.logger.Error("ReadBlock : MNet is not open");
                return null;
            }
            short bufsize = Convert.ToInt16((int)(nLen * 2));
            byte[] buf = new byte[bufsize];
            try
            {
                int num2 = mdReceiveByte(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                if (num2 != 0)
                {
                    this.logger.Error(string.Format("ReadBlock(byte) : ErrCode({0}), Address({1})", num2, dev.ToString()));
                    this.SetErrorEvent(string.Format("ReadBlock(byte) : ErrCode({0}), Address({1})", num2, dev.ToString()));
                    return null;
                }
                return buf;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return null;
            }
        }

        public override string ReadString(MNetDev dev, int nLen)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadString : MNet is not open");
                    return string.Empty;
                }
                short bufsize = (short)(nLen * 2);
                byte[] buf = new byte[bufsize + 1];
                lock (m_objReceiveByteLock)
                {
                    short num2 = mdReceiveByte(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("ReadString : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadString : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return string.Empty;
                    }
                }
                buf[bufsize] = 0;
                int index = 0;
                index = 0;
                while (index < bufsize)
                {
                    if (buf[index] == 0)
                    {
                        break;
                    }
                    index++;
                }
                return Encoding.Default.GetString(buf, 0, index).Trim();
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        public override ushort ReadWord(MNetDev dev)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadWord : MNet is not open");
                    return 0;
                }
                short bufsize = 2;
                ushort[] buf = new ushort[1];
                lock (m_objReciveLock)
                {
                    short num2 = mdReceive(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("ReadWord : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadWord : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return 0;
                    }
                }
                return buf[0];
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return 0;
            }
        }

        public override short ReadWord2(MNetDev dev)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadWord2 : MNet is not open");
                    return 0;
                }
                short bufsize = 2;
                short[] buf = new short[1];
                lock (m_objReciveLock)
                {
                    short num2 = mdReceive(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("ReadWord2 : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadWord2 : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return 0;
                    }
                }
                return buf[0];
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return 0;
            }
        }

        public override ushort[] ReadWordBlock(MNetDev dev, int nBlockSize)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("ReadWordBlock : MNet is not open");
                    return null;
                }
                short bufsize = (short)(nBlockSize * 2);
                ushort[] buf = new ushort[nBlockSize];
                lock (m_objReciveLock)
                {
                    short num2 = mdReceive(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("ReadWordBlock : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("ReadWordBlock : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return null;
                    }
                }
                return buf;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return null;
            }
        }

        private void SetErrorEvent(string errorMessage)
        {
            this.OnErrorMessage(errorMessage);
        }

        public override bool WriteBit(MNetDev dev, bool value)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("WriteBit : MNet is not open");
                    return false;
                }
                lock (m_objBitLock)
                {
                    short num = value ? mdDevSet(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr) : mdDevRst(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr);
                    if (num != 0)
                    {
                        this.logger.Error(string.Format("WriteBit : ErrCode({0}), Address({1})", num, dev.ToString()));
                        this.SetErrorEvent(string.Format("WriteBit : ErrCode({0}), Address({1})", num, dev.ToString()));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return false;
            }
        }

        public override bool WriteWord(MNetDev dev, ushort value)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("WriteWord : MNet is not open");
                    return false;
                }
                short bufsize = 2;
                ushort[] buf = new ushort[] { value };
                lock (m_objSendLock)
                {
                    short num2 = mdSend(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("WriteWord : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("WriteWord : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return false;
            }
        }

        public override bool WriteWord2(MNetDev dev, short value)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("WriteWord2 : MNet is not open");
                    return false;
                }
                short bufsize = 2;
                short[] buf = new short[] { value };
                lock (m_objSendLock)
                {
                    short num2 = mdSend(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                    if (num2 != 0)
                    {
                        this.logger.Error(string.Format("WriteWord2 : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        this.SetErrorEvent(string.Format("WriteWord2 : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return false;
            }
        }

        public override void WriteWordBlock(MNetDev dev, ushort[] buf, int nSize)
        {
            try
            {
                if (this.m_nPath == 0)
                {
                    this.logger.Error("WriteWordBlock : MNet is not open");
                }
                else
                {
                    short bufsize = (short)(nSize * 2);
                    lock (m_objSendLock)
                    {
                        short num2 = mdSend(this.m_nPath, this.m_nStation, (short)dev.Type, (short)dev.Addr, ref bufsize, buf);
                        if (num2 != 0)
                        {
                            this.logger.Error(string.Format("WriteWordBlock : ErrCode({0}), Address({1})", num2, dev.ToString()));
                            this.SetErrorEvent(string.Format("WriteWordBlock : ErrCode({0}), Address({1})", num2, dev.ToString()));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public override ushort Group
        {
            get
            {
                short num = (short)(this.m_nStation & 0xff);
                return (((num >= 0x81) && (num <= 160)) ? ((ushort)(num - 0x80)) : ((ushort)0));
            }
            set
            {
                if ((value != 0) && ((value >= 1) && (value <= 0x20)))
                {
                    this.m_nStation = (ushort)((this.m_nStation & 0xff00) | ((value + 0x80) & 0xff));
                }
            }
        }

        public override bool IsOpened
        {
            get
            {
                return (this.m_nPath != 0);
            }
        }

        public override ushort Network
        {
            get
            {
                return (ushort)((this.m_nStation >> 8) & 0xff);
            }
            set
            {
                if ((value >= 0) && (value <= 0xef))
                {
                    this.m_nStation = (ushort)((this.m_nStation & 0xff) | ((value << 8) & 0xff00));
                }
            }
        }

        public override ushort Station
        {
            get
            {
                ushort num = (ushort)(this.m_nStation & 0xff);
                return (((num >= 0x81) && (num <= 160)) ? ((ushort)0) : num);
            }
            set
            {
                this.m_nStation = (ushort)((this.m_nStation & 0xff00) | (value & 0xff));
            }
        }

        public delegate void ErrorMessage(string message);
    }
}
