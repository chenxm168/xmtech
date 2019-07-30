using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_CFSORTINFO_SLOTINFO_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String slotno= "";
		private String glsex= "";

        public S6F11_CFSORTINFO_SLOTINFO_COUNT(String slotno, String glsex)
        {
			this.slotno = slotno;
			this.glsex = glsex;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			String[] sArray =  glsex.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "GLSEX", glsex);
			else
				ownerList.add(Uint1Format.TYPE, 1, "GLSEX", glsex);

            return ownerList;
        }

    }
}