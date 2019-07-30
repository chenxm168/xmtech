using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_CFEQPPORTREPLY_TOOL_COUNT_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ptid= "";
		private String csid= "";
		private String tspt= "";
		private String stif= "";
		private String ptmd= "";
		private String unloadtype= "";
		private String splitmode= "";
		private String sortmode= "";

        public S1F6_CFEQPPORTREPLY_TOOL_COUNT_PORT_COUNT(String ptid, String csid, String tspt, String stif, String ptmd, String unloadtype, String splitmode, String sortmode)
        {
			this.ptid = ptid;
			this.csid = csid;
			this.tspt = tspt;
			this.stif = stif;
			this.ptmd = ptmd;
			this.unloadtype = unloadtype;
			this.splitmode = splitmode;
			this.sortmode = sortmode;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 8;

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
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptmd).Length, "PTMD", ptmd);
			else
				ownerList.add(AsciiFormat.TYPE, 3, "PTMD", ptmd);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unloadtype).Length, "UNLOADTYPE", unloadtype);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "UNLOADTYPE", unloadtype);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(splitmode).Length, "SPLITMODE", splitmode);
			else
				ownerList.add(AsciiFormat.TYPE, 4, "SPLITMODE", splitmode);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sortmode).Length, "SORTMODE", sortmode);
			else
				ownerList.add(AsciiFormat.TYPE, 4, "SORTMODE", sortmode);

            return ownerList;
        }

    }
}