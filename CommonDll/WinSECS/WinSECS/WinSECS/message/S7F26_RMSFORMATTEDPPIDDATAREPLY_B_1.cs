using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_B_1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String toolid_1, String ppid, String softrev, String item_0, String toolid, String sub_recipeid)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(7, false);
            trx.Function = 26;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 5, "", "") as ListFormat;
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid_1).Length, "TOOLID_1", toolid_1);
			else
				listNode_0.add(AsciiFormat.TYPE, 22, "TOOLID_1", toolid_1);
			String[] sArray =  ppid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			sArray =  softrev.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "SOFTREV", softrev);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "SOFTREV", softrev);
			sArray =  item_0.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "", item_0);
			else
				listNode_0.add(Uint1Format.TYPE, 0, "", item_0);
			ListFormat listNode_TOOL_COUNT = listNode_0.add(ListFormat.TYPE, 1, "TOOL_COUNT", "") as ListFormat;
			ListFormat listNode_1 = listNode_TOOL_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  toolid.Split(' ');
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, sArray.Length, "TOOLID", toolid);
			else
				listNode_1.add(AsciiFormat.TYPE, 16, "TOOLID", toolid);
			sArray =  sub_recipeid.Split(' ');
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, sArray.Length, "SUB_RECIPEID", sub_recipeid);
			else
				listNode_1.add(AsciiFormat.TYPE, 20, "SUB_RECIPEID", sub_recipeid);

            return trx;

        }
    }


}