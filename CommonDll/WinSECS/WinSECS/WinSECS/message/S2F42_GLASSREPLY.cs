using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_GLASSREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String hcack, String ipid_cp, String ipidack, String icid_cp, String icidack, String opid_cp, String opidack, String ocid_cp, String ocidack, String sslots_cp, String sslotsack, String tslots_cp, String tslotsack)
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
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 6, "", "") as ListFormat;
			ListFormat listNode_2 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ipid_cp).Length, "IPID_CP", ipid_cp);
			else
				listNode_2.add(AsciiFormat.TYPE, 10, "IPID_CP", ipid_cp);
			sArray =  ipidack.Split(' ');
			if (isNoPadding)
				listNode_2.add(BinaryFormat.TYPE, sArray.Length, "IPIDACK", ipidack);
			else
				listNode_2.add(BinaryFormat.TYPE, 1, "IPIDACK", ipidack);
			ListFormat listNode_3 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(icid_cp).Length, "ICID_CP", icid_cp);
			else
				listNode_3.add(AsciiFormat.TYPE, 10, "ICID_CP", icid_cp);
			sArray =  icidack.Split(' ');
			if (isNoPadding)
				listNode_3.add(BinaryFormat.TYPE, sArray.Length, "ICIDACK", icidack);
			else
				listNode_3.add(BinaryFormat.TYPE, 1, "ICIDACK", icidack);
			ListFormat listNode_4 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(opid_cp).Length, "OPID_CP", opid_cp);
			else
				listNode_4.add(AsciiFormat.TYPE, 10, "OPID_CP", opid_cp);
			sArray =  opidack.Split(' ');
			if (isNoPadding)
				listNode_4.add(BinaryFormat.TYPE, sArray.Length, "OPIDACK", opidack);
			else
				listNode_4.add(BinaryFormat.TYPE, 1, "OPIDACK", opidack);
			ListFormat listNode_5 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ocid_cp).Length, "OCID_CP", ocid_cp);
			else
				listNode_5.add(AsciiFormat.TYPE, 10, "OCID_CP", ocid_cp);
			sArray =  ocidack.Split(' ');
			if (isNoPadding)
				listNode_5.add(BinaryFormat.TYPE, sArray.Length, "OCIDACK", ocidack);
			else
				listNode_5.add(BinaryFormat.TYPE, 1, "OCIDACK", ocidack);
			ListFormat listNode_6 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(sslots_cp).Length, "SSLOTS_CP", sslots_cp);
			else
				listNode_6.add(AsciiFormat.TYPE, 10, "SSLOTS_CP", sslots_cp);
			sArray =  sslotsack.Split(' ');
			if (isNoPadding)
				listNode_6.add(BinaryFormat.TYPE, sArray.Length, "SSLOTSACK", sslotsack);
			else
				listNode_6.add(BinaryFormat.TYPE, 1, "SSLOTSACK", sslotsack);
			ListFormat listNode_7 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(tslots_cp).Length, "TSLOTS_CP", tslots_cp);
			else
				listNode_7.add(AsciiFormat.TYPE, 10, "TSLOTS_CP", tslots_cp);
			sArray =  tslotsack.Split(' ');
			if (isNoPadding)
				listNode_7.add(BinaryFormat.TYPE, sArray.Length, "TSLOTSACK", tslotsack);
			else
				listNode_7.add(BinaryFormat.TYPE, 1, "TSLOTSACK", tslotsack);

            return trx;

        }
    }


}