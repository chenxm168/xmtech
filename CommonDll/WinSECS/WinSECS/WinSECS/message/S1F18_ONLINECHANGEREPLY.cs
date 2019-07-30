using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F18_ONLINECHANGEREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String onlack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 18;

			String[] sArray =  onlack.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "ONLACK", onlack);
			else
				trx.add(Uint1Format.TYPE, 1, "ONLACK", onlack);

            return trx;

        }
    }


}