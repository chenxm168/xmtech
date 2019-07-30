using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_GLASSEVENT_TYPE1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String lotid, String ipid, String opid, String icid, String ocid, String jobid, String ppid, String processid, String partid, String stepid, String glasstype, String glassid, String fslotno, String tslotno, String eo_val, String unitid, String result, String judgement, String ngcode)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 11;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			String[] sArray =  dataid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "DATAID", dataid);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "DATAID", dataid);
			sArray =  ceid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint2Format.TYPE, sArray.Length, "CEID", ceid);
			else
				listNode_0.add(Uint2Format.TYPE, 1, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid.Split(' ');
			if (isNoPadding)
				listNode_2.add(Uint1Format.TYPE, sArray.Length, "RPTID", rptid);
			else
				listNode_2.add(Uint1Format.TYPE, 1, "RPTID", rptid);
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 4, "", "") as ListFormat;
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				listNode_3.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			sArray =  mcmd.Split(' ');
			if (isNoPadding)
				listNode_3.add(Uint1Format.TYPE, sArray.Length, "MCMD", mcmd);
			else
				listNode_3.add(Uint1Format.TYPE, 1, "MCMD", mcmd);
			sArray =  eqst.Split(' ');
			if (isNoPadding)
				listNode_3.add(Uint1Format.TYPE, sArray.Length, "EQST", eqst);
			else
				listNode_3.add(Uint1Format.TYPE, 1, "EQST", eqst);
			sArray =  bywho.Split(' ');
			if (isNoPadding)
				listNode_3.add(Uint1Format.TYPE, sArray.Length, "BYWHO", bywho);
			else
				listNode_3.add(Uint1Format.TYPE, 1, "BYWHO", bywho);
			ListFormat listNode_4 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid1.Split(' ');
			if (isNoPadding)
				listNode_4.add(Uint1Format.TYPE, sArray.Length, "RPTID1", rptid1);
			else
				listNode_4.add(Uint1Format.TYPE, 1, "RPTID1", rptid1);
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			ListFormat listNode_6 = listNode_5.add(ListFormat.TYPE, 14, "", "") as ListFormat;
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotid).Length, "LOTID", lotid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "OPID", opid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "ICID", icid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processid).Length, "PROCESSID", processid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partid).Length, "PARTID", partid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepid).Length, "STEPID", stepid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(fslotno).Length, "FSLOTNO", fslotno);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "FSLOTNO", fslotno);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(tslotno).Length, "TSLOTNO", tslotno);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "TSLOTNO", tslotno);
			ListFormat listNode_7 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  eo_val.Split(' ');
			if (isNoPadding)
				listNode_7.add(Uint1Format.TYPE, sArray.Length, "EO_VAL", eo_val);
			else
				listNode_7.add(Uint1Format.TYPE, 1, "EO_VAL", eo_val);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				listNode_7.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			ListFormat listNode_8 = listNode_5.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(result).Length, "RESULT", result);
			else
				listNode_8.add(AsciiFormat.TYPE, 6, "RESULT", result);
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(judgement).Length, "JUDGEMENT", judgement);
			else
				listNode_8.add(AsciiFormat.TYPE, 6, "JUDGEMENT", judgement);
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ngcode).Length, "NGCODE", ngcode);
			else
				listNode_8.add(AsciiFormat.TYPE, 6, "NGCODE", ngcode);

            return trx;

        }
    }


}