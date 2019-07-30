using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_GLASSMEVENT
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String ipid, String opid, String icid, String ocid, String isif, String osif, String unloadmode, List<S6F11_GLASSMEVENT_GLASS_COUNT> glass_count)
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
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 8, "", "") as ListFormat;
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid).Length, "IPID", ipid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "IPID", ipid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid).Length, "OPID", opid);
			else
				listNode_5.add(AsciiFormat.TYPE, 2, "OPID", opid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid).Length, "ICID", icid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "ICID", icid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid).Length, "OCID", ocid);
			else
				listNode_5.add(AsciiFormat.TYPE, 16, "OCID", ocid);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(isif).Length, "ISIF", isif);
			else
				listNode_5.add(AsciiFormat.TYPE, 105, "ISIF", isif);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(osif).Length, "OSIF", osif);
			else
				listNode_5.add(AsciiFormat.TYPE, 105, "OSIF", osif);
			sArray =  unloadmode.Split(' ');
			if (isNoPadding)
				listNode_5.add(Uint1Format.TYPE, sArray.Length, "UNLOADMODE", unloadmode);
			else
				listNode_5.add(Uint1Format.TYPE, 1, "UNLOADMODE", unloadmode);
			ListFormat listNode_GLASS_COUNT = listNode_5.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S6F11_GLASSMEVENT_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}