using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S7F105_RMSPPIDCHANGETIMEREQUEST
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private List<String> ppid_count= new List<String>();

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

		public List<String> PPID_COUNT
		{
			get { return ppid_count; }
			set { ppid_count = value; }
		}


        public S7F105_RMSPPIDCHANGETIMEREQUEST(SECSTransaction trx)
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
			this.ppid_count = CPrivateUtility.getStringListItems(trx.Children[0] as ListFormat);

        }
    }
}