using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S6F102_LOTLISTREPLY
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String ack6= "";
		private List<String> lot_count= new List<String>();

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

		public String ACK6
		{
			get { return ack6; }
			set { ack6 = value; }
		}

		public List<String> LOT_COUNT
		{
			get { return lot_count; }
			set { lot_count = value; }
		}


        public S6F102_LOTLISTREPLY(SECSTransaction trx)
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
			this.ack6 = listNode_0.Children[0].Value;
			this.lot_count = CPrivateUtility.getStringListItems(listNode_0.Children[1] as ListFormat);

        }
    }
}