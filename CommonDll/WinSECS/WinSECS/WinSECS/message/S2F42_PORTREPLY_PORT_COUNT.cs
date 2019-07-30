using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_PORTREPLY_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ptid_cp= "";
		private String cpack= "";

        public S2F42_PORTREPLY_PORT_COUNT(String ptid_cp, String cpack)
        {
			this.ptid_cp = ptid_cp;
			this.cpack = cpack;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptid_cp).Length, "PTID_CP", ptid_cp);
			else
				ownerList.add(AsciiFormat.TYPE, 10, "PTID_CP", ptid_cp);
			String[] sArray =  cpack.Split(' ');
			if (isNoPadding)
				ownerList.add(BinaryFormat.TYPE, sArray.Length, "CPACK", cpack);
			else
				ownerList.add(BinaryFormat.TYPE, 1, "CPACK", cpack);

            return ownerList;
        }

    }
}