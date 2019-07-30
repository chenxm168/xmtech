using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_JOBPROCESSEVENT
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String ipid, String opid, String icid, String ocid, String jobid, String totalgstate, List<S6F11_JOBPROCESSEVENT_GLASS_COUNT> glass_count, String rptid2, String utype, String unloadtype, String splitmode, String porttype)
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
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid.Split(' ');
			if (isNoPadding)
				listNode_2.add(Uint1Format.TYPE, sArray.Length, "RPTID", rptid);
			else
				listNode_2.add(Uint1Format.TYPE, 1, "RPTID", rptid);
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 4, "", "") as ListFormat;
			sArray =  toolid.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "TOOLID", toolid);
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
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 7, "", "") as ListFormat;
			sArray =  ipid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "IPID", ipid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			sArray =  opid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "OPID", opid);
			sArray =  icid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "ICID", icid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "ICID", icid);
			sArray =  ocid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "OCID", ocid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			sArray =  jobid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "JOBID", jobid);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			sArray =  totalgstate.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "TOTALGSTATE", totalgstate);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "TOTALGSTATE", totalgstate);
			ListFormat listNode_GLASS_COUNT = listNode_5.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S6F11_JOBPROCESSEVENT_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}
			ListFormat listNode_6 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid2.Split(' ');
			if (isNoPadding)
				listNode_6.add(Uint1Format.TYPE, sArray.Length, "RPTID2", rptid2);
			else
				listNode_6.add(Uint1Format.TYPE, 1, "RPTID2", rptid2);
			ListFormat listNode_7 = listNode_6.add(ListFormat.TYPE, 4, "", "") as ListFormat;
			sArray =  utype.Split(' ');
			if (isNoPadding)
				listNode_7.add(Uint1Format.TYPE, sArray.Length, "UTYPE", utype);
			else
				listNode_7.add(Uint1Format.TYPE, 1, "UTYPE", utype);
			sArray =  unloadtype.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "UNLOADTYPE", unloadtype);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "UNLOADTYPE", unloadtype);
			sArray =  splitmode.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "SPLITMODE", splitmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 4, "SPLITMODE", splitmode);
			sArray =  porttype.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "PORTTYPE", porttype);
			else
				listNode_7.add(AsciiFormat.TYPE, 6, "PORTTYPE", porttype);

            return trx;

        }
    }


}