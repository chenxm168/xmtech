using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT> ecid_count= new List<S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT> ECID_COUNT
		{
			get { return ecid_count; }
			set { ecid_count = value; }
		}


        public S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT()
        {
        }

        public S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			ListFormat listNode_ECID_COUNT = listFormat.Children[1] as ListFormat;
			for (int i = 0; i < listNode_ECID_COUNT.Length; i++)
			{
				S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT vList = new S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT();
				vList.FillItemValue(listNode_ECID_COUNT.Children[i] as ListFormat);
				this.ecid_count.Add(vList);
			}

        }
    }
}
