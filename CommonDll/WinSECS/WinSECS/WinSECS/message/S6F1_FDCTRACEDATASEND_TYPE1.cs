using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F1_FDCTRACEDATASEND_TYPE1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String trid, String smpln, String stime, String toolid, List<String> sv_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 1;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 5, "", "") as ListFormat;
			String[] sArray =  trid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint2Format.TYPE, sArray.Length, "TRID", trid);
			else
				listNode_0.add(Uint2Format.TYPE, 1, "TRID", trid);
			sArray =  smpln.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint4Format.TYPE, sArray.Length, "SMPLN", smpln);
			else
				listNode_0.add(Uint4Format.TYPE, 1, "SMPLN", smpln);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stime).Length, "STIME", stime);
			else
				listNode_0.add(AsciiFormat.TYPE, 14, "STIME", stime);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				listNode_0.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			listNode_0.add(CPrivateUtility.getMessage(isNoPadding, sv_count, AsciiFormat.TYPE, "SV", -1));

            return trx;

        }
    }


}