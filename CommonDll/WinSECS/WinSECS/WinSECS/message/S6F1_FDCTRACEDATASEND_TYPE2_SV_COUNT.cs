using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F1_FDCTRACEDATASEND_TYPE2_SV_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String svname= "";
		private String sv= "";

        public S6F1_FDCTRACEDATASEND_TYPE2_SV_COUNT(String svname, String sv)
        {
			this.svname = svname;
			this.sv = sv;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(svname).Length, "SVNAME", svname);
			else
				ownerList.add(AsciiFormat.TYPE, 40, "SVNAME", svname);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sv).Length, "SV", sv);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "SV", sv);

            return ownerList;
        }

    }
}