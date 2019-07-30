using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class CTestMessage
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

        private String pfcd = "";
        private String dataid = "";
        private String ceid = "";
        private List<CTestMessage_DSIDCOUNT> dsidcount = null;

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

        public String PFCD
        {
            get { return pfcd; }
            set { pfcd = value; }
        }

        public String DATAID
        {
            get { return dataid; }
            set { dataid = value; }
        }

        public String CEID
        {
            get { return ceid; }
            set { ceid = value; }
        }

        public List<CTestMessage_DSIDCOUNT> DSIDCOUNT
        {
            get { return dsidcount; }
            set { dsidcount = value; }
        }


        public CTestMessage(SECSTransaction trx)
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

        public static SECSTransaction makeTransaction(bool isNoPadding, String pfcd, String dataid, String ceid, List<CTestMessage_DSIDCOUNT> dsidcount)
        {
            SECSTransaction trx = new SECSTransaction();

            trx.setStreamNWbit(6, true);
            trx.Function = 9;

            ListFormat listNode_0 = trx.add(ListFormat.TYPE, 4, "", "") as ListFormat;
            listNode_0.add(BinaryFormat.TYPE, 1, "PFCD", pfcd);
            listNode_0.add(Uint2Format.TYPE, 1, "DATAID", dataid);
            listNode_0.add(Uint2Format.TYPE, 1, "CEID", ceid);
            ListFormat listNode_DSIDCOUNT = listNode_0.add(ListFormat.TYPE, 10, "DSIDCOUNT", "") as ListFormat;
            foreach (CTestMessage_DSIDCOUNT item in dsidcount)
            {
                listNode_DSIDCOUNT.add(item.getMessage(isNoPadding));
            }

            return trx;
        }

        public void FillItemValue(SECSTransaction trx)
        {
            ListFormat listNode_1 = trx.Children[0] as ListFormat;
            this.pfcd = listNode_1.Children[0].Value;
            this.dataid = listNode_1.Children[1].Value;
            this.ceid = listNode_1.Children[2].Value;
            ListFormat listNode_DSIDCOUNT = listNode_1.Children[3] as ListFormat;
            for (int i = 0; i < listNode_DSIDCOUNT.Length; i++)
            {
                CTestMessage_DSIDCOUNT vList = new CTestMessage_DSIDCOUNT();
                vList.FillItemValue(listNode_DSIDCOUNT.Children[i] as ListFormat);
                this.dsidcount.Add(vList);
            }

        }
    }
}