using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S7F25_NOUSE
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String mode= "";
		private String ppid= "";

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

		public String MODE
		{
			get { return mode; }
			set { mode = value; }
		}

		public String PPID
		{
			get { return ppid; }
			set { ppid = value; }
		}


        public S7F25_NOUSE(SECSTransaction trx)
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
			this.mode = listNode_0.Children[0].Value;
			this.ppid = listNode_0.Children[1].Value;

        }
    }
}