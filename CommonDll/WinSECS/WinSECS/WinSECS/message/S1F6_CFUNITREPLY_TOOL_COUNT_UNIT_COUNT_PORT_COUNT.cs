using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String slotno= "";
		private String glassid= "";
		private String frompm= "";

        public S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT(String slotno, String glassid, String frompm)
        {
			this.slotno = slotno;
			this.glassid = glassid;
			this.frompm = frompm;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(frompm).Length, "FROMPM", frompm);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "FROMPM", frompm);

            return ownerList;
        }

    }
}