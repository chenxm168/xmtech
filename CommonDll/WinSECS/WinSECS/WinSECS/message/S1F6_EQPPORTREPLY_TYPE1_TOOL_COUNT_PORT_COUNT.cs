using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPPORTREPLY_TYPE1_TOOL_COUNT_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ptid= "";
		private String csid= "";
		private String tspt= "";
		private String stif= "";

        public S1F6_EQPPORTREPLY_TYPE1_TOOL_COUNT_PORT_COUNT(String ptid, String csid, String tspt, String stif)
        {
			this.ptid = ptid;
			this.csid = csid;
			this.tspt = tspt;
			this.stif = stif;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 4;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptid).Length, "PTID", ptid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "PTID", ptid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(csid).Length, "CSID", csid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "CSID", csid);
			String[] sArray =  tspt.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "TSPT", tspt);
			else
				ownerList.add(Uint1Format.TYPE, 1, "TSPT", tspt);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stif).Length, "STIF", stif);
			else
				ownerList.add(AsciiFormat.TYPE, 105, "STIF", stif);

            return ownerList;
        }

    }
}