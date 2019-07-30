using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F17_ONLINEDATETIMEREQUEST
    {
        public static SECSTransaction makeTransaction(bool isNoPadding )
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, true);
            trx.Function = 17;


            return trx;

        }
    }


}