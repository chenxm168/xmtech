using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String tiack= "";
		private List<S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT> eoid_count= new List<S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT>();

        public S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT(String toolid, String tiack, List<S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT> eoid_count)
        {
			this.toolid = toolid;
			this.tiack = tiack;
			this.eoid_count = eoid_count;

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
				ownerList.add(Uint1Format.TYPE, sArray.Length, "TIACK", tiack);
			else
				ownerList.add(Uint1Format.TYPE, 1, "TIACK", tiack);
			ListFormat listNode_EOID_COUNT = ownerList.add(ListFormat.TYPE, -1, "EOID_COUNT", "") as ListFormat;
			if(eoid_count != null)
			{
				foreach (S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT item in eoid_count)
				{
					listNode_EOID_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}