using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S2F42_OPCallRequestAck
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ackc2)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 42;

			String[] sArray =  ackc2.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "ACKC2", ackc2);
			else
				trx.add(AsciiFormat.TYPE, 1, "ACKC2", ackc2);

            return trx;

        }
    }


}