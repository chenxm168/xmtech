using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F23_FDCTRACEINITREQUEST
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String trid= "";
		private String dsper= "";
		private String totsmp= "";
		private String repgsz= "";
		private String toolid= "";
		private List<String> svid_count= new List<String>();

        public BasicTransactionInfo BasicTrxInfo
        {
            get { return basicTrxInfo; }
            set { basicTrxInfo = value; }
        }
        public SECSTransaction SECSTrx
        {
            get { return trx; }
            set { trx = value; }
        }

		public String TRID
		{
			get { return trid; }
			set { trid = value; }
		}

		public String DSPER
		{
			get { return dsper; }
			set { dsper = value; }
		}

		public String TOTSMP
		{
			get { return totsmp; }
			set { totsmp = value; }
		}

		public String REPGSZ
		{
			get { return repgsz; }
			set { repgsz = value; }
		}

		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public List<String> SVID_COUNT
		{
			get { return svid_count; }
			set { svid_count = value; }
		}


        public S2F23_FDCTRACEINITREQUEST(SECSTransaction trx)
        {
            this.trx = trx;
            this.basicTrxInfo = new BasicTransactionInfo(trx);
            FillItemValue(trx);
        }

        public void dispose()
        {
            basicTrxInfo.dispose();
            basicTrxInfo = null;
            trx = null;
        }

        public void FillItemValue(SECSTransaction trx)
        {
			ListFormat listNode_0 = trx.Children[0] as ListFormat;
			this.trid = listNode_0.Children[0].Value;
			this.dsper = listNode_0.Children[1].Value;
			this.totsmp = listNode_0.Children[2].Value;
			this.repgsz = listNode_0.Children[3].Value;
			this.toolid = listNode_0.Children[4].Value;
			this.svid_count = CPrivateUtility.getStringListItems(listNode_0.Children[5] as ListFormat);

        }
    }
}