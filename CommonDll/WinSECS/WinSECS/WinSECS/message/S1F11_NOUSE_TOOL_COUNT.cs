using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F11_NOUSE_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<String> svid_count= new List<String>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<String> SVID_COUNT
		{
			get { return svid_count; }
			set { svid_count = value; }
		}


        public S1F11_NOUSE_TOOL_COUNT()
        {
        }

        public S1F11_NOUSE_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			this.svid_count = CPrivateUtility.getStringListItems(listFormat.Children[1] as ListFormat);

        }
    }
}
