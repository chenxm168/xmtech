using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_PORTREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String hcack, List<S2F42_PORTREPLY_PORT_COUNT> port_count)
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
			ListFormat listNode_PORT_COUNT = listNode_0.add(ListFormat.TYPE, -1, "PORT_COUNT", "") as ListFormat;
			if(port_count != null)
			{
				foreach (S2F42_PORTREPLY_PORT_COUNT item in port_count)
				{
					listNode_PORT_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}