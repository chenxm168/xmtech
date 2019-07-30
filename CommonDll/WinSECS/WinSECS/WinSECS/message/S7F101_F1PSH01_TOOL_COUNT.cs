using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F101_F1PSH01_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String processid= "";
		private String stepid= "";

       
		public String PROCESSID
		{
			get { return processid; }
			set { processid = value; }
		}

		public String STEPID
		{
			get { return stepid; }
			set { stepid = value; }
		}


        public S7F101_F1PSH01_TOOL_COUNT()
        {
        }

        public S7F101_F1PSH01_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.processid = listFormat.Children[0].Value;
			this.stepid = listFormat.Children[1].Value;

        }
    }
}
