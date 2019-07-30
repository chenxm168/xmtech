using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String name2= "";
		private String value2= "";

        public S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT(String name2, String value2)
        {
			this.name2 = name2;
			this.value2 = value2;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(name2).Length, "NAME2", name2);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "NAME2", name2);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(value2).Length, "VALUE2", value2);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "VALUE2", value2);

            return ownerList;
        }

    }
}