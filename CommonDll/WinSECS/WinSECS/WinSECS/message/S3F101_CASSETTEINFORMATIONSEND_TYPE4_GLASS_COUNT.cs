using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F101_CASSETTEINFORMATIONSEND_TYPE4_GLASS_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String slotno= "";
		private String processid= "";
		private String partid= "";
		private String stepid= "";
		private String glasstype= "";
		private String lotid= "";
		private String glassid= "";
		private String ppid= "";
		private String cellgrade= "";
		private String lotaction= "";

       
		public String SLOTNO
		{
			get { return slotno; }
			set { slotno = value; }
		}

		public String PROCESSID
		{
			get { return processid; }
			set { processid = value; }
		}

		public String PARTID
		{
			get { return partid; }
			set { partid = value; }
		}

		public String STEPID
		{
			get { return stepid; }
			set { stepid = value; }
		}

		public String GLASSTYPE
		{
			get { return glasstype; }
			set { glasstype = value; }
		}

		public String LOTID
		{
			get { return lotid; }
			set { lotid = value; }
		}

		public String GLASSID
		{
			get { return glassid; }
			set { glassid = value; }
		}

		public String PPID
		{
			get { return ppid; }
			set { ppid = value; }
		}

		public String CELLGRADE
		{
			get { return cellgrade; }
			set { cellgrade = value; }
		}

		public String LOTACTION
		{
			get { return lotaction; }
			set { lotaction = value; }
		}


        public S3F101_CASSETTEINFORMATIONSEND_TYPE4_GLASS_COUNT()
        {
        }

        public S3F101_CASSETTEINFORMATIONSEND_TYPE4_GLASS_COUNT(ListFormat rootFormat)
        {
        }

        public void FillItemValue(ListFormat listFormat)
        {
			this.slotno = listFormat.Children[0].Value;
			this.processid = listFormat.Children[1].Value;
			this.partid = listFormat.Children[2].Value;
			this.stepid = listFormat.Children[3].Value;
			this.glasstype = listFormat.Children[4].Value;
			this.lotid = listFormat.Children[5].Value;
			this.glassid = listFormat.Children[6].Value;
			this.ppid = listFormat.Children[7].Value;
			this.cellgrade = listFormat.Children[8].Value;
			this.lotaction = listFormat.Children[9].Value;

        }
    }
}
