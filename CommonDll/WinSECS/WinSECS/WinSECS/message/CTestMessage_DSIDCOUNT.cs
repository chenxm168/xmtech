using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class CTestMessage_DSIDCOUNT
    {
        private ListFormat ownerList = new ListFormat();

        private String dsid = "";
        private List<String> dvcount = new List<String>();


        public String DSID
        {
            get { return dsid; }
            set { dsid = value; }
        }

        public List<String> DVCOUNT
        {
            get { return dvcount; }
            set { dvcount = value; }
        }


        public CTestMessage_DSIDCOUNT()
        {
        }

        public CTestMessage_DSIDCOUNT(ListFormat rootFormat)
        {
        }

        public CTestMessage_DSIDCOUNT(String dsid, List<String> dvcount)
        {
            this.dsid = dsid;
            this.dvcount = dvcount;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

            ownerList.add(Uint2Format.TYPE, 1, "DSID", dsid);
            ownerList.add(CPrivateUtility.getMessage(isNoPadding, dvcount, AsciiFormat.TYPE, "DVVALUE", 1));

            return ownerList;
        }

        public void FillItemValue(ListFormat listFormat)
        {
            this.dsid = listFormat.Children[0].Value;
            this.dvcount = CPrivateUtility.getStringListItems(listFormat.Children[1] as ListFormat);

        }
    }
}