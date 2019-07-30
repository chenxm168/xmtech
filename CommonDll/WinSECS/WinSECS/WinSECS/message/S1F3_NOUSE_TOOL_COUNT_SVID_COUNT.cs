using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F3_NOUSE_TOOL_COUNT_SVID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String unitid= "";
		private String svid= "";

       
		public String UNITID
		{
			get { return unitid; }
			set { unitid = value; }
		}

		public String SVID
		{
			get { return svid; }
			set { svid = value; }
		}


        public S1F3_NOUSE_TOOL_COUNT_SVID_COUNT()
        {
        }

        public S1F3_NOUSE_TOOL_COUNT_SVID_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.unitid = listFormat.Children[0].Value;
			this.svid = listFormat.Children[1].Value;

        }
    }
}
