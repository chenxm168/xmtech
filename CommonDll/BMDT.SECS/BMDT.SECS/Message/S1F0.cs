using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S1F0
    {
        public static SECSTransaction makeTransaction(bool isNoPadding ,long systembyte)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 0;

            trx.Systembyte = systembyte;
            return trx;

        }
    }


}