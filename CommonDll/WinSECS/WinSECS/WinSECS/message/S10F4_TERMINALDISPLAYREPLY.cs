using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S10F4_TERMINALDISPLAYREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ack10)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(10, false);
            trx.Function = 4;

			String[] sArray =  ack10.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "ACK10", ack10);
			else
				trx.add(Uint1Format.TYPE, 1, "ACK10", ack10);

            return trx;

        }
    }


}