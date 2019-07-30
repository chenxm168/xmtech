using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ecid= "";
		private String ecv= "";

       
		public String ECID
		{
			get { return ecid; }
			set { ecid = value; }
		}

		public String ECV
		{
			get { return ecv; }
			set { ecv = value; }
		}


        public S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT()
        {
        }

        public S2F15_ECMEQPNEWCONSTANTREQUEST_TOOL_COUNT_ECID_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.ecid = listFormat.Children[0].Value;
			this.ecv = listFormat.Children[1].Value;

        }
    }
}
