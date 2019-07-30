using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class TransactionTimerTimeout
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String shead)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(9, false);
            trx.Function = 9;

			String[] sArray =  shead.Split(' ');
			if (isNoPadding)
				trx.add(BinaryFormat.TYPE, sArray.Length, "SHEAD", shead);
			else
				trx.add(BinaryFormat.TYPE, 10, "SHEAD", shead);

            return trx;

        }
    }


}