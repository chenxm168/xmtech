using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F24_REPLY_1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ack7)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(7, false);
            trx.Function = 24;

			String[] sArray =  ack7.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "ACK7", ack7);
			else
				trx.add(Uint1Format.TYPE, 1, "ACK7", ack7);

            return trx;

        }
    }


}