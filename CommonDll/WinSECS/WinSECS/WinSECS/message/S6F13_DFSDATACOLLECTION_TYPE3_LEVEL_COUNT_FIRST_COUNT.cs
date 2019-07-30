using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_DFSDATACOLLECTION_TYPE3_LEVEL_COUNT_FIRST_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String name= "";
		private String value= "";

        public S6F13_DFSDATACOLLECTION_TYPE3_LEVEL_COUNT_FIRST_COUNT(String name, String value)
        {
			this.name = name;
			this.value = value;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(name).Length, "NAME", name);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "NAME", name);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(value).Length, "VALUE", value);
			else
				ownerList.add(AsciiFormat.TYPE, 80, "VALUE", value);

            return ownerList;
        }

    }
}