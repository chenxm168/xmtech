using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String alcd= "";
		private String alid= "";
		private String altx= "";
		private String unitid= "";
		private String altm= "";

       
		public String ALCD
		{
			get { return alcd; }
			set { alcd = value; }
		}

		public String ALID
		{
			get { return alid; }
			set { alid = value; }
		}

		public String ALTX
		{
			get { return altx; }
			set { altx = value; }
		}

		public String UNITID
		{
			get { return unitid; }
			set { unitid = value; }
		}

		public String ALTM
		{
			get { return altm; }
			set { altm = value; }
		}


        public S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT()
        {
        }

        public S5F103_ALARMRESETREQEUST_TOOL_COUNT_ALARM_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.alcd = listFormat.Children[0].Value;
			this.alid = listFormat.Children[1].Value;
			this.altx = listFormat.Children[2].Value;
			this.unitid = listFormat.Children[3].Value;
			this.altm = listFormat.Children[4].Value;

        }
    }
}
