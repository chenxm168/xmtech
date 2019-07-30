using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F104_ALARMRESETREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String tiack= "";
		private List<S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT> alarm_count= new List<S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT>();

        public S5F104_ALARMRESETREPLY_TOOL_COUNT(String toolid, String tiack, List<S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT> alarm_count)
        {
			this.toolid = toolid;
			this.tiack = tiack;
			this.alarm_count = alarm_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 3;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			String[] sArray =  tiack.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "TIACK", tiack);
			else
				ownerList.add(Uint1Format.TYPE, 1, "TIACK", tiack);
			ListFormat listNode_ALARM_COUNT = ownerList.add(ListFormat.TYPE, -1, "ALARM_COUNT", "") as ListFormat;
			if(alarm_count != null)
			{
				foreach (S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT item in alarm_count)
				{
					listNode_ALARM_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}