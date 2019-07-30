using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    class S2F18_DateTimeDataRequestAck
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


        public S2F18_DateTimeDataRequestAck(SECSTransaction trx)
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

        public static SECSTransaction makeTransaction(bool isNoPadding , String time)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 18;

			String[] sArray =  time.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "TIME", time);
			else
				trx.add(AsciiFormat.TYPE, 14, "TIME", time);

            return trx;
        }

        public void FillItemValue(SECSTransaction trx)
        {
			this.time = trx.Children[0].Value;

        }
    }
}