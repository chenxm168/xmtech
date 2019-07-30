using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_JOBPROCESSEVENT_GLASS_COUNT
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
		private String out_slotno= "";
		private String vcr_glassid= "";

        public S6F11_JOBPROCESSEVENT_GLASS_COUNT(String slotno, String processid, String partid, String stepid, String glasstype, String lotid, String glassid, String ppid, String cellgrade, String lotaction, String out_slotno, String vcr_glassid)
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
			this.out_slotno = out_slotno;
			this.vcr_glassid = vcr_glassid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 12;

			String[] sArray =  slotno.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			sArray =  processid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PROCESSID", processid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			sArray =  partid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PARTID", partid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			sArray =  stepid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "STEPID", stepid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			sArray =  glasstype.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "GLASSTYPE", glasstype);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			sArray =  lotid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "LOTID", lotid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			sArray =  glassid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "GLASSID", glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			sArray =  ppid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			sArray =  cellgrade.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "CELLGRADE", cellgrade);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "CELLGRADE", cellgrade);
			sArray =  lotaction.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "LOTACTION", lotaction);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTACTION", lotaction);
			sArray =  out_slotno.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "OUT_SLOTNO", out_slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "OUT_SLOTNO", out_slotno);
			sArray =  vcr_glassid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "VCR_GLASSID", vcr_glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "VCR_GLASSID", vcr_glassid);

            return ownerList;
        }

    }
}