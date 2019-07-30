using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F102_ALARMLISTREPLY_TYPE2
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , List<S5F102_ALARMLISTREPLY_TYPE2_TOOL_COUNT> tool_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(5, false);
            trx.Function = 102;

			ListFormat listNode_TOOL_COUNT = trx.add(ListFormat.TYPE, -1, "TOOL_COUNT", "") as ListFormat;
			if(tool_count != null)
			{
				foreach (S5F102_ALARMLISTREPLY_TYPE2_TOOL_COUNT item in tool_count)
				{
					listNode_TOOL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}