using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F41_iSPECIFICAREARWCOMMAND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private String rwtype_cp= "";
		private String rwtype= "";
		private String addr_cp= "";
		private String address= "";
		private String length_cp= "";
		private String length= "";
		private String data_cp= "";
		private String data= "";
		private String seqno_cp= "";
		private String seqno= "";

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

		public String RCMD
		{
			get { return rcmd; }
			set { rcmd = value; }
		}

		public String RWTYPE_CP
		{
			get { return rwtype_cp; }
			set { rwtype_cp = value; }
		}

		public String RWTYPE
		{
			get { return rwtype; }
			set { rwtype = value; }
		}

		public String ADDR_CP
		{
			get { return addr_cp; }
			set { addr_cp = value; }
		}

		public String ADDRESS
		{
			get { return address; }
			set { address = value; }
		}

		public String LENGTH_CP
		{
			get { return length_cp; }
			set { length_cp = value; }
		}

		public String LENGTH
		{
			get { return length; }
			set { length = value; }
		}

		public String DATA_CP
		{
			get { return data_cp; }
			set { data_cp = value; }
		}

		public String DATA
		{
			get { return data; }
			set { data = value; }
		}

		public String SEQNO_CP
		{
			get { return seqno_cp; }
			set { seqno_cp = value; }
		}

		public String SEQNO
		{
			get { return seqno; }
			set { seqno = value; }
		}


        public S2F41_iSPECIFICAREARWCOMMAND(SECSTransaction trx)
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
			this.rcmd = listNode_0.Children[0].Value;
			ListFormat listNode_1 = listNode_0.Children[1] as ListFormat;
			ListFormat listNode_2 = listNode_1.Children[0] as ListFormat;
			this.rwtype_cp = listNode_2.Children[0].Value;
			this.rwtype = listNode_2.Children[1].Value;
			ListFormat listNode_3 = listNode_1.Children[1] as ListFormat;
			this.addr_cp = listNode_3.Children[0].Value;
			this.address = listNode_3.Children[1].Value;
			ListFormat listNode_4 = listNode_1.Children[2] as ListFormat;
			this.length_cp = listNode_4.Children[0].Value;
			this.length = listNode_4.Children[1].Value;
			ListFormat listNode_5 = listNode_1.Children[3] as ListFormat;
			this.data_cp = listNode_5.Children[0].Value;
			this.data = listNode_5.Children[1].Value;
			ListFormat listNode_6 = listNode_1.Children[4] as ListFormat;
			this.seqno_cp = listNode_6.Children[0].Value;
			this.seqno = listNode_6.Children[1].Value;

        }
    }
}