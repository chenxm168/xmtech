using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F12_FDCEQPSTATUSNAMELISTREPLY_TYPE2_TOOL_COUNT_SVID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String svid= "";
		private String svname= "";
		private String unitid= "";

        public S1F12_FDCEQPSTATUSNAMELISTREPLY_TYPE2_TOOL_COUNT_SVID_COUNT(String svid, String svname, String unitid)
        {
			this.svid = svid;
			this.svname = svname;
			this.unitid = unitid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			String[] sArray =  svid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "SVID", svid);
			else
				ownerList.add(Uint2Format.TYPE, 1, "SVID", svid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(svname).Length, "SVNAME", svname);
			else
				ownerList.add(AsciiFormat.TYPE, 40, "SVNAME", svname);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);

            return ownerList;
        }

    }
}