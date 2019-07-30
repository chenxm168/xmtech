using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String eoid= "";
		private String eov= "";

       
		public String EOID
		{
			get { return eoid; }
			set { eoid = value; }
		}

		public String EOV
		{
			get { return eov; }
			set { eov = value; }
		}


        public S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT()
        {
        }

        public S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.eoid = listFormat.Children[0].Value;
			this.eov = listFormat.Children[1].Value;

        }
    }
}
