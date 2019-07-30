
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using System;

    public class LinkSignalScanReceivedEventArgs : EventArgs
    {
        private int iOverCount;
        private Block plcScanBlock;
        private string strReadBitString;

        public LinkSignalScanReceivedEventArgs(string readBitString, int overCount, Block scanBlock)
        {
            this.strReadBitString = readBitString;
            this.iOverCount = overCount;
            this.plcScanBlock = scanBlock;
        }

        public int OverCount
        {
            get
            {
                return this.iOverCount;
            }
        }

        public Block PlcScanBlock
        {
            get
            {
                return this.plcScanBlock;
            }
        }

        public string ReadBitString
        {
            get
            {
                return this.strReadBitString;
            }
        }
    }
}
