using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_B
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ppid, String mdln, String softrev, List<S7F26_RMSFORMATTEDPPIDDATAREPLY_B_TOOL_COUNT> tool_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(7, false);
            trx.Function = 26;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 4, "", "") as ListFormat;
			String[] sArray =  ppid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			sArray =  mdln.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "MDLN", mdln);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "MDLN", mdln);
			sArray =  softrev.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "SOFTREV", softrev);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "SOFTREV", softrev);
			ListFormat listNode_TOOL_COUNT = listNode_0.add(ListFormat.TYPE, -1, "TOOL_COUNT", "") as ListFormat;
			if(tool_count != null)
			{
				foreach (S7F26_RMSFORMATTEDPPIDDATAREPLY_B_TOOL_COUNT item in tool_count)
				{
					listNode_TOOL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}