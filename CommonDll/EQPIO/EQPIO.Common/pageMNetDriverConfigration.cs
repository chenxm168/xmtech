using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class pageMNetDriverConfigration// : UserControl
	{
        //private IContainer components = null;

        //private DataGridView dataGridView;

        //private readonly string m_strConfigPath = "..\\EQPIO\\Config\\MelsecBoard\\MNetConfig.xml";

        //private Configuration boardConifg;

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
        //    base.Name = "pageMNetDriverConfigration";
        //    base.Size = new System.Drawing.Size(570, 420);
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        //    ResumeLayout(false);
        //}

        //public pageMNetDriverConfigration()
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
        //        m_dataTable.Columns.Add("colDriverName");
        //        m_dataTable.Columns.Add("colChannel");
        //        m_dataTable.Columns.Add("colNetworkNo");
        //        m_dataTable.Columns.Add("colStationNo");
        //        m_dataTable.Columns.Add("colPLCMapFile");
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
        //        dataGridViewTextBoxColumn.DataPropertyName = "colDriverName";
        //        dataGridViewTextBoxColumn.HeaderText = "DriverName";
        //        dataGridViewTextBoxColumn.Name = dataGridViewTextBoxColumn.DataPropertyName;
        //        dataGridViewTextBoxColumn.ReadOnly = true;
        //        dataGridViewTextBoxColumn.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn2.DataPropertyName = "colChannel";
        //        dataGridViewTextBoxColumn2.HeaderText = "Channel";
        //        dataGridViewTextBoxColumn2.Name = dataGridViewTextBoxColumn2.DataPropertyName;
        //        dataGridViewTextBoxColumn2.ReadOnly = true;
        //        dataGridViewTextBoxColumn2.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn3.DataPropertyName = "colNetworkNo";
        //        dataGridViewTextBoxColumn3.HeaderText = "NetworkNo";
        //        dataGridViewTextBoxColumn3.Name = dataGridViewTextBoxColumn3.DataPropertyName;
        //        dataGridViewTextBoxColumn3.ReadOnly = true;
        //        dataGridViewTextBoxColumn3.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn4.DataPropertyName = "colStationNo";
        //        dataGridViewTextBoxColumn4.HeaderText = "StationNo";
        //        dataGridViewTextBoxColumn4.Name = dataGridViewTextBoxColumn4.DataPropertyName;
        //        dataGridViewTextBoxColumn4.ReadOnly = true;
        //        dataGridViewTextBoxColumn4.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn5.DataPropertyName = "colPLCMapFile";
        //        dataGridViewTextBoxColumn5.HeaderText = "PLCMapFile";
        //        dataGridViewTextBoxColumn5.Name = dataGridViewTextBoxColumn5.DataPropertyName;
        //        dataGridViewTextBoxColumn5.ReadOnly = true;
        //        dataGridViewTextBoxColumn5.Visible = true;
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn2);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn3);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn4);
        //        dataGridView.Columns.Add(dataGridViewTextBoxColumn5);
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
        //            boardConifg = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
        //            if (boardConifg == null)
        //            {
        //                return;
        //            }
        //            ConnectionInfo[] connectionInfo = boardConifg.ConnectionInfo;
        //            foreach (ConnectionInfo connectionInfo2 in connectionInfo)
        //            {
        //                m_dataTable.Rows.Add(connectionInfo2.DriverName, connectionInfo2.Channel, connectionInfo2.NetworkNo, connectionInfo2.StationNo, connectionInfo2.PLCMapFile);
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
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.DriverName.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][0].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.Channel.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][1].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.NetworkNo.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][2].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.StationNo.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][3].ToString());
        //                xmlAttr = xmlDocument.CreateAttribute(ConfigManager.ConfigAttribute.PLCMapFile.ToString());
        //                XmlMaker(xmlDocument, ref xmlEle, ref xmlAttr, m_dataTable.Rows[i][4].ToString());
        //            }
        //            documentElement.AppendChild(xmlEle);
        //        }
        //        xmlDocument.Save(m_strConfigPath);
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
        //            FrmMNetDriver frmMNetDriver = new FrmMNetDriver();
        //            if (frmMNetDriver.ShowDialog() == DialogResult.OK)
        //            {
        //                m_dataTable.Rows.Add(frmMNetDriver.DriverName, frmMNetDriver.Channel, frmMNetDriver.NetworkNo, frmMNetDriver.StationNo, frmMNetDriver.PLCMapFile);
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
        //        FrmMNetDriver frmMNetDriver = new FrmMNetDriver(dataRow["colDriverName"].ToString(), dataRow["colChannel"].ToString(), dataRow["colNetworkNo"].ToString(), dataRow["colStationNo"].ToString(), dataRow["colPLCMapFile"].ToString());
        //        if (frmMNetDriver.ShowDialog() == DialogResult.OK)
        //        {
        //            dataRow["colDriverName"] = frmMNetDriver.DriverName;
        //            dataRow["colChannel"] = frmMNetDriver.Channel;
        //            dataRow["colNetworkNo"] = frmMNetDriver.NetworkNo;
        //            dataRow["colStationNo"] = frmMNetDriver.StationNo;
        //            dataRow["colPLCMapFile"] = frmMNetDriver.PLCMapFile;
        //        }
        //    }
        //    catch (Exception)
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
