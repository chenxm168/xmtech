using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F24_FDCTRACEINITREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String tiaack)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 24;

			String[] sArray =  tiaack.Split(' ');
			if (isNoPadding)
				trx.add(Uint1Format.TYPE, sArray.Length, "TIAACK", tiaack);
			else
				trx.add(Uint1Format.TYPE, 1, "TIAACK", tiaack);

            return trx;

        }
    }


}