using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_iEQPPORTREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S1F6_iEQPPORTREPLY_TOOL_COUNT_PORT_COUNT> port_count= new List<S1F6_iEQPPORTREPLY_TOOL_COUNT_PORT_COUNT>();

        public S1F6_iEQPPORTREPLY_TOOL_COUNT(String toolid, List<S1F6_iEQPPORTREPLY_TOOL_COUNT_PORT_COUNT> port_count)
        {
			this.toolid = toolid;
			this.port_count = port_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_PORT_COUNT = ownerList.add(ListFormat.TYPE, -1, "PORT_COUNT", "") as ListFormat;
			if(port_count != null)
			{
				foreach (S1F6_iEQPPORTREPLY_TOOL_COUNT_PORT_COUNT item in port_count)
				{
					listNode_PORT_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}