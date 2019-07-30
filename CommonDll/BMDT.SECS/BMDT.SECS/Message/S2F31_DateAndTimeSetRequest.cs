using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    class S2F31_DateAndTimeSetRequest
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String time= "";

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

		public String TIME
		{
			get { return time; }
			set { time = value; }
		}


        public S2F31_DateAndTimeSetRequest(SECSTransaction trx)
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
			this.time = trx.Children[0].Value;

        }
    }
}