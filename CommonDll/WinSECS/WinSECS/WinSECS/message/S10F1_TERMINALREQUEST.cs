using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S10F1_TERMINALREQUEST
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String tid, String text)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(10, true);
            trx.Function = 1;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  tid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "TID", tid);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "TID", tid);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(text).Length, "TEXT", text);
			else
				listNode_0.add(AsciiFormat.TYPE, 80, "TEXT", text);

            return trx;

        }
    }


}