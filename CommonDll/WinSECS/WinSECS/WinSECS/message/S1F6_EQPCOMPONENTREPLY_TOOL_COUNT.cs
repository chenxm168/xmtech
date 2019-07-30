using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPCOMPONENTREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S1F6_EQPCOMPONENTREPLY_TOOL_COUNT_GLASS_COUNT> glass_count= new List<S1F6_EQPCOMPONENTREPLY_TOOL_COUNT_GLASS_COUNT>();

        public S1F6_EQPCOMPONENTREPLY_TOOL_COUNT(String toolid, List<S1F6_EQPCOMPONENTREPLY_TOOL_COUNT_GLASS_COUNT> glass_count)
        {
			this.toolid = toolid;
			this.glass_count = glass_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_GLASS_COUNT = ownerList.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S1F6_EQPCOMPONENTREPLY_TOOL_COUNT_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}