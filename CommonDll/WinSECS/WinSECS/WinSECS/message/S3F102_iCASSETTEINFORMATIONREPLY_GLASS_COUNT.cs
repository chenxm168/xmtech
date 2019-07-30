using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F102_iCASSETTEINFORMATIONREPLY_GLASS_COUNT
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

        public S3F102_iCASSETTEINFORMATIONREPLY_GLASS_COUNT(String slotno, String processid, String partid, String stepid, String glasstype, String lotid, String glassid, String ppid, String cellgrade, String direction, String glasssize, String stdcell, String lotkind, String pairslotno, String pairipid, String pairicid, String pairlotid, String pairglassid, String cutrule, String result)
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
			this.direction = direction;
			this.glasssize = glasssize;
			this.stdcell = stdcell;
			this.lotkind = lotkind;
			this.pairslotno = pairslotno;
			this.pairipid = pairipid;
			this.pairicid = pairicid;
			this.pairlotid = pairlotid;
			this.pairglassid = pairglassid;
			this.cutrule = cutrule;
			this.result = result;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 20;

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
				ownerList.add(AsciiFormat.TYPE, 20, "CELLGRADE", cellgrade);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(direction).Length, "DIRECTION", direction);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "DIRECTION", direction);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasssize).Length, "GLASSSIZE", glasssize);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "GLASSSIZE", glasssize);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stdcell).Length, "STDCELL", stdcell);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "STDCELL", stdcell);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotkind).Length, "LOTKIND", lotkind);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTKIND", lotkind);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairslotno).Length, "PAIRSLOTNO", pairslotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "PAIRSLOTNO", pairslotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairipid).Length, "PAIRIPID", pairipid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "PAIRIPID", pairipid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairicid).Length, "PAIRICID", pairicid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "PAIRICID", pairicid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairlotid).Length, "PAIRLOTID", pairlotid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "PAIRLOTID", pairlotid);
			String[] sArray =  pairglassid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PAIRGLASSID", pairglassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PAIRGLASSID", pairglassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(cutrule).Length, "CUTRULE", cutrule);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "CUTRULE", cutrule);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(result).Length, "RESULT", result);
			else
				ownerList.add(AsciiFormat.TYPE, 6, "RESULT", result);

            return ownerList;
        }

    }
}