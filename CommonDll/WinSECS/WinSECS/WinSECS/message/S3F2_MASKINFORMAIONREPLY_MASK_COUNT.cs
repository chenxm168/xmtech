using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S3F2_MASKINFORMAIONREPLY_MASK_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String librayid= "";
		private String materialid= "";
		private String location= "";
		private String state= "";
		private String masktype= "";

        public S3F2_MASKINFORMAIONREPLY_MASK_COUNT(String librayid, String materialid, String location, String state, String masktype)
        {
			this.librayid = librayid;
			this.materialid = materialid;
			this.location = location;
			this.state = state;
			this.masktype = masktype;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 5;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(librayid).Length, "LIBRAYID", librayid);
			else
				ownerList.add(AsciiFormat.TYPE, 2, "LIBRAYID", librayid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(materialid).Length, "MATERIALID", materialid);
			else
				ownerList.add(AsciiFormat.TYPE, 20, "MATERIALID", materialid);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(location).Length, "LOCATION", location);
			else
				ownerList.add(AsciiFormat.TYPE, 10, "LOCATION", location);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(state).Length, "STATE", state);
			else
				ownerList.add(AsciiFormat.TYPE, 10, "STATE", state);
			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(masktype).Length, "MASKTYPE", masktype);
			else
				ownerList.add(AsciiFormat.TYPE, 10, "MASKTYPE", masktype);

            return ownerList;
        }

    }
}