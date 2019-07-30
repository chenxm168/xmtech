using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S5F2_ALARMREPORTREPLY
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ack5= "";

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

		public String ACK5
		{
			get { return ack5; }
			set { ack5 = value; }
		}


        public S5F2_ALARMREPORTREPLY(SECSTransaction trx)
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
			this.ack5 = trx.Children[0].Value;

        }
    }
}