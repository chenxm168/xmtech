using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    class S5F2_AlarmReportAck
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ackc5= "";

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

		public String ACKC5
		{
			get { return ackc5; }
			set { ackc5 = value; }
		}


        public S5F2_AlarmReportAck(SECSTransaction trx)
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
			this.ackc5 = trx.Children[0].Value;

        }
    }
}