using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F2_ONLINEDATA
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String mdln, String mode)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 2;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(mdln).Length, "MDLN", mdln);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "MDLN", mdln);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(mode).Length, "MODE", mode);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "MODE", mode);

            return trx;

        }
    }


}