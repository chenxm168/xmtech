using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_GLASSDATACOLLECTION_TYPE4
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String ipidname, String ipid, String opidname, String opid, String icidname, String icid, String ocidname, String ocid, String jobidname, String jobid, String host_slotnoname, String host_slotno, String processidname, String processid, String partidname, String partid, String stepidname, String stepid, String glasstypename, String glasstype, String lotidname, String lotid, String host_glassidname, String host_glassid, String ppidname, String ppid, String glassstatename, String gstate, String out_slotnoname, String out_slotno, String vcr_glassidname, String vcr_glassid, String glass_sizename, String glass_size, String rptid1, List<S6F13_GLASSDATACOLLECTION_TYPE4_LEVEL_COUNT> level_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 13;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			String[] sArray =  dataid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "DATAID", dataid);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "DATAID", dataid);
			sArray =  ceid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "CEID", ceid);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid.Split(' ');
			if (isNoPadding)
				listNode_2.add(Uint1Format.TYPE, sArray.Length, "RPTID", rptid);
			else
				listNode_2.add(Uint1Format.TYPE, 1, "RPTID", rptid);
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 17, "", "") as ListFormat;
			ListFormat listNode_4 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipidname).Length, "IPIDNAME", ipidname);
			else
				listNode_4.add(AsciiFormat.TYPE, 10, "IPIDNAME", ipidname);
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_4.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			ListFormat listNode_5 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opidname).Length, "OPIDNAME", opidname);
			else
				listNode_5.add(AsciiFormat.TYPE, 10, "OPIDNAME", opidname);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "OPID", opid);
			ListFormat listNode_6 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icidname).Length, "ICIDNAME", icidname);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "ICIDNAME", icidname);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "ICID", icid);
			ListFormat listNode_7 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocidname).Length, "OCIDNAME", ocidname);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "OCIDNAME", ocidname);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				listNode_7.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			ListFormat listNode_8 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobidname).Length, "JOBIDNAME", jobidname);
			else
				listNode_8.add(AsciiFormat.TYPE, 10, "JOBIDNAME", jobidname);
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				listNode_8.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			ListFormat listNode_9 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(host_slotnoname).Length, "HOST_SLOTNONAME", host_slotnoname);
			else
				listNode_9.add(AsciiFormat.TYPE, 10, "HOST_SLOTNONAME", host_slotnoname);
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(host_slotno).Length, "HOST_SLOTNO", host_slotno);
			else
				listNode_9.add(AsciiFormat.TYPE, 2, "HOST_SLOTNO", host_slotno);
			ListFormat listNode_10 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processidname).Length, "PROCESSIDNAME", processidname);
			else
				listNode_10.add(AsciiFormat.TYPE, 10, "PROCESSIDNAME", processidname);
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processid).Length, "PROCESSID", processid);
			else
				listNode_10.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			ListFormat listNode_11 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partidname).Length, "PARTIDNAME", partidname);
			else
				listNode_11.add(AsciiFormat.TYPE, 10, "PARTIDNAME", partidname);
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partid).Length, "PARTID", partid);
			else
				listNode_11.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			ListFormat listNode_12 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepidname).Length, "STEPIDNAME", stepidname);
			else
				listNode_12.add(AsciiFormat.TYPE, 10, "STEPIDNAME", stepidname);
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepid).Length, "STEPID", stepid);
			else
				listNode_12.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			ListFormat listNode_13 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstypename).Length, "GLASSTYPENAME", glasstypename);
			else
				listNode_13.add(AsciiFormat.TYPE, 10, "GLASSTYPENAME", glasstypename);
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				listNode_13.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			ListFormat listNode_14 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotidname).Length, "LOTIDNAME", lotidname);
			else
				listNode_14.add(AsciiFormat.TYPE, 10, "LOTIDNAME", lotidname);
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotid).Length, "LOTID", lotid);
			else
				listNode_14.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			ListFormat listNode_15 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(host_glassidname).Length, "HOST_GLASSIDNAME", host_glassidname);
			else
				listNode_15.add(AsciiFormat.TYPE, 10, "HOST_GLASSIDNAME", host_glassidname);
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(host_glassid).Length, "HOST_GLASSID", host_glassid);
			else
				listNode_15.add(AsciiFormat.TYPE, 20, "HOST_GLASSID", host_glassid);
			ListFormat listNode_16 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppidname).Length, "PPIDNAME", ppidname);
			else
				listNode_16.add(AsciiFormat.TYPE, 10, "PPIDNAME", ppidname);
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				listNode_16.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_17 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_17.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassstatename).Length, "GLASSSTATENAME", glassstatename);
			else
				listNode_17.add(AsciiFormat.TYPE, 10, "GLASSSTATENAME", glassstatename);
			if (isNoPadding)
				listNode_17.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(gstate).Length, "GSTATE", gstate);
			else
				listNode_17.add(AsciiFormat.TYPE, 1, "GSTATE", gstate);
			ListFormat listNode_18 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(out_slotnoname).Length, "OUT_SLOTNONAME", out_slotnoname);
			else
				listNode_18.add(AsciiFormat.TYPE, 10, "OUT_SLOTNONAME", out_slotnoname);
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(out_slotno).Length, "OUT_SLOTNO", out_slotno);
			else
				listNode_18.add(AsciiFormat.TYPE, 2, "OUT_SLOTNO", out_slotno);
			ListFormat listNode_19 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(vcr_glassidname).Length, "VCR_GLASSIDNAME", vcr_glassidname);
			else
				listNode_19.add(AsciiFormat.TYPE, 10, "VCR_GLASSIDNAME", vcr_glassidname);
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(vcr_glassid).Length, "VCR_GLASSID", vcr_glassid);
			else
				listNode_19.add(AsciiFormat.TYPE, 20, "VCR_GLASSID", vcr_glassid);
			ListFormat listNode_20 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glass_sizename).Length, "GLASS_SIZENAME", glass_sizename);
			else
				listNode_20.add(AsciiFormat.TYPE, 10, "GLASS_SIZENAME", glass_sizename);
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glass_size).Length, "GLASS_SIZE", glass_size);
			else
				listNode_20.add(AsciiFormat.TYPE, 2, "GLASS_SIZE", glass_size);
			ListFormat listNode_21 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid1.Split(' ');
			if (isNoPadding)
				listNode_21.add(Uint1Format.TYPE, sArray.Length, "RPTID1", rptid1);
			else
				listNode_21.add(Uint1Format.TYPE, 1, "RPTID1", rptid1);
			ListFormat listNode_LEVEL_COUNT = listNode_21.add(ListFormat.TYPE, -1, "LEVEL_COUNT", "") as ListFormat;
			if(level_count != null)
			{
				foreach (S6F13_GLASSDATACOLLECTION_TYPE4_LEVEL_COUNT item in level_count)
				{
					listNode_LEVEL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}