using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_B_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String sub_recipeid= "";

        public S7F26_RMSFORMATTEDPPIDDATAREPLY_B_TOOL_COUNT(String toolid, String sub_recipeid)
        {
			this.toolid = toolid;
			this.sub_recipeid = sub_recipeid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

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

            return ownerList;
        }

    }
}