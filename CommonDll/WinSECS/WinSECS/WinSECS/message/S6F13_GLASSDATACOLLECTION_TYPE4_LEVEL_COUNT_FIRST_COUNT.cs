using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_GLASSDATACOLLECTION_TYPE4_LEVEL_COUNT_FIRST_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String name1= "";
		private String value1= "";

        public S6F13_GLASSDATACOLLECTION_TYPE4_LEVEL_COUNT_FIRST_COUNT(String name1, String value1)
        {
			this.name1 = name1;
			this.value1 = value1;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(name1).Length, "NAME1", name1);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "NAME1", name1);
			String[] sArray =  value1.Split(' ');
			if (isNoPadding)
				ownerList.add(XFormat.TYPE, sArray.Length, "VALUE1", value1);
			else
				ownerList.add(XFormat.TYPE, 80, "VALUE1", value1);

            return ownerList;
        }

    }
}