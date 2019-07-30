using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S6F113_CurrentEQPDataReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String unitid, List<S6F113_CurrentEQPDataReport_> item_0)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 113;

			String[] sArray =  unitid.Split(' ');
			if (isNoPadding)
				trx.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				trx.add(AsciiFormat.TYPE, 20, "UNITID", unitid);
			ListFormat listNode_0 = trx.add(ListFormat.TYPE, -1, "", "") as ListFormat;
			if(item_0 != null)
			{
				foreach (S6F113_CurrentEQPDataReport_ item in item_0)
				{
					listNode_0.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}