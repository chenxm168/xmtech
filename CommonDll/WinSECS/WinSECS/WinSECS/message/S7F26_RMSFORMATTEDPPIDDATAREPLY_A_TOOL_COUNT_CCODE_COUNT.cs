using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ccode= "";
		private List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT> pparm_count= new List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT>();

        public S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT(String ccode, List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT> pparm_count)
        {
			this.ccode = ccode;
			this.pparm_count = pparm_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  ccode.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "CCODE", ccode);
			else
				ownerList.add(Uint2Format.TYPE, 1, "CCODE", ccode);
			ListFormat listNode_PPARM_COUNT = ownerList.add(ListFormat.TYPE, -1, "PPARM_COUNT", "") as ListFormat;
			if(pparm_count != null)
			{
				foreach (S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT item in pparm_count)
				{
					listNode_PPARM_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}