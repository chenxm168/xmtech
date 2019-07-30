using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String pparmname= "";
		private String pparmvalue= "";

       
		public String PPARMNAME
		{
			get { return pparmname; }
			set { pparmname = value; }
		}

		public String PPARMVALUE
		{
			get { return pparmvalue; }
			set { pparmvalue = value; }
		}


        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT()
        {
        }

        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.pparmname = listFormat.Children[0].Value;
			this.pparmvalue = listFormat.Children[1].Value;

        }
    }
}
