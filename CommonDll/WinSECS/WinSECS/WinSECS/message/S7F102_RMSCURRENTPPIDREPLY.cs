using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F102_RMSCURRENTPPIDREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , List<String> ppid_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(7, false);
            trx.Function = 102;

			trx.add(CPrivateUtility.getMessage(isNoPadding, ppid_count, AsciiFormat.TYPE, "PPID", 20));

            return trx;

        }
    }


}