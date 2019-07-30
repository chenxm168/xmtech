using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace BMDT.SECS.Message
{
    public class S6F151_TackTimeReport
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String unitid, String statime, String endtime, String samcount, String avetrackvalue, String maxtrackvalue, String mintrackvalue)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 151;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 7, "", "") as ListFormat;
			String[] sArray =  unitid.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "UNITID", unitid);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "UNITID", unitid);
			sArray =  statime.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "STATIME", statime);
			else
				listNode_0.add(AsciiFormat.TYPE, 16, "STATIME", statime);
			sArray =  endtime.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "ENDTIME", endtime);
			else
				listNode_0.add(AsciiFormat.TYPE, 16, "ENDTIME", endtime);
			sArray =  samcount.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "SAMCOUNT", samcount);
			else
				listNode_0.add(AsciiFormat.TYPE, 10, "SAMCOUNT", samcount);
			sArray =  avetrackvalue.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "AVETRACKVALUE", avetrackvalue);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "AVETRACKVALUE", avetrackvalue);
			sArray =  maxtrackvalue.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "MAXTRACKVALUE", maxtrackvalue);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "MAXTRACKVALUE", maxtrackvalue);
			sArray =  mintrackvalue.Split(' ');
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, sArray.Length, "MINTRACKVALUE", mintrackvalue);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "MINTRACKVALUE", mintrackvalue);

            return trx;

        }
    }


}