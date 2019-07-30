using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F5_NOUSE_4_TYPE1
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String sfcd= "";

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

		public String SFCD
		{
			get { return sfcd; }
			set { sfcd = value; }
		}


        public S1F5_NOUSE_4_TYPE1(SECSTransaction trx)
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
			this.sfcd = trx.Children[0].Value;

        }
    }
}