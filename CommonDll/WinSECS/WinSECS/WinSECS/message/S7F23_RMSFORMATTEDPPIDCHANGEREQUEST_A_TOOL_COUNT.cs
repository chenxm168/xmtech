using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String sub_recipeid= "";
		private List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT> ccode_count= new List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT>();

       
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

		public List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT> CCODE_COUNT
		{
			get { return ccode_count; }
			set { ccode_count = value; }
		}


        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT()
        {
        }

        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			this.sub_recipeid = listFormat.Children[1].Value;
			ListFormat listNode_CCODE_COUNT = listFormat.Children[2] as ListFormat;
			for (int i = 0; i < listNode_CCODE_COUNT.Length; i++)
			{
				S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT vList = new S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT();
				vList.FillItemValue(listNode_CCODE_COUNT.Children[i] as ListFormat);
				this.ccode_count.Add(vList);
			}

        }
    }
}
