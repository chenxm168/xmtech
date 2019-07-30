using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    class S2F113_CurrentEQPDataRequest
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String unitid= "";

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

		public String UNITID
		{
			get { return unitid; }
			set { unitid = value; }
		}


        public S2F113_CurrentEQPDataRequest(SECSTransaction trx)
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
			this.unitid = trx.Children[0].Value;

        }
    }
}