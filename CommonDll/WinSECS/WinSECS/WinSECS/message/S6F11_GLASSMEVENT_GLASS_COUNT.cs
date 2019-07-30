using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_GLASSMEVENT_GLASS_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String lotid= "";
		private String glassid= "";
		private String fslotno= "";
		private String tslotno= "";

        public S6F11_GLASSMEVENT_GLASS_COUNT(String lotid, String glassid, String fslotno, String tslotno)
        {
			this.lotid = lotid;
			this.glassid = glassid;
			this.fslotno = fslotno;
			this.tslotno = tslotno;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 4;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotid).Length, "LOTID", lotid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(fslotno).Length, "FSLOTNO", fslotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "FSLOTNO", fslotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(tslotno).Length, "TSLOTNO", tslotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "TSLOTNO", tslotno);

            return ownerList;
        }

    }
}