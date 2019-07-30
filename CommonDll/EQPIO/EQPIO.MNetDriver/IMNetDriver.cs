
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class IMNetDriver
    {
        private ILog logger = LogManager.GetLogger(typeof(IMNetDriver));
        private static List<int> mPathList = new List<int>();

        public event ErrorMessage OnErrorMessage;

        public abstract void Close();
        ~IMNetDriver()
        {
            this.Close();
        }

        public abstract bool Open(int channel);
        public abstract string ReadBit(MNetDev dev);
        public abstract string ReadBitString(MNetDev dev, int nLen);
        public abstract string ReadBlock(MNetDev dev, short nSize);
        public abstract byte[] ReadBlock(MNetDev dev, int nLen);
        public abstract string ReadString(MNetDev dev, int nLen);
        public abstract ushort ReadWord(MNetDev dev);
        public abstract short ReadWord2(MNetDev dev);
        public abstract ushort[] ReadWordBlock(MNetDev dev, int nBlockSize);
        private void SetErrorEvent(string errorMessage)
        {
            this.OnErrorMessage(errorMessage);
        }

        public abstract bool WriteBit(MNetDev dev, bool value);
        public abstract bool WriteWord(MNetDev dev, ushort value);
        public abstract bool WriteWord2(MNetDev dev, short value);
        public abstract void WriteWordBlock(MNetDev dev, ushort[] buf, int nSize);

        public abstract ushort Group { get; set; }

        public abstract bool IsOpened { get; }

        public abstract ushort Network { get; set; }

        public abstract ushort Station { get; set; }

        public delegate void ErrorMessage(string message);
    }
}
