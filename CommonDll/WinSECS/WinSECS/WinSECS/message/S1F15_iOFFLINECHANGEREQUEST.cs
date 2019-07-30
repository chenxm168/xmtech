using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F15_iOFFLINECHANGEREQUEST
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

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

		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}


        public S1F15_iOFFLINECHANGEREQUEST(SECSTransaction trx)
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
			this.toolid = trx.Children[0].Value;

        }
    }
}