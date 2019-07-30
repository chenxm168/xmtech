using System.ComponentModel;
using System.Drawing;
//using System.Windows.Forms;

namespace EQPIO.Common
{
    public class datagridConfigration //: UserControl
    {
        //private IContainer components = null;

        //private DataGridView dataGridView;

        //private DataGridViewTextBoxColumn LocalName;

        //private DataGridViewTextBoxColumn DriverName;

        //private DataGridViewTextBoxColumn Channel;

        //private DataGridViewTextBoxColumn PLCMapFile;

        //private DataGridViewButtonColumn btnPLCMapFile;

        //private DataGridViewTextBoxColumn IpAddress;

        //private DataGridViewTextBoxColumn MelsecPort;

        //private DataGridViewTextBoxColumn FixedBufferPort;

        //private DataGridViewCheckBoxColumn IsMelsecEnabled;

        //private DataGridViewCheckBoxColumn IsFixedBufferEnabled;

        //private DataGridViewTextBoxColumn CodeType;

        //private DataGridViewTextBoxColumn NetworkNo;

        //private DataGridViewTextBoxColumn PCNo;

        //private DataGridViewTextBoxColumn StationNo;

        //private DataGridViewTextBoxColumn CPUType;

        //private DataGridViewTextBoxColumn MessageType;

        //private DataGridViewTextBoxColumn HostName;

        //private DataGridViewTextBoxColumn HostIP;

        //private DataGridViewTextBoxColumn ProducerExchange;

        //private DataGridViewTextBoxColumn ProducerRoutingKey;

        //private DataGridViewTextBoxColumn ConsumerExchange;

        //private DataGridViewTextBoxColumn ConsumerRoutingKey;

        //private DataGridViewTextBoxColumn ConsumerQueue;

        //private DataGridViewTextBoxColumn Value;

        //public DataGridView DataGridView;

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
        //    LocalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    DriverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    Channel = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    PLCMapFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    btnPLCMapFile = new System.Windows.Forms.DataGridViewButtonColumn();
        //    IpAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    MelsecPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    FixedBufferPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    IsMelsecEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        //    IsFixedBufferEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        //    CodeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    NetworkNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    PCNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    StationNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    CPUType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    MessageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    HostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    HostIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ProducerExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ProducerRoutingKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ConsumerExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ConsumerRoutingKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ConsumerQueue = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
        //    SuspendLayout();
        //    dataGridView.AllowUserToAddRows = false;
        //    dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    dataGridView.Columns.AddRange(LocalName, DriverName, Channel, PLCMapFile, btnPLCMapFile, IpAddress, MelsecPort, FixedBufferPort, IsMelsecEnabled, IsFixedBufferEnabled, CodeType, NetworkNo, PCNo, StationNo, CPUType, MessageType, HostName, HostIP, ProducerExchange, ProducerRoutingKey, ConsumerExchange, ConsumerRoutingKey, ConsumerQueue, Value);
        //    dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
        //    dataGridView.Location = new System.Drawing.Point(0, 0);
        //    dataGridView.MultiSelect = false;
        //    dataGridView.Name = "dataGridView";
        //    dataGridView.RowTemplate.Height = 23;
        //    dataGridView.Size = new System.Drawing.Size(570, 420);
        //    dataGridView.TabIndex = 2;
        //    LocalName.HeaderText = "LocalName";
        //    LocalName.Name = "LocalName";
        //    LocalName.Visible = false;
        //    DriverName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        //    DriverName.HeaderText = "DriverName";
        //    DriverName.Name = "DriverName";
        //    DriverName.Visible = false;
        //    Channel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
        //    Channel.HeaderText = "Channel";
        //    Channel.Name = "Channel";
        //    Channel.Visible = false;
        //    Channel.Width = 77;
        //    PLCMapFile.HeaderText = "PLCMapFile";
        //    PLCMapFile.Name = "PLCMapFile";
        //    PLCMapFile.Visible = false;
        //    btnPLCMapFile.HeaderText = "...";
        //    btnPLCMapFile.Name = "btnPLCMapFile";
        //    btnPLCMapFile.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        //    btnPLCMapFile.Visible = false;
        //    btnPLCMapFile.Width = 15;
        //    IpAddress.HeaderText = "IpAddress";
        //    IpAddress.Name = "IpAddress";
        //    IpAddress.Visible = false;
        //    MelsecPort.HeaderText = "MelsecPort";
        //    MelsecPort.Name = "MelsecPort";
        //    MelsecPort.Visible = false;
        //    FixedBufferPort.HeaderText = "FiexBufferPort";
        //    FixedBufferPort.Name = "FixedBufferPort";
        //    FixedBufferPort.Visible = false;
        //    IsMelsecEnabled.HeaderText = "MelsecEnabled";
        //    IsMelsecEnabled.Name = "IsMelsecEnabled";
        //    IsMelsecEnabled.Visible = false;
        //    IsFixedBufferEnabled.HeaderText = "FixedBufferEnabled";
        //    IsFixedBufferEnabled.Name = "IsFixedBufferEnabled";
        //    IsFixedBufferEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
        //    IsFixedBufferEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        //    IsFixedBufferEnabled.Visible = false;
        //    CodeType.HeaderText = "CodeType";
        //    CodeType.Name = "CodeType";
        //    CodeType.Visible = false;
        //    NetworkNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
        //    NetworkNo.HeaderText = "NetworkNo";
        //    NetworkNo.Name = "NetworkNo";
        //    NetworkNo.Visible = false;
        //    NetworkNo.Width = 92;
        //    PCNo.HeaderText = "PCNo";
        //    PCNo.Name = "PCNo";
        //    PCNo.Visible = false;
        //    StationNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
        //    StationNo.HeaderText = "StationNo";
        //    StationNo.Name = "StationNo";
        //    StationNo.Visible = false;
        //    StationNo.Width = 84;
        //    CPUType.HeaderText = "CPUType";
        //    CPUType.Name = "CPUType";
        //    CPUType.Visible = false;
        //    MessageType.HeaderText = "MessageType";
        //    MessageType.Name = "MessageType";
        //    MessageType.Visible = false;
        //    HostName.HeaderText = "HostName";
        //    HostName.Name = "HostName";
        //    HostName.Visible = false;
        //    HostIP.HeaderText = "HostIP";
        //    HostIP.Name = "HostIP";
        //    HostIP.Visible = false;
        //    ProducerExchange.HeaderText = "ProducerExchange";
        //    ProducerExchange.Name = "ProducerExchange";
        //    ProducerExchange.Visible = false;
        //    ProducerRoutingKey.HeaderText = "ProducerRoutingKey";
        //    ProducerRoutingKey.Name = "ProducerRoutingKey";
        //    ProducerRoutingKey.Visible = false;
        //    ConsumerExchange.HeaderText = "ConsumerExchange";
        //    ConsumerExchange.Name = "ConsumerExchange";
        //    ConsumerExchange.Visible = false;
        //    ConsumerRoutingKey.HeaderText = "ConsumerRoutingKey";
        //    ConsumerRoutingKey.Name = "ConsumerRoutingKey";
        //    ConsumerRoutingKey.Visible = false;
        //    ConsumerQueue.HeaderText = "ConsumerQueue";
        //    ConsumerQueue.Name = "ConsumerQueue";
        //    ConsumerQueue.Visible = false;
        //    Value.HeaderText = "Value";
        //    Value.Name = "Value";
        //    Value.Visible = false;
        //    base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
        //    base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    base.Controls.Add(dataGridView);
        //    base.Name = "datagridConfigration";
        //    base.Size = new System.Drawing.Size(570, 420);
        //    ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        //    ResumeLayout(false);
        //}

        //public datagridConfigration()
        //{
        //    InitializeComponent();
        //}

        //public void VisibleColumn(int index)
        //{
        //    try
        //    {
        //        dataGridView.Columns[index].Visible = true;
        //    }
        //    catch
        //    {
        //    }
        //}

        //public void RemoveRow(int index)
        //{
        //    try
        //    {
        //        if (index <= dataGridView.Rows.Count)
        //        {
        //            dataGridView.Rows.RemoveAt(index);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //public void AddRow()
        //{
        //    try
        //    {
        //    }
        //    catch
        //    {
        //    }
        //}
    }
}
