using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT
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
		private String direction= "";
		private String glasssize= "";
		private String stdcell= "";
		private String lotkind= "";
		private String pairslotno= "";
		private String pairipid= "";
		private String pairicid= "";
		private String pairlotid= "";
		private String pairglassid= "";
		private String cutrule= "";
		private String result= "";

       
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

		public String DIRECTION
		{
			get { return direction; }
			set { direction = value; }
		}

		public String GLASSSIZE
		{
			get { return glasssize; }
			set { glasssize = value; }
		}

		public String STDCELL
		{
			get { return stdcell; }
			set { stdcell = value; }
		}

		public String LOTKIND
		{
			get { return lotkind; }
			set { lotkind = value; }
		}

		public String PAIRSLOTNO
		{
			get { return pairslotno; }
			set { pairslotno = value; }
		}

		public String PAIRIPID
		{
			get { return pairipid; }
			set { pairipid = value; }
		}

		public String PAIRICID
		{
			get { return pairicid; }
			set { pairicid = value; }
		}

		public String PAIRLOTID
		{
			get { return pairlotid; }
			set { pairlotid = value; }
		}

		public String PAIRGLASSID
		{
			get { return pairglassid; }
			set { pairglassid = value; }
		}

		public String CUTRULE
		{
			get { return cutrule; }
			set { cutrule = value; }
		}

		public String RESULT
		{
			get { return result; }
			set { result = value; }
		}


        public S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT()
        {
        }

        public S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT(ListFormat rootFormat)
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
			this.direction = listFormat.Children[9].Value;
			this.glasssize = listFormat.Children[10].Value;
			this.stdcell = listFormat.Children[11].Value;
			this.lotkind = listFormat.Children[12].Value;
			this.pairslotno = listFormat.Children[13].Value;
			this.pairipid = listFormat.Children[14].Value;
			this.pairicid = listFormat.Children[15].Value;
			this.pairlotid = listFormat.Children[16].Value;
			this.pairglassid = listFormat.Children[17].Value;
			this.cutrule = listFormat.Children[18].Value;
			this.result = listFormat.Children[19].Value;

        }
    }
}
