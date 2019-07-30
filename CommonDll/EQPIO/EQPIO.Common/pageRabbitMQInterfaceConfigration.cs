using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class pageRabbitMQInterfaceConfigration // : UserControl
	{
        //private IContainer components = null;

        //private DataGridView dataGridView;

        //private readonly string m_strConfigPath = "..\\EQPIO\\Config\\MQ\\EQInterface.xml";

        //private Objects mqConfig;

        //private DataTable m_dataTable = new DataTable();

        //private bool m_bSuccessConfigtration = false;

        //public bool SuccessConfigtration ;

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
        //    dataGridView = new System.Windows.Forms.DataGridView();
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
        //    SuspendLayout();
        //    dataGridView.AllowUserToResizeColumns = false;
        //    dataGridView.AllowUserToResizeRows = false;
        //    dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
        //    dataGridView.Location = new System.Drawing.Point(0, 0);
        //    dataGridView.MultiSelect = false;
        //    dataGridView.Name = "dataGridView";
        //    dataGridView.RowTemplate.Height = 23;
        //    dataGridView.Size = new System.Drawing.Size(570, 420);
        //    dataGridView.TabIndex = 0;
        //    base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
        //    base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    base.Controls.Add(dataGridView);
        //    base.Name = "pageRabbitMQInterfaceConfigration";
        //    base.Size = new System.Drawing.Size(570, 420);
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        //    ResumeLayout(false);
        //}

        //public pageRabbitMQInterfaceConfigration()
        //{
        //    InitializeComponent();
        //    if (InitDataGridViewColumn() && InitDataTableColumns())
        //    {
        //        dataGridView.DataSource = m_dataTable;
        //    }
        //    LoadConfigtration();
        //}

        //private bool InitDataTableColumns()
        //{
        //    try
        //    {
        //        m_dataTable.Columns.Add("colMessageType");
        //        m_dataTable.Columns.Add("colHostName");
        //        m_dataTable.Columns.Add("colHostIP");
        //        m_dataTable.Columns.Add("colProducerExchange");
        //        m_dataTable.Columns.Add("colProducerRoutingKey");
        //        m_dataTable.Columns.Add("colConsumerExchange");
        //        m_dataTable.Columns.Add("colConsumerRoutingKey");
        //        m_dataTable.Columns.Add("colConsumerQueue");
        //        m_dataTable.Columns.Add("colValue");
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private bool InitDataGridViewColumn()
        //{
        //    try
        //    {
        //        dataGridView.Dock = DockStyle.Fill;
        //        dataGridView.AllowUserToAddRows = false;
        //        dataGridView.RowHeadersVisible = false;
        //        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        dataGridView.ScrollBars = ScrollBars.None;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn.DataPropertyName = "colMessageType";
        //        dataGridViewTextBoxColumn.HeaderText = "MessageType";
        //        dataGridViewTextBoxColumn.Name = dataGridViewTextBoxColumn.DataPropertyName;
        //        dataGridViewTextBoxColumn.ReadOnly = true;
        //        dataGridViewTextBoxColumn.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn2.DataPropertyName = "colHostName";
        //        dataGridViewTextBoxColumn2.HeaderText = "HostName";
        //        dataGridViewTextBoxColumn2.Name = dataGridViewTextBoxColumn2.DataPropertyName;
        //        dataGridViewTextBoxColumn2.ReadOnly = true;
        //        dataGridViewTextBoxColumn2.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn3.DataPropertyName = "colHostIP";
        //        dataGridViewTextBoxColumn3.HeaderText = "HostIPColumn";
        //        dataGridViewTextBoxColumn3.Name = dataGridViewTextBoxColumn3.DataPropertyName;
        //        dataGridViewTextBoxColumn3.ReadOnly = true;
        //        dataGridViewTextBoxColumn3.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn4.DataPropertyName = "colProducerExchange";
        //        dataGridViewTextBoxColumn4.HeaderText = "ProducerExchange";
        //        dataGridViewTextBoxColumn4.Name = dataGridViewTextBoxColumn4.DataPropertyName;
        //        dataGridViewTextBoxColumn4.ReadOnly = true;
        //        dataGridViewTextBoxColumn4.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn5.DataPropertyName = "colProducerRoutingKey";
        //        dataGridViewTextBoxColumn5.HeaderText = "ProducerRoutingKey";
        //        dataGridViewTextBoxColumn5.Name = dataGridViewTextBoxColumn5.DataPropertyName;
        //        dataGridViewTextBoxColumn5.ReadOnly = true;
        //        dataGridViewTextBoxColumn5.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn6.DataPropertyName = "colConsumerExchange";
        //        dataGridViewTextBoxColumn6.HeaderText = "ConsumerExchange";
        //        dataGridViewTextBoxColumn6.Name = dataGridViewTextBoxColumn6.DataPropertyName;
        //        dataGridViewTextBoxColumn6.ReadOnly = true;
        //        dataGridViewTextBoxColumn6.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn7.DataPropertyName = "colConsumerRoutingKey";
        //        dataGridViewTextBoxColumn7.HeaderText = "ConsumerRoutingKey";
        //        dataGridViewTextBoxColumn7.Name = dataGridViewTextBoxColumn7.DataPropertyName;
        //        dataGridViewTextBoxColumn7.ReadOnly = true;
        //        dataGridViewTextBoxColumn7.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn8.DataPropertyName = "colConsumerQueue";
        //        dataGridViewTextBoxColumn8.HeaderText = "ConsumerQueue";
        //        dataGridViewTextBoxColumn8.Name = dataGridViewTextBoxColumn8.DataPropertyName;
        //        dataGridViewTextBoxColumn8.ReadOnly = true;
        //        dataGridViewTextBoxColumn8.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn9.DataPropertyName = "colValue";
        //        dataGridViewTextBoxColumn9.HeaderText = "Value";
        //        dataGridViewTextBoxColumn9.Name = dataGridViewTextBoxColumn9.DataPropertyName;
        //        dataGridViewTextBoxColumn9.ReadOnly = true;
        //        dataGridViewTextBoxColumn9.Visible = true;
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn2);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn3);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn4);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn5);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn6);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn7);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn8);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn9);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private void LoadConfigtration()
        //{
        //    if (File.Exists(m_strConfigPath))
        //    {
        //        try
        //        {
        //            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Objects));
        //            XmlDocument xmlDocument = new XmlDocument();
        //            xmlDocument.Load(m_strConfigPath);
        //            XmlNode xmlNode = xmlDocument.SelectSingleNode("Objects");
        //            mqConfig = (Objects)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
        //            if (mqConfig == null)
        //            {
        //                return;
        //            }
        //            Property[] property = mqConfig.Property;
        //            foreach (Property property2 in property)
        //            {
        //                m_dataTable.Rows.Add(property2.MessageType, property2.HostName, property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, property2.Value);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        m_bSuccessConfigtration = true;
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

        //public void Save()
        //{
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml("<Objects></Objects>");
        //    XmlNode xmlNode = xmlDocument.SelectSingleNode("Objects");
        //    XmlElement documentElement = xmlDocument.DocumentElement;
        //    XmlElement xmlEle = null;
        //    XmlAttribute xmlAttr = null;
        //    try
        //    {
        //        if (m_dataTable.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < m_dataTable.Rows.Count; i++)
        //            {
        //                xmlEle = xmlDocument.CreateElement("Property");
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.MessageType.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][0].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.HostName.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][1].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.HostIP.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][2].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.ProducerExchange.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][3].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.ProducerRoutingKey.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][4].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.ConsumerExchange.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][5].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.ConsumerRoutingKey.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][6].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.ConsumerQueue.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][7].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.Value.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][8].ToString().ToLower());
        //                documentElement.AppendChild(xmlEle);
        //            }
        //            xmlDocument.Save(m_strConfigPath);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //public bool AddRow()
        //{
        //    try
        //    {
        //        if (m_dataTable != null)
        //        {
        //            FrmRabbitMQInterface frmRabbitMQInterface = new FrmRabbitMQInterface();
        //            if (frmRabbitMQInterface.ShowDialog() == DialogResult.OK)
        //            {
        //                m_dataTable.Rows.Add(frmRabbitMQInterface.MessageType, frmRabbitMQInterface.HostName, frmRabbitMQInterface.HostIP, frmRabbitMQInterface.ProducerExchange, frmRabbitMQInterface.ProducerRoutingKey, frmRabbitMQInterface.ConsumerExchange, frmRabbitMQInterface.ConsumerRoutingKey, frmRabbitMQInterface.ConsumerQueue, frmRabbitMQInterface.Value);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool ModifyRow()
        //{
        //    try
        //    {
        //        DataRow dataRow = m_dataTable.Rows[dataGridView.SelectedCells[0].RowIndex];
        //        FrmRabbitMQInterface frmRabbitMQInterface = new FrmRabbitMQInterface(dataRow["colMessageType"].ToString(), dataRow["colHostName"].ToString(), dataRow["colHostIP"].ToString(), dataRow["colProducerExchange"].ToString(), dataRow["colProducerRoutingKey"].ToString(), dataRow["colConsumerExchange"].ToString(), dataRow["colConsumerRoutingKey"].ToString(), dataRow["colConsumerQueue"].ToString(), dataRow["colValue"].ToString());
        //        if (frmRabbitMQInterface.ShowDialog() == DialogResult.OK)
        //        {
        //            dataRow["colMessageType"] = frmRabbitMQInterface.MessageType;
        //            dataRow["colHostName"] = frmRabbitMQInterface.HostName;
        //            dataRow["colHostIP"] = frmRabbitMQInterface.HostIP;
        //            dataRow["colProducerExchange"] = frmRabbitMQInterface.ProducerExchange;
        //            dataRow["colProducerRoutingKey"] = frmRabbitMQInterface.ProducerRoutingKey;
        //            dataRow["colConsumerExchange"] = frmRabbitMQInterface.ConsumerExchange;
        //            dataRow["colConsumerRoutingKey"] = frmRabbitMQInterface.ConsumerRoutingKey;
        //            dataRow["colConsumerQueue"] = frmRabbitMQInterface.ConsumerQueue;
        //            dataRow["colValue"] = frmRabbitMQInterface.Value;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool DelRow()
        //{
        //    try
        //    {
        //        if (dataGridView.SelectedCells.Count > 0)
        //        {
        //            m_dataTable.Rows.Remove(m_dataTable.Rows[dataGridView.SelectedCells[0].RowIndex]);
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}
	}
}
