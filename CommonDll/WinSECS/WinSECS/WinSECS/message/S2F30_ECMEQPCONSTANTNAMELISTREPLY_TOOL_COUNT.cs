using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String tiack= "";
		private List<S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT> ecid_count= new List<S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT>();

        public S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT(String toolid, String tiack, List<S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT> ecid_count)
        {
			this.toolid = toolid;
			this.tiack = tiack;
			this.ecid_count = ecid_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			String[] sArray =  toolid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			sArray =  tiack.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "TIACK", tiack);
			else
				ownerList.add(Uint1Format.TYPE, 1, "TIACK", tiack);
			ListFormat listNode_ECID_COUNT = ownerList.add(ListFormat.TYPE, -1, "ECID_COUNT", "") as ListFormat;
			if(ecid_count != null)
			{
				foreach (S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT item in ecid_count)
				{
					listNode_ECID_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}