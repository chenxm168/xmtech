using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class S2F41_iEQPCOMMAND
    {
        private BasicTransactionInfo basicTrxInfo;
        private SECSTransaction trx;

		private String rcmd= "";
		private String toolid_cp= "";
		private String toolid= "";
		private String usop_cp= "";
		private String usop= "";
		private String unit_cp= "";
		private String unit= "";
		private String ppid_cp= "";
		private String ppid= "";
		private String eqmode_cp= "";
		private String eqmode= "";
		private String split_cp= "";
		private String splitmode= "";
		private String recive_cp= "";
		private String recivemode= "";
		private String name_cp= "";
		private String itemname= "";
		private String value_cp= "";
		private String itemvalue= "";
		private String text_cp= "";
		private String text= "";

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

		public String TOOLID_CP
		{
			get { return toolid_cp; }
			set { toolid_cp = value; }
		}

		public String TOOLID
		{
			get { return toolid; }
			set { toolid = value; }
		}

		public String USOP_CP
		{
			get { return usop_cp; }
			set { usop_cp = value; }
		}

		public String USOP
		{
			get { return usop; }
			set { usop = value; }
		}

		public String UNIT_CP
		{
			get { return unit_cp; }
			set { unit_cp = value; }
		}

		public String UNIT
		{
			get { return unit; }
			set { unit = value; }
		}

		public String PPID_CP
		{
			get { return ppid_cp; }
			set { ppid_cp = value; }
		}

		public String PPID
		{
			get { return ppid; }
			set { ppid = value; }
		}

		public String EQMODE_CP
		{
			get { return eqmode_cp; }
			set { eqmode_cp = value; }
		}

		public String EQMODE
		{
			get { return eqmode; }
			set { eqmode = value; }
		}

		public String SPLIT_CP
		{
			get { return split_cp; }
			set { split_cp = value; }
		}

		public String SPLITMODE
		{
			get { return splitmode; }
			set { splitmode = value; }
		}

		public String RECIVE_CP
		{
			get { return recive_cp; }
			set { recive_cp = value; }
		}

		public String RECIVEMODE
		{
			get { return recivemode; }
			set { recivemode = value; }
		}

		public String NAME_CP
		{
			get { return name_cp; }
			set { name_cp = value; }
		}

		public String ITEMNAME
		{
			get { return itemname; }
			set { itemname = value; }
		}

		public String VALUE_CP
		{
			get { return value_cp; }
			set { value_cp = value; }
		}

		public String ITEMVALUE
		{
			get { return itemvalue; }
			set { itemvalue = value; }
		}

		public String TEXT_CP
		{
			get { return text_cp; }
			set { text_cp = value; }
		}

		public String TEXT
		{
			get { return text; }
			set { text = value; }
		}


        public S2F41_iEQPCOMMAND(SECSTransaction trx)
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
			this.toolid_cp = listNode_2.Children[0].Value;
			this.toolid = listNode_2.Children[1].Value;
			ListFormat listNode_3 = listNode_1.Children[1] as ListFormat;
			this.usop_cp = listNode_3.Children[0].Value;
			this.usop = listNode_3.Children[1].Value;
			ListFormat listNode_4 = listNode_1.Children[2] as ListFormat;
			this.unit_cp = listNode_4.Children[0].Value;
			this.unit = listNode_4.Children[1].Value;
			ListFormat listNode_5 = listNode_1.Children[3] as ListFormat;
			this.ppid_cp = listNode_5.Children[0].Value;
			this.ppid = listNode_5.Children[1].Value;
			ListFormat listNode_6 = listNode_1.Children[4] as ListFormat;
			this.eqmode_cp = listNode_6.Children[0].Value;
			this.eqmode = listNode_6.Children[1].Value;
			ListFormat listNode_7 = listNode_1.Children[5] as ListFormat;
			this.split_cp = listNode_7.Children[0].Value;
			this.splitmode = listNode_7.Children[1].Value;
			ListFormat listNode_8 = listNode_1.Children[6] as ListFormat;
			this.recive_cp = listNode_8.Children[0].Value;
			this.recivemode = listNode_8.Children[1].Value;
			ListFormat listNode_9 = listNode_1.Children[7] as ListFormat;
			this.name_cp = listNode_9.Children[0].Value;
			this.itemname = listNode_9.Children[1].Value;
			ListFormat listNode_10 = listNode_1.Children[8] as ListFormat;
			this.value_cp = listNode_10.Children[0].Value;
			this.itemvalue = listNode_10.Children[1].Value;
			ListFormat listNode_11 = listNode_1.Children[9] as ListFormat;
			this.text_cp = listNode_11.Children[0].Value;
			this.text = listNode_11.Children[1].Value;

        }
    }
}