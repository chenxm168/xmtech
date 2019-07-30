using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    class S2F17_DateTimeRequest
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;


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


        public S2F17_DateTimeRequest(SECSTransaction trx)
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

        public static SECSTransaction makeTransaction(bool isNoPadding )
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, true);
            trx.Function = 17;


            return trx;
        }

        public void FillItemValue(SECSTransaction trx)
        {

        }
    }
}