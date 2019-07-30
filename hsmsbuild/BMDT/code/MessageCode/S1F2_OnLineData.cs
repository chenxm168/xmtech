using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S1F2_OnLineData
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String mdln, String softrev)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(1, false);
            trx.Function = 2;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  mdln.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "MDLN", mdln);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "MDLN", mdln);
			sArray =  softrev.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "SOFTREV", softrev);
			else
				listNode_0.add(AsciiFormat.TYPE, 6, "SOFTREV", softrev);

            return trx;

        }
    }


}