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
	public class pageEIPDriverConfigration// : UserControl
	{
        //private IContainer components = null;

        //private DataGridView dataGridView;

        //private readonly string m_strConfigPath = "..\\EQPIO\\Config\\EIP\\EIPDriverConfig.xml";

        //private EIPDriverConfig eipConfig;

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
        //    base.Name = "pageEIPDriverConfigration";
        //    base.Size = new System.Drawing.Size(570, 420);
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        //    ResumeLayout(false);
        //}

        //public pageEIPDriverConfigration()
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
        //        m_dataTable.Columns.Add("colEIPMapFile");
        //        m_dataTable.Columns.Add("colLog4NetPath");
        //        m_dataTable.Columns.Add("colLogRootDir");
        //        m_dataTable.Columns.Add("colTimeOutCheckList");
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
        //        dataGridViewTextBoxColumn2.DataPropertyName = "colEIPMapFile";
        //        dataGridViewTextBoxColumn2.HeaderText = "EIPMapFile";
        //        dataGridViewTextBoxColumn2.Name = dataGridViewTextBoxColumn2.DataPropertyName;
        //        dataGridViewTextBoxColumn2.ReadOnly = true;
        //        dataGridViewTextBoxColumn2.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn3.DataPropertyName = "colLog4NetPath";
        //        dataGridViewTextBoxColumn3.HeaderText = "Log4NetPath";
        //        dataGridViewTextBoxColumn3.Name = dataGridViewTextBoxColumn3.DataPropertyName;
        //        dataGridViewTextBoxColumn3.ReadOnly = true;
        //        dataGridViewTextBoxColumn3.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn4.DataPropertyName = "colLogRootDir";
        //        dataGridViewTextBoxColumn4.HeaderText = "LogRootDir";
        //        dataGridViewTextBoxColumn4.Name = dataGridViewTextBoxColumn4.DataPropertyName;
        //        dataGridViewTextBoxColumn4.ReadOnly = true;
        //        dataGridViewTextBoxColumn4.Visible = true;
        //        DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        //        dataGridViewTextBoxColumn5.DataPropertyName = "colTimeOutCheckList";
        //        dataGridViewTextBoxColumn5.HeaderText = "TimeOutCheckList";
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
        //            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EIPDriverConfig));
        //            XmlDocument xmlDocument = new XmlDocument();
        //            xmlDocument.Load(m_strConfigPath);
        //            XmlNode xmlNode = xmlDocument.SelectSingleNode("EIPDriverConfig");
        //            eipConfig = (EIPDriverConfig)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
        //            if (eipConfig == null)
        //            {
        //                return;
        //            }
        //            m_dataTable.Rows.Add(eipConfig.DriverName, eipConfig.EIPMapFile, eipConfig.Log4NetPath, eipConfig.LogRootDir, eipConfig.TimeOutCheckList);
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
        //    xmlDocument.LoadXml("<EIPDriverConfig></EIPDriverConfig>");
        //    XmlNode xmlNode = xmlDocument.SelectSingleNode("EIPDriverConfig");
        //    XmlElement documentElement = xmlDocument.DocumentElement;
        //    XmlElement xmlElement = null;
        //    try
        //    {
        //        if (m_dataTable.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < m_dataTable.Rows.Count; i++)
        //            {
        //                xmlElement = xmlDocument.CreateElement("DriverName");
        //                xmlElement.InnerText = m_dataTable.Rows[i][0].ToString();
        //                documentElement.AppendChild(xmlElement);
        //                xmlElement = xmlDocument.CreateElement("EIPMapFile");
        //                xmlElement.InnerText = m_dataTable.Rows[i][1].ToString();
        //                documentElement.AppendChild(xmlElement);
        //                xmlElement = xmlDocument.CreateElement("Log4NetPath");
        //                xmlElement.InnerText = m_dataTable.Rows[i][2].ToString();
        //                documentElement.AppendChild(xmlElement);
        //                xmlElement = xmlDocument.CreateElement("LogRootDir");
        //                xmlElement.InnerText = m_dataTable.Rows[i][3].ToString();
        //                documentElement.AppendChild(xmlElement);
        //                xmlElement = xmlDocument.CreateElement("TimeOutCheckList");
        //                xmlElement.InnerText = m_dataTable.Rows[i][4].ToString();
        //                documentElement.AppendChild(xmlElement);
        //            }
        //            xmlDocument.Save(m_strConfigPath);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //public bool AddRow()
        //{
        //    try
        //    {
        //        if (m_dataTable != null)
        //        {
        //            if (m_dataTable.Rows.Count > 0)
        //            {
        //                MessageBox.Show("You can apply only one set", "EIP Config", MessageBoxButtons.OK);
        //                return false;
        //            }
        //            FrmEIPDriver frmEIPDriver = new FrmEIPDriver();
        //            if (frmEIPDriver.ShowDialog() == DialogResult.OK)
        //            {
        //                m_dataTable.Rows.Add(frmEIPDriver.DriverName, frmEIPDriver.EIPMapFile, frmEIPDriver.Log4NetPath, frmEIPDriver.LogRootDir, frmEIPDriver.TimeOutCheckList);
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
        //        FrmEIPDriver frmEIPDriver = new FrmEIPDriver(dataRow["colDriverName"].ToString(), dataRow["colEIPMapFile"].ToString(), dataRow["colLog4NetPath"].ToString(), dataRow["colLogRootDir"].ToString(), dataRow["colTimeOutCheckList"].ToString());
        //        if (frmEIPDriver.ShowDialog() == DialogResult.OK)
        //        {
        //            dataRow["colDriverName"] = frmEIPDriver.DriverName;
        //            dataRow["colEIPMapFile"] = frmEIPDriver.EIPMapFile;
        //            dataRow["colLog4NetPath"] = frmEIPDriver.Log4NetPath;
        //            dataRow["colLogRootDir"] = frmEIPDriver.LogRootDir;
        //            dataRow["colTimeOutCheckList"] = frmEIPDriver.TimeOutCheckList;
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
