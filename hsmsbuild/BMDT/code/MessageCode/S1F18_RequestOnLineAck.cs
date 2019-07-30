using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S1F18_RequestOnLineAck
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String onlack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 18;

			String[] sArray =  onlack.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "ONLACK", onlack);
			else
				trx.add(AsciiFormat.TYPE, 1, "ONLACK", onlack);

            return trx;

        }
    }


}