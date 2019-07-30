
namespace EQPIO.MNetProtocol
{
    using System;

    public class ScanReceivedEventArgs : EventArgs
    {
        private bool bFlag;
        private string strBlockName;
        private string strItemName;
        private string strMultiBlockName;
        private string strUnitName;

        public ScanReceivedEventArgs(string multiBlock, string blockName, string itemName, bool flag, string unitName)
        {
            this.strMultiBlockName = multiBlock;
            this.strBlockName = blockName;
            this.strItemName = itemName;
            this.bFlag = flag;
            this.strUnitName = unitName;
        }

        public string BlockName
        {
            get
            {
                return this.strBlockName;
            }
        }

        public bool Flag
        {
            get
            {
                return this.bFlag;
            }
        }

        public string ItemName
        {
            get
            {
                return this.strItemName;
            }
        }

        public string MultiBlock
        {
            get
            {
                return this.strMultiBlockName;
            }
        }

        public string UnitName
        {
            get
            {
                return this.strUnitName;
            }
        }
    }
}
