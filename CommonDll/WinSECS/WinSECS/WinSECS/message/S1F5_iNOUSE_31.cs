using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F5_iNOUSE_31
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String sfcd= "";
		private List<String> tool_count= new List<String>();

        public BasicTransactionInfo BasicTrxInfo
        {
            get { return basicTrxInfo; }
            set { basicTrxInfo = value; }
        }
        public SECSTransaction SECSTrx
        {
            get { return trx; }
            set { trx = value; }
        }

		public String SFCD
		{
			get { return sfcd; }
			set { sfcd = value; }
		}

		public List<String> TOOL_COUNT
		{
			get { return tool_count; }
			set { tool_count = value; }
		}


        public S1F5_iNOUSE_31(SECSTransaction trx)
        {
            this.trx = trx;
            this.basicTrxInfo = new BasicTransactionInfo(trx);
            FillItemValue(trx);
        }

        public void dispose()
        {
            basicTrxInfo.dispose();
            basicTrxInfo = null;
            trx = null;
        }

        public void FillItemValue(SECSTransaction trx)
        {
			ListFormat listNode_0 = trx.Children[0] as ListFormat;
			this.sfcd = listNode_0.Children[0].Value;
			this.tool_count = CPrivateUtility.getStringListItems(listNode_0.Children[1] as ListFormat);

        }
    }
}