using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_iEQPREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String hcack, String rcmd_cp, String rcmd, String toolid_cp, String toolid, String usop_cp, String usop, String unit_cp, String unit, String ppid_cp, String ppid, String eqmode_cp, String eqmode, String split_cp, String splitmode, String recive_cp, String recivemode, String name_cp, String itemname, String value_cp, String itemvalue, String text_cp, String text)
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
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 11, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rcmd_cp.Split(' ');
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, sArray.Length, "RCMD_CP", rcmd_cp);
			else
				listNode_2.add(AsciiFormat.TYPE, 6, "RCMD_CP", rcmd_cp);
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(rcmd).Length, "RCMD", rcmd);
			else
				listNode_2.add(AsciiFormat.TYPE, 80, "RCMD", rcmd);
			ListFormat listNode_3 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  toolid_cp.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "TOOLID_CP", toolid_cp);
			else
				listNode_3.add(AsciiFormat.TYPE, 6, "TOOLID_CP", toolid_cp);
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				listNode_3.add(AsciiFormat.TYPE, 80, "TOOLID", toolid);
			ListFormat listNode_4 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  usop_cp.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "USOP_CP", usop_cp);
			else
				listNode_4.add(AsciiFormat.TYPE, 6, "USOP_CP", usop_cp);
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(usop).Length, "USOP", usop);
			else
				listNode_4.add(AsciiFormat.TYPE, 80, "USOP", usop);
			ListFormat listNode_5 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  unit_cp.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "UNIT_CP", unit_cp);
			else
				listNode_5.add(AsciiFormat.TYPE, 6, "UNIT_CP", unit_cp);
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unit).Length, "UNIT", unit);
			else
				listNode_5.add(AsciiFormat.TYPE, 80, "UNIT", unit);
			ListFormat listNode_6 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  ppid_cp.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "PPID_CP", ppid_cp);
			else
				listNode_6.add(AsciiFormat.TYPE, 6, "PPID_CP", ppid_cp);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				listNode_6.add(AsciiFormat.TYPE, 80, "PPID", ppid);
			ListFormat listNode_7 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  eqmode_cp.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "EQMODE_CP", eqmode_cp);
			else
				listNode_7.add(AsciiFormat.TYPE, 6, "EQMODE_CP", eqmode_cp);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(eqmode).Length, "EQMODE", eqmode);
			else
				listNode_7.add(AsciiFormat.TYPE, 80, "EQMODE", eqmode);
			ListFormat listNode_8 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  split_cp.Split(' ');
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, sArray.Length, "SPLIT_CP", split_cp);
			else
				listNode_8.add(AsciiFormat.TYPE, 6, "SPLIT_CP", split_cp);
			if (isNoPadding)
				listNode_8.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(splitmode).Length, "SPLITMODE", splitmode);
			else
				listNode_8.add(AsciiFormat.TYPE, 80, "SPLITMODE", splitmode);
			ListFormat listNode_9 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  recive_cp.Split(' ');
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, sArray.Length, "RECIVE_CP", recive_cp);
			else
				listNode_9.add(AsciiFormat.TYPE, 6, "RECIVE_CP", recive_cp);
			if (isNoPadding)
				listNode_9.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(recivemode).Length, "RECIVEMODE", recivemode);
			else
				listNode_9.add(AsciiFormat.TYPE, 80, "RECIVEMODE", recivemode);
			ListFormat listNode_10 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  name_cp.Split(' ');
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, sArray.Length, "NAME_CP", name_cp);
			else
				listNode_10.add(AsciiFormat.TYPE, 6, "NAME_CP", name_cp);
			if (isNoPadding)
				listNode_10.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(itemname).Length, "ITEMNAME", itemname);
			else
				listNode_10.add(AsciiFormat.TYPE, 80, "ITEMNAME", itemname);
			ListFormat listNode_11 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  value_cp.Split(' ');
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, sArray.Length, "VALUE_CP", value_cp);
			else
				listNode_11.add(AsciiFormat.TYPE, 6, "VALUE_CP", value_cp);
			if (isNoPadding)
				listNode_11.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(itemvalue).Length, "ITEMVALUE", itemvalue);
			else
				listNode_11.add(AsciiFormat.TYPE, 80, "ITEMVALUE", itemvalue);
			ListFormat listNode_12 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  text_cp.Split(' ');
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, sArray.Length, "TEXT_CP", text_cp);
			else
				listNode_12.add(AsciiFormat.TYPE, 6, "TEXT_CP", text_cp);
			if (isNoPadding)
				listNode_12.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(text).Length, "TEXT", text);
			else
				listNode_12.add(AsciiFormat.TYPE, 80, "TEXT", text);

            return trx;

        }
    }


}