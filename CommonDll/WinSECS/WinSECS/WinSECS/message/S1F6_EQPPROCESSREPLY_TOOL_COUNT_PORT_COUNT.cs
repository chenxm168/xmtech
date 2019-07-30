using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ptid= "";
		private String csid= "";
		private String jobid= "";
		private String jstate= "";
		private String stif= "";
		private String totalgstate= "";
		private String ptmd= "";
		private String unloadtype= "";
		private String ptst= "";
		private String splitmode= "";
		private String sortmode= "";
		private List<S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT> glass_count= new List<S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT>();

        public S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT(String ptid, String csid, String jobid, String jstate, String stif, String totalgstate, String ptmd, String unloadtype, String ptst, String splitmode, String sortmode, List<S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT> glass_count)
        {
			this.ptid = ptid;
			this.csid = csid;
			this.jobid = jobid;
			this.jstate = jstate;
			this.stif = stif;
			this.totalgstate = totalgstate;
			this.ptmd = ptmd;
			this.unloadtype = unloadtype;
			this.ptst = ptst;
			this.splitmode = splitmode;
			this.sortmode = sortmode;
			this.glass_count = glass_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 12;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptid).Length, "PTID", ptid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "PTID", ptid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(csid).Length, "CSID", csid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "CSID", csid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			String[] sArray =  jstate.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "JSTATE", jstate);
			else
				ownerList.add(Uint1Format.TYPE, 1, "JSTATE", jstate);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stif).Length, "STIF", stif);
			else
				ownerList.add(AsciiFormat.TYPE, 105, "STIF", stif);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(totalgstate).Length, "TOTALGSTATE", totalgstate);
			else
				ownerList.add(AsciiFormat.TYPE, 105, "TOTALGSTATE", totalgstate);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptmd).Length, "PTMD", ptmd);
			else
				ownerList.add(AsciiFormat.TYPE, 3, "PTMD", ptmd);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unloadtype).Length, "UNLOADTYPE", unloadtype);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "UNLOADTYPE", unloadtype);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptst).Length, "PTST", ptst);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "PTST", ptst);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(splitmode).Length, "SPLITMODE", splitmode);
			else
				ownerList.add(AsciiFormat.TYPE, 4, "SPLITMODE", splitmode);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sortmode).Length, "SORTMODE", sortmode);
			else
				ownerList.add(AsciiFormat.TYPE, 4, "SORTMODE", sortmode);
			ListFormat listNode_GLASS_COUNT = ownerList.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}