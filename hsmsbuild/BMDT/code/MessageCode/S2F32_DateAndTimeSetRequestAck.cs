using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S2F32_DateAndTimeSetRequestAck
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String tiack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 32;

			String[] sArray =  tiack.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "TIACK", tiack);
			else
				trx.add(AsciiFormat.TYPE, 1, "TIACK", tiack);

            return trx;

        }
    }


}