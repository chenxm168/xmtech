using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String sub_recipeid= "";

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public String SUB_RECIPEID
		{
			get { return sub_recipeid; }
			set { sub_recipeid = value; }
		}


        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT()
        {
        }

        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			this.sub_recipeid = listFormat.Children[1].Value;

        }
    }
}
