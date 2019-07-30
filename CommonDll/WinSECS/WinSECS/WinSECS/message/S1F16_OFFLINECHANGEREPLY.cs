using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F16_OFFLINECHANGEREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String oflack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 16;

			String[] sArray =  oflack.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "OFLACK", oflack);
			else
				trx.add(Uint1Format.TYPE, 1, "OFLACK", oflack);

            return trx;

        }
    }


}