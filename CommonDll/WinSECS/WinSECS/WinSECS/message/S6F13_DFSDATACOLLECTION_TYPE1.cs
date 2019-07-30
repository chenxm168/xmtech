using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_DFSDATACOLLECTION_TYPE1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String ipidname, String ipid, String opidname, String opid, String icidname, String icid, String ocidname, String ocid, String jobidname, String jobid, String slotname, String slotno, String processidname, String processid, String partidname, String partid, String stepidname, String stepid, String glasstypename, String glasstype, String glassidname, String glassid, String ppidname, String ppid, String glassstatename, String gstate, String rptid1, String rawpathname, String rawpath, String sumpathname, String sumpath, String imgpathname, String imgpath, String diskname, String disk)
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
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 13, "", "") as ListFormat;
			ListFormat listNode_4 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ipidname.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "IPIDNAME", ipidname);
			else
				listNode_4.add(AsciiFormat.TYPE, 10, "IPIDNAME", ipidname);
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_4.add(AsciiFormat.TYPE, 20, "IPID", ipid);
			ListFormat listNode_5 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  opidname.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "OPIDNAME", opidname);
			else
				listNode_5.add(AsciiFormat.TYPE, 10, "OPIDNAME", opidname);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "OPID", opid);
			ListFormat listNode_6 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  icidname.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "ICIDNAME", icidname);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "ICIDNAME", icidname);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_6.add(AsciiFormat.TYPE, 16, "ICID", icid);
			ListFormat listNode_7 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ocidname.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "OCIDNAME", ocidname);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "OCIDNAME", ocidname);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				listNode_7.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			ListFormat listNode_8 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  jobidname.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "JOBIDNAME", jobidname);
			else
				listNode_8.add(AsciiFormat.TYPE, 10, "JOBIDNAME", jobidname);
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				listNode_8.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			ListFormat listNode_9 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  slotname.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "SLOTNAME", slotname);
			else
				listNode_9.add(AsciiFormat.TYPE, 10, "SLOTNAME", slotname);
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(slotno).Length, "SLOTNO", slotno);
			else
				listNode_9.add(AsciiFormat.TYPE, 20, "SLOTNO", slotno);
			ListFormat listNode_10 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  processidname.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "PROCESSIDNAME", processidname);
			else
				listNode_10.add(AsciiFormat.TYPE, 10, "PROCESSIDNAME", processidname);
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(processid).Length, "PROCESSID", processid);
			else
				listNode_10.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			ListFormat listNode_11 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  partidname.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "PARTIDNAME", partidname);
			else
				listNode_11.add(AsciiFormat.TYPE, 10, "PARTIDNAME", partidname);
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(partid).Length, "PARTID", partid);
			else
				listNode_11.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			ListFormat listNode_12 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  stepidname.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "STEPIDNAME", stepidname);
			else
				listNode_12.add(AsciiFormat.TYPE, 10, "STEPIDNAME", stepidname);
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stepid).Length, "STEPID", stepid);
			else
				listNode_12.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			ListFormat listNode_13 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glasstypename.Split(' ');
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, sArray.Length, "GLASSTYPENAME", glasstypename);
			else
				listNode_13.add(AsciiFormat.TYPE, 10, "GLASSTYPENAME", glasstypename);
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				listNode_13.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			ListFormat listNode_14 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glassidname.Split(' ');
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, sArray.Length, "GLASSIDNAME", glassidname);
			else
				listNode_14.add(AsciiFormat.TYPE, 10, "GLASSIDNAME", glassidname);
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glassid).Length, "GLASSID", glassid);
			else
				listNode_14.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			ListFormat listNode_15 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ppidname.Split(' ');
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, sArray.Length, "PPIDNAME", ppidname);
			else
				listNode_15.add(AsciiFormat.TYPE, 10, "PPIDNAME", ppidname);
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				listNode_15.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_16 = listNode_3.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  glassstatename.Split(' ');
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, sArray.Length, "GLASSSTATENAME", glassstatename);
			else
				listNode_16.add(AsciiFormat.TYPE, 10, "GLASSSTATENAME", glassstatename);
			sArray =  gstate.Split(' ');
			if (isNoPadding)
				listNode_16.add(Uint1Format.TYPE, sArray.Length, "GSTATE", gstate);
			else
				listNode_16.add(Uint1Format.TYPE, 1, "GSTATE", gstate);
			ListFormat listNode_17 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid1.Split(' ');
			if (isNoPadding)
				listNode_17.add(Uint1Format.TYPE, sArray.Length, "RPTID1", rptid1);
			else
				listNode_17.add(Uint1Format.TYPE, 1, "RPTID1", rptid1);
			ListFormat listNode_LEVEL_COUNT = listNode_17.add(ListFormat.TYPE, 1, "LEVEL_COUNT", "") as ListFormat;
			ListFormat listNode_18 = listNode_LEVEL_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			ListFormat listNode_FIRST_COUNT = listNode_18.add(ListFormat.TYPE, 4, "FIRST_COUNT", "") as ListFormat;
			ListFormat listNode_19 = listNode_FIRST_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rawpathname.Split(' ');
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, sArray.Length, "RAWPATHNAME", rawpathname);
			else
				listNode_19.add(AsciiFormat.TYPE, 16, "RAWPATHNAME", rawpathname);
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(rawpath).Length, "RAWPATH", rawpath);
			else
				listNode_19.add(AsciiFormat.TYPE, 80, "RAWPATH", rawpath);
			ListFormat listNode_20 = listNode_FIRST_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  sumpathname.Split(' ');
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, sArray.Length, "SUMPATHNAME", sumpathname);
			else
				listNode_20.add(AsciiFormat.TYPE, 16, "SUMPATHNAME", sumpathname);
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sumpath).Length, "SUMPATH", sumpath);
			else
				listNode_20.add(AsciiFormat.TYPE, 80, "SUMPATH", sumpath);
			ListFormat listNode_21 = listNode_FIRST_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  imgpathname.Split(' ');
			if (isNoPadding)
				listNode_21.add(AsciiFormat.TYPE, sArray.Length, "IMGPATHNAME", imgpathname);
			else
				listNode_21.add(AsciiFormat.TYPE, 16, "IMGPATHNAME", imgpathname);
			if (isNoPadding)
				listNode_21.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(imgpath).Length, "IMGPATH", imgpath);
			else
				listNode_21.add(AsciiFormat.TYPE, 80, "IMGPATH", imgpath);
			ListFormat listNode_22 = listNode_FIRST_COUNT.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  diskname.Split(' ');
			if (isNoPadding)
				listNode_22.add(AsciiFormat.TYPE, sArray.Length, "DISKNAME", diskname);
			else
				listNode_22.add(AsciiFormat.TYPE, 16, "DISKNAME", diskname);
			if (isNoPadding)
				listNode_22.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(disk).Length, "DISK", disk);
			else
				listNode_22.add(AsciiFormat.TYPE, 80, "DISK", disk);
			ListFormat listNode_23 = listNode_18.add(ListFormat.TYPE, 0, "", "") as ListFormat;

            return trx;

        }
    }


}