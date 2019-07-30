using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String sub_recipeid= "";
		private List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT> ccode_count= new List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT>();

        public S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT(String toolid, String sub_recipeid, List<S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT> ccode_count)
        {
			this.toolid = toolid;
			this.sub_recipeid = sub_recipeid;
			this.ccode_count = ccode_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			String[] sArray =  toolid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "TOOLID", toolid);
			sArray =  sub_recipeid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "SUB_RECIPEID", sub_recipeid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "SUB_RECIPEID", sub_recipeid);
			ListFormat listNode_CCODE_COUNT = ownerList.add(ListFormat.TYPE, -1, "CCODE_COUNT", "") as ListFormat;
			if(ccode_count != null)
			{
				foreach (S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT item in ccode_count)
				{
					listNode_CCODE_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}