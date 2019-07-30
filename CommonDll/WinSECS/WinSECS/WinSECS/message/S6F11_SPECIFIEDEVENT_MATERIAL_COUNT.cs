using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_SPECIFIEDEVENT_MATERIAL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String mname= "";
		private String mvalue= "";

        public S6F11_SPECIFIEDEVENT_MATERIAL_COUNT(String mname, String mvalue)
        {
			this.mname = mname;
			this.mvalue = mvalue;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(mname).Length, "MNAME", mname);
			else
				ownerList.add(AsciiFormat.TYPE, 40, "MNAME", mname);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(mvalue).Length, "MVALUE", mvalue);
			else
				ownerList.add(AsciiFormat.TYPE, 40, "MVALUE", mvalue);

            return ownerList;
        }

    }
}