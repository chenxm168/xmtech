using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String pparmname= "";
		private String pparmvalue= "";

        public S7F26_RMSFORMATTEDPPIDDATAREPLY_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT(String pparmname, String pparmvalue)
        {
			this.pparmname = pparmname;
			this.pparmvalue = pparmvalue;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  pparmname.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PPARMNAME", pparmname);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "PPARMNAME", pparmname);
			sArray =  pparmvalue.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PPARMVALUE", pparmvalue);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "PPARMVALUE", pparmvalue);

            return ownerList;
        }

    }
}