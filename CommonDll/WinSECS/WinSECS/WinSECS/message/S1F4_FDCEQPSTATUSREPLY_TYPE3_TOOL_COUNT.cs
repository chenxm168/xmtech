using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String tiack= "";
		private List<S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT_SVID_COUNT> svid_count= new List<S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT_SVID_COUNT>();

        public S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT(String toolid, String tiack, List<S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT_SVID_COUNT> svid_count)
        {
			this.toolid = toolid;
			this.tiack = tiack;
			this.svid_count = svid_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			String[] sArray =  tiack.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "TIACK", tiack);
			else
				ownerList.add(Uint2Format.TYPE, 1, "TIACK", tiack);
			ListFormat listNode_SVID_COUNT = ownerList.add(ListFormat.TYPE, -1, "SVID_COUNT", "") as ListFormat;
			if(svid_count != null)
			{
				foreach (S1F4_FDCEQPSTATUSREPLY_TYPE3_TOOL_COUNT_SVID_COUNT item in svid_count)
				{
					listNode_SVID_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}