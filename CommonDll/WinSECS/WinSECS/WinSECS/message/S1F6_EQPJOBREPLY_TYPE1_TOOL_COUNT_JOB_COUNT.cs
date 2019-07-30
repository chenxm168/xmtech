using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ipid= "";
		private String opid= "";
		private String icid= "";
		private String ocid= "";
		private String jobid= "";
		private String jstate= "";
		private String stif= "";
		private String totalgstate= "";
		private List<S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT_GLASS_COUNT> glass_count= new List<S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT_GLASS_COUNT>();

        public S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT(String ipid, String opid, String icid, String ocid, String jobid, String jstate, String stif, String totalgstate, List<S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT_GLASS_COUNT> glass_count)
        {
			this.ipid = ipid;
			this.opid = opid;
			this.icid = icid;
			this.ocid = ocid;
			this.jobid = jobid;
			this.jstate = jstate;
			this.stif = stif;
			this.totalgstate = totalgstate;
			this.glass_count = glass_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 9;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "OPID", opid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "ICID", icid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "OCID", ocid);
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
				ownerList.add(AsciiFormat.TYPE, 20, "STIF", stif);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(totalgstate).Length, "TOTALGSTATE", totalgstate);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "TOTALGSTATE", totalgstate);
			ListFormat listNode_GLASS_COUNT = ownerList.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S1F6_EQPJOBREPLY_TYPE1_TOOL_COUNT_JOB_COUNT_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}