using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F101_OPERATORCALLSEND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String item_0= "";
		private String tid= "";
		private String ptid= "";
		private String msg= "";
		private String toolid= "";

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

		public String ITEM_0
		{
			get { return item_0; }
			set { item_0 = value; }
		}

		public String TID
		{
			get { return tid; }
			set { tid = value; }
		}

		public String PTID
		{
			get { return ptid; }
			set { ptid = value; }
		}

		public String MSG
		{
			get { return msg; }
			set { msg = value; }
		}

		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}


        public S2F101_OPERATORCALLSEND(SECSTransaction trx)
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
			this.item_0 = listNode_0.Children[0].Value;
			this.tid = listNode_0.Children[1].Value;
			this.ptid = listNode_0.Children[2].Value;
			this.msg = listNode_0.Children[3].Value;
			this.toolid = listNode_0.Children[4].Value;

        }
    }
}