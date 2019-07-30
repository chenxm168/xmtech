using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S7F106_RMSPPIDCHANGETIMEREPLY_PPID_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String ppid= "";
		private String time= "";

        public S7F106_RMSPPIDCHANGETIMEREPLY_PPID_COUNT(String ppid, String time)
        {
			this.ppid = ppid;
			this.time = time;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  ppid.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "PPID", ppid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			sArray =  time.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "TIME", time);
			else
				ownerList.add(AsciiFormat.TYPE, 14, "TIME", time);

            return ownerList;
        }

    }
}