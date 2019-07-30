using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_JOBDATACOLLECTION_TYPE3
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String ipidname, String ipid, String opidname, String opid, String icidname, String icid, String ocidname, String ocid, String jobidname, String jobid, String processidname, String processid, String partidname, String partid, String stepidname, String stepid, String tot_gstatename, String tot_gstate, String rptid1, List<S6F13_JOBDATACOLLECTION_TYPE3_LEVEL_COUNT> level_count)
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
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 9, "", "") as ListFormat;
			ListFormat listNode_4 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipidname).Length, "IPIDNAME", ipidname);
			else
				listNode_4.add(AsciiFormat.TYPE, 10, "IPIDNAME", ipidname);
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_4.add(AsciiFormat.TYPE, 20, "IPID", ipid);
			ListFormat listNode_5 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opidname).Length, "OPIDNAME", opidname);
			else
				listNode_5.add(AsciiFormat.TYPE, 10, "OPIDNAME", opidname);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "OPID", opid);
			ListFormat listNode_6 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icidname).Length, "ICIDNAME", icidname);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "ICIDNAME", icidname);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "ICID", icid);
			ListFormat listNode_7 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocidname).Length, "OCIDNAME", ocidname);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "OCIDNAME", ocidname);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				listNode_7.add(AsciiFormat.TYPE, 20, "OCID", ocid);
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
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processidname).Length, "PROCESSIDNAME", processidname);
			else
				listNode_9.add(AsciiFormat.TYPE, 10, "PROCESSIDNAME", processidname);
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processid).Length, "PROCESSID", processid);
			else
				listNode_9.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			ListFormat listNode_10 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partidname).Length, "PARTIDNAME", partidname);
			else
				listNode_10.add(AsciiFormat.TYPE, 10, "PARTIDNAME", partidname);
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partid).Length, "PARTID", partid);
			else
				listNode_10.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			ListFormat listNode_11 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepidname).Length, "STEPIDNAME", stepidname);
			else
				listNode_11.add(AsciiFormat.TYPE, 10, "STEPIDNAME", stepidname);
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepid).Length, "STEPID", stepid);
			else
				listNode_11.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			ListFormat listNode_12 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(tot_gstatename).Length, "TOT_GSTATENAME", tot_gstatename);
			else
				listNode_12.add(AsciiFormat.TYPE, 10, "TOT_GSTATENAME", tot_gstatename);
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(tot_gstate).Length, "TOT_GSTATE", tot_gstate);
			else
				listNode_12.add(AsciiFormat.TYPE, 20, "TOT_GSTATE", tot_gstate);
			ListFormat listNode_13 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid1.Split(' ');
			if (isNoPadding)
				listNode_13.add(Uint1Format.TYPE, sArray.Length, "RPTID1", rptid1);
			else
				listNode_13.add(Uint1Format.TYPE, 1, "RPTID1", rptid1);
			ListFormat listNode_LEVEL_COUNT = listNode_13.add(ListFormat.TYPE, -1, "LEVEL_COUNT", "") as ListFormat;
			if(level_count != null)
			{
				foreach (S6F13_JOBDATACOLLECTION_TYPE3_LEVEL_COUNT item in level_count)
				{
					listNode_LEVEL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}