using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F4_FDCEQPSTATUSREPLY_TYPE1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , List<S1F4_FDCEQPSTATUSREPLY_TYPE1_TOOL_COUNT> tool_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 4;

			ListFormat listNode_TOOL_COUNT = trx.add(ListFormat.TYPE, -1, "TOOL_COUNT", "") as ListFormat;
			if(tool_count != null)
			{
				foreach (S1F4_FDCEQPSTATUSREPLY_TYPE1_TOOL_COUNT item in tool_count)
				{
					listNode_TOOL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}