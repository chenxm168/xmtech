using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_iEQPUNITREPLY_TOOL_COUNT_UNIT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid= "";
		private String unit_state= "";

        public S1F6_iEQPUNITREPLY_TOOL_COUNT_UNIT_COUNT(String unitid, String unit_state)
        {
			this.unitid = unitid;
			this.unit_state = unit_state;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			String[] sArray =  unit_state.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "UNIT_STATE", unit_state);
			else
				ownerList.add(Uint1Format.TYPE, 1, "UNIT_STATE", unit_state);

            return ownerList;
        }

    }
}