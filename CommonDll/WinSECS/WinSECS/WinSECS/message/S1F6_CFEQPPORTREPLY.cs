using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_CFEQPPORTREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String sfcd, List<S1F6_CFEQPPORTREPLY_TOOL_COUNT> tool_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 6;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  sfcd.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "SFCD", sfcd);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "SFCD", sfcd);
			ListFormat listNode_TOOL_COUNT = listNode_0.add(ListFormat.TYPE, -1, "TOOL_COUNT", "") as ListFormat;
			if(tool_count != null)
			{
				foreach (S1F6_CFEQPPORTREPLY_TOOL_COUNT item in tool_count)
				{
					listNode_TOOL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}