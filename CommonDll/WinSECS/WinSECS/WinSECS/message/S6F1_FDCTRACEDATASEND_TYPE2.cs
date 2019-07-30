using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F1_FDCTRACEDATASEND_TYPE2
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String trid, String smpln, String stime, String toolid, String stime1, List<S6F1_FDCTRACEDATASEND_TYPE2_SV_COUNT> sv_count)
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
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stime1).Length, "STIME1", stime1);
			else
				listNode_1.add(AsciiFormat.TYPE, 14, "STIME1", stime1);
			ListFormat listNode_SV_COUNT = listNode_1.add(ListFormat.TYPE, -1, "SV_COUNT", "") as ListFormat;
			if(sv_count != null)
			{
				foreach (S6F1_FDCTRACEDATASEND_TYPE2_SV_COUNT item in sv_count)
				{
					listNode_SV_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}