using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ccode= "";
		private List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT> pparm_count= new List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT>();

       
		public String CCODE
		{
			get { return ccode; }
			set { ccode = value; }
		}

		public List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT> PPARM_COUNT
		{
			get { return pparm_count; }
			set { pparm_count = value; }
		}


        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT()
        {
        }

        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.ccode = listFormat.Children[0].Value;
			ListFormat listNode_PPARM_COUNT = listFormat.Children[1] as ListFormat;
			for (int i = 0; i < listNode_PPARM_COUNT.Length; i++)
			{
				S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT vList = new S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A_TOOL_COUNT_CCODE_COUNT_PPARM_COUNT();
				vList.FillItemValue(listNode_PPARM_COUNT.Children[i] as ListFormat);
				this.pparm_count.Add(vList);
			}

        }
    }
}
