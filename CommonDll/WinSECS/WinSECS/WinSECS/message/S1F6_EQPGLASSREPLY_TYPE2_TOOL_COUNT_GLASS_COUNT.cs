using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPGLASSREPLY_TYPE2_TOOL_COUNT_GLASS_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String lotid= "";
		private String ipid= "";
		private String opid= "";
		private String icid= "";
		private String ocid= "";
		private String jobid= "";
		private String processid= "";
		private String partid= "";
		private String stepid= "";
		private String ppid= "";
		private String glasstype= "";
		private String glassid= "";
		private String slotno= "";
		private String glass_state= "";
		private String unitid= "";
		private String out_slotno= "";
		private String vcr_glassid= "";

        public S1F6_EQPGLASSREPLY_TYPE2_TOOL_COUNT_GLASS_COUNT(String lotid, String ipid, String opid, String icid, String ocid, String jobid, String processid, String partid, String stepid, String ppid, String glasstype, String glassid, String slotno, String glass_state, String unitid, String out_slotno, String vcr_glassid)
        {
			this.lotid = lotid;
			this.ipid = ipid;
			this.opid = opid;
			this.icid = icid;
			this.ocid = ocid;
			this.jobid = jobid;
			this.processid = processid;
			this.partid = partid;
			this.stepid = stepid;
			this.ppid = ppid;
			this.glasstype = glasstype;
			this.glassid = glassid;
			this.slotno = slotno;
			this.glass_state = glass_state;
			this.unitid = unitid;
			this.out_slotno = out_slotno;
			this.vcr_glassid = vcr_glassid;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 17;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotid).Length, "LOTID", lotid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "OPID", opid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "ICID", icid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				ownerList.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
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
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			String[] sArray =  glass_state.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "GLASS_STATE", glass_state);
			else
				ownerList.add(Uint1Format.TYPE, 1, "GLASS_STATE", glass_state);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(out_slotno).Length, "OUT_SLOTNO", out_slotno);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "OUT_SLOTNO", out_slotno);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(vcr_glassid).Length, "VCR_GLASSID", vcr_glassid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "VCR_GLASSID", vcr_glassid);

            return ownerList;
        }

    }
}