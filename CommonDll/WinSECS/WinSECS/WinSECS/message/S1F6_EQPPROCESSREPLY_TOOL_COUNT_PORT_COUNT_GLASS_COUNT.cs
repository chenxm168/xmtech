using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String slotno= "";
		private String ipid= "";
		private String icid= "";
		private String processid= "";
		private String partid= "";
		private String stepid= "";
		private String glasstype= "";
		private String lotid= "";
		private String hglassid= "";
		private String rglassid= "";
		private String ppid= "";
		private String cellgrade= "";
		private String direction= "";
		private String glasssize= "";
		private String stdcell= "";
		private String lotkind= "";
		private String runline= "";
		private String result= "";
		private String judgement= "";
		private String ngcode= "";
		private String position= "";
		private String pairslotno= "";
		private String pairipid= "";
		private String pairicid= "";
		private String pairlotid= "";
		private String pairhglassid= "";
		private String pairrglassid= "";
		private String cutrule= "";

        public S1F6_EQPPROCESSREPLY_TOOL_COUNT_PORT_COUNT_GLASS_COUNT(String slotno, String ipid, String icid, String processid, String partid, String stepid, String glasstype, String lotid, String hglassid, String rglassid, String ppid, String cellgrade, String direction, String glasssize, String stdcell, String lotkind, String runline, String result, String judgement, String ngcode, String position, String pairslotno, String pairipid, String pairicid, String pairlotid, String pairhglassid, String pairrglassid, String cutrule)
        {
			this.slotno = slotno;
			this.ipid = ipid;
			this.icid = icid;
			this.processid = processid;
			this.partid = partid;
			this.stepid = stepid;
			this.glasstype = glasstype;
			this.lotid = lotid;
			this.hglassid = hglassid;
			this.rglassid = rglassid;
			this.ppid = ppid;
			this.cellgrade = cellgrade;
			this.direction = direction;
			this.glasssize = glasssize;
			this.stdcell = stdcell;
			this.lotkind = lotkind;
			this.runline = runline;
			this.result = result;
			this.judgement = judgement;
			this.ngcode = ngcode;
			this.position = position;
			this.pairslotno = pairslotno;
			this.pairipid = pairipid;
			this.pairicid = pairicid;
			this.pairlotid = pairlotid;
			this.pairhglassid = pairhglassid;
			this.pairrglassid = pairrglassid;
			this.cutrule = cutrule;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 28;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "ICID", icid);
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
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(hglassid).Length, "HGLASSID", hglassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "HGLASSID", hglassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(rglassid).Length, "RGLASSID", rglassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "RGLASSID", rglassid);
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
			String[] sArray =  runline.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "RUNLINE", runline);
			else
				ownerList.add(Uint1Format.TYPE, 40, "RUNLINE", runline);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(result).Length, "RESULT", result);
			else
				ownerList.add(AsciiFormat.TYPE, 6, "RESULT", result);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(judgement).Length, "JUDGEMENT", judgement);
			else
				ownerList.add(AsciiFormat.TYPE, 6, "JUDGEMENT", judgement);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ngcode).Length, "NGCODE", ngcode);
			else
				ownerList.add(AsciiFormat.TYPE, 6, "NGCODE", ngcode);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(position).Length, "POSITION", position);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "POSITION", position);
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
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairhglassid).Length, "PAIRHGLASSID", pairhglassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PAIRHGLASSID", pairhglassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(pairrglassid).Length, "PAIRRGLASSID", pairrglassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PAIRRGLASSID", pairrglassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(cutrule).Length, "CUTRULE", cutrule);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "CUTRULE", cutrule);

            return ownerList;
        }

    }
}