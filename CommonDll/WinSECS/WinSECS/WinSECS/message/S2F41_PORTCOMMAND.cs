using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F41_PORTCOMMAND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private List<S2F41_PORTCOMMAND_PORT_COUNT> port_count= new List<S2F41_PORTCOMMAND_PORT_COUNT>();

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

		public List<S2F41_PORTCOMMAND_PORT_COUNT> PORT_COUNT
		{
			get { return port_count; }
			set { port_count = value; }
		}


        public S2F41_PORTCOMMAND(SECSTransaction trx)
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
			ListFormat listNode_PORT_COUNT = listNode_0.Children[1] as ListFormat;
			for (int i = 0; i < listNode_PORT_COUNT.Length; i++)
			{
				S2F41_PORTCOMMAND_PORT_COUNT vList = new S2F41_PORTCOMMAND_PORT_COUNT();
				vList.FillItemValue(listNode_PORT_COUNT.Children[i] as ListFormat);
				this.port_count.Add(vList);
			}

        }
    }
}