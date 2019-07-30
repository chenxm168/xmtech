using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S2F42_iSPECIFICAREARWREPLY
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String hcack, String rcmd_cp, String rcmd, String rwtype_cp, String rwtype, String addr_cp, String address, String length_cp, String length, String data_cp, String data, String seqno_cp, String seqno)
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
			sArray =  rcmd_cp.Split(' ');
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, sArray.Length, "RCMD_CP", rcmd_cp);
			else
				listNode_2.add(AsciiFormat.TYPE, 6, "RCMD_CP", rcmd_cp);
			if (isNoPadding)
				listNode_2.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(rcmd).Length, "RCMD", rcmd);
			else
				listNode_2.add(AsciiFormat.TYPE, 4, "RCMD", rcmd);
			ListFormat listNode_3 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  rwtype_cp.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "RWTYPE_CP", rwtype_cp);
			else
				listNode_3.add(AsciiFormat.TYPE, 6, "RWTYPE_CP", rwtype_cp);
			sArray =  rwtype.Split(' ');
			if (isNoPadding)
				listNode_3.add(AsciiFormat.TYPE, sArray.Length, "RWTYPE", rwtype);
			else
				listNode_3.add(AsciiFormat.TYPE, 4, "RWTYPE", rwtype);
			ListFormat listNode_4 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  addr_cp.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "ADDR_CP", addr_cp);
			else
				listNode_4.add(AsciiFormat.TYPE, 6, "ADDR_CP", addr_cp);
			sArray =  address.Split(' ');
			if (isNoPadding)
				listNode_4.add(AsciiFormat.TYPE, sArray.Length, "ADDRESS", address);
			else
				listNode_4.add(AsciiFormat.TYPE, 6, "ADDRESS", address);
			ListFormat listNode_5 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  length_cp.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "LENGTH_CP", length_cp);
			else
				listNode_5.add(AsciiFormat.TYPE, 6, "LENGTH_CP", length_cp);
			sArray =  length.Split(' ');
			if (isNoPadding)
				listNode_5.add(AsciiFormat.TYPE, sArray.Length, "LENGTH", length);
			else
				listNode_5.add(AsciiFormat.TYPE, 4, "LENGTH", length);
			ListFormat listNode_6 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  data_cp.Split(' ');
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, sArray.Length, "DATA_CP", data_cp);
			else
				listNode_6.add(AsciiFormat.TYPE, 6, "DATA_CP", data_cp);
			if (isNoPadding)
				listNode_6.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(data).Length, "DATA", data);
			else
				listNode_6.add(AsciiFormat.TYPE, 1000, "DATA", data);
			ListFormat listNode_7 = listNode_1.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			sArray =  seqno_cp.Split(' ');
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, sArray.Length, "SEQNO_CP", seqno_cp);
			else
				listNode_7.add(AsciiFormat.TYPE, 6, "SEQNO_CP", seqno_cp);
			if (isNoPadding)
				listNode_7.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(seqno).Length, "SEQNO", seqno);
			else
				listNode_7.add(AsciiFormat.TYPE, 80, "SEQNO", seqno);

            return trx;

        }
    }


}