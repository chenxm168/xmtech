using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    class S2F41_OPCallRequest
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private String opcall= "";
		private String message= "";

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

		public String OPCALL
		{
			get { return opcall; }
			set { opcall = value; }
		}

		public String MESSAGE
		{
			get { return message; }
			set { message = value; }
		}


        public S2F41_OPCallRequest(SECSTransaction trx)
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
			this.rcmd = trx.Children[0].Value;
			ListFormat listNode_0 = trx.Children[1] as ListFormat;
			ListFormat listNode_1 = listNode_0.Children[0] as ListFormat;
			this.opcall = listNode_1.Children[0].Value;
			this.message = listNode_1.Children[1].Value;

        }
    }
}