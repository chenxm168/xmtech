using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_iEQPSTATUSREPLY_TOOL_COUNT_EOID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String eoid= "";
		private String eov= "";

        public S1F6_iEQPSTATUSREPLY_TOOL_COUNT_EOID_COUNT(String eoid, String eov)
        {
			this.eoid = eoid;
			this.eov = eov;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  eoid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EOID", eoid);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EOID", eoid);
			sArray =  eov.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EOV", eov);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EOV", eov);

            return ownerList;
        }

    }
}