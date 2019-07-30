using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S3F0_ABORTTRANSACTION
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


        public S3F0_ABORTTRANSACTION(SECSTransaction trx)
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

            trx.setStreamNWbit(3, false);
            trx.Function = 0;


            return trx;
        }

        public void FillItemValue(SECSTransaction trx)
        {

        }
    }
}