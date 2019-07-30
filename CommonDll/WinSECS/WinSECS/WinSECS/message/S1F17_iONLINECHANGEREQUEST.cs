using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F17_iONLINECHANGEREQUEST
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String mcmd= "";
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

		public String MCMD
		{
			get { return mcmd; }
			set { mcmd = value; }
		}

		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}


        public S1F17_iONLINECHANGEREQUEST(SECSTransaction trx)
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
			this.mcmd = listNode_0.Children[0].Value;
			this.toolid = listNode_0.Children[1].Value;

        }
    }
}