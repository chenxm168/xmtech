using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F11_MASKEVENT_TYPE2
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String dataid, String ceid, String rptid, String toolid, String mcmd, String eqst, String bywho, String rptid1, String toolid1, String ppc, String maskslot, String maskid, String maskstate, String maskkind)
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
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 1, "", "") as ListFormat;
			ListFormat listNode_6 = listNode_5.add(ListFormat.TYPE, 6, "", "") as ListFormat;
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid1).Length, "TOOLID1", toolid1);
			else
				listNode_6.add(AsciiFormat.TYPE, 9, "TOOLID1", toolid1);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppc).Length, "PPC", ppc);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "PPC", ppc);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskslot).Length, "MASKSLOT", maskslot);
			else
				listNode_6.add(AsciiFormat.TYPE, 2, "MASKSLOT", maskslot);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskid).Length, "MASKID", maskid);
			else
				listNode_6.add(AsciiFormat.TYPE, 20, "MASKID", maskid);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskstate).Length, "MASKSTATE", maskstate);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "MASKSTATE", maskstate);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(maskkind).Length, "MASKKIND", maskkind);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "MASKKIND", maskkind);

            return trx;

        }
    }


}