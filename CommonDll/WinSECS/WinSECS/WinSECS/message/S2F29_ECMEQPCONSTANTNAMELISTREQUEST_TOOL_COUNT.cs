using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F29_ECMEQPCONSTANTNAMELISTREQUEST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<String> ecid_count= new List<String>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<String> ECID_COUNT
		{
			get { return ecid_count; }
			set { ecid_count = value; }
		}


        public S2F29_ECMEQPCONSTANTNAMELISTREQUEST_TOOL_COUNT()
        {
        }

        public S2F29_ECMEQPCONSTANTNAMELISTREQUEST_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			this.ecid_count = CPrivateUtility.getStringListItems(listFormat.Children[1] as ListFormat);

        }
    }
}
