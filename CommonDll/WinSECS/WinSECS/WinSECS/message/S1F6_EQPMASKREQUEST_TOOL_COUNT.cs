using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPMASKREQUEST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT> mask_count= new List<S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT>();

        public S1F6_EQPMASKREQUEST_TOOL_COUNT(String toolid, List<S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT> mask_count)
        {
			this.toolid = toolid;
			this.mask_count = mask_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_MASK_COUNT = ownerList.add(ListFormat.TYPE, -1, "MASK_COUNT", "") as ListFormat;
			if(mask_count != null)
			{
				foreach (S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT item in mask_count)
				{
					listNode_MASK_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}