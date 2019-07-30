using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S6F111_CurrentRecipeDataReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String unitid, String ppid, List<S6F111_CurrentRecipeDataReport_> item_0)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 111;
            ppid = ppid.PadRight(ConstDef.PPID_LEN, ' ');
            unitid = unitid.PadRight(ConstDef.UNNIT_LEN, ' ');
			String[] sArray =  unitid.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				trx.add(AsciiFormat.TYPE, 20, "UNITID", unitid);
			sArray =  ppid.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				trx.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_0 = trx.add(ListFormat.TYPE, -1, "", "") as ListFormat;
			if(item_0 != null)
			{
				foreach (S6F111_CurrentRecipeDataReport_ item in item_0)
				{
					listNode_0.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}