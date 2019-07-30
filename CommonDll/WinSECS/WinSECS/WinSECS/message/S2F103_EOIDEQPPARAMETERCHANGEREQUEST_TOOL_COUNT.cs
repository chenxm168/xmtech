using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT> eoid_count= new List<S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT> EOID_COUNT
		{
			get { return eoid_count; }
			set { eoid_count = value; }
		}


        public S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT()
        {
        }

        public S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			ListFormat listNode_EOID_COUNT = listFormat.Children[1] as ListFormat;
			for (int i = 0; i < listNode_EOID_COUNT.Length; i++)
			{
				S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT vList = new S2F103_EOIDEQPPARAMETERCHANGEREQUEST_TOOL_COUNT_EOID_COUNT();
				vList.FillItemValue(listNode_EOID_COUNT.Children[i] as ListFormat);
				this.eoid_count.Add(vList);
			}

        }
    }
}
