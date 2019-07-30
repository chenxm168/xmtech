using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F102_SVSTATUSREPLY_SVID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String svname= "";
		private String sv= "";

        public S1F102_SVSTATUSREPLY_SVID_COUNT(String svname, String sv)
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
				ownerList.add(AsciiFormat.TYPE, 10, "SVNAME", svname);
			String[] sArray =  sv.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "SV", sv);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "SV", sv);

            return ownerList;
        }

    }
}