using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ppid= "";
		private String mdln= "";
		private String softrev= "";
		private String mode= "";
		private List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT> tool_count= new List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT>();

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

		public String PPID
		{
			get { return ppid; }
			set { ppid = value; }
		}

		public String MDLN
		{
			get { return mdln; }
			set { mdln = value; }
		}

		public String SOFTREV
		{
			get { return softrev; }
			set { softrev = value; }
		}

		public String MODE
		{
			get { return mode; }
			set { mode = value; }
		}

		public List<S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT> TOOL_COUNT
		{
			get { return tool_count; }
			set { tool_count = value; }
		}


        public S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B(SECSTransaction trx)
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
			this.ppid = listNode_0.Children[0].Value;
			this.mdln = listNode_0.Children[1].Value;
			this.softrev = listNode_0.Children[2].Value;
			this.mode = listNode_0.Children[3].Value;
			ListFormat listNode_TOOL_COUNT = listNode_0.Children[4] as ListFormat;
			for (int i = 0; i < listNode_TOOL_COUNT.Length; i++)
			{
				S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT vList = new S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B_TOOL_COUNT();
				vList.FillItemValue(listNode_TOOL_COUNT.Children[i] as ListFormat);
				this.tool_count.Add(vList);
			}

        }
    }
}