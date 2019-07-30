using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ecid= "";
		private String ecname= "";
		private String ecmin= "";
		private String ecmax= "";
		private String ecdef= "";
		private String unitid= "";

        public S2F30_ECMEQPCONSTANTNAMELISTREPLY_TOOL_COUNT_ECID_COUNT(String ecid, String ecname, String ecmin, String ecmax, String ecdef, String unitid)
        {
			this.ecid = ecid;
			this.ecname = ecname;
			this.ecmin = ecmin;
			this.ecmax = ecmax;
			this.ecdef = ecdef;
			this.unitid = unitid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 6;

			String[] sArray =  ecid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint2Format.TYPE, sArray.Length, "ECID", ecid);
			else
				ownerList.add(Uint2Format.TYPE, 1, "ECID", ecid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ecname).Length, "ECNAME", ecname);
			else
				ownerList.add(AsciiFormat.TYPE, 40, "ECNAME", ecname);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ecmin).Length, "ECMIN", ecmin);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "ECMIN", ecmin);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ecmax).Length, "ECMAX", ecmax);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "ECMAX", ecmax);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ecdef).Length, "ECDEF", ecdef);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "ECDEF", ecdef);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);

            return ownerList;
        }

    }
}