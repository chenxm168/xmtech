using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F102_CASSETTEINFORMATIONREPLY_TYPE4_GLASS_COUNT
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

        public S3F102_CASSETTEINFORMATIONREPLY_TYPE4_GLASS_COUNT(String slotno, String processid, String partid, String stepid, String glasstype, String lotid, String glassid, String ppid, String cellgrade, String lotaction)
        {
			this.slotno = slotno;
			this.processid = processid;
			this.partid = partid;
			this.stepid = stepid;
			this.glasstype = glasstype;
			this.lotid = lotid;
			this.glassid = glassid;
			this.ppid = ppid;
			this.cellgrade = cellgrade;
			this.lotaction = lotaction;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 10;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processid).Length, "PROCESSID", processid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partid).Length, "PARTID", partid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepid).Length, "STEPID", stepid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotid).Length, "LOTID", lotid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(cellgrade).Length, "CELLGRADE", cellgrade);
			else
				ownerList.add(AsciiFormat.TYPE, 128, "CELLGRADE", cellgrade);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotaction).Length, "LOTACTION", lotaction);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTACTION", lotaction);

            return ownerList;
        }

    }
}