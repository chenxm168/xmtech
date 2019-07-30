
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MNet1EScanUnit : IMNetScanUnit
    {
        private BinaryReader _MelsecReader = null;
        private BinaryWriter _MelsecWriter = null;
        private Make1EProtocol _mProtocol;
        private Dictionary<string, string> bitCompareList = new Dictionary<string, string>();
        private TcpClient CommandClient;
        private ILog logger = LogManager.GetLogger(typeof(MNet1EScanUnit));
        private bool m_bMelsecEnabled = false;
        private bool m_bReconnectFlag = false;
        private bool m_bScanFlag = false;
        private DataGathering m_cacheDataGathering = new DataGathering();
        private int m_iMelsecPortNo;
        private int m_iNetworkNo;
        private int m_iStationNo;
        private DeviceMemory m_Memory;
        private object m_objLockbitCompareList = new object();
        private string m_strIPAddress;
        private Thread m_tReconnectionThread;
        private Thread m_tScanThread;
        private EQPIO.Common.BlockMap plcBlockMap;
        private EQPIO.Common.Scan plcScan;
        private object readWriteObj = new object();

        public MNet1EScanUnit(ConnectionInfo conn, DataGathering dataGathering, EQPIO.Common.BlockMap blockMap, int melsecProtNo)
        {
            this.ConnectionUnitName = conn.LocalName;
            this.m_strIPAddress = conn.IpAddress;
            this.m_iMelsecPortNo = melsecProtNo;
            this.m_iNetworkNo = Convert.ToInt32(conn.NetworkNo);
            this.m_iStationNo = Convert.ToInt32(conn.PCNo);
            this.m_bMelsecEnabled = conn.IsMelsecEnabled == "true";
            this.plcScan = dataGathering.Scan;
            this.plcBlockMap = blockMap;
            this._mProtocol = new Make1EProtocol();
        }

        public override void CacheThreadStart()
        {
            throw new NotImplementedException();
        }

        public override void CacheThreadStop()
        {
            this.ThreadStop();
        }

        public override void Close()
        {
            if (this._MelsecReader != null)
            {
                this._MelsecReader.Close();
            }
            if (this._MelsecWriter != null)
            {
                this._MelsecWriter.Close();
            }
            try
            {
                if (this.CommandClient.Connected)
                {
                    this.CommandClient.Client.Close();
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private bool Connect()
        {
            try
            {
                if (this.m_bMelsecEnabled)
                {
                    this.CommandClient = new TcpClient(this.m_strIPAddress, this.m_iMelsecPortNo);
                    this.CommandClient.ReceiveTimeout = 0x2710;
                    this.CommandClient.Client.Blocking = true;
                    this._MelsecReader = new BinaryReader(this.CommandClient.GetStream());
                    this._MelsecWriter = new BinaryWriter(this.CommandClient.GetStream());
                    this.logger.Info(string.Format("[Connection] Melsec Connect Address : {0} , PortNo : {1}", this.m_strIPAddress, this.m_iMelsecPortNo));
                }
                this.IsConnection = true;
                return true;
            }
            catch (SocketException exception)
            {
                this.IsConnection = false;
                this.logger.Error(string.Format("[Connection] Ethernet SocketException : {0}", exception.Message));
                return false;
            }
            catch (Exception exception2)
            {
                this.IsConnection = false;
                this.logger.Error(string.Format("[Connection] Ethernet Error : {0}", exception2.Message));
                return false;
            }
        }

        public override void Init()
        {
            if (!this.Connect())
            {
                this.StartReconnect();
            }
            else
            {
                this.OnConnected(new ConnectedEventArgs(this));
            }
        }

        public override void ListUpCacheBlock()
        {
            throw new NotImplementedException();
        }

        private string ReadBinaryWord(string device, string address, int length, string deviceCode)
        {
            byte[] buffer;
            int num2;
            if (deviceCode == "W")
            {
                buffer = this._mProtocol.GetRead(device, address, length, true, Make1EProtocol.BlockType.Word);
            }
            else
            {
                buffer = this._mProtocol.GetRead(device, address, length, false, Make1EProtocol.BlockType.Word);
            }
            string str = this.ReadWriter(buffer, IMNetScanUnit.ReceiveStatus.Read, length);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadBinaryWord) address : {0}", address));
                return null;
            }
            str = str.TrimEnd(new char[1]);
            int num = 0;
            string str2 = string.Empty;
            for (num2 = 0; num2 < (str.Length / 4); num2++)
            {
                num = Convert.ToInt32(str, 0x10);
                str2 = str2 + Convert.ToString(num, 2).PadLeft(0x10, '0');
            }
            string str3 = string.Empty;
            for (num2 = 0; num2 < (str2.Length / 0x10); num2++)
            {
                str3 = str2.Substring(num2 * 0x10, 0x10) + str3;
            }
            return str3;
        }

        private string ReadBitProtocol(string device, string address, int length)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, true, Make1EProtocol.BlockType.Bit);
            return this.ReadWriter(dataArry, IMNetScanUnit.ReceiveStatus.Read, length);
        }

        public override string ReadCacheData(string deviceCode, string address, string points)
        {
            try
            {
                string str = string.Empty;
                if (this.m_Memory != null)
                {
                    return string.Empty;
                }
                return str;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        private string Reader(IMNetScanUnit.ReceiveStatus status, int length, IMNetScanUnit.ProtocolType type)
        {
            try
            {
                int num;
                string str = string.Empty;
                byte[] buffer = new byte[2];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[2];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                str = Encoding.ASCII.GetString(buffer);
                if (str != "00")
                {
                    this.logger.Info(string.Format("[Receive] Receive NG : Plc Name : {0} , ReadData : {1}", this.ConnectionUnitName, str));
                    return null;
                }
                if (status == IMNetScanUnit.ReceiveStatus.Write)
                {
                    return str;
                }
                if (type == IMNetScanUnit.ProtocolType.Bit)
                {
                    num = ((length % 2) == 0) ? length : (length + 1);
                }
                else
                {
                    num = ((length % 2) == 0) ? (length * 4) : ((length + 1) * 4);
                }
                buffer = new byte[num];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer);
            }
            catch (IOException exception)
            {
                this.logger.Error(string.Format("[Error] IOException Error : {0}", exception.Message));
                return null;
            }
            catch (Exception exception2)
            {
                this.logger.Error(string.Format("[Error] Message : {0}", exception2.Message));
                return null;
            }
        }

        private string ReadWriter(byte[] dataArry, IMNetScanUnit.ReceiveStatus status, int length)
        {
            string str;
            try
            {
                if (this._MelsecWriter == null)
                {
                    str = null;
                }
                else
                {
                    lock (this.readWriteObj)
                    {
                        this._MelsecWriter.Write(dataArry, 0, dataArry.Length);
                        str = this.Reader(status, length, IMNetScanUnit.ProtocolType.Word);
                    }
                }
            }
            catch (IOException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                str = null;
            }
            catch (ObjectDisposedException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                str = null;
            }
            return str;
        }

        private void ReConnectThreadProc()
        {
            try
            {
                while (this.m_bReconnectFlag)
                {
                    if (this.Connect())
                    {
                        this.m_bReconnectFlag = false;
                        this.OnConnected(new ConnectedEventArgs(this));
                    }
                    Thread.Sleep(0x3e8);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        private void Scan()
        {
            if (plcScan.MultiBlock != null)
            {
                MultiBlock[] multiBlock = plcScan.MultiBlock;
                Block block;
                foreach (MultiBlock multiBlock2 in multiBlock)
                {
                    if (!(multiBlock2.StartUp.ToLower() == "false"))
                    {
                        lock (m_objLockbitCompareList)
                        {
                            Block[] block2 = multiBlock2.Block;
                            for (int j = 0; j < block2.Length; j++)
                            {
                                block = block2[j];
                                if (block != null)
                                {
                                    Block block3 = (from blockName in plcBlockMap.Block
                                                    where blockName.Name == block.Name
                                                    select blockName).FirstOrDefault();
                                    if (block3 != null)
                                    {
                                        switch (block3.DeviceCode)
                                        {
                                            case "B":
                                                ScanBit(multiBlock2, block3);
                                                break;
                                            case "W":
                                            case "R":
                                                ScanWord(multiBlock2, block3);
                                                break;
                                        }
                                    }
                                }
                            }
                            Thread.Sleep(multiBlock2.Interval);
                        }
                    }
                }
            }
        }

        private void ScanBit(MultiBlock multiBlock, Block compareBlock)
        {
            int num = 0;
            string str = string.Empty;
            try
            {
                if (!this.bitCompareList.ContainsKey(compareBlock.Name))
                {
                    string str2 = this.ReadBitProtocol(compareBlock.DeviceCode, compareBlock.HeadDevice, compareBlock.Points);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        this.bitCompareList.Add(compareBlock.Name, str2);
                        this.logger.Info(string.Format("[{0}] Block Scan Started...", compareBlock.Name));
                    }
                    else
                    {
                        return;
                    }
                }
                str = this.ReadBitProtocol(compareBlock.DeviceCode, compareBlock.HeadDevice, compareBlock.Points);
                if ((!string.IsNullOrEmpty(str) && (this.bitCompareList[compareBlock.Name] != null)) && (!this.bitCompareList[compareBlock.Name].Equals(str) && (str.Length == compareBlock.Points)))
                {
                    for (int i = 0; i < compareBlock.Item.Length; i++)
                    {
                        num = int.Parse(compareBlock.Item[i].Offset);
                        if (this.bitCompareList[compareBlock.Name].Length < num)
                        {
                            this.logger.Error("Index Error");
                        }
                        else
                        {
                            char ch = this.bitCompareList[compareBlock.Name][num];
                            if (!ch.Equals(str[num]))
                            {
                                this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, compareBlock.Name, compareBlock.Item[i].Name, str[num] == '1', this.ConnectionUnitName));
                                this.logger.Info(string.Format("[EVENT] Local Name : {0} , Item Name: {1}, Item Value : {2}", compareBlock.Name.Substring(0, compareBlock.Name.IndexOf("_")), compareBlock.Item[i].Name, str[num]));
                            }
                        }
                    }
                    this.bitCompareList[compareBlock.Name] = str;
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void ScanThreadProc()
        {
            try
            {
                while (this.m_bScanFlag)
                {
                    this.Scan();
                    Thread.Sleep(0);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        private void ScanWord(MultiBlock multiBlock, Block compareBlock)
        {
            int num = 0;
            string data = string.Empty;
            try
            {
                if (!this.bitCompareList.ContainsKey(compareBlock.Name))
                {
                    data = this.ReadBinaryWord(compareBlock.DeviceCode, compareBlock.HeadDevice, compareBlock.Points, compareBlock.DeviceCode);
                    if (data == null)
                    {
                        return;
                    }
                    data = MProtocolUtils.CharRevcrse(data);
                    this.bitCompareList.Add(compareBlock.Name, data);
                    this.logger.Info(string.Format("[{0}] Block Scan Started...", compareBlock.Name));
                }
                data = this.ReadBinaryWord(compareBlock.DeviceCode, compareBlock.HeadDevice, compareBlock.Points, compareBlock.DeviceCode);
                if (data != null)
                {
                    data = MProtocolUtils.CharRevcrse(data);
                    if ((((((multiBlock.Name != "TRACE") && (multiBlock.Name != "FDC")) && !multiBlock.IsTRACE) && !multiBlock.IsFDC) && ((multiBlock.Name != "RGA") && !multiBlock.IsRGA)) && !this.bitCompareList[compareBlock.Name].Equals(data))
                    {
                        foreach (Item item in compareBlock.Item)
                        {
                            string[] strArray = item.Offset.Split(new char[] { ':' });
                            if (strArray.Length > 1)
                            {
                                num = (int.Parse(strArray[0]) * 0x10) + int.Parse(strArray[1]);
                                char ch = this.bitCompareList[compareBlock.Name][num];
                                if (!ch.Equals(data[num]))
                                {
                                    this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, compareBlock.Name, item.Name, data[num] == '1', this.ConnectionUnitName));
                                    this.logger.Info(string.Format("[EVENT] Local Name : {0} , BlockName : {1}, Item Name: {2}, Item Value : {3}", new object[] { this.ConnectionUnitName, compareBlock.Name, item.Name, data[num] }));
                                }
                            }
                            else if (!this.bitCompareList[compareBlock.Name].Equals(data))
                            {
                                this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, compareBlock.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(data), 2) > 0, this.ConnectionUnitName));
                                this.logger.Info(string.Format("[EVENT] Local Name : {0} , BlockName : {1}, Item Name: {2}, Item Value : {3}", new object[] { this.ConnectionUnitName, compareBlock.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(data), 2) }));
                            }
                        }
                        this.bitCompareList[compareBlock.Name] = data;
                    }
                }
            }
            catch (Exception exception)
            {
                if (this.bitCompareList.ContainsKey(compareBlock.Name))
                {
                    this.bitCompareList[compareBlock.Name] = data;
                }
                this.logger.Error(exception);
            }
        }

        public override void StartReconnect()
        {
            this.m_bReconnectFlag = true;
            this.m_tReconnectionThread = new Thread(new System.Threading.ThreadStart(this.ReConnectThreadProc));
            this.m_tReconnectionThread.Name = "ReConnectThreadProc";
            this.m_tReconnectionThread.IsBackground = true;
            this.m_tReconnectionThread.Start();
        }

        public override void StopReconnect()
        {
            this.m_bReconnectFlag = false;
            if (this.m_tReconnectionThread != null)
            {
                this.m_tReconnectionThread = null;
            }
        }

        public override void ThreadClose()
        {
            if ((this.m_tScanThread != null) && this.m_tScanThread.IsAlive)
            {
                this.m_tScanThread.Abort();
            }
            if ((this.m_tReconnectionThread != null) && this.m_tReconnectionThread.IsAlive)
            {
                this.m_tReconnectionThread.Abort();
            }
            this.ThreadStop();
        }

        public override void ThreadStart()
        {
            this.m_bScanFlag = true;
            this.m_tScanThread = new Thread(new System.Threading.ThreadStart(this.ScanThreadProc));
            this.m_tScanThread.Name = "ScanThreadProc";
            this.m_tScanThread.IsBackground = true;
            this.m_tScanThread.Start();
            this.m_tReconnectionThread = null;
        }

        public override void ThreadStop()
        {
            this.m_bScanFlag = false;
            this.m_bReconnectFlag = false;
            this.m_tScanThread = null;
            this.m_tReconnectionThread = null;
            if ((this.m_tScanThread != null) && this.m_tScanThread.IsAlive)
            {
                this.m_tScanThread.Abort();
            }
            if (this.bitCompareList != null)
            {
                lock (this.m_objLockbitCompareList)
                {
                    this.bitCompareList.Clear();
                }
            }
        }

        public override void WriteCacheData(string deviceCode, string address, string points)
        {
        }

        public override EQPIO.Common.BlockMap BlockMap
        {
            get
            {
                return this.plcBlockMap;
            }
            set
            {
                this.plcBlockMap = value;
            }
        }

        public override string ConnectionUnitName { get; set; }

        public override bool IsConnection { get; set; }

        public override string MultiBlockName { get; set; }

        public override Dictionary<Block, string> ScanDataCollectDictionary { get; set; }
    }
}
