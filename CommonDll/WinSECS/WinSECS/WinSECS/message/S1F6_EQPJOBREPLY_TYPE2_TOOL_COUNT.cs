using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT_JOB_COUNT> job_count= new List<S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT_JOB_COUNT>();

        public S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT(String toolid, List<S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT_JOB_COUNT> job_count)
        {
			this.toolid = toolid;
			this.job_count = job_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_JOB_COUNT = ownerList.add(ListFormat.TYPE, -1, "JOB_COUNT", "") as ListFormat;
			if(job_count != null)
			{
				foreach (S1F6_EQPJOBREPLY_TYPE2_TOOL_COUNT_JOB_COUNT item in job_count)
				{
					listNode_JOB_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}