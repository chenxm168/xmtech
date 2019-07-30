using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S6F11_ControlOfflineChangeReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ceid, String rptid, String crst, String eqst, String unitid)
        {
            SECSTransaction trx = new SECSTransaction();
            unitid = unitid.PadRight(ConstDef.UNNIT_LEN, ' ');
            trx.setStreamNWbit(6, true);
            trx.Function = 11;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  ceid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "CEID", ceid);
			else
				listNode_0.add(AsciiFormat.TYPE, 3, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 1, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid.Split(' ');
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, sArray.Length, "RPTID", rptid);
			else
				listNode_2.add(AsciiFormat.TYPE, 3, "RPTID", rptid);
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			sArray =  crst.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "CRST", crst);
			else
				listNode_3.add(AsciiFormat.TYPE, 1, "CRST", crst);
			sArray =  eqst.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "EQST", eqst);
			else
				listNode_3.add(AsciiFormat.TYPE, 1, "EQST", eqst);
			sArray =  unitid.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				listNode_3.add(AsciiFormat.TYPE, 20, "UNITID", unitid);

            return trx;

        }
    }


}