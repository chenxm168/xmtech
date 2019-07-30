using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_TESTEVENT
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String item_0, String ipid, String item_1, String opid, String item_2, String icid, String item_3, String ocid, String item_4, String jobid, String item_5, String slotno, String item_6, String processid, String item_7, String partid, String item_8, String stepid, String item_9, String glasstype, String item_10, String lotid, String item_11, String glassid, String item_12, String ppid, String item_13, String reason, String item_14, String code, String item_15, String operid)
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
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 16, "", "") as ListFormat;
			ListFormat listNode_6 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_0.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "", item_0);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "", item_0);
			sArray =  ipid.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "IPID", ipid);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			ListFormat listNode_7 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_1.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "", item_1);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "", item_1);
			sArray =  opid.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "OPID", opid);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "OPID", opid);
			ListFormat listNode_8 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_2.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "", item_2);
			else
				listNode_8.add(AsciiFormat.TYPE, 10, "", item_2);
			sArray =  icid.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "ICID", icid);
			else
				listNode_8.add(AsciiFormat.TYPE, 16, "ICID", icid);
			ListFormat listNode_9 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_3.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "", item_3);
			else
				listNode_9.add(AsciiFormat.TYPE, 10, "", item_3);
			sArray =  ocid.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "OCID", ocid);
			else
				listNode_9.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			ListFormat listNode_10 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_4.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "", item_4);
			else
				listNode_10.add(AsciiFormat.TYPE, 10, "", item_4);
			sArray =  jobid.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "JOBID", jobid);
			else
				listNode_10.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			ListFormat listNode_11 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_5.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "", item_5);
			else
				listNode_11.add(AsciiFormat.TYPE, 10, "", item_5);
			sArray =  slotno.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "SLOTNO", slotno);
			else
				listNode_11.add(AsciiFormat.TYPE, 2, "SLOTNO", slotno);
			ListFormat listNode_12 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_6.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "", item_6);
			else
				listNode_12.add(AsciiFormat.TYPE, 10, "", item_6);
			sArray =  processid.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "PROCESSID", processid);
			else
				listNode_12.add(AsciiFormat.TYPE, 20, "PROCESSID", processid);
			ListFormat listNode_13 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_7.Split(' ');
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, sArray.Length, "", item_7);
			else
				listNode_13.add(AsciiFormat.TYPE, 10, "", item_7);
			sArray =  partid.Split(' ');
			if (isNoPadding)
				listNode_13.add(AsciiFormat.TYPE, sArray.Length, "PARTID", partid);
			else
				listNode_13.add(AsciiFormat.TYPE, 20, "PARTID", partid);
			ListFormat listNode_14 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_8.Split(' ');
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, sArray.Length, "", item_8);
			else
				listNode_14.add(AsciiFormat.TYPE, 10, "", item_8);
			sArray =  stepid.Split(' ');
			if (isNoPadding)
				listNode_14.add(AsciiFormat.TYPE, sArray.Length, "STEPID", stepid);
			else
				listNode_14.add(AsciiFormat.TYPE, 20, "STEPID", stepid);
			ListFormat listNode_15 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_9.Split(' ');
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, sArray.Length, "", item_9);
			else
				listNode_15.add(AsciiFormat.TYPE, 10, "", item_9);
			sArray =  glasstype.Split(' ');
			if (isNoPadding)
				listNode_15.add(AsciiFormat.TYPE, sArray.Length, "GLASSTYPE", glasstype);
			else
				listNode_15.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			ListFormat listNode_16 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_10.Split(' ');
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, sArray.Length, "", item_10);
			else
				listNode_16.add(AsciiFormat.TYPE, 10, "", item_10);
			sArray =  lotid.Split(' ');
			if (isNoPadding)
				listNode_16.add(AsciiFormat.TYPE, sArray.Length, "LOTID", lotid);
			else
				listNode_16.add(AsciiFormat.TYPE, 16, "LOTID", lotid);
			ListFormat listNode_17 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_11.Split(' ');
			if (isNoPadding)
				listNode_17.add(AsciiFormat.TYPE, sArray.Length, "", item_11);
			else
				listNode_17.add(AsciiFormat.TYPE, 10, "", item_11);
			sArray =  glassid.Split(' ');
			if (isNoPadding)
				listNode_17.add(AsciiFormat.TYPE, sArray.Length, "GLASSID", glassid);
			else
				listNode_17.add(AsciiFormat.TYPE, 20, "GLASSID", glassid);
			ListFormat listNode_18 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_12.Split(' ');
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, sArray.Length, "", item_12);
			else
				listNode_18.add(AsciiFormat.TYPE, 10, "", item_12);
			sArray =  ppid.Split(' ');
			if (isNoPadding)
				listNode_18.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				listNode_18.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			ListFormat listNode_19 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_13.Split(' ');
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, sArray.Length, "", item_13);
			else
				listNode_19.add(AsciiFormat.TYPE, 10, "", item_13);
			sArray =  reason.Split(' ');
			if (isNoPadding)
				listNode_19.add(AsciiFormat.TYPE, sArray.Length, "REASON", reason);
			else
				listNode_19.add(AsciiFormat.TYPE, 80, "REASON", reason);
			ListFormat listNode_20 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_14.Split(' ');
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, sArray.Length, "", item_14);
			else
				listNode_20.add(AsciiFormat.TYPE, 10, "", item_14);
			sArray =  code.Split(' ');
			if (isNoPadding)
				listNode_20.add(AsciiFormat.TYPE, sArray.Length, "CODE", code);
			else
				listNode_20.add(AsciiFormat.TYPE, 20, "CODE", code);
			ListFormat listNode_21 = listNode_5.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  item_15.Split(' ');
			if (isNoPadding)
				listNode_21.add(AsciiFormat.TYPE, sArray.Length, "", item_15);
			else
				listNode_21.add(AsciiFormat.TYPE, 10, "", item_15);
			sArray =  operid.Split(' ');
			if (isNoPadding)
				listNode_21.add(AsciiFormat.TYPE, sArray.Length, "OPERID", operid);
			else
				listNode_21.add(AsciiFormat.TYPE, 2, "OPERID", operid);

            return trx;

        }
    }


}