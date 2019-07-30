using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F102
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String toolid, String item_0)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(7, false);
            trx.Function = 102;

			ListFormat listNode_PPID_COUNT = trx.add(ListFormat.TYPE, 3, "PPID_COUNT", "") as ListFormat;
			String[] sArray =  toolid.Split(' ');
			if (isNoPadding)
				listNode_PPID_COUNT.add(AsciiFormat.TYPE, sArray.Length, "TOOLID", toolid);
			else
				listNode_PPID_COUNT.add(AsciiFormat.TYPE, 22, "TOOLID", toolid);
			sArray =  item_0.Split(' ');
			if (isNoPadding)
				listNode_PPID_COUNT.add(Uint1Format.TYPE, sArray.Length, "", item_0);
			else
				listNode_PPID_COUNT.add(Uint1Format.TYPE, 1, "", item_0);
			ListFormat listNode_0 = listNode_PPID_COUNT.add(ListFormat.TYPE, 0, "", "") as ListFormat;

            return trx;

        }
    }


}