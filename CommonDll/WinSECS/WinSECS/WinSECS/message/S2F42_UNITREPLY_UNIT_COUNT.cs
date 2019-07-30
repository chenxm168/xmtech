using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_UNITREPLY_UNIT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid_cp= "";
		private String unitidack= "";

        public S2F42_UNITREPLY_UNIT_COUNT(String unitid_cp, String unitidack)
        {
			this.unitid_cp = unitid_cp;
			this.unitidack = unitidack;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  unitid_cp.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "UNITID_CP", unitid_cp);
			else
				ownerList.add(AsciiFormat.TYPE, 6, "UNITID_CP", unitid_cp);
			sArray =  unitidack.Split(' ');
			if (isNoPadding)
				ownerList.add(BinaryFormat.TYPE, sArray.Length, "UNITIDACK", unitidack);
			else
				ownerList.add(BinaryFormat.TYPE, 1, "UNITIDACK", unitidack);

            return ownerList;
        }

    }
}