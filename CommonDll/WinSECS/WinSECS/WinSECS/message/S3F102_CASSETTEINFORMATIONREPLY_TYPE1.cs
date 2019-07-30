using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F102_CASSETTEINFORMATIONREPLY_TYPE1
    {
        public static SECSTransaction makeTransaction(bool isNoPadding , String ack6, String ptid, String csid, String jobid, String device, List<S3F102_CASSETTEINFORMATIONREPLY_TYPE1_GLASS_COUNT> glass_count)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(3, false);
            trx.Function = 102;

			ListFormat listNode_0 = trx.add(ListFormat.TYPE, 2, "", "") as ListFormat;
			ListFormat listNode_1 = listNode_0.add(ListFormat.TYPE, 5, "", "") as ListFormat;
			String[] sArray =  ack6.Split(' ');
			if (isNoPadding)
				listNode_1.add(Uint1Format.TYPE, sArray.Length, "ACK6", ack6);
			else
				listNode_1.add(Uint1Format.TYPE, 1, "ACK6", ack6);
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ptid).Length, "PTID", ptid);
			else
				listNode_1.add(AsciiFormat.TYPE, 2, "PTID", ptid);
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(csid).Length, "CSID", csid);
			else
				listNode_1.add(AsciiFormat.TYPE, 16, "CSID", csid);
			if (isNoPadding)
				listNode_1.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(jobid).Length, "JOBID", jobid);
			else
				listNode_1.add(AsciiFormat.TYPE, 20, "JOBID", jobid);
			sArray =  device.Split(' ');
			if (isNoPadding)
				listNode_1.add(Uint1Format.TYPE, sArray.Length, "DEVICE", device);
			else
				listNode_1.add(Uint1Format.TYPE, 1, "DEVICE", device);
			ListFormat listNode_GLASS_COUNT = listNode_0.add(ListFormat.TYPE, -1, "GLASS_COUNT", "") as ListFormat;
			if(glass_count != null)
			{
				foreach (S3F102_CASSETTEINFORMATIONREPLY_TYPE1_GLASS_COUNT item in glass_count)
				{
					listNode_GLASS_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return trx;

        }
    }


}