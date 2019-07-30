using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F102_SVSTATUSREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ack, String toolid, List<S1F102_SVSTATUSREPLY_SVID_COUNT> svid_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 102;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			String[] sArray =  ack.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "ACK", ack);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "ACK", ack);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				listNode_0.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_SVID_COUNT = listNode_0.add(ListFormat.TYPE, -1, "SVID_COUNT", "") as ListFormat;
			if(svid_count != null)
			{
				foreach (S1F102_SVSTATUSREPLY_SVID_COUNT item in svid_count)
				{
					listNode_SVID_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}