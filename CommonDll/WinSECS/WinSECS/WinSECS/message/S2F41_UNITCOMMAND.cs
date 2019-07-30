using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F41_UNITCOMMAND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private List<S2F41_UNITCOMMAND_UNIT_COUNT> unit_count= new List<S2F41_UNITCOMMAND_UNIT_COUNT>();

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

		public List<S2F41_UNITCOMMAND_UNIT_COUNT> UNIT_COUNT
		{
			get { return unit_count; }
			set { unit_count = value; }
		}


        public S2F41_UNITCOMMAND(SECSTransaction trx)
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
			ListFormat listNode_UNIT_COUNT = listNode_0.Children[1] as ListFormat;
			for (int i = 0; i < listNode_UNIT_COUNT.Length; i++)
			{
				S2F41_UNITCOMMAND_UNIT_COUNT vList = new S2F41_UNITCOMMAND_UNIT_COUNT();
				vList.FillItemValue(listNode_UNIT_COUNT.Children[i] as ListFormat);
				this.unit_count.Add(vList);
			}

        }
    }
}