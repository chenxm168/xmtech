using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F41_PORTCOMMAND_PORT_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ptid_cp= "";
		private String ptid= "";

       
		public String PTID_CP
		{
			get { return ptid_cp; }
			set { ptid_cp = value; }
		}

		public String PTID
		{
			get { return ptid; }
			set { ptid = value; }
		}


        public S2F41_PORTCOMMAND_PORT_COUNT()
        {
        }

        public S2F41_PORTCOMMAND_PORT_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.ptid_cp = listFormat.Children[0].Value;
			this.ptid = listFormat.Children[1].Value;

        }
    }
}
