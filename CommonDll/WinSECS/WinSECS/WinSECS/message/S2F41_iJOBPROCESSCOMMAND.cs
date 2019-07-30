using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F41_iJOBPROCESSCOMMAND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private String ipid_cp= "";
		private String ipid= "";
		private String icid_cp= "";
		private String icid= "";
		private String opid_cp= "";
		private String opid= "";
		private String ocid_cp= "";
		private String ocid= "";
		private String jobid_cp= "";
		private String jobid= "";
		private String stif_cp= "";
		private String stif= "";

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

		public String IPID_CP
		{
			get { return ipid_cp; }
			set { ipid_cp = value; }
		}

		public String IPID
		{
			get { return ipid; }
			set { ipid = value; }
		}

		public String ICID_CP
		{
			get { return icid_cp; }
			set { icid_cp = value; }
		}

		public String ICID
		{
			get { return icid; }
			set { icid = value; }
		}

		public String OPID_CP
		{
			get { return opid_cp; }
			set { opid_cp = value; }
		}

		public String OPID
		{
			get { return opid; }
			set { opid = value; }
		}

		public String OCID_CP
		{
			get { return ocid_cp; }
			set { ocid_cp = value; }
		}

		public String OCID
		{
			get { return ocid; }
			set { ocid = value; }
		}

		public String JOBID_CP
		{
			get { return jobid_cp; }
			set { jobid_cp = value; }
		}

		public String JOBID
		{
			get { return jobid; }
			set { jobid = value; }
		}

		public String STIF_CP
		{
			get { return stif_cp; }
			set { stif_cp = value; }
		}

		public String STIF
		{
			get { return stif; }
			set { stif = value; }
		}


        public S2F41_iJOBPROCESSCOMMAND(SECSTransaction trx)
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
			this.ipid_cp = listNode_2.Children[0].Value;
			this.ipid = listNode_2.Children[1].Value;
			ListFormat listNode_3 = listNode_1.Children[1] as ListFormat;
			this.icid_cp = listNode_3.Children[0].Value;
			this.icid = listNode_3.Children[1].Value;
			ListFormat listNode_4 = listNode_1.Children[2] as ListFormat;
			this.opid_cp = listNode_4.Children[0].Value;
			this.opid = listNode_4.Children[1].Value;
			ListFormat listNode_5 = listNode_1.Children[3] as ListFormat;
			this.ocid_cp = listNode_5.Children[0].Value;
			this.ocid = listNode_5.Children[1].Value;
			ListFormat listNode_6 = listNode_1.Children[4] as ListFormat;
			this.jobid_cp = listNode_6.Children[0].Value;
			this.jobid = listNode_6.Children[1].Value;
			ListFormat listNode_7 = listNode_1.Children[5] as ListFormat;
			this.stif_cp = listNode_7.Children[0].Value;
			this.stif = listNode_7.Children[1].Value;

        }
    }
}