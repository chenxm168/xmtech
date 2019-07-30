using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F2_MASKINFORMAIONREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String toolid, List<S3F2_MASKINFORMAIONREPLY_MASK_COUNT> mask_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(3, false);
            trx.Function = 2;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				listNode_0.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_MASK_COUNT = listNode_0.add(ListFormat.TYPE, -1, "MASK_COUNT", "") as ListFormat;
			if(mask_count != null)
			{
				foreach (S3F2_MASKINFORMAIONREPLY_MASK_COUNT item in mask_count)
				{
					listNode_MASK_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}