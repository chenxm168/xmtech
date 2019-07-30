
namespace EQPIO.MNetProtocol
{
    using System;
    using System.Collections.Generic;

    public class FDCScanReceivedEventArgs : EventArgs
    {
        private Dictionary<string, string> dicItemList;
        private string strBlockName;

        public FDCScanReceivedEventArgs(string blockName, Dictionary<string, string> itemlist)
        {
            this.strBlockName = blockName;
            this.dicItemList = itemlist;
        }

        public string BlockName
        {
            get
            {
                return this.strBlockName;
            }
        }

        public Dictionary<string, string> ItemList
        {
            get
            {
                return this.dicItemList;
            }
        }
    }
}
