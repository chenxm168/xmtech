using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class ConversationTimeout
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String mexp, String edid)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(9, false);
            trx.Function = 13;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(mexp).Length, "MEXP", mexp);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "MEXP", mexp);
			String[] sArray =  edid.Split(' ');
			if (isNoPadding)
				listNode_0.add(XFormat.TYPE, sArray.Length, "EDID", edid);
			else
				listNode_0.add(XFormat.TYPE, 64, "EDID", edid);

            return trx;

        }
    }


}