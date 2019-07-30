using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid= "";
		private String eqst= "";
		private List<S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT> port_count= new List<S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT>();

        public S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT(String unitid, String eqst, List<S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT> port_count)
        {
			this.unitid = unitid;
			this.eqst = eqst;
			this.port_count = port_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			String[] sArray =  eqst.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EQST", eqst);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EQST", eqst);
			ListFormat listNode_PORT_COUNT = ownerList.add(ListFormat.TYPE, -1, "PORT_COUNT", "") as ListFormat;
			if(port_count != null)
			{
				foreach (S1F6_CFUNITREPLY_TOOL_COUNT_UNIT_COUNT_PORT_COUNT item in port_count)
				{
					listNode_PORT_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}