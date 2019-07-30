using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F1_AREYOUTHERE_TOHOST
    {
        public static SECSTransaction makeTransaction(bool isNoPadding )
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, true);
            trx.Function = 1;


            return trx;

        }
    }


}