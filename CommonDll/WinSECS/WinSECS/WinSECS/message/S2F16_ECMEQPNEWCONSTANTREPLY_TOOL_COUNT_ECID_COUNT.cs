using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F16_ECMEQPNEWCONSTANTREPLY_TOOL_COUNT_ECID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ecid= "";
		private String eac= "";

        public S2F16_ECMEQPNEWCONSTANTREPLY_TOOL_COUNT_ECID_COUNT(String ecid, String eac)
        {
			this.ecid = ecid;
			this.eac = eac;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  ecid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "ECID", ecid);
			else
				ownerList.add(Uint2Format.TYPE, 1, "ECID", ecid);
			sArray =  eac.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EAC", eac);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EAC", eac);

            return ownerList;
        }

    }
}