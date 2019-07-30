using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT_SVID_COUNT> svid_count= new List<S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT_SVID_COUNT>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT_SVID_COUNT> SVID_COUNT
		{
			get { return svid_count; }
			set { svid_count = value; }
		}


        public S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT()
        {
        }

        public S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			ListFormat listNode_SVID_COUNT = listFormat.Children[1] as ListFormat;
			for (int i = 0; i < listNode_SVID_COUNT.Length; i++)
			{
				S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT_SVID_COUNT vList = new S1F3_FDCEQPSTATUSREQUEST_TOOL_COUNT_SVID_COUNT();
				vList.FillItemValue(listNode_SVID_COUNT.Children[i] as ListFormat);
				this.svid_count.Add(vList);
			}

        }
    }
}
