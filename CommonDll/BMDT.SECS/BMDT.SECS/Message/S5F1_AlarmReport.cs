using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S5F1_AlarmReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String alst, String alcd, String alid, String altx, String unitid)
        {
            SECSTransaction trx = new SECSTransaction();
            unitid = unitid.PadRight(ConstDef.UNNIT_LEN, ' ');
            altx = altx.PadRight(ConstDef.ALTX_LEN, ' ');
            trx.setStreamNWbit(5, true);
            trx.Function = 1;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 5, "", "") as ListFormat;
			String[] sArray =  alst.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "ALST", alst);
			else
				listNode_0.add(AsciiFormat.TYPE, 1, "ALST", alst);
			sArray =  alcd.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "ALCD", alcd);
			else
				listNode_0.add(AsciiFormat.TYPE, 1, "ALCD", alcd);
			sArray =  alid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "ALID", alid);
			else
				listNode_0.add(AsciiFormat.TYPE, 5, "ALID", alid);
			sArray =  altx.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "ALTX", altx);
			else
				listNode_0.add(AsciiFormat.TYPE, 40, "ALTX", altx);
			sArray =  unitid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "UNITID", unitid);

            return trx;

        }
    }


}