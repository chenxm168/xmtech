using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class UnrecognizedDeviceId
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String mhead)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(9, false);
            trx.Function = 1;

			String[] sArray =  mhead.Split(' ');
			if (isNoPadding)
				trx.add(BinaryFormat.TYPE, sArray.Length, "MHEAD", mhead);
			else
				trx.add(BinaryFormat.TYPE, 10, "MHEAD", mhead);

            return trx;

        }
    }


}