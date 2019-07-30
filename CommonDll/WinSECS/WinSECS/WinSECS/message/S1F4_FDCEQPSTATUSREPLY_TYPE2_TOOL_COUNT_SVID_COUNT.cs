using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F4_FDCEQPSTATUSREPLY_TYPE2_TOOL_COUNT_SVID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid= "";
		private String svid= "";
		private String sv= "";

        public S1F4_FDCEQPSTATUSREPLY_TYPE2_TOOL_COUNT_SVID_COUNT(String unitid, String svid, String sv)
        {
			this.unitid = unitid;
			this.svid = svid;
			this.sv = sv;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			String[] sArray =  svid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "SVID", svid);
			else
				ownerList.add(Uint2Format.TYPE, 1, "SVID", svid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sv).Length, "SV", sv);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "SV", sv);

            return ownerList;
        }

    }
}