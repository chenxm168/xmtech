using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F1_ALARMREPORTEVENT
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String alcd, String alid, String altx, String unitid)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(5, true);
            trx.Function = 1;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 4, "", "") as ListFormat;
			String[] sArray =  alcd.Split(' ');
			if (isNoPadding)
				listNode_0.add(BinaryFormat.TYPE, sArray.Length, "ALCD", alcd);
			else
				listNode_0.add(BinaryFormat.TYPE, 1, "ALCD", alcd);
			sArray =  alid.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint4Format.TYPE, sArray.Length, "ALID", alid);
			else
				listNode_0.add(Uint4Format.TYPE, 1, "ALID", alid);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(altx).Length, "ALTX", altx);
			else
				listNode_0.add(AsciiFormat.TYPE, 80, "ALTX", altx);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				listNode_0.add(AsciiFormat.TYPE, 9, "UNITID", unitid);

            return trx;

        }
    }


}