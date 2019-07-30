using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F102_OPERATORCALLREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ack2)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 102;

			String[] sArray =  ack2.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "ACK2", ack2);
			else
				trx.add(Uint1Format.TYPE, 1, "ACK2", ack2);

            return trx;

        }
    }


}