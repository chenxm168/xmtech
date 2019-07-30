using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S6F11_ProcessStateReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ceid, String rptid, String crst, String eqst, String stageid, String unitid, String rptid1, String pnlid, String bluid, String pnljudge)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 11;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			String[] sArray =  ceid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "CEID", ceid);
			else
				listNode_0.add(AsciiFormat.TYPE, 3, "CEID", ceid);
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid.Split(' ');
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, sArray.Length, "RPTID", rptid);
			else
				listNode_2.add(AsciiFormat.TYPE, 3, "RPTID", rptid);
			ListFormat listNode_3 = listNode_2.add(ListFormat.TYPE, 4, "", "") as ListFormat;
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
			sArray =  stageid.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "STAGEID", stageid);
			else
				listNode_3.add(AsciiFormat.TYPE, 1, "STAGEID", stageid);
			sArray =  unitid.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				listNode_3.add(AsciiFormat.TYPE, 20, "UNITID", unitid);
			ListFormat listNode_4 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rptid1.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "RPTID1", rptid1);
			else
				listNode_4.add(AsciiFormat.TYPE, 3, "RPTID1", rptid1);
			ListFormat listNode_5 = listNode_4.add(ListFormat.TYPE, 3, "", "") as ListFormat;
			sArray =  pnlid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "PNLID", pnlid);
			else
				listNode_5.add(AsciiFormat.TYPE, 20, "PNLID", pnlid);
			sArray =  bluid.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "BLUID", bluid);
			else
				listNode_5.add(AsciiFormat.TYPE, 50, "BLUID", bluid);
			sArray =  pnljudge.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "PNLJUDGE", pnljudge);
			else
				listNode_5.add(AsciiFormat.TYPE, 5, "PNLJUDGE", pnljudge);

            return trx;

        }
    }


}