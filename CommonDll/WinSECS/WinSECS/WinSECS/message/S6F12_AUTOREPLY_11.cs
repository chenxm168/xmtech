using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S6F12_AUTOREPLY_11
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ack6= "";

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

		public String ACK6
		{
			get { return ack6; }
			set { ack6 = value; }
		}


        public S6F12_AUTOREPLY_11(SECSTransaction trx)
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
			this.ack6 = trx.Children[0].Value;

        }
    }
}