using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    public class S6F111_CurrentRecipeDataReport_
    {
        private ListFormat ownerList = new ListFormat();

		private String dvname= "";
		private String dv= "";

        public S6F111_CurrentRecipeDataReport_(String dvname, String dv)
        {
			this.dvname = dvname;
			this.dv = dv;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			String[] sArray =  dvname.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "DVNAME", dvname);
			else
				ownerList.add(AsciiFormat.TYPE, 15, "DVNAME", dvname);
			sArray =  dv.Split(' ');
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, sArray.Length, "DV", dv);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "DV", dv);

            return ownerList;
        }

    }
}