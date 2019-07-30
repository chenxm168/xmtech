using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F103_ALARMRESETREQEUST_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private List<S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT> alarm_count= new List<S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT>();

       
		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT> ALARM_COUNT
		{
			get { return alarm_count; }
			set { alarm_count = value; }
		}


        public S5F103_ALARMRESETREQEUST_TOOL_COUNT()
        {
        }

        public S5F103_ALARMRESETREQEUST_TOOL_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.toolid = listFormat.Children[0].Value;
			ListFormat listNode_ALARM_COUNT = listFormat.Children[1] as ListFormat;
			for (int i = 0; i < listNode_ALARM_COUNT.Length; i++)
			{
				S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT vList = new S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT();
				vList.FillItemValue(listNode_ALARM_COUNT.Children[i] as ListFormat);
				this.alarm_count.Add(vList);
			}

        }
    }
}
