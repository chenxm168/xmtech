using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class pageMNetProtocolConfigration //: UserControl
	{
        //private readonly string m_strConfigPath = "..\\EQPIO\\Config\\MelsecEthernet\\MNetConfig.xml";

        //private Configuration ethernetConifg;

        //private DataTable m_dataTable = new DataTable();

        //private bool m_bSuccessConfigtration = false;

        //private IContainer components = null;

        //private DataGridView dataGridView;

        //public bool SuccessConfigtration ;

        //public pageMNetProtocolConfigration()
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
        //        m_dataTable.Columns.Add("colLocalName");
        //        m_dataTable.Columns.Add("colPLCMapFile");
        //        m_dataTable.Columns.Add("colIpAddress");
        //        m_dataTable.Columns.Add("colMelsecPort");
        //        m_dataTable.Columns.Add("colFixedBufferPort");
        //        m_dataTable.Columns.Add("colIsMelsecEnabled");
        //        m_dataTable.Columns.Add("colIsFixedBufferEnabled");
        //        m_dataTable.Columns.Add("colCodeType");
        //        m_dataTable.Columns.Add("colNetworkNo");
        //        m_dataTable.Columns.Add("colPCNo");
        //        m_dataTable.Columns.Add("colCPUType");
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
        //        dataGridViewTextBoxColumn.DataPropertyName = "colLocalName";
        //        dataGridViewTextBoxColumn.HeaderText = "LocalName";
        //        dataGridViewTextBoxColumn.Name = dataGridViewTextBoxColumn.DataPropertyName;
        //        dataGridViewTextBoxColumn.ReadOnly = true;
        //        dataGridViewTextBoxColumn.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn2.DataPropertyName = "colPLCMapFile";
        //        dataGridViewTextBoxColumn2.HeaderText = "PLCMapFile";
        //        dataGridViewTextBoxColumn2.Name = dataGridViewTextBoxColumn2.DataPropertyName;
        //        dataGridViewTextBoxColumn2.ReadOnly = true;
        //        dataGridViewTextBoxColumn2.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn3.DataPropertyName = "colIpAddress";
        //        dataGridViewTextBoxColumn3.HeaderText = "IPAddress";
        //        dataGridViewTextBoxColumn3.Name = dataGridViewTextBoxColumn3.DataPropertyName;
        //        dataGridViewTextBoxColumn3.ReadOnly = true;
        //        dataGridViewTextBoxColumn3.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn4.DataPropertyName = "colMelsecPort";
        //        dataGridViewTextBoxColumn4.HeaderText = "MelsecPort";
        //        dataGridViewTextBoxColumn4.Name = dataGridViewTextBoxColumn4.DataPropertyName;
        //        dataGridViewTextBoxColumn4.ReadOnly = true;
        //        dataGridViewTextBoxColumn4.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn5.DataPropertyName = "colFixedBufferPort";
        //        dataGridViewTextBoxColumn5.HeaderText = "FixedBufferPort";
        //        dataGridViewTextBoxColumn5.Name = dataGridViewTextBoxColumn5.DataPropertyName;
        //        dataGridViewTextBoxColumn5.ReadOnly = true;
        //        dataGridViewTextBoxColumn5.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn6.DataPropertyName = "colIsMelsecEnabled";
        //        dataGridViewTextBoxColumn6.HeaderText = "IsMelsecEnabled";
        //        dataGridViewTextBoxColumn6.Name = dataGridViewTextBoxColumn6.DataPropertyName;
        //        dataGridViewTextBoxColumn6.ReadOnly = true;
        //        dataGridViewTextBoxColumn6.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn7.DataPropertyName = "colIsFixedBufferEnabled";
        //        dataGridViewTextBoxColumn7.HeaderText = "IsMelsecEnabled";
        //        dataGridViewTextBoxColumn7.Name = dataGridViewTextBoxColumn7.DataPropertyName;
        //        dataGridViewTextBoxColumn7.ReadOnly = true;
        //        dataGridViewTextBoxColumn7.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn8.DataPropertyName = "colCodeType";
        //        dataGridViewTextBoxColumn8.HeaderText = "CodeType";
        //        dataGridViewTextBoxColumn8.Name = dataGridViewTextBoxColumn8.DataPropertyName;
        //        dataGridViewTextBoxColumn8.ReadOnly = true;
        //        dataGridViewTextBoxColumn8.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn9.DataPropertyName = "colNetworkNo";
        //        dataGridViewTextBoxColumn9.HeaderText = "NetworkNo";
        //        dataGridViewTextBoxColumn9.Name = dataGridViewTextBoxColumn9.DataPropertyName;
        //        dataGridViewTextBoxColumn9.ReadOnly = true;
        //        dataGridViewTextBoxColumn9.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn10.DataPropertyName = "colPCNo";
        //        dataGridViewTextBoxColumn10.HeaderText = "PCNo";
        //        dataGridViewTextBoxColumn10.Name = dataGridViewTextBoxColumn10.DataPropertyName;
        //        dataGridViewTextBoxColumn10.ReadOnly = true;
        //        dataGridViewTextBoxColumn10.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn11.DataPropertyName = "colCPUType";
        //        dataGridViewTextBoxColumn11.HeaderText = "CPUType";
        //        dataGridViewTextBoxColumn11.Name = dataGridViewTextBoxColumn11.DataPropertyName;
        //        dataGridViewTextBoxColumn11.ReadOnly = true;
        //        dataGridViewTextBoxColumn11.Visible = true;
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn2);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn3);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn4);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn5);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn6);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn7);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn8);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn9);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn10);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn11);
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
        //            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
        //            XmlDocument xmlDocument = new XmlDocument();
        //            xmlDocument.Load(m_strConfigPath);
        //            XmlNode xmlNode = xmlDocument.SelectSingleNode("Configuration");
        //            ethernetConifg = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
        //            if (ethernetConifg == null)
        //            {
        //                return;
        //            }
        //            ConnectionInfo[] connectionInfo = ethernetConifg.ConnectionInfo;
        //            foreach (ConnectionInfo connectionInfo2 in connectionInfo)
        //            {
        //                m_dataTable.Rows.Add(connectionInfo2.LocalName, connectionInfo2.PLCMapFile, connectionInfo2.IpAddress, connectionInfo2.MelsecPort, connectionInfo2.FixedBufferPort, connectionInfo2.IsMelsecEnabled, connectionInfo2.IsFixedBufferEnabled, connectionInfo2.CodeType, connectionInfo2.NetworkNo, connectionInfo2.PCNo, connectionInfo2.CPUType);
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
        //    xmlDocument.LoadXml("<Configuration></Configuration>");
        //    XmlNode xmlNode = xmlDocument.SelectSingleNode("Configuration");
        //    XmlElement documentElement = xmlDocument.DocumentElement;
        //    XmlElement xmlEle = null;
        //    XmlAttribute xmlAttr = null;
        //    try
        //    {
        //        if (m_dataTable.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < m_dataTable.Rows.Count; i++)
        //            {
        //                xmlEle = xmlDocument.CreateElement("ConnectionInfo");
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.LocalName.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][0].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.PLCMapFile.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][1].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.IpAddress.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][2].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.MelsecPort.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][3].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.FixedBufferPort.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][4].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.IsMelsecEnabled.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][5].ToString().ToLower());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.IsFixedBufferEnabled.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][6].ToString().ToLower());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.CodeType.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][7].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.NetworkNo.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][8].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.PCNo.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][9].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.CPUType.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][10].ToString());
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
        //            FrmMNetProtocol frmMNetProtocol = new FrmMNetProtocol();
        //            if (frmMNetProtocol.ShowDialog() == DialogResult.OK)
        //            {
        //                this.m_dataTable.Rows.Add(new object[] { frmMNetProtocol.LocalName, frmMNetProtocol.PLCMapFile, frmMNetProtocol.IpAddress, string.Format("{0},{1}", frmMNetProtocol.MelsecPort1, frmMNetProtocol.MelsecPort2), frmMNetProtocol.FixedBufferPort, "true", "false", frmMNetProtocol.CodeType, frmMNetProtocol.NetworkNo, frmMNetProtocol.PCNo, frmMNetProtocol.CPUType });

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
        //        FrmMNetProtocol frmMNetProtocol = new FrmMNetProtocol(dataRow["colLocalName"].ToString(), dataRow["colPLCMapFile"].ToString(), dataRow["colIpAddress"].ToString(), dataRow["colMelsecPort"].ToString(), dataRow["colFixedBufferPort"].ToString(), dataRow["colCodeType"].ToString(), dataRow["colNetworkNo"].ToString(), dataRow["colPCNo"].ToString(), dataRow["colCPUType"].ToString());
        //        if (frmMNetProtocol.ShowDialog() == DialogResult.OK)
        //        {
        //            dataRow["colLocalName"] = frmMNetProtocol.LocalName;
        //            dataRow["colPLCMapFile"] = frmMNetProtocol.PLCMapFile;
        //            dataRow["colIpAddress"] = frmMNetProtocol.IpAddress;
        //            dataRow["colMelsecPort"] = string.Format("{0},{1}", frmMNetProtocol.MelsecPort1, frmMNetProtocol.MelsecPort2);

        //            dataRow["colFixedBufferPort"] = frmMNetProtocol.FixedBufferPort;
        //            dataRow["colCodeType"] = frmMNetProtocol.CodeType;
        //            dataRow["colNetworkNo"] = frmMNetProtocol.NetworkNo;
        //            dataRow["colPCNo"] = frmMNetProtocol.PCNo;
        //            dataRow["colCPUType"] = frmMNetProtocol.CPUType;
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
        //    dataGridView.AllowUserToAddRows = false;
        //    dataGridView.AllowUserToDeleteRows = false;
        //    dataGridView.AllowUserToResizeColumns = false;
        //    dataGridView.AllowUserToResizeRows = false;
        //    dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
        //    dataGridView.Location = new System.Drawing.Point(0, 0);
        //    dataGridView.MultiSelect = false;
        //    dataGridView.Name = "dataGridView";
        //    dataGridView.ReadOnly = true;
        //    dataGridView.RowTemplate.Height = 23;
        //    dataGridView.ShowEditingIcon = false;
        //    dataGridView.Size = new System.Drawing.Size(570, 420);
        //    dataGridView.TabIndex = 0;
        //    base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
        //    base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    base.Controls.Add(dataGridView);
        //    base.Name = "pageMNetProtocolConfigration";
        //    base.Size = new System.Drawing.Size(570, 420);
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        //    ResumeLayout(false);
        //}
	}
}
