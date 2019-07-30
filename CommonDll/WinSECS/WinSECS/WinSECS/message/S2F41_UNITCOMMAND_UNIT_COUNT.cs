using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F41_UNITCOMMAND_UNIT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid_cp= "";
		private String unitid= "";

       
		public String UNITID_CP
		{
			get { return unitid_cp; }
			set { unitid_cp = value; }
		}

		public String UNITID
		{
			get { return unitid; }
			set { unitid = value; }
		}


        public S2F41_UNITCOMMAND_UNIT_COUNT()
        {
        }

        public S2F41_UNITCOMMAND_UNIT_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.unitid_cp = listFormat.Children[0].Value;
			this.unitid = listFormat.Children[1].Value;

        }
    }
}
