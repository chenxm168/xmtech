using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String maskloc= "";
		private String maskid= "";

        public S1F6_EQPMASKREQUEST_TOOL_COUNT_MASK_COUNT(String maskloc, String maskid)
        {
			this.maskloc = maskloc;
			this.maskid = maskid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskloc).Length, "MASKLOC", maskloc);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "MASKLOC", maskloc);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskid).Length, "MASKID", maskid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "MASKID", maskid);

            return ownerList;
        }

    }
}