using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_JOBDATACOLLECTION
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String ipidname, String ipid, String opidname, String opid, String icidname, String icid, String ocidname, String ocid, String jobidname, String jobid, String host_slotnoname, String host_slotno, String processidname, String processid, String partidname, String partid, String stepidname, String stepid, String glasstypename, String glasstype, String lotidname, String lotid, String host_glassidname, String host_glassid, String ppidname, String ppid, String glassstatename, String gstate, String out_slotnoname, String out_slotno, String vcr_glassidname, String vcr_glassid, String glass_sizename, String glass_size, String rptid1, List<S6F13_JOBDATACOLLECTION_LEVEL_COUNT> level_count)
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
			sArray =  ipidname.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "IPIDNAME", ipidname);
			else
				listNode_4.add(AsciiFormat.TYPE, 10, "IPIDNAME", ipidname);
			sArray =  ipid.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "IPID", ipid);
			else
				listNode_4.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			ListFormat listNode_5 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  opidname.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "OPIDNAME", opidname);
			else
				listNode_5.add(AsciiFormat.TYPE, 10, "OPIDNAME", opidname);
			sArray =  opid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "OPID", opid);
			ListFormat listNode_6 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  icidname.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "ICIDNAME", icidname);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "ICIDNAME", icidname);
			sArray =  icid.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "ICID", icid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "ICID", icid);
			ListFormat listNode_7 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ocidname.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "OCIDNAME", ocidname);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "OCIDNAME", ocidname);
			sArray =  ocid.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "OCID", ocid);
			else
				listNode_7.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			ListFormat listNode_8 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  jobidname.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "JOBIDNAME", jobidname);
			else
				listNode_8.add(AsciiFormat.TYPE, 10, "JOBIDNAME", jobidname);
			sArray =  jobid.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "JOBID", jobid);
			else
				listNode_8.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			ListFormat listNode_9 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  host_slotnoname.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "HOST_SLOTNONAME", host_slotnoname);
			else
				listNode_9.add(AsciiFormat.TYPE, 10, "HOST_SLOTNONAME", host_slotnoname);
			sArray =  host_slotno.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "HOST_SLOTNO", host_slotno);
			else
				listNode_9.add(AsciiFormat.TYPE, 2, "HOST_SLOTNO", host_slotno);
			ListFormat listNode_10 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  processidname.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "PROCESSIDNAME", processidname);
			else
				listNode_10.add(AsciiFormat.TYPE, 10, "PROCESSIDNAME", processidname);
			sArray =  processid.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "PROCESSID", processid);
			else
				listNode_10.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			ListFormat listNode_11 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  partidname.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "PARTIDNAME", partidname);
			else
				listNode_11.add(AsciiFormat.TYPE, 10, "PARTIDNAME", partidname);
			sArray =  partid.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "PARTID", partid);
			else
				listNode_11.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			ListFormat listNode_12 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  stepidname.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "STEPIDNAME", stepidname);
			else
				listNode_12.add(AsciiFormat.TYPE, 10, "STEPIDNAME", stepidname);
			sArray =  stepid.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "STEPID", stepid);
			else
				listNode_12.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			ListFormat listNode_13 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glasstypename.Split(' ');
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, sArray.Length, "GLASSTYPENAME", glasstypename);
			else
				listNode_13.add(AsciiFormat.TYPE, 10, "GLASSTYPENAME", glasstypename);
			sArray =  glasstype.Split(' ');
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, sArray.Length, "GLASSTYPE", glasstype);
			else
				listNode_13.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			ListFormat listNode_14 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  lotidname.Split(' ');
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, sArray.Length, "LOTIDNAME", lotidname);
			else
				listNode_14.add(AsciiFormat.TYPE, 10, "LOTIDNAME", lotidname);
			sArray =  lotid.Split(' ');
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, sArray.Length, "LOTID", lotid);
			else
				listNode_14.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			ListFormat listNode_15 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  host_glassidname.Split(' ');
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, sArray.Length, "HOST_GLASSIDNAME", host_glassidname);
			else
				listNode_15.add(AsciiFormat.TYPE, 10, "HOST_GLASSIDNAME", host_glassidname);
			sArray =  host_glassid.Split(' ');
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, sArray.Length, "HOST_GLASSID", host_glassid);
			else
				listNode_15.add(AsciiFormat.TYPE, 20, "HOST_GLASSID", host_glassid);
			ListFormat listNode_16 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ppidname.Split(' ');
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, sArray.Length, "PPIDNAME", ppidname);
			else
				listNode_16.add(AsciiFormat.TYPE, 10, "PPIDNAME", ppidname);
			sArray =  ppid.Split(' ');
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				listNode_16.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_17 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glassstatename.Split(' ');
			if (isNoPadding)
				listNode_17.add(AsciiFormat.TYPE, sArray.Length, "GLASSSTATENAME", glassstatename);
			else
				listNode_17.add(AsciiFormat.TYPE, 10, "GLASSSTATENAME", glassstatename);
			sArray =  gstate.Split(' ');
			if (isNoPadding)
				listNode_17.add(Uint1Format.TYPE, sArray.Length, "GSTATE", gstate);
			else
				listNode_17.add(Uint1Format.TYPE, 1, "GSTATE", gstate);
			ListFormat listNode_18 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  out_slotnoname.Split(' ');
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, sArray.Length, "OUT_SLOTNONAME", out_slotnoname);
			else
				listNode_18.add(AsciiFormat.TYPE, 10, "OUT_SLOTNONAME", out_slotnoname);
			sArray =  out_slotno.Split(' ');
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, sArray.Length, "OUT_SLOTNO", out_slotno);
			else
				listNode_18.add(AsciiFormat.TYPE, 2, "OUT_SLOTNO", out_slotno);
			ListFormat listNode_19 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  vcr_glassidname.Split(' ');
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, sArray.Length, "VCR_GLASSIDNAME", vcr_glassidname);
			else
				listNode_19.add(AsciiFormat.TYPE, 10, "VCR_GLASSIDNAME", vcr_glassidname);
			sArray =  vcr_glassid.Split(' ');
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, sArray.Length, "VCR_GLASSID", vcr_glassid);
			else
				listNode_19.add(AsciiFormat.TYPE, 20, "VCR_GLASSID", vcr_glassid);
			ListFormat listNode_20 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glass_sizename.Split(' ');
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, sArray.Length, "GLASS_SIZENAME", glass_sizename);
			else
				listNode_20.add(AsciiFormat.TYPE, 10, "GLASS_SIZENAME", glass_sizename);
			sArray =  glass_size.Split(' ');
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, sArray.Length, "GLASS_SIZE", glass_size);
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
				foreach (S6F13_JOBDATACOLLECTION_LEVEL_COUNT item in level_count)
				{
					listNode_LEVEL_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}