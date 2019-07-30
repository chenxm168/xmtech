using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_iPROCESSEVENT_TYPE2
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String ptid, String csid, String ipid, String icid, String jobidp, String totalgstate, List<S6F11_iPROCESSEVENT_TYPE2_GLASS_COUNT> glass_count, String rptid2, String receivemode, String unloadtype, String splitmode, String ptst, String ptmd, String bcrmode, String vcrmode, String sortmode, String linemode, String inspectionmode)
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
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ceid).Length, "CEID", ceid);
			else
				listNode_0.add(AsciiFormat.TYPE, 4, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 3, "", "") as ListFormat;
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
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 7, "", "") as ListFormat;
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptid).Length, "PTID", ptid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "PTID", ptid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(csid).Length, "CSID", csid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "CSID", csid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "ICID", icid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobidp).Length, "JOBIDp", jobidp);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "JOBIDp", jobidp);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(totalgstate).Length, "TOTALGSTATE", totalgstate);
			else
				listNode_5.add(AsciiFormat.TYPE, 105, "TOTALGSTATE", totalgstate);
			ListFormat listNode_GLASS_COUNT = listNode_5.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S6F11_iPROCESSEVENT_TYPE2_GLASS_COUNT item in glass_count)
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
			ListFormat listNode_7 = listNode_6.add(ListFormat.TYPE, 10, "", "") as ListFormat;
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(receivemode).Length, "RECEIVEMODE", receivemode);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "RECEIVEMODE", receivemode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unloadtype).Length, "UNLOADTYPE", unloadtype);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "UNLOADTYPE", unloadtype);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(splitmode).Length, "SPLITMODE", splitmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 4, "SPLITMODE", splitmode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptst).Length, "PTST", ptst);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "PTST", ptst);
			sArray =  ptmd.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "PTMD", ptmd);
			else
				listNode_7.add(AsciiFormat.TYPE, 3, "PTMD", ptmd);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(bcrmode).Length, "BCRMODE", bcrmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "BCRMODE", bcrmode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(vcrmode).Length, "VCRMODE", vcrmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 2, "VCRMODE", vcrmode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sortmode).Length, "SORTMODE", sortmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 4, "SORTMODE", sortmode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(linemode).Length, "LINEMODE", linemode);
			else
				listNode_7.add(AsciiFormat.TYPE, 4, "LINEMODE", linemode);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(inspectionmode).Length, "INSPECTIONMODE", inspectionmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 4, "INSPECTIONMODE", inspectionmode);

            return trx;

        }
    }


}