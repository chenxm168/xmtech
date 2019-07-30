using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S10F3_TERMINALDISPLAYSEND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String tid= "";
		private String text= "";

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

		public String TID
		{
			get { return tid; }
			set { tid = value; }
		}

		public String TEXT
		{
			get { return text; }
			set { text = value; }
		}


        public S10F3_TERMINALDISPLAYSEND(SECSTransaction trx)
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
			this.tid = listNode_0.Children[0].Value;
			this.text = listNode_0.Children[1].Value;

        }
    }
}