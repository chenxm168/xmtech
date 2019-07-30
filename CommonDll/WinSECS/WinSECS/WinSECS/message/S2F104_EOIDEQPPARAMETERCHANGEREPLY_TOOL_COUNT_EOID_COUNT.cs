using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String eoid= "";
		private String eac= "";

        public S2F104_EOIDEQPPARAMETERCHANGEREPLY_TOOL_COUNT_EOID_COUNT(String eoid, String eac)
        {
			this.eoid = eoid;
			this.eac = eac;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  eoid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EOID", eoid);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EOID", eoid);
			sArray =  eac.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "EAC", eac);
			else
				ownerList.add(Uint1Format.TYPE, 1, "EAC", eac);

            return ownerList;
        }

    }
}