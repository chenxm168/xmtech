using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S3F101_iCASSETTEINFORMATIONSEND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ptid= "";
		private String csid= "";
		private String jobid= "";
		private List<S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT> glass_count= new List<S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT>();

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

		public String PTID
		{
			get { return ptid; }
			set { ptid = value; }
		}

		public String CSID
		{
			get { return csid; }
			set { csid = value; }
		}

		public String JOBID
		{
			get { return jobid; }
			set { jobid = value; }
		}

		public List<S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT> GLASS_COUNT
		{
			get { return glass_count; }
			set { glass_count = value; }
		}


        public S3F101_iCASSETTEINFORMATIONSEND(SECSTransaction trx)
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
			ListFormat listNode_1 = listNode_0.Children[0] as ListFormat;
			this.ptid = listNode_1.Children[0].Value;
			this.csid = listNode_1.Children[1].Value;
			this.jobid = listNode_1.Children[2].Value;
			ListFormat listNode_GLASS_COUNT = listNode_0.Children[1] as ListFormat;
			for (int i = 0; i < listNode_GLASS_COUNT.Length; i++)
			{
				S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT vList = new S3F101_iCASSETTEINFORMATIONSEND_GLASS_COUNT();
				vList.FillItemValue(listNode_GLASS_COUNT.Children[i] as ListFormat);
				this.glass_count.Add(vList);
			}

        }
    }
}