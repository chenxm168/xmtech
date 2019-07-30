using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_UNITREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String hcack, List<S2F42_UNITREPLY_UNIT_COUNT> unit_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(2, false);
            trx.Function = 42;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  hcack.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "HCACK", hcack);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "HCACK", hcack);
			ListFormat listNode_UNIT_COUNT = listNode_0.add(ListFormat.TYPE, -1, "UNIT_COUNT", "") as ListFormat;
			if(unit_count != null)
			{
				foreach (S2F42_UNITREPLY_UNIT_COUNT item in unit_count)
				{
					listNode_UNIT_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}