using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S1F11_NOUSE
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private List<S1F11_NOUSE_TOOL_COUNT> tool_count= new List<S1F11_NOUSE_TOOL_COUNT>();

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

		public List<S1F11_NOUSE_TOOL_COUNT> TOOL_COUNT
		{
			get { return tool_count; }
			set { tool_count = value; }
		}


        public S1F11_NOUSE(SECSTransaction trx)
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
			ListFormat listNode_TOOL_COUNT = trx.Children[0] as ListFormat;
			for (int i = 0; i < listNode_TOOL_COUNT.Length; i++)
			{
				S1F11_NOUSE_TOOL_COUNT vList = new S1F11_NOUSE_TOOL_COUNT();
				vList.FillItemValue(listNode_TOOL_COUNT.Children[i] as ListFormat);
				this.tool_count.Add(vList);
			}

        }
    }
}