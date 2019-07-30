using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ack5= "";
		private String alcd= "";
		private String alid= "";
		private String altx= "";
		private String unitid= "";
		private String altm= "";

        public S5F104_ALARMRESETREPLY_TOOL_COUNT_ALARM_COUNT(String ack5, String alcd, String alid, String altx, String unitid, String altm)
        {
			this.ack5 = ack5;
			this.alcd = alcd;
			this.alid = alid;
			this.altx = altx;
			this.unitid = unitid;
			this.altm = altm;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 6;

			String[] sArray =  ack5.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "ACK5", ack5);
			else
				ownerList.add(Uint1Format.TYPE, 1, "ACK5", ack5);
			sArray =  alcd.Split(' ');
			if (isNoPadding)
				ownerList.add(BinaryFormat.TYPE, sArray.Length, "ALCD", alcd);
			else
				ownerList.add(BinaryFormat.TYPE, 1, "ALCD", alcd);
			sArray =  alid.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint4Format.TYPE, sArray.Length, "ALID", alid);
			else
				ownerList.add(Uint4Format.TYPE, 1, "ALID", alid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(altx).Length, "ALTX", altx);
			else
				ownerList.add(AsciiFormat.TYPE, 80, "ALTX", altx);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(unitid).Length, "UNITID", unitid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "UNITID", unitid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(altm).Length, "ALTM", altm);
			else
				ownerList.add(AsciiFormat.TYPE, 14, "ALTM", altm);

            return ownerList;
        }

    }
}