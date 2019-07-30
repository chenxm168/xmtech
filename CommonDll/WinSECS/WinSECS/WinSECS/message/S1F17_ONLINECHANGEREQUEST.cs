using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F17_ONLINECHANGEREQUEST
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String mcmd= "";

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


        public S1F17_ONLINECHANGEREQUEST(SECSTransaction trx)
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
			this.mcmd = trx.Children[0].Value;

        }
    }
}