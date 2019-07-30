using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F104_RMSPPIDEXISTENCEREPLY_PPID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ppid= "";
		private String ack= "";

        public S7F104_RMSPPIDEXISTENCEREPLY_PPID_COUNT(String ppid, String ack)
        {
			this.ppid = ppid;
			this.ack = ack;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  ppid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			sArray =  ack.Split(' ');
			if (isNoPadding)
				ownerList.add(Uint1Format.TYPE, sArray.Length, "ACK", ack);
			else
				ownerList.add(Uint1Format.TYPE, 1, "ACK", ack);

            return ownerList;
        }

    }
}