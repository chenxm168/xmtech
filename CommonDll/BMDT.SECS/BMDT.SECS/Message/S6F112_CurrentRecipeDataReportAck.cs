using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    class S6F112_CurrentRecipeDataReportAck
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ackc6= "";

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

		public String ACKC6
		{
			get { return ackc6; }
			set { ackc6 = value; }
		}


        public S6F112_CurrentRecipeDataReportAck(SECSTransaction trx)
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
			this.ackc6 = trx.Children[0].Value;

        }
    }
}