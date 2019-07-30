
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MNetOnDemand
    {
        private string _address;
        private BinaryReader _MelsecReader = null;
        private BinaryWriter _MelsecWriter = null;
        private int _portNo;
        private TcpClient CommandClient;
        private ConnectionInfo conInfo;
        private ILog logger = LogManager.GetLogger(typeof(MNetOnDemand));
        private Thread onDemandThread;
        private BlockMap plcBlockMap;
        private OnDemand plcOnDemand;
        private bool reConnectFlag = false;
        private Thread reConnectionThread;

        public event ConnectedEventHandler OnConnected;

        public event DisconnectedEventHandler OnDisconnected;

        public event EventHandler<ScanReceivedEventArgs> ScanReceived;

        public MNetOnDemand(ConnectionInfo conn, DataGathering dataGathering, BlockMap plcMap)
        {
            this._portNo = Convert.ToInt32(conn.FixedBufferPort);
            this._address = conn.IpAddress;
            this.Name = conn.LocalName;
            this.conInfo = conn;
            this.plcBlockMap = plcMap;
            this.plcOnDemand = dataGathering.OnDemand;
        }

        public void Close()
        {
            this._MelsecReader.Close();
            this._MelsecWriter.Close();
            try
            {
                if (this.CommandClient.Connected)
                {
                    this.CommandClient.Client.Close();
                }
            }
            catch
            {
            }
        }

        private bool Connect()
        {
            try
            {
                this.CommandClient = new TcpClient(this._address, this._portNo);
                this.CommandClient.ReceiveTimeout = 0x2710;
                this._MelsecReader = new BinaryReader(this.CommandClient.GetStream());
                this._MelsecWriter = new BinaryWriter(this.CommandClient.GetStream());
                this.IsConnection = true;
                this.logger.Info(string.Format("[Connection] Melsec OnDemand Address : {0} , PortNo : {1}", this._address, this._portNo));
                return true;
            }
            catch (SocketException exception)
            {
                this.IsConnection = false;
                this.logger.Error(string.Format("[Connection] OnDemand Ethernet SocketException : {0}", exception.Message));
                return false;
            }
            catch (Exception exception2)
            {
                this.IsConnection = false;
                this.logger.Error(string.Format("[Connection] OnDemand Ethernet Error : {0}", exception2.Message));
                return false;
            }
        }

        public void Init()
        {
            if (!this.Connect())
            {
                this.ReConnect();
            }
            else
            {
                this.OnConnected(this);
            }
        }

        private void KindOndemandEvent(string address, int value)
        {
            Block[] block2 = plcOnDemand.Block;
            Block block;
            for (int i = 0; i < block2.Length; i++)
            {
                block = block2[i];
                Block block3 = (from blockName in plcBlockMap.Block
                                where blockName.Name == block.Name
                                select blockName).FirstOrDefault();
                if (block3 != null && block3.Item != null)
                {
                    Item[] item = block3.Item;
                    foreach (Item item2 in item)
                    {
                        if (block.DeviceCode == "R" || block.DeviceCode == "R" || block.DeviceCode == "M" || block.DeviceCode == "D")
                        {
                            address = Convert.ToInt32(address, 16).ToString();
                        }
                        if (item2.ItemAddress == address && item2.Value != value.ToString())
                        {
                            OnScanReceived(new ScanReceivedEventArgs(string.Empty, block3.Name, item2.Name, (value == 1) ? true : false, Name));
                            item2.Value = value.ToString();
                            break;
                        }
                    }
                }
            }
        }

        private void OnDemandScan()
        {
            try
            {
                if (this._MelsecReader != null)
                {
                    string str = string.Empty;
                    byte[] buffer = new byte[4];
                    byte[] buffer2 = new byte[4];
                    this._MelsecReader.Read(buffer, 0, buffer.Length);
                    this._MelsecReader.Read(buffer2, 0, buffer2.Length);
                    this.logger.Info("OnDemandScan Received lengrhBuffer");
                    byte[] buffer3 = new byte[Convert.ToInt32(Encoding.ASCII.GetString(buffer2), 0x10) * 4];
                    this._MelsecReader.Read(buffer3, 0, buffer3.Length);
                    str = Encoding.ASCII.GetString(buffer3);
                    string s = "E000";
                    byte[] bytes = new byte[4];
                    bytes = Encoding.ASCII.GetBytes(s);
                    this._MelsecWriter.Write(bytes);
                    if (string.IsNullOrEmpty(str))
                    {
                        this.logger.Error("string.IsNullOrEmpty(receiveProtocol)");
                    }
                    else
                    {
                        this.logger.Info(string.Format("receiveProtocol : {0}", str));
                        this.KindOndemandEvent(str.Substring(0, 4), Convert.ToInt32(str.Substring(4)));
                        this.logger.Info(string.Format("[Event] Melsec OnDemand Address : {0} , Value : {1}, CPUType : {2}", str.Substring(0, 4), Convert.ToInt32(str.Substring(4)), this.conInfo.CPUType));
                    }
                }
            }
            catch (FormatException)
            {
                this.logger.Error("FormatException");
                this.OnDisconnected(this);
            }
            catch (IOException)
            {
                this.logger.Error("IOException");
                this.OnDisconnected(this);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void OndemandThreadProc()
        {
            try
            {
                while (this.IsConnection)
                {
                    this.OnDemandScan();
                    Thread.Sleep(200);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        protected virtual void OnScanReceived(ScanReceivedEventArgs e)
        {
            EventHandler<ScanReceivedEventArgs> scanReceived = this.ScanReceived;
            if (scanReceived != null)
            {
                scanReceived(this, e);
            }
        }

        public void ReConnect()
        {
            this.reConnectFlag = true;
            this.reConnectionThread = new Thread(new System.Threading.ThreadStart(this.ReConnectThreadProc));
            this.reConnectionThread.IsBackground = true;
            this.reConnectionThread.Start();
        }

        private void ReConnectThreadProc()
        {
            try
            {
                while (this.reConnectFlag)
                {
                    Thread.Sleep(0x3e8);
                    if (this.Connect())
                    {
                        this.reConnectFlag = false;
                        this.OnConnected(this);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        public void StopConnect()
        {
            if (this.reConnectionThread != null)
            {
                this.reConnectionThread = null;
            }
        }

        public void ThreadClose()
        {
            if ((this.onDemandThread != null) && this.onDemandThread.IsAlive)
            {
                this.onDemandThread.Abort();
            }
            if ((this.reConnectionThread != null) && this.reConnectionThread.IsAlive)
            {
                this.reConnectionThread.Abort();
            }
        }

        public void ThreadStart()
        {
            this.onDemandThread = new Thread(new System.Threading.ThreadStart(this.OndemandThreadProc));
            this.onDemandThread.IsBackground = true;
            this.onDemandThread.Start();
            this.reConnectionThread = null;
        }

        public void ThreadStop()
        {
            this.onDemandThread = null;
            this.reConnectionThread = null;
        }

        public bool IsConnection { get; set; }

        public string Name { get; set; }

        public delegate void ConnectedEventHandler(object sender);

        public delegate void DisconnectedEventHandler(object sender);
    }
}
