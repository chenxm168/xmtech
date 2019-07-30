using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S1F16_RequestOffLineAck
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String oflack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 16;

			String[] sArray =  oflack.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "OFLACK", oflack);
			else
				trx.add(AsciiFormat.TYPE, 1, "OFLACK", oflack);

            return trx;

        }
    }


}