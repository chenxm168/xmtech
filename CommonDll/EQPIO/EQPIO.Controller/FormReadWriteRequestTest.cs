using EQPIO.Common;
using EQPIO.Controller.Proxy;
using EQPIO.MessageData;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Controller
{
	public class FormReadWriteRequestTest //: Form
	{
        //private enum enumSearchType
        //{
        //    transaction,
        //    block,
        //    address,
        //    userdefine
        //}

        //public class BlockGroup
        //{
        //    [XmlElement]
        //    public Block[] Block
        //    {
        //        get;
        //        set;
        //    }
        //}

        //public class UserDefine
        //{
        //    [XmlElement]
        //    public BlockGroup BlockGroup
        //    {
        //        get;
        //        set;
        //    }
        //}

        //private ILog logger = LogManager.GetLogger(typeof(FormReadWriteRequestTest));

        //private ControlManager m_ControlManager = null;

        //private DataTable m_dataTable = new DataTable();

        //private ArrayList m_arrSearchResult = new ArrayList();

        //private int m_iMaxSimilarCount = 30;

        //private Dictionary<string, int> m_dicMultiBlockConfigure = new Dictionary<string, int>();

        //private Dictionary<string, UserDefine> m_dicUserDefine = new Dictionary<string, UserDefine>();

        //private IContainer components = null;

        //private GroupBox groupBox1;

        //private TextBox textBoxSearchTextInput;

        //private Button buttonFind;

        //private RadioButton radioButtonRead;

        //private RadioButton radioButtonWrite;

        //private Button buttonRequest;

        //private DataGridView dataGridView1;

        //private ComboBox comboBoxSerachType;

        //private TextBox textBoxNotFound;

        //private Panel panel1;

        //private DataGridViewTextBoxColumn colBlock;

        //private DataGridViewTextBoxColumn colItem;

        //private DataGridViewTextBoxColumn colValue;

        //private DataGridViewTextBoxColumn colRepresentation;

        //private ComboBox comboBoxUserMultiBlock;

        //private CheckBox checkBoxMultiBlock;

        //private MenuStrip menuStrip1;

        //private ToolStripMenuItem fileToolStripMenuItem;

        //private ToolStripMenuItem ModeToolStripMenuItem;

        //private ToolStripMenuItem SingleToolStripMenuItem;

        //private ToolStripMenuItem MultiToolStripMenuItem;

        //private ToolStripMenuItem SaveToolStripMenuItem;

        //public new event EventHandler OnClosed;

        //public FormReadWriteRequestTest()
        //{
        //    InitializeComponent();
        //}

        //public FormReadWriteRequestTest(ControlManager controlManager)
        //{
        //    InitializeComponent();
        //    m_ControlManager = controlManager;
        //    comboBoxSerachType.SelectedIndex = 0;
        //    InitDataTableColumns();
        //}

        //private bool InitDataTableColumns()
        //{
        //    try
        //    {
        //        m_dataTable.Columns.Add("colBlock");
        //        m_dataTable.Columns.Add("colItem");
        //        m_dataTable.Columns.Add("colValue");
        //        m_dataTable.Columns.Add("colRepresentation");
        //        dataGridView1.DataSource = m_dataTable;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private bool SimilarSearch(enumSearchType searchType)
        //{
        //    try
        //    {
        //        m_arrSearchResult.Clear();
        //        string value = textBoxSearchTextInput.Text.ToLower();
        //        if (m_ControlManager != null)
        //        {
        //            Dictionary<string, PLCMap>.Enumerator enumerator;
        //            KeyValuePair<string, PLCMap> current;
        //            int num;
        //            switch (searchType)
        //            {
        //            case enumSearchType.transaction:
        //                if (m_ControlManager.ProtocolProxy != null)
        //                {
        //                    enumerator = m_ControlManager.ProtocolProxy.MapList.GetEnumerator();
        //                    try
        //                    {
        //                        while (enumerator.MoveNext())
        //                        {
        //                            current = enumerator.Current;
        //                            if (current.Value.transaction != null && current.Value.transaction.Send != null && current.Value.transaction.Send.Trx != null)
        //                            {
        //                                Trx[] trx = current.Value.transaction.Send.Trx;
        //                                foreach (Trx trx2 in trx)
        //                                {
        //                                    if (("SEARCH" + trx2.Name.ToLower()).IndexOf(value) > 0)
        //                                    {
        //                                        m_arrSearchResult.Add(trx2.Name);
        //                                    }
        //                                }
        //                            }
        //                            if (current.Value.transaction != null && current.Value.transaction.Receive != null && current.Value.transaction.Receive.Trx != null)
        //                            {
        //                                Trx[] trx = current.Value.transaction.Receive.Trx;
        //                                foreach (Trx trx2 in trx)
        //                                {
        //                                    if (("SEARCH" + trx2.Name.ToLower()).IndexOf(value) > 0)
        //                                    {
        //                                        m_arrSearchResult.Add(trx2.Name);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        ((IDisposable)enumerator).Dispose();
        //                    }
        //                }
        //                else if (m_ControlManager.NetProxy != null)
        //                {
        //                    if (m_ControlManager.NetProxy.Transaction != null && m_ControlManager.NetProxy.Transaction.Send != null && m_ControlManager.NetProxy.Transaction.Send.Trx != null)
        //                    {
        //                        Trx[] trx = m_ControlManager.NetProxy.Transaction.Send.Trx;
        //                        foreach (Trx trx2 in trx)
        //                        {
        //                            if (("SEARCH" + trx2.Name.ToLower()).IndexOf(value) > 0)
        //                            {
        //                                m_arrSearchResult.Add(trx2.Name);
        //                            }
        //                        }
        //                    }
        //                    if (m_ControlManager.NetProxy.Transaction != null && m_ControlManager.NetProxy.Transaction.Receive != null && m_ControlManager.NetProxy.Transaction.Receive.Trx != null)
        //                    {
        //                        Trx[] trx = m_ControlManager.NetProxy.Transaction.Receive.Trx;
        //                        foreach (Trx trx2 in trx)
        //                        {
        //                            if (("SEARCH" + trx2.Name.ToLower()).IndexOf(value) > 0)
        //                            {
        //                                m_arrSearchResult.Add(trx2.Name);
        //                            }
        //                        }
        //                    }
        //                }
        //                break;
        //            case enumSearchType.block:
        //            {
        //                List<Block>.Enumerator enumerator2;
        //                if (m_ControlManager.ProtocolProxy != null)
        //                {
        //                    enumerator = m_ControlManager.ProtocolProxy.MapList.GetEnumerator();
        //                    try
        //                    {
        //                        while (enumerator.MoveNext())
        //                        {
        //                            current = enumerator.Current;
        //                            enumerator2 = current.Value.blockMap.Block.GetEnumerator();
        //                            try
        //                            {
        //                                while (enumerator2.MoveNext())
        //                                {
        //                                    Block current2 = enumerator2.Current;
        //                                    if (("SEARCH" + current2.Name.ToLower()).IndexOf(value) > 0)
        //                                    {
        //                                        m_arrSearchResult.Add(current2.Name);
        //                                    }
        //                                }
        //                            }
        //                            finally
        //                            {
        //                                ((IDisposable)enumerator2).Dispose();
        //                            }
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        ((IDisposable)enumerator).Dispose();
        //                    }
        //                }
        //                else if (m_ControlManager.NetProxy != null)
        //                {
        //                    enumerator2 = m_ControlManager.NetProxy.BlockMap.Block.GetEnumerator();
        //                    try
        //                    {
        //                        while (enumerator2.MoveNext())
        //                        {
        //                            Block current2 = enumerator2.Current;
        //                            if (("SEARCH" + current2.Name.ToLower()).IndexOf(value) > 0)
        //                            {
        //                                m_arrSearchResult.Add(current2.Name);
        //                            }
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        ((IDisposable)enumerator2).Dispose();
        //                    }
        //                }
        //                break;
        //            }
        //            default:
        //                num = ((searchType != enumSearchType.userdefine) ? 1 : 0);
        //                goto IL_0590;
        //            case enumSearchType.address:
        //                {
        //                    num = 0;
        //                    goto IL_0590;
        //                }
        //                IL_0590:
        //                if (num != 0 && searchType != enumSearchType.userdefine)
        //                {
        //                    break;
        //                }
        //                break;
        //            }
        //        }
        //        if (m_arrSearchResult.Count > 0)
        //        {
        //            if (m_arrSearchResult.Count > m_iMaxSimilarCount)
        //            {
        //                m_arrSearchResult.Clear();
        //            }
        //            else
        //            {
        //                panel1.Controls.Clear();
        //                panel1.AutoScroll = true;
        //                for (int j = 0; j < m_arrSearchResult.Count; j++)
        //                {
        //                    string arg = m_arrSearchResult[j] as string;
        //                    LinkLabel linkLabel = new LinkLabel();
        //                    linkLabel.Text =string.Format("{0}. {1}", j, arg);

        //                    linkLabel.Dock = DockStyle.Top;
        //                    linkLabel.Click += linkLabel_Click;
        //                    panel1.Controls.Add(linkLabel);
        //                }
        //            }
        //        }
        //        return m_arrSearchResult.Count > 0;
        //    }
        //    catch (Exception message)
        //    {
        //        logger.Error(message);
        //        return false;
        //    }
        //}

        //private void linkLabel_Click(object sender, EventArgs e)
        //{
        //    LinkLabel linkLabel = sender as LinkLabel;
        //    string[] array = linkLabel.Text.Split('.');
        //    textBoxSearchTextInput.Text = (m_arrSearchResult[Convert.ToInt32(array[0])] as string);
        //    buttonFind_Click(null, null);
        //}

        //private void buttonFind_Click(object sender, EventArgs e)
        //{
        //    panel1.Visible = false;
        //    if (string.IsNullOrEmpty(textBoxSearchTextInput.Text))
        //    {
        //        textBoxNotFound.Visible = true;
        //        textBoxNotFound.Text = "You should select the type and enter the search term";
        //        groupBox1.Text = "";
        //        RequestEnableChange(false);
        //    }
        //    else if (comboBoxSerachType.SelectedIndex == 0)
        //    {
        //        TransactionSearch();
        //    }
        //    else if (comboBoxSerachType.SelectedIndex == 1)
        //    {
        //        BlockSearch();
        //    }
        //    else if (comboBoxSerachType.SelectedIndex == 2)
        //    {
        //        AddressSearch();
        //    }
        //    else if (comboBoxSerachType.SelectedIndex == 3)
        //    {
        //        UserDefineSearch();
        //    }
        //}

        //private void TransactionSearch()
        //{
        //    Trx trx = null;
        //    if (m_ControlManager != null)
        //    {
        //        if (m_ControlManager.ProtocolProxy != null)
        //        {
        //            foreach (KeyValuePair<string, PLCMap> map in m_ControlManager.ProtocolProxy.MapList)
        //            {
        //                if (map.Value.transaction == null || map.Value.transaction.Send == null || map.Value.transaction.Send.Trx == null)
        //                {
        //                    break;
        //                }
        //                trx = (from trxName in map.Value.transaction.Send.Trx
        //                where trxName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //                select trxName).FirstOrDefault();
        //                if (trx != null)
        //                {
        //                    break;
        //                }
        //                if (map.Value.transaction == null || map.Value.transaction.Receive == null || map.Value.transaction.Receive.Trx == null)
        //                {
        //                    break;
        //                }
        //                trx = (from trxName in map.Value.transaction.Receive.Trx
        //                where trxName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //                select trxName).FirstOrDefault();
        //                if (trx != null)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        else if (m_ControlManager.NetProxy != null)
        //        {
        //            trx = (from trxName in m_ControlManager.NetProxy.Transaction.Send.Trx
        //            where trxName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //            select trxName).FirstOrDefault();
        //            if (trx == null)
        //            {
        //                trx = (from trxName in m_ControlManager.NetProxy.Transaction.Receive.Trx
        //                where trxName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //                select trxName).FirstOrDefault();
        //            }
        //        }
        //    }
        //    m_dataTable.Rows.Clear();
        //    if (trx == null)
        //    {
        //        if (!SimilarSearch(enumSearchType.transaction))
        //        {
        //            textBoxNotFound.Visible = true;
        //            textBoxNotFound.Text = string.Format("Can not found Trx [{0}]", this.textBoxSearchTextInput.Text);
        //            groupBox1.Text = "";
        //            RequestEnableChange(false);
        //        }
        //        else
        //        {
        //            panel1.Visible = true;
        //            textBoxNotFound.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        textBoxNotFound.Visible = false;
        //        groupBox1.Text = trx.Name;
        //        RequestEnableChange(true);
        //        MultiBlock[] array = new MultiBlock[trx.MultiBlock.Length + 1];
        //        for (int i = 0; i < trx.MultiBlock.Length; i++)
        //        {
        //            array[i] = trx.MultiBlock[i];
        //        }
        //        MultiBlock multiBlock = new MultiBlock();
        //        Block block = new Block();
        //        if (trx.Key.IndexOf("_W_") > 0)
        //        {
        //            block.Name = trx.Key.Substring(trx.Key.IndexOf(".") + 1, trx.Key.LastIndexOf(".") - (trx.Key.IndexOf(".") + 1));
        //        }
        //        else if (trx.Key.IndexOf("_B_") > 0)
        //        {
        //            string text = trx.Key.Substring(trx.Key.IndexOf(".") + 1, trx.Key.LastIndexOf(".") - (trx.Key.IndexOf(".") + 1));
        //            block.Name = text.Substring(0, text.IndexOf("_")) + "_B_" + trx.Key.Substring(trx.Key.LastIndexOf(".") + 1, trx.Key.Length - (trx.Key.LastIndexOf(".") + 1));
        //        }
        //        multiBlock.Block = new Block[1];
        //        multiBlock.Block[0] = block;
        //        array[trx.MultiBlock.Length] = multiBlock;
        //        MultiBlock[] array2 = array;
        //        foreach (MultiBlock multiblock in array2)
        //        {
        //            MakeRequestData(multiblock);
        //        }
        //    }
        //}

        //private void BlockSearch()
        //{
        //    Block block = null;
        //    if (m_ControlManager != null)
        //    {
        //        if (m_ControlManager.ProtocolProxy != null)
        //        {
        //            foreach (KeyValuePair<string, PLCMap> map in m_ControlManager.ProtocolProxy.MapList)
        //            {
        //                block = (from blockName in map.Value.blockMap.Block
        //                where blockName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //                select blockName).FirstOrDefault();
        //            }
        //        }
        //        else if (m_ControlManager.NetProxy != null)
        //        {
        //            block = (from blockName in m_ControlManager.NetProxy.BlockMap.Block
        //            where blockName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //            select blockName).FirstOrDefault();
        //        }
        //    }
        //    if (!checkBoxMultiBlock.Checked)
        //    {
        //        m_dataTable.Rows.Clear();
        //    }
        //    if (block == null)
        //    {
        //        if (!SimilarSearch(enumSearchType.block))
        //        {
        //            textBoxNotFound.Visible = true;
        //            textBoxNotFound.Text = string.Format("Can not found Block [{0}]",this.textBoxSearchTextInput.Text);
        //            groupBox1.Text = "";
        //            RequestEnableChange(false);
        //        }
        //        else
        //        {
        //            panel1.Visible = true;
        //            textBoxNotFound.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        bool flag = false;
        //        if (checkBoxMultiBlock.Checked)
        //        {
        //            if (!m_dicMultiBlockConfigure.ContainsKey(block.Name))
        //            {
        //                m_dicMultiBlockConfigure.Add(block.Name, block.Points);
        //            }
        //            else
        //            {
        //                flag = true;
        //            }
        //        }
        //        textBoxNotFound.Visible = false;
        //        groupBox1.Text = block.Name;
        //        RequestEnableChange(true);
        //        if (!flag)
        //        {
        //            MultiBlock multiBlock = new MultiBlock();
        //            multiBlock.Block = new Block[1];
        //            multiBlock.Block[0] = block;
        //            MakeRequestData(multiBlock);
        //        }
        //    }
        //}

        //private void AddressSearch()
        //{
        //    bool flag = false;
        //    string text = string.Empty;
        //    string text2 = string.Empty;
        //    if (textBoxSearchTextInput.Text.Length < 2)
        //    {
        //        flag = true;
        //    }
        //    else
        //    {
        //        text = textBoxSearchTextInput.Text.ToLower().Substring(0, 1);
        //        text2 = textBoxSearchTextInput.Text.ToLower().Substring(1, textBoxSearchTextInput.Text.Length - 1);
        //        int num;
        //        switch (text)
        //        {
        //        default:
        //            num = ((text == "m") ? 1 : 0);
        //            break;
        //        case "b":
        //        case "w":
        //        case "r":
        //        case "d":
        //            num = 1;
        //            break;
        //        }
        //        if (num == 0)
        //        {
        //            flag = true;
        //        }
        //    }
        //    m_dataTable.Rows.Clear();
        //    if (flag)
        //    {
        //        textBoxNotFound.Visible = true;
        //        textBoxNotFound.Text = "Address Search Ipnut Example : B0 (DeviceCode+Address)";
        //        groupBox1.Text = "";
        //        RequestEnableChange(false);
        //    }
        //    else
        //    {
        //        Block block = null;
        //        string empty = string.Empty;
        //        if (m_ControlManager != null)
        //        {
        //            List<Block>.Enumerator enumerator2;
        //            if (m_ControlManager.ProtocolProxy != null)
        //            {
        //                foreach (KeyValuePair<string, PLCMap> map in m_ControlManager.ProtocolProxy.MapList)
        //                {
        //                    enumerator2 = map.Value.blockMap.Block.GetEnumerator();
        //                    try
        //                    {
        //                        while (enumerator2.MoveNext())
        //                        {
        //                            Block current = enumerator2.Current;
        //                            if (current.DeviceCode.ToLower() == text)
        //                            {
        //                                empty = ((current.HeadDevice.ToLower().Length > 6) ? current.HeadDevice.ToLower().Substring(current.HeadDevice.ToLower().Length - 6, 6) : ((current.HeadDevice.ToLower().Length >= 6) ? current.HeadDevice.ToLower() : current.HeadDevice.ToLower().PadLeft(6, '0')));
        //                                if (empty == text2.PadLeft(6, '0'))
        //                                {
        //                                    block = current;
        //                                    break;
        //                                }
        //                                int num2 = 0;
        //                                int num3 = 0;
        //                                if (current.DeviceCode == "R" || current.DeviceCode == "ZR" || current.DeviceCode == "M" || current.DeviceCode == "D")
        //                                {
        //                                    num2 = Convert.ToInt32(empty);
        //                                    num3 = num2 + Convert.ToInt32(current.Points) - 1;
        //                                }
        //                                else
        //                                {
        //                                    num2 = Convert.ToInt32(empty, 16);
        //                                    num3 = num2 + Convert.ToInt32(current.Points) - 1;
        //                                }
        //                                if (text == "R" || text == "ZR" || current.DeviceCode == "M" || current.DeviceCode == "D")
        //                                {
        //                                    if (num2 <= Convert.ToInt32(text2) && Convert.ToInt32(text2) <= num3)
        //                                    {
        //                                        block = current;
        //                                        break;
        //                                    }
        //                                }
        //                                else if (num2 <= Convert.ToInt32(text2, 16) && Convert.ToInt32(text2, 16) <= num3)
        //                                {
        //                                    block = current;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        ((IDisposable)enumerator2).Dispose();
        //                    }
        //                }
        //            }
        //            else if (m_ControlManager.NetProxy != null)
        //            {
        //                enumerator2 = m_ControlManager.NetProxy.BlockMap.Block.GetEnumerator();
        //                try
        //                {
        //                    while (enumerator2.MoveNext())
        //                    {
        //                        Block current = enumerator2.Current;
        //                        empty = ((current.HeadDevice.ToLower().Length > 6) ? current.HeadDevice.ToLower().Substring(current.HeadDevice.ToLower().Length - 6, 6) : ((current.HeadDevice.ToLower().Length >= 6) ? current.HeadDevice.ToLower() : current.HeadDevice.ToLower().PadLeft(6, '0')));
        //                        if (empty == text2.PadLeft(6, '0'))
        //                        {
        //                            block = current;
        //                            break;
        //                        }
        //                    }
        //                }
        //                finally
        //                {
        //                    ((IDisposable)enumerator2).Dispose();
        //                }
        //            }
        //            if (block == null)
        //            {
        //                textBoxNotFound.Visible = true;
        //                textBoxNotFound.Text = string.Format("Can not found Address [{0}]",textBoxSearchTextInput.Text);
        //                groupBox1.Text = "";
        //                RequestEnableChange(false);
        //            }
        //            else
        //            {
        //                bool flag2 = false;
        //                if (checkBoxMultiBlock.Checked)
        //                {
        //                    if (!m_dicMultiBlockConfigure.ContainsKey(block.Name))
        //                    {
        //                        m_dicMultiBlockConfigure.Add(block.Name, block.Points);
        //                    }
        //                    else
        //                    {
        //                        flag2 = true;
        //                    }
        //                }
        //                textBoxNotFound.Visible = false;
        //                groupBox1.Text = block.Name;
        //                RequestEnableChange(true);
        //                if (!flag2)
        //                {
        //                    MultiBlock multiBlock = new MultiBlock();
        //                    multiBlock.Block = new Block[1];
        //                    multiBlock.Block[0] = block;
        //                    MakeRequestData(multiBlock);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void UserDefineSearch()
        //{
        //    Block block = null;
        //    if (m_ControlManager != null)
        //    {
        //        if (m_ControlManager.ProtocolProxy != null)
        //        {
        //            foreach (KeyValuePair<string, PLCMap> map in m_ControlManager.ProtocolProxy.MapList)
        //            {
        //                block = (from blockName in map.Value.blockMap.Block
        //                where blockName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //                select blockName).FirstOrDefault();
        //            }
        //        }
        //        else if (m_ControlManager.NetProxy != null)
        //        {
        //            block = (from blockName in m_ControlManager.NetProxy.BlockMap.Block
        //            where blockName.Name.ToLower() == textBoxSearchTextInput.Text.ToLower()
        //            select blockName).FirstOrDefault();
        //        }
        //    }
        //    if (block != null)
        //    {
        //        textBoxNotFound.Visible = false;
        //        groupBox1.Text = block.Name;
        //        RequestEnableChange(true);
        //        MultiBlock multiBlock = new MultiBlock();
        //        multiBlock.Block = new Block[1];
        //        multiBlock.Block[0] = block;
        //        MakeRequestData(multiBlock);
        //    }
        //}

        //private void MakeRequestData(MultiBlock multiblock)
        //{
        //    if (multiblock != null && multiblock.Block != null)
        //    {
        //        Block block2 = null;
        //        Block[] block3 = multiblock.Block;
        //        Block block;
        //        for (int i = 0; i < block3.Length; i++)
        //        {
        //            block = block3[i];
        //            block2 = null;
        //            if (m_ControlManager.ProtocolProxy != null)
        //            {
        //                foreach (KeyValuePair<string, PLCMap> map in m_ControlManager.ProtocolProxy.MapList)
        //                {
        //                    block2 = (from blockName in map.Value.blockMap.Block
        //                    where blockName.Name == block.Name
        //                    select blockName).FirstOrDefault();
        //                    if (block2 != null)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            else if (m_ControlManager.NetProxy != null)
        //            {
        //                block2 = (from blockName in m_ControlManager.NetProxy.BlockMap.Block
        //                where blockName.Name == block.Name
        //                select blockName).FirstOrDefault();
        //            }
        //            if (block2 != null)
        //            {
        //                m_dataTable.Rows.Add(block2.Name, "", "", "");
        //                string str = string.Empty;
        //                Item[] item = block2.Item;
        //                foreach (Item item2 in item)
        //                {
        //                    switch (item2.Representation)
        //                    {
        //                    case "A":
        //                        str = " (ASCII)";
        //                        break;
        //                    case "I":
        //                        str = " (INTERGER)";
        //                        break;
        //                    case "B":
        //                        str = " (BIT) ";
        //                        break;
        //                    case "H":
        //                        str = " (Hex)";
        //                        break;
        //                    }
        //                    m_dataTable.Rows.Add("", item2.Name, "", item2.Representation + str);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void RequestEnableChange(bool enable)
        //{
        //    radioButtonRead.Enabled = enable;
        //    radioButtonWrite.Enabled = enable;
        //    buttonRequest.Enabled = enable;
        //}

        //private void buttonRequest_Click(object sender, EventArgs e)
        //{
        //    MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
        //    PLCMessageBody pLCMessageBody = new PLCMessageBody();
        //    Dictionary<string, string> dictionary = new Dictionary<string, string>();
        //    string key = string.Empty;
        //    string empty = string.Empty;
        //    string empty2 = string.Empty;
        //    for (int i = 0; i < m_dataTable.Rows.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(m_dataTable.Rows[i]["colBlock"].ToString()))
        //        {
        //            key = m_dataTable.Rows[i]["colBlock"].ToString();
        //            if (radioButtonRead.Checked)
        //            {
        //                pLCMessageBody.ReadDataList.Add(key, new Dictionary<string, string>());
        //            }
        //            else if (radioButtonWrite.Checked)
        //            {
        //                pLCMessageBody.WriteDataList.Add(key, new Dictionary<string, string>());
        //                dictionary = new Dictionary<string, string>();
        //            }
        //        }
        //        else if (!string.IsNullOrEmpty(m_dataTable.Rows[i]["colItem"].ToString()) && !radioButtonRead.Checked)
        //        {
        //            empty = m_dataTable.Rows[i]["colItem"].ToString();
        //            empty2 = m_dataTable.Rows[i]["colValue"].ToString();
        //            dictionary.Add(empty, empty2);
        //            pLCMessageBody.WriteDataList[key] = dictionary;
        //        }
        //    }
        //    messageData.MessageBody = pLCMessageBody;
        //    messageData.MessageType = "MNet";
        //    messageData.MachineName = "Virtual";
        //    messageData.Transaction = "11";
        //    if (m_ControlManager != null)
        //    {
        //        if (radioButtonRead.Checked)
        //        {
        //            messageData.MessageName = "ReadRequest";
        //            if (m_ControlManager.ProtocolProxy != null)
        //            {
        //                messageData = m_ControlManager.VirtualMQRequestReceived(messageData);
        //            }
        //            else if (m_ControlManager.NetProxy != null)
        //            {
        //                messageData = m_ControlManager.NetProxy.ReadData(messageData);
        //            }
        //            FillReadData(messageData);
        //        }
        //        else if (radioButtonWrite.Checked)
        //        {
        //            messageData.MessageName = "WriteRequest";
        //            if (m_ControlManager.ProtocolProxy != null)
        //            {
        //                m_ControlManager.VirtualMQRequestReceived(messageData);
        //            }
        //            else if (m_ControlManager.NetProxy != null)
        //            {
        //                m_ControlManager.NetProxy.WriteData(messageData);
        //            }
        //        }
        //    }
        //}

        //private void FillReadData(MessageData<PLCMessageBody> message)
        //{
        //    string key = string.Empty;
        //    string empty = string.Empty;
        //    string empty2 = string.Empty;
        //    for (int i = 0; i < m_dataTable.Rows.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(m_dataTable.Rows[i]["colBlock"].ToString()))
        //        {
        //            key = m_dataTable.Rows[i]["colBlock"].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(m_dataTable.Rows[i]["colItem"].ToString()))
        //        {
        //            empty = m_dataTable.Rows[i]["colItem"].ToString();
        //            if (message.MessageBody.ReadDataList.ContainsKey(key) && message.MessageBody.ReadDataList[key].ContainsKey(empty))
        //            {
        //                m_dataTable.Rows[i]["colValue"] = message.MessageBody.ReadDataList[key][empty];
        //            }
        //        }
        //    }
        //}

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(m_dataTable.Rows[e.RowIndex]["colBlock"].ToString()))
        //    {
        //        dataGridView1.Rows[e.RowIndex].ReadOnly = true;
        //        e.CellStyle.BackColor = Color.YellowGreen;
        //    }
        //}

        //private void textBoxSearchTextInput_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Return)
        //    {
        //        buttonFind_Click(sender, e);
        //    }
        //}

        //protected override void OnFormClosed(FormClosedEventArgs e)
        //{
        //    this.OnClosed(this, null);
        //    base.OnFormClosed(e);
        //}

        //private void checkBoxMultiBlock_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!checkBoxMultiBlock.Checked)
        //    {
        //        m_dicMultiBlockConfigure.Clear();
        //    }
        //}

        //private void comboBoxSearchType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    textBoxSearchTextInput.Text = string.Empty;
        //    m_dataTable.Rows.Clear();
        //    m_dicUserDefine.Clear();
        //    comboBoxUserMultiBlock.SelectedIndex = -1;
        //    RequestEnableChange(false);
        //    if (comboBoxSerachType.SelectedIndex == 0 || comboBoxSerachType.SelectedIndex == 1 || comboBoxSerachType.SelectedIndex == 2)
        //    {
        //        textBoxSearchTextInput.Visible = true;
        //        comboBoxUserMultiBlock.Visible = false;
        //        buttonFind.Enabled = true;
        //    }
        //    else if (comboBoxSerachType.SelectedIndex == 3)
        //    {
        //        textBoxSearchTextInput.Visible = false;
        //        comboBoxUserMultiBlock.Visible = true;
        //        comboBoxUserMultiBlock.Items.Clear();
        //        buttonFind.Enabled = false;
        //        string[] files = Directory.GetFiles("..\\EQPIO\\Config\\LinkSignal\\UserDefine\\");
        //        UserDefine userDefine = null;
        //        string[] array = files;
        //        foreach (string text in array)
        //        {
        //            userDefine = ReadUserDefineFile(text);
        //            if (userDefine != null)
        //            {
        //                m_dicUserDefine.Add(text.Substring(text.LastIndexOf("\\") + 1, text.Substring(text.LastIndexOf("\\") + 1).Length - 4), userDefine);
        //                comboBoxUserMultiBlock.Items.Add(text.Substring(text.LastIndexOf("\\") + 1, text.Substring(text.LastIndexOf("\\") + 1).Length - 4));
        //            }
        //        }
        //    }
        //}

        //private UserDefine ReadUserDefineFile(string strOpenFileName)
        //{
        //    UserDefine result = null;
        //    XmlDocument xmlDocument = new XmlDocument();
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserDefine));
        //    XmlNode xmlNode = null;
        //    if (File.Exists(strOpenFileName))
        //    {
        //        xmlDocument.Load(strOpenFileName);
        //        xmlNode = xmlDocument.SelectSingleNode("UserDefine");
        //        result = (UserDefine)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
        //    }
        //    return result;
        //}

        //private void comboBoxUserMultiBlock_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    m_dataTable.Rows.Clear();
        //    if (m_dicUserDefine.ContainsKey(comboBoxUserMultiBlock.Text))
        //    {
        //        UserDefine userDefine = m_dicUserDefine[comboBoxUserMultiBlock.Text];
        //        Block[] block = userDefine.BlockGroup.Block;
        //        foreach (Block block2 in block)
        //        {
        //            textBoxSearchTextInput.Text = block2.Name;
        //            buttonFind_Click(null, null);
        //        }
        //    }
        //}

        //private void SingleToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SingleToolStripMenuItem.Checked = true;
        //    MultiToolStripMenuItem.Checked = false;
        //    checkBoxMultiBlock.Checked = false;
        //    comboBoxSerachType.Enabled = true;
        //    SaveToolStripMenuItem.Visible = false;
        //}

        //private void MultiToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SingleToolStripMenuItem.Checked = false;
        //    MultiToolStripMenuItem.Checked = true;
        //    checkBoxMultiBlock.Checked = true;
        //    comboBoxSerachType.SelectedIndex = 1;
        //    comboBoxSerachType.Enabled = false;
        //    SaveToolStripMenuItem.Visible = true;
        //}

        //private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (m_dicMultiBlockConfigure.Count > 0)
        //    {
        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.LoadXml("<UserDefine></UserDefine>");
        //        XmlNode xmlNode = xmlDocument.SelectSingleNode("UserDefine");
        //        XmlElement documentElement = xmlDocument.DocumentElement;
        //        XmlElement xmlElement = null;
        //        XmlElement xmlEle = null;
        //        XmlAttribute xmlAttr = null;
        //        string text = string.Empty;
        //        try
        //        {
        //            SaveFileDialog saveFileDialog = new SaveFileDialog();
        //            saveFileDialog.InitialDirectory = Path.GetFullPath(Application.StartupPath + "\\..\\EQPIO\\Config\\LinkSignal\\UserDefine\\");
        //            saveFileDialog.DefaultExt = "xml";
        //            saveFileDialog.Filter = "*.xml|*.xml";
        //            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
        //            {
        //                text = saveFileDialog.FileName;
        //            }
        //            if (!string.IsNullOrEmpty(text))
        //            {
        //                xmlElement = xmlDocument.CreateElement("BlockGroup");
        //                foreach (KeyValuePair<string, int> item in m_dicMultiBlockConfigure)
        //                {
        //                    xmlEle = xmlDocument.CreateElement("Block");
        //                    xmlAttr = xmlDocument.CreateAttribute("Name");
        //                    XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, item.Key.ToString());
        //                    xmlElement.AppendChild(xmlEle);
        //                }
        //                documentElement.AppendChild(xmlElement);
        //                xmlDocument.Save(text);
        //            }
        //        }
        //        catch (Exception message)
        //        {
        //            logger.Error(message);
        //        }
        //    }
        //}

        //private void XmlMaker(XmlDocument docConnect, ref XmlElement xmlEle, ref XmlAttribute xmlAttr, string value)
        //{
        //    try
        //    {
        //        xmlAttr.Value = value;
        //        xmlEle.Attributes.Append(xmlAttr);
        //    }
        //    catch
        //    {
        //    }
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && components != null)
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private void InitializeComponent()
        //{
        //    groupBox1 = new System.Windows.Forms.GroupBox();
        //    checkBoxMultiBlock = new System.Windows.Forms.CheckBox();
        //    panel1 = new System.Windows.Forms.Panel();
        //    textBoxNotFound = new System.Windows.Forms.TextBox();
        //    dataGridView1 = new System.Windows.Forms.DataGridView();
        //    colBlock = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    colRepresentation = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    radioButtonWrite = new System.Windows.Forms.RadioButton();
        //    buttonRequest = new System.Windows.Forms.Button();
        //    radioButtonRead = new System.Windows.Forms.RadioButton();
        //    textBoxSearchTextInput = new System.Windows.Forms.TextBox();
        //    buttonFind = new System.Windows.Forms.Button();
        //    comboBoxSerachType = new System.Windows.Forms.ComboBox();
        //    comboBoxUserMultiBlock = new System.Windows.Forms.ComboBox();
        //    menuStrip1 = new System.Windows.Forms.MenuStrip();
        //    fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        //    ModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        //    SingleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        //    MultiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        //    SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        //    groupBox1.SuspendLayout();
        //    ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        //    menuStrip1.SuspendLayout();
        //    SuspendLayout();
        //    groupBox1.Controls.Add(checkBoxMultiBlock);
        //    groupBox1.Controls.Add(panel1);
        //    groupBox1.Controls.Add(textBoxNotFound);
        //    groupBox1.Controls.Add(dataGridView1);
        //    groupBox1.Controls.Add(radioButtonWrite);
        //    groupBox1.Controls.Add(buttonRequest);
        //    groupBox1.Controls.Add(radioButtonRead);
        //    groupBox1.Location = new System.Drawing.Point(12, 70);
        //    groupBox1.Name = "groupBox1";
        //    groupBox1.Size = new System.Drawing.Size(725, 348);
        //    groupBox1.TabIndex = 0;
        //    groupBox1.TabStop = false;
        //    groupBox1.Text = "Ready...";
        //    checkBoxMultiBlock.AutoSize = true;
        //    checkBoxMultiBlock.Location = new System.Drawing.Point(600, 313);
        //    checkBoxMultiBlock.Name = "checkBoxMultiBlock";
        //    checkBoxMultiBlock.Size = new System.Drawing.Size(123, 16);
        //    checkBoxMultiBlock.TabIndex = 7;
        //    checkBoxMultiBlock.Text = "MultiSearchMode";
        //    checkBoxMultiBlock.UseVisualStyleBackColor = true;
        //    checkBoxMultiBlock.Visible = false;
        //    checkBoxMultiBlock.CheckedChanged += new System.EventHandler(checkBoxMultiBlock_CheckedChanged);
        //    panel1.AutoScroll = true;
        //    panel1.BackColor = System.Drawing.Color.White;
        //    panel1.Location = new System.Drawing.Point(93, 45);
        //    panel1.Name = "panel1";
        //    panel1.Size = new System.Drawing.Size(539, 194);
        //    panel1.TabIndex = 6;
        //    panel1.Visible = false;
        //    textBoxNotFound.BackColor = System.Drawing.Color.Red;
        //    textBoxNotFound.Font = new System.Drawing.Font("굴림", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
        //    textBoxNotFound.ForeColor = System.Drawing.Color.White;
        //    textBoxNotFound.Location = new System.Drawing.Point(138, 45);
        //    textBoxNotFound.Name = "textBoxNotFound";
        //    textBoxNotFound.ReadOnly = true;
        //    textBoxNotFound.Size = new System.Drawing.Size(449, 21);
        //    textBoxNotFound.TabIndex = 5;
        //    textBoxNotFound.Visible = false;
        //    dataGridView1.AllowUserToAddRows = false;
        //    dataGridView1.AllowUserToResizeColumns = false;
        //    dataGridView1.AllowUserToResizeRows = false;
        //    dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        //    dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    dataGridView1.Columns.AddRange(colBlock, colItem, colValue, colRepresentation);
        //    dataGridView1.Location = new System.Drawing.Point(6, 14);
        //    dataGridView1.MultiSelect = false;
        //    dataGridView1.Name = "dataGridView1";
        //    dataGridView1.RowHeadersVisible = false;
        //    dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
        //    dataGridView1.RowTemplate.Height = 23;
        //    dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        //    dataGridView1.Size = new System.Drawing.Size(712, 289);
        //    dataGridView1.TabIndex = 1;
        //    dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
        //    colBlock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        //    colBlock.DataPropertyName = "colBlock";
        //    colBlock.FillWeight = 40f;
        //    colBlock.HeaderText = "Block";
        //    colBlock.Name = "colBlock";
        //    colBlock.ReadOnly = true;
        //    colBlock.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        //    colBlock.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
        //    colItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        //    colItem.DataPropertyName = "colItem";
        //    colItem.FillWeight = 40f;
        //    colItem.HeaderText = "Item";
        //    colItem.Name = "colItem";
        //    colItem.ReadOnly = true;
        //    colItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        //    colItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
        //    colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        //    colValue.DataPropertyName = "colValue";
        //    colValue.FillWeight = 20f;
        //    colValue.HeaderText = "Value";
        //    colValue.Name = "colValue";
        //    colValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        //    colValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
        //    colRepresentation.DataPropertyName = "colRepresentation";
        //    colRepresentation.HeaderText = "Representation";
        //    colRepresentation.Name = "colRepresentation";
        //    colRepresentation.ReadOnly = true;
        //    colRepresentation.Width = 114;
        //    radioButtonWrite.AutoSize = true;
        //    radioButtonWrite.Checked = true;
        //    radioButtonWrite.Enabled = false;
        //    radioButtonWrite.Location = new System.Drawing.Point(326, 312);
        //    radioButtonWrite.Name = "radioButtonWrite";
        //    radioButtonWrite.Size = new System.Drawing.Size(50, 16);
        //    radioButtonWrite.TabIndex = 4;
        //    radioButtonWrite.TabStop = true;
        //    radioButtonWrite.Text = "Write";
        //    radioButtonWrite.UseVisualStyleBackColor = true;
        //    buttonRequest.Enabled = false;
        //    buttonRequest.Location = new System.Drawing.Point(381, 309);
        //    buttonRequest.Name = "buttonRequest";
        //    buttonRequest.Size = new System.Drawing.Size(75, 23);
        //    buttonRequest.TabIndex = 0;
        //    buttonRequest.Text = "Request";
        //    buttonRequest.UseVisualStyleBackColor = true;
        //    buttonRequest.Click += new System.EventHandler(buttonRequest_Click);
        //    radioButtonRead.AutoSize = true;
        //    radioButtonRead.Enabled = false;
        //    radioButtonRead.Location = new System.Drawing.Point(268, 312);
        //    radioButtonRead.Name = "radioButtonRead";
        //    radioButtonRead.Size = new System.Drawing.Size(52, 16);
        //    radioButtonRead.TabIndex = 3;
        //    radioButtonRead.Text = "Read";
        //    radioButtonRead.UseVisualStyleBackColor = true;
        //    textBoxSearchTextInput.Location = new System.Drawing.Point(224, 38);
        //    textBoxSearchTextInput.Name = "textBoxSearchTextInput";
        //    textBoxSearchTextInput.Size = new System.Drawing.Size(379, 21);
        //    textBoxSearchTextInput.TabIndex = 1;
        //    textBoxSearchTextInput.KeyDown += new System.Windows.Forms.KeyEventHandler(textBoxSearchTextInput_KeyDown);
        //    buttonFind.Location = new System.Drawing.Point(609, 37);
        //    buttonFind.Name = "buttonFind";
        //    buttonFind.Size = new System.Drawing.Size(121, 23);
        //    buttonFind.TabIndex = 2;
        //    buttonFind.Text = "Find";
        //    buttonFind.UseVisualStyleBackColor = true;
        //    buttonFind.Click += new System.EventHandler(buttonFind_Click);
        //    comboBoxSerachType.FormattingEnabled = true;
        //    comboBoxSerachType.Items.AddRange(new object[4]
        //    {
        //        "TRANSACTION",
        //        "BLOCK",
        //        "ADDRESS",
        //        "USERDEFINE(MULTIBLOCK)"
        //    });
        //    comboBoxSerachType.Location = new System.Drawing.Point(18, 38);
        //    comboBoxSerachType.Name = "comboBoxSerachType";
        //    comboBoxSerachType.Size = new System.Drawing.Size(200, 20);
        //    comboBoxSerachType.TabIndex = 5;
        //    comboBoxSerachType.SelectedIndexChanged += new System.EventHandler(comboBoxSearchType_SelectedIndexChanged);
        //    comboBoxUserMultiBlock.FormattingEnabled = true;
        //    comboBoxUserMultiBlock.Location = new System.Drawing.Point(225, 37);
        //    comboBoxUserMultiBlock.Name = "comboBoxUserMultiBlock";
        //    comboBoxUserMultiBlock.Size = new System.Drawing.Size(378, 20);
        //    comboBoxUserMultiBlock.TabIndex = 6;
        //    comboBoxUserMultiBlock.Visible = false;
        //    comboBoxUserMultiBlock.SelectedIndexChanged += new System.EventHandler(comboBoxUserMultiBlock_SelectedIndexChanged);
        //    menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
        //    {
        //        fileToolStripMenuItem,
        //        ModeToolStripMenuItem
        //    });
        //    menuStrip1.Location = new System.Drawing.Point(0, 0);
        //    menuStrip1.Name = "menuStrip1";
        //    menuStrip1.Size = new System.Drawing.Size(749, 24);
        //    menuStrip1.TabIndex = 7;
        //    menuStrip1.Text = "menuStrip1";
        //    fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        //    fileToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
        //    fileToolStripMenuItem.Text = "File (&F)";
        //    ModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
        //    {
        //        SingleToolStripMenuItem,
        //        MultiToolStripMenuItem,
        //        SaveToolStripMenuItem
        //    });
        //    ModeToolStripMenuItem.Name = "ModeToolStripMenuItem";
        //    ModeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
        //    ModeToolStripMenuItem.Text = "Mode";
        //    SingleToolStripMenuItem.Checked = true;
        //    SingleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        //    SingleToolStripMenuItem.Name = "SingleToolStripMenuItem";
        //    SingleToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        //    SingleToolStripMenuItem.Text = "Single";
        //    SingleToolStripMenuItem.Click += new System.EventHandler(SingleToolStripMenuItem_Click);
        //    MultiToolStripMenuItem.Name = "MultiToolStripMenuItem";
        //    MultiToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        //    MultiToolStripMenuItem.Text = "Multi";
        //    MultiToolStripMenuItem.Click += new System.EventHandler(MultiToolStripMenuItem_Click);
        //    SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
        //    SaveToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        //    SaveToolStripMenuItem.Text = "Save";
        //    SaveToolStripMenuItem.Visible = false;
        //    SaveToolStripMenuItem.Click += new System.EventHandler(SaveToolStripMenuItem_Click);
        //    base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
        //    base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    base.ClientSize = new System.Drawing.Size(749, 430);
        //    base.Controls.Add(comboBoxUserMultiBlock);
        //    base.Controls.Add(comboBoxSerachType);
        //    base.Controls.Add(buttonFind);
        //    base.Controls.Add(textBoxSearchTextInput);
        //    base.Controls.Add(groupBox1);
        //    base.Controls.Add(menuStrip1);
        //    base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        //    base.MainMenuStrip = menuStrip1;
        //    base.MaximizeBox = false;
        //    base.MinimizeBox = false;
        //    base.Name = "FormReadWriteRequestTest";
        //    base.ShowIcon = false;
        //    Text = "Read/Write Request";
        //    groupBox1.ResumeLayout(false);
        //    groupBox1.PerformLayout();
        //    ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        //    menuStrip1.ResumeLayout(false);
        //    menuStrip1.PerformLayout();
        //    ResumeLayout(false);
        //    PerformLayout();
        //}
	}
}
