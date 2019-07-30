using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S6F3_ProcessDataReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ceid, String unitid, String pnlid, String ppid, List<S6F3_ProcessDataReport_> item_0)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 3;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  ceid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "CEID", ceid);
			else
				listNode_0.add(AsciiFormat.TYPE, 3, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 5, "", "") as ListFormat;
			sArray =  unitid.Split(' ');
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				listNode_1.add(AsciiFormat.TYPE, 20, "UNITID", unitid);
			sArray =  pnlid.Split(' ');
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, sArray.Length, "PNLID", pnlid);
			else
				listNode_1.add(AsciiFormat.TYPE, 20, "PNLID", pnlid);
			sArray =  ppid.Split(' ');
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				listNode_1.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, -1, "", "") as ListFormat;
			if(item_0 != null)
			{
				foreach (S6F3_ProcessDataReport_ item in item_0)
				{
					listNode_2.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}