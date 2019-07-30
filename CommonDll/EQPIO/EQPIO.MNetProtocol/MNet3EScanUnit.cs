
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MNet3EScanUnit : IMNetScanUnit
    {
        private TcpClient CommandClient;
        private ILog logger = LogManager.GetLogger(typeof(MNet3EScanUnit));
        private string m_bDirectAccessConnect = "false";
        private bool m_bReconnectFlag = false;
        private bool m_bRunVirtualEQPScan = false;
        private bool m_bScanFlag = false;
        private bool m_bScanStartUpCheck = true;
        private DataGathering m_cacheDataGathering = new DataGathering();
        private DeviceMemory m_DeviceMemory;
        private Dictionary<string, string> m_dicScanData = new Dictionary<string, string>();
        private int m_iCacheRefreshCycle = 300;
        private int m_iMelsecPortNo;
        private int m_iNetworkNo;
        private int m_iStationNo;
        private BinaryReader m_MelsecReader = null;
        private BinaryWriter m_MelsecWriter = null;
        private object m_objDicScanDataLock = new object();
        private object m_objReadWriteer = new object();
        private EQPIO.Common.BlockMap m_plcBlockMap;
        private EQPIO.Common.Scan m_plcScan;
        private string m_strIPAddress;
        private Thread m_tReconnectionThread;
        private Thread m_tScanThread;
        private Make3EProtocol mProtocol;

        public MNet3EScanUnit(ConnectionInfo conn, DataGathering dataGathering, EQPIO.Common.BlockMap blockMap, int melsecProtNo)
        {
            this.ConnectionUnitName = conn.LocalName;
            this.m_strIPAddress = conn.IpAddress;
            this.m_iMelsecPortNo = melsecProtNo;
            this.m_iNetworkNo = Convert.ToInt32(conn.NetworkNo);
            this.m_iStationNo = Convert.ToInt32(conn.PCNo);
            this.m_plcScan = dataGathering.Scan;
            this.m_plcBlockMap = blockMap;
            this.m_bDirectAccessConnect = (conn.DirectAccess != null) ? conn.DirectAccess : "false";
            this.mProtocol = new Make3EProtocol(this.m_iNetworkNo.ToString(), this.m_iStationNo.ToString());
            this.mProtocol.Disconnected += new EventHandler<DisconnectedEventArgs>(this.mProtocol_Disconnected);
            this.mProtocol.Init();
            this.mProtocol.LoggingFlag = false;
        }

        private void AllDataCacheProc(object obj)
        {
            MultiBlock multiblock = (MultiBlock)obj;
            try
            {
                while (this.m_bScanFlag)
                {
                    Thread.Sleep(1);
                    DateTime now = DateTime.Now;
                    byte[] src = this.ReadMultiBlock(multiblock, true);
                    if (multiblock.Name.IndexOf("bit") > 0)
                    {
                        Buffer.BlockCopy(src, 0, (byte[])this.m_DeviceMemory.MemoryList["B"], int.Parse(multiblock.Name.Substring(multiblock.Name.IndexOf("bit") + 3, multiblock.Name.Length - (multiblock.Name.IndexOf("bit") + 3))) * 0xf00, src.Length);
                    }
                    else if (multiblock.Name.IndexOf("word") > 0)
                    {
                        Buffer.BlockCopy(src, 0, (byte[])this.m_DeviceMemory.MemoryList["W"], int.Parse(multiblock.Name.Substring(multiblock.Name.IndexOf("word") + 4, multiblock.Name.Length - (multiblock.Name.IndexOf("word") + 4))) * 0xf00, src.Length);
                    }
                    TimeSpan span = (TimeSpan)(DateTime.Now - now);
                    int millisecondsTimeout = Convert.ToInt32(multiblock.Interval) - ((int)span.TotalMilliseconds);
                    if (millisecondsTimeout > 0)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
            finally
            {
                this.logger.Fatal("CacheThread : " + multiblock.Name);
            }
        }

        public override void CacheThreadStart()
        {
            this.m_bScanFlag = true;
            try
            {
                foreach (MultiBlock block in this.m_cacheDataGathering.Scan.MultiBlock)
                {
                    if ((block != null) && (block.Block != null))
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(this.AllDataCacheProc))
                        {
                            IsBackground = true,
                            Name = block.Name
                        };
                        if (block.Interval <= 100)
                        {
                            thread.Priority = ThreadPriority.Highest;
                        }
                        thread.Start(block);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public override void CacheThreadStop()
        {
            this.ThreadStop();
        }

        public override void Close()
        {
            if (this.m_MelsecReader != null)
            {
                this.m_MelsecReader.Close();
            }
            if (this.m_MelsecWriter != null)
            {
                this.m_MelsecWriter.Close();
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
                this.CommandClient = new TcpClient(this.m_strIPAddress, this.m_iMelsecPortNo);
                this.CommandClient.ReceiveTimeout = 0x2710;
                this.CommandClient.Client.Blocking = true;
                this.m_MelsecReader = new BinaryReader(this.CommandClient.GetStream());
                this.m_MelsecWriter = new BinaryWriter(this.CommandClient.GetStream());
                if ((this.m_MelsecReader != null) && (this.m_MelsecWriter != null))
                {
                    this.mProtocol.SetMelsetReaderWriter(this.m_MelsecReader, this.m_MelsecWriter, this.ConnectionUnitName);
                    this.IsConnection = true;
                    this.logger.Info(string.Format("[Connection] Melsec Connect Address : {0} , PortNo : {1}", this.m_strIPAddress, this.m_iMelsecPortNo));
                }
                else
                {
                    this.IsConnection = false;
                    this.logger.Error("Reader or Writer is NULL");
                }
                return this.IsConnection;
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

        private string ConverBitString(string str)
        {
            string data = string.Empty;
            try
            {
                for (int i = 0; (i + 4) <= str.Length; i += 4)
                {
                    int num2 = Convert.ToInt32(str.Substring(i, 4), 0x10);
                    data = data + MProtocolUtils.CharRevcrse(Convert.ToString(num2, 2).PadLeft(0x10, '0'));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
            return MProtocolUtils.CharRevcrse(data);
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

        private void InitMemory(int bPoints, int wPoints)
        {
            try
            {
                this.m_DeviceMemory = new DeviceMemory();
                this.m_DeviceMemory.Add_Device("B", 160, DeviceMemory.DeviceType.Bit, bPoints);
                this.m_DeviceMemory.Add_Device("W", 180, DeviceMemory.DeviceType.Word, wPoints);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public override void ListUpCacheBlock()
        {
            int num = -1;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            try
            {
                MultiBlock block2;
                Block block3;
                foreach (Block block in this.m_plcBlockMap.Block)
                {
                    if (block.DeviceCode == "B")
                    {
                        int num5 = Convert.ToInt32(block.HeadDevice, 0x10) + block.Points;
                        if ((num == -1) || (num > num5))
                        {
                            num = num5 - block.Points;
                        }
                        if ((num2 == -1) || (num2 < num5))
                        {
                            num2 = num5;
                        }
                    }
                    else if (block.DeviceCode == "W")
                    {
                        int num6 = Convert.ToInt32(block.HeadDevice, 0x10) + block.Points;
                        if ((num3 == -1) || (num3 > num6))
                        {
                            num3 = num6 - block.Points;
                        }
                        if ((num4 == -1) || (num4 < num6))
                        {
                            num4 = num6;
                        }
                    }
                }
                this.InitMemory(num2 - num, num4 - num3);
                this.m_cacheDataGathering.Scan = new EQPIO.Common.Scan();
                int num7 = 0x80;
                int num8 = ((num2 - num) / 0x3c00) + ((((num2 - num) % 0x3c00) > 0) ? 1 : 0);
                int num9 = ((num4 - num3) / 960) + ((((num4 - num3) % 960) > 0) ? 1 : 0);
                num7 = num8 + num9;
                this.m_cacheDataGathering.Scan.MultiBlock = new MultiBlock[num7];
                int index = 0;
                int num11 = 0;
                bool flag = false;
                while ((num <= num2) && !flag)
                {
                    block2 = new MultiBlock
                    {
                        Block = new Block[1]
                    };
                    block3 = new Block
                    {
                        Name = "bit" + index.ToString(),
                        DeviceCode = "B",
                        HeadDevice = Convert.ToString(num, 0x10)
                    };
                    if ((num2 - num) >= 0x3c00)
                    {
                        block3.Points = 0x3c00;
                        num += 0x3c00;
                    }
                    else
                    {
                        block3.Points = num2 - num;
                        num += num2 - num;
                        flag = true;
                    }
                    block2.Name = "mb_" + block3.Name;
                    block2.Block[0] = block3;
                    block2.Interval = this.m_iCacheRefreshCycle;
                    this.m_cacheDataGathering.Scan.MultiBlock[index] = block2;
                    index++;
                }
                flag = false;
                while ((num3 <= num4) && !flag)
                {
                    block2 = new MultiBlock
                    {
                        Block = new Block[1]
                    };
                    block3 = new Block
                    {
                        Name = "word" + num11.ToString(),
                        DeviceCode = "W",
                        HeadDevice = Convert.ToString(num3, 0x10)
                    };
                    if ((num4 - num3) >= 960)
                    {
                        block3.Points = 960;
                        num3 += 960;
                    }
                    else
                    {
                        block3.Points = num4 - num3;
                        num3 += num4 - num3;
                        flag = true;
                    }
                    block2.Name = "mb_" + block3.Name;
                    block2.Block[0] = block3;
                    block2.Interval = 200;
                    this.m_cacheDataGathering.Scan.MultiBlock[num11 + index] = block2;
                    num11++;
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void mProtocol_Disconnected(object sender, DisconnectedEventArgs e)
        {
            this.OnDisconnected(e);
        }

        private string ReadBitProtocol(string device, string address, int length)
        {
            Make3EProtocol.BlockType bit = Make3EProtocol.BlockType.Bit;
            byte[] dataArry = this.mProtocol.GetSingleRead(device, address, length, bit);
            return this.ReadWriter(dataArry, IMNetScanUnit.ReceiveStatus.Read);
        }

        public override string ReadCacheData(string deviceCode, string address, string points)
        {
            try
            {
                if ((this.m_DeviceMemory == null) || (this.m_DeviceMemory.MemoryList == null))
                {
                    return string.Empty;
                }
                string str = string.Empty;
                byte[] dst = null;
                int srcOffset = 0;
                if (this.m_DeviceMemory.MemoryList.ContainsKey(deviceCode))
                {
                    if (deviceCode == "B")
                    {
                        srcOffset = (Convert.ToInt32(address) / 0x10) * 4;
                        dst = new byte[4];
                        Buffer.BlockCopy((byte[])this.m_DeviceMemory.MemoryList["B"], srcOffset, dst, 0, 4);
                        str = MProtocolUtils.CharRevcrse(this.ConverBitString(Encoding.ASCII.GetString(dst)))[Convert.ToInt32(address) - (srcOffset * 4)].ToString();
                    }
                    else if (deviceCode == "W")
                    {
                        srcOffset = Convert.ToInt32(address) * 4;
                        dst = new byte[4 * Convert.ToInt32(points)];
                        Buffer.BlockCopy((byte[])this.m_DeviceMemory.MemoryList["W"], srcOffset, dst, 0, 4 * Convert.ToInt32(points));
                        str = Encoding.ASCII.GetString(dst);
                        string str2 = string.Empty;
                        for (int i = 0; i < (str.Length / 4); i++)
                        {
                            str2 = str2 + Convert.ToChar(int.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(str.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
                        }
                        str = str2;
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return string.Empty;
            }
        }

        private string Reader(IMNetScanUnit.ReceiveStatus status)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            try
            {
                string str3;
                byte[] buffer = new byte[14];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                str = Encoding.ASCII.GetString(buffer);
                int num = Convert.ToInt32(str, 0x10);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                str2 = Encoding.ASCII.GetString(buffer);
                if (str2 != "0000")
                {
                    this.logger.Info(string.Format("[Receive] Receive NG : Plc Name : {0} , ReadData : {1}", this.ConnectionUnitName, str2));
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        str3 = Encoding.ASCII.GetString(buffer);
                        this.logger.Error(string.Format("[Receive] Receive NG : Garbage String : {0}", str3));
                        this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return string.Empty;
                }
                if (status == IMNetScanUnit.ReceiveStatus.Write)
                {
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        str3 = Encoding.ASCII.GetString(buffer);
                        this.logger.Error(string.Format("[Receive] Receive NG : Garbage String : {0}", str3));
                        this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return str2;
                }
                int num2 = Convert.ToInt32(str, 0x10);
                if (num2 <= 4)
                {
                    this.logger.Error(string.Format("Invalid Data Length : {0}", num2));
                    return string.Empty;
                }
                buffer = new byte[num2 - 4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer);
            }
            catch (IOException exception)
            {
                this.logger.Error(string.Format("IOException Error : {0}", exception.Message));
                this.OnDisconnected(new DisconnectedEventArgs(this));
                return string.Empty;
            }
            catch (Exception exception2)
            {
                this.logger.Error(string.Format("Message : {0}", exception2.Message));
                this.OnDisconnected(new DisconnectedEventArgs(this));
                return string.Empty;
            }
        }

        private byte[] Reader(IMNetScanUnit.ReceiveStatus status, bool isCacheFun)
        {
            try
            {
                string str = string.Empty;
                string str2 = string.Empty;
                byte[] buffer = new byte[14];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                str2 = Encoding.ASCII.GetString(buffer);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                if (Encoding.ASCII.GetString(buffer) != "0000")
                {
                    return null;
                }
                if (status != IMNetScanUnit.ReceiveStatus.Write)
                {
                    int num = Convert.ToInt32(str2, 0x10);
                    if (num <= 4)
                    {
                        return null;
                    }
                    buffer = new byte[num - 4];
                    this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                    str = Encoding.ASCII.GetString(buffer);
                }
                return buffer;
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

        private string ReadMultiBlock(MultiBlock multiblock)
        {
            if (multiblock == null)
            {
                return string.Empty;
            }
            foreach (Block block in multiblock.Block)
            {
                if (block == null)
                {
                    return string.Empty;
                }
            }
            string str = string.Empty;
            int num = 0;
            try
            {
                byte[] multiblockRead = this.mProtocol.GetMultiblockRead(multiblock);
                str = this.ReadWriter(multiblockRead, IMNetScanUnit.ReceiveStatus.Read);
                foreach (Block block in multiblock.Block)
                {
                    if (block != null)
                    {
                        if (block.DeviceCode == "B")
                        {
                            num += (int)Math.Ceiling((double)(((double)block.Points) / 16.0));
                        }
                        else if ((block.DeviceCode == "W") || (block.DeviceCode == "R"))
                        {
                            num += block.Points;
                        }
                    }
                }
                num *= 4;
                if ((num == 0) || (str.Length < num))
                {
                    return string.Empty;
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("expectLength: {0}, readWord.Length: {1}, Error: {2}", num, str.Length, exception));
                return string.Empty;
            }
            return str;
        }

        private byte[] ReadMultiBlock(MultiBlock multiblock, bool isCacheFun)
        {
            if (multiblock == null)
            {
                return null;
            }
            foreach (Block block in multiblock.Block)
            {
                if (block == null)
                {
                    return null;
                }
            }
            byte[] multiblockRead = this.mProtocol.GetMultiblockRead(multiblock);
            return this.ReadWriter(multiblockRead, IMNetScanUnit.ReceiveStatus.Read, isCacheFun);
        }

        public Dictionary<string, string> ReadRWordOnce(Block block, bool isHex, string networkNo, string pcNo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                return this.mProtocol.ReadRWordOnce(block, isHex, networkNo, pcNo);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
            return dictionary;
        }

        public Dictionary<string, string> ReadWord(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                return this.mProtocol.ReadWord(block, isHex);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
            return dictionary;
        }

        public Dictionary<string, string> ReadWordOnce(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                return this.mProtocol.ReadWordOnce(block, isHex);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
            return dictionary;
        }

        private string ReadWriter(byte[] dataArry, IMNetScanUnit.ReceiveStatus status)
        {
            string str;
            try
            {
                if ((this.m_MelsecWriter == null) || (this.m_MelsecReader == null))
                {
                    str = "0";
                }
                else
                {
                    lock (this.m_objReadWriteer)
                    {
                        this.m_MelsecWriter.Write(dataArry, 0, dataArry.Length);
                        str = this.Reader(status);
                    }
                }
            }
            catch (IOException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                str = string.Empty;
            }
            catch (ObjectDisposedException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                str = string.Empty;
            }
            return str;
        }

        private byte[] ReadWriter(byte[] dataArry, IMNetScanUnit.ReceiveStatus status, bool isCacheFun)
        {
            byte[] buffer;
            try
            {
                if (this.m_MelsecWriter == null)
                {
                    buffer = null;
                }
                else
                {
                    lock (this.m_objReadWriteer)
                    {
                        this.m_MelsecWriter.Write(dataArry, 0, dataArry.Length);
                        buffer = this.Reader(status, isCacheFun);
                    }
                }
            }
            catch (IOException)
            {
                buffer = null;
            }
            catch (ObjectDisposedException)
            {
                buffer = null;
            }
            return buffer;
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
            Block bitScanTargetBlock = null;
            DateTime now = DateTime.Now;
            TimeSpan span = new TimeSpan();
            if (this.m_plcScan.MultiBlock == null)
            {
                this.logger.Error("ScanBlock is not set");
            }
            else
            {
                foreach (MultiBlock block2 in this.m_plcScan.MultiBlock)
                {
                    if (this.m_bScanStartUpCheck)
                    {
                        Globalproperties.Instance.UpdateScanStatus(enumConnectionType.PLCEthernet, this.ConnectionUnitName, block2, block2.StartUp.ToUpper() == "TRUE");
                        if (block2.StartUp.ToLower() == "false")
                        {
                            continue;
                        }
                    }
                    else if (!Globalproperties.Instance.ScanStatusPLCEhternet.MultiBlockOnOff[this.ConnectionUnitName][block2])
                    {
                        continue;
                    }
                    if ((this.m_bDirectAccessConnect.ToLower() != "true") || ((block2.PCNo == this.m_iStationNo.ToString()) && (block2.NetworkNo == this.m_iNetworkNo.ToString())))
                    {
                        lock (this.m_objDicScanDataLock)
                        {
                            now = DateTime.Now;
                            Func<Block, bool> predicate = null;
                            foreach (Block block in block2.Block)
                            {
                                if (block != null)
                                {
                                    if (predicate == null)
                                    {
                                        predicate = blockName => blockName.Name == block.Name;
                                    }
                                    bitScanTargetBlock = this.m_plcBlockMap.Block.Where<Block>(predicate).FirstOrDefault<Block>();
                                    if (bitScanTargetBlock != null)
                                    {
                                        string deviceCode = bitScanTargetBlock.DeviceCode;
                                        if (deviceCode != null)
                                        {
                                            if (!(deviceCode == "B"))
                                            {
                                                if (deviceCode == "W")
                                                {
                                                    goto Label_023D;
                                                }
                                                if ((deviceCode == "R") || (deviceCode == "ZR"))
                                                {
                                                    goto Label_0248;
                                                }
                                            }
                                            else
                                            {
                                                this.ScanBit(block2, bitScanTargetBlock);
                                            }
                                        }
                                    }
                                }
                                continue;
                            Label_023D:
                                this.ScanWord(block2, bitScanTargetBlock);
                                continue;
                            Label_0248:
                                this.ScanRWord(block2, bitScanTargetBlock, block2.NetworkNo, block2.PCNo);
                            }
                            span = (TimeSpan)(DateTime.Now - now);
                            if ((block2.Interval - ((int)span.TotalMilliseconds)) > 0)
                            {
                                Thread.Sleep((int)(block2.Interval - ((int)span.TotalMilliseconds)));
                            }
                            else
                            {
                                Thread.Sleep(0);
                            }
                        }
                    }
                }
                this.m_bScanStartUpCheck = false;
            }
        }

        private void ScanBit(MultiBlock multiBlock, Block bitScanTargetBlock)
        {
            string readBitString = string.Empty;
            int num = 0;
            try
            {
                if (multiBlock.Name == "LinkSignal")
                {
                    readBitString = this.ReadBitProtocol(bitScanTargetBlock.DeviceCode, bitScanTargetBlock.HeadDevice, bitScanTargetBlock.Points);
                    this.OnLinkSignalScanReceived(new LinkSignalScanReceivedEventArgs(readBitString, 0, bitScanTargetBlock));
                }
                else
                {
                    if (!this.m_dicScanData.ContainsKey(bitScanTargetBlock.Name))
                    {
                        string str2 = this.ReadBitProtocol(bitScanTargetBlock.DeviceCode, bitScanTargetBlock.HeadDevice, bitScanTargetBlock.Points);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this.m_dicScanData.Add(bitScanTargetBlock.Name, str2);
                            this.logger.Info(string.Format("[{0}] Block Scan Started...", bitScanTargetBlock.Name));
                        }
                        else
                        {
                            this.logger.Error(string.Format("[{0}] Block Scan Start Failed...", bitScanTargetBlock.Name));
                            return;
                        }
                    }
                    readBitString = this.ReadBitProtocol(bitScanTargetBlock.DeviceCode, bitScanTargetBlock.HeadDevice, bitScanTargetBlock.Points);
                    if ((!string.IsNullOrEmpty(readBitString) && (this.m_dicScanData[bitScanTargetBlock.Name] != null)) && !this.m_dicScanData[bitScanTargetBlock.Name].Equals(readBitString))
                    {
                        for (int i = 0; i < bitScanTargetBlock.Item.Length; i++)
                        {
                            num = int.Parse(bitScanTargetBlock.Item[i].Offset);
                            if (this.m_dicScanData[bitScanTargetBlock.Name].Length < num)
                            {
                                this.logger.Error("Index Error");
                            }
                            else
                            {
                                char ch = this.m_dicScanData[bitScanTargetBlock.Name][num];
                                if (!ch.Equals(readBitString[num]))
                                {
                                    if (false)
                                    {
                                        string name = bitScanTargetBlock.Name;
                                        for (int j = 2; j <= 4; j++)
                                        {
                                            name = "L" + j.ToString() + name.Substring(name.IndexOf("_"), name.Length - name.IndexOf("_"));
                                            this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, name, bitScanTargetBlock.Item[i].Name, readBitString[num] == '1', this.ConnectionUnitName));
                                            Thread.Sleep(0);
                                        }
                                    }
                                    else if (this.m_bRunVirtualEQPScan)
                                    {
                                        this.OnVirtaulEQPScanReceived(new ScanReceivedEventArgs(multiBlock.Name, bitScanTargetBlock.Name, bitScanTargetBlock.Item[i].Name, readBitString[num] == '1', this.ConnectionUnitName));
                                    }
                                    else
                                    {
                                        this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, bitScanTargetBlock.Name, bitScanTargetBlock.Item[i].Name, readBitString[num] == '1', this.ConnectionUnitName));
                                    }
                                    if (!Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(bitScanTargetBlock.Item[i].Name.ToUpper()))
                                    {
                                        if (this.m_bRunVirtualEQPScan)
                                        {
                                            this.logger.Info(string.Format("[vEQP-EVENT] Local Name : {0} , Item Name: {1}, Item Value : {2}, index : {3}", new object[] { bitScanTargetBlock.Name.Substring(0, bitScanTargetBlock.Name.IndexOf("_")), bitScanTargetBlock.Item[i].Name, readBitString[num], num }));
                                        }
                                        else
                                        {
                                            this.logger.Info(string.Format("[EVENT] Local Name : {0} , Item Name: {1}, Item Value : {2}, index : {3}", new object[] { bitScanTargetBlock.Name.Substring(0, bitScanTargetBlock.Name.IndexOf("_")), bitScanTargetBlock.Item[i].Name, readBitString[num], num }));
                                        }
                                    }
                                }
                            }
                        }
                        this.m_dicScanData[bitScanTargetBlock.Name] = readBitString;
                    }
                }
            }
            catch (Exception exception)
            {
                if (this.m_dicScanData.ContainsKey(bitScanTargetBlock.Name))
                {
                    this.m_dicScanData[bitScanTargetBlock.Name] = readBitString;
                }
                this.logger.Error(exception);
            }
        }

        private void ScanMultiBlock(object obj)
        {
            MultiBlock multiblock = (MultiBlock)obj;
            int num = 0;
            try
            {
                while (this.m_bScanFlag)
                {
                    int num2;
                    int num3;
                    string str2;
                    Thread.Sleep(1);
                    DateTime now = DateTime.Now;
                    if (!this.m_dicScanData.ContainsKey(multiblock.Name))
                    {
                        string str = this.ReadMultiBlock(multiblock);
                        if (string.IsNullOrEmpty(str))
                        {
                            continue;
                        }
                        this.m_dicScanData.Add(multiblock.Name, str);
                        num2 = 0;
                        num3 = 0;
                        str2 = string.Empty;
                        foreach (Block block2 in multiblock.Block)
                        {
                            if (block2 != null)
                            {
                                if (block2.DeviceCode == "B")
                                {
                                    num3 = ((int)Math.Ceiling((double)(((double)block2.Points) / 16.0))) * 4;
                                }
                                else if (((block2.DeviceCode == "W") || (block2.DeviceCode == "R")) || (block2.DeviceCode == "ZR"))
                                {
                                    num3 = block2.Points * 4;
                                }
                                if ((num2 + num3) <= str.Length)
                                {
                                    str2 = str.Substring(num2, num3);
                                    str2 = MProtocolUtils.CharRevcrse(this.ConverBitString(str2));
                                    this.m_dicScanData.Add(block2.Name, str2);
                                    num2 += num3;
                                }
                            }
                        }
                    }
                    string str3 = this.ReadMultiBlock(multiblock);
                    if (!string.IsNullOrEmpty(str3) && (this.m_dicScanData[multiblock.Name] != null))
                    {
                        if (!this.m_dicScanData[multiblock.Name].Equals(str3))
                        {
                            num2 = 0;
                            num3 = 0;
                            str2 = string.Empty;
                            foreach (Block block2 in multiblock.Block)
                            {
                                if (block2 != null)
                                {
                                    if (block2.DeviceCode == "B")
                                    {
                                        num3 = ((int)Math.Ceiling((double)(((double)block2.Points) / 16.0))) * 4;
                                    }
                                    else if ((block2.DeviceCode == "W") || (block2.DeviceCode == "R"))
                                    {
                                        num3 = block2.Points * 4;
                                    }
                                    str2 = str3.Substring(num2, num3);
                                    str2 = MProtocolUtils.CharRevcrse(this.ConverBitString(str2));
                                    if (!this.m_dicScanData[block2.Name].Equals(str2))
                                    {
                                        char ch;
                                        if (block2.DeviceCode == "B")
                                        {
                                            for (int i = 0; i < block2.Item.Length; i++)
                                            {
                                                num = int.Parse(block2.Item[i].Offset);
                                                ch = this.m_dicScanData[block2.Name][num];
                                                if (!ch.Equals(str2[num]))
                                                {
                                                    this.OnScanReceived(new ScanReceivedEventArgs(multiblock.Name, block2.Name, block2.Item[i].Name, str2[num] == '1', this.ConnectionUnitName));
                                                    this.logger.Info(string.Format("[EVENT] Local Name : {0}, Block Name : {1}, Item Name: {2}, Item Value : {3}, index : {4}", new object[] { this.ConnectionUnitName, block2.Name, block2.Item[i].Name, str2[num], num }));
                                                }
                                            }
                                        }
                                        else if (((block2.DeviceCode == "W") || (block2.DeviceCode == "R")) || (block2.DeviceCode == "ZR"))
                                        {
                                            foreach (Item item in block2.Item)
                                            {
                                                string[] strArray = item.Offset.Split(new char[] { ':' });
                                                if (strArray.Length > 1)
                                                {
                                                    num = (int.Parse(strArray[0]) * 0x10) + int.Parse(strArray[1]);
                                                    ch = this.m_dicScanData[block2.Name][num];
                                                    if (!ch.Equals(str2[num]))
                                                    {
                                                        this.OnScanReceived(new ScanReceivedEventArgs(multiblock.Name, block2.Name, item.Name, str2[num] == '1', this.ConnectionUnitName));
                                                        this.logger.Info(string.Format("[EVENT] Local Name : {0} , Block Name : {1}, Item Name: {2}, Item Value : {3}", new object[] { this.ConnectionUnitName, block2.Name, item.Name, str2[num] }));
                                                    }
                                                }
                                                else if (!this.m_dicScanData[block2.Name].Equals(str2))
                                                {
                                                    this.OnScanReceived(new ScanReceivedEventArgs(multiblock.Name, block2.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(str2), 0x10) > 0, this.ConnectionUnitName));
                                                    this.logger.Info(string.Format("[EVENT] Local Name : {0} , BlockName : {1}, Item Name: {2}, Item Value : {3}", new object[] { this.ConnectionUnitName, block2.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(str2), 0x10) }));
                                                }
                                            }
                                        }
                                        this.m_dicScanData[block2.Name] = str2;
                                    }
                                    num2 += num3;
                                }
                            }
                            this.m_dicScanData[multiblock.Name] = str3;
                        }
                        TimeSpan span = (TimeSpan)(DateTime.Now - now);
                        int millisecondsTimeout = Convert.ToInt32(multiblock.Interval) - ((int)span.TotalMilliseconds);
                        if (millisecondsTimeout >= 0)
                        {
                            Thread.Sleep(millisecondsTimeout);
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void ScanRWord(MultiBlock multiBlock, Block wordScanTargetBlock, string networkNo, string pcNo)
        {
            try
            {
                Dictionary<string, string> dictionary;
                if ((((multiBlock.Name == "TRACE") || (multiBlock.Name == "FDC")) || multiBlock.IsTRACE) || multiBlock.IsFDC)
                {
                    dictionary = new Dictionary<string, string>();
                    dictionary = this.ReadRWordOnce(wordScanTargetBlock, false, networkNo, pcNo);
                    this.OnFDCScanReceived(new FDCScanReceivedEventArgs(wordScanTargetBlock.Name, dictionary));
                }
                else if ((multiBlock.Name == "RGA") || multiBlock.IsRGA)
                {
                    dictionary = new Dictionary<string, string>();
                    dictionary = this.ReadRWordOnce(wordScanTargetBlock, false, networkNo, pcNo);
                    this.OnRGAScanReceived(new FDCScanReceivedEventArgs(wordScanTargetBlock.Name, dictionary));
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

        private void ScanWord(MultiBlock multiBlock, Block wordScanTargetBlock)
        {
            string str = string.Empty;
            int num = 0;
            try
            {
                Dictionary<string, string> dictionary;
                if ((((multiBlock.Name == "TRACE") || (multiBlock.Name == "FDC")) || multiBlock.IsTRACE) || multiBlock.IsFDC)
                {
                    dictionary = new Dictionary<string, string>();
                    dictionary = this.ReadWordOnce(wordScanTargetBlock, true);
                    this.OnFDCScanReceived(new FDCScanReceivedEventArgs(wordScanTargetBlock.Name, dictionary));
                }
                else if ((multiBlock.Name == "RGA") || multiBlock.IsRGA)
                {
                    dictionary = new Dictionary<string, string>();
                    dictionary = this.ReadWordOnce(wordScanTargetBlock, true);
                    this.OnRGAScanReceived(new FDCScanReceivedEventArgs(wordScanTargetBlock.Name, dictionary));
                }
                else
                {
                    if (!this.m_dicScanData.ContainsKey(wordScanTargetBlock.Name))
                    {
                        string str2 = this.mProtocol.ReadBinaryWord(wordScanTargetBlock.DeviceCode, wordScanTargetBlock.HeadDevice, wordScanTargetBlock.Points, Make3EProtocol.BlockType.Word);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = MProtocolUtils.CharRevcrse(str2);
                            this.m_dicScanData.Add(wordScanTargetBlock.Name, str2);
                            this.logger.Info(string.Format("[{0}] Block Scan Started...", wordScanTargetBlock.Name));
                        }
                        else
                        {
                            this.logger.Error(string.Format("[{0}] Block Scan Start Failed...", wordScanTargetBlock.Name));
                            return;
                        }
                    }
                    str = this.mProtocol.ReadBinaryWord(wordScanTargetBlock.DeviceCode, wordScanTargetBlock.HeadDevice, wordScanTargetBlock.Points, Make3EProtocol.BlockType.Word);
                    if (!string.IsNullOrEmpty(str) && (this.m_dicScanData[wordScanTargetBlock.Name] != null))
                    {
                        str = MProtocolUtils.CharRevcrse(str);
                        if (!this.m_dicScanData[wordScanTargetBlock.Name].Equals(str))
                        {
                            foreach (Item item in wordScanTargetBlock.Item)
                            {
                                string[] strArray = item.Offset.Split(new char[] { ':' });
                                if (strArray.Length > 1)
                                {
                                    num = (int.Parse(strArray[0]) * 0x10) + int.Parse(strArray[1]);
                                    char ch = this.m_dicScanData[wordScanTargetBlock.Name][num];
                                    if (!ch.Equals(str[num]))
                                    {
                                        this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, wordScanTargetBlock.Name, item.Name, str[num] == '1', this.ConnectionUnitName));
                                        this.logger.Info(string.Format("[EVENT] Local Name : {0} , BlockName : {1}, Item Name: {2}, Item Value : {3}", new object[] { wordScanTargetBlock.Name.Substring(0, wordScanTargetBlock.Name.IndexOf("_")), wordScanTargetBlock.Name, item.Name, str[num] }));
                                    }
                                }
                                else if (!this.m_dicScanData[wordScanTargetBlock.Name].Equals(str))
                                {
                                    this.OnScanReceived(new ScanReceivedEventArgs(multiBlock.Name, wordScanTargetBlock.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(str), 2) > 0, this.ConnectionUnitName));
                                    this.logger.Info(string.Format("[EVENT] Local Name : {0} , BlockName : {1}, Item Name: {2}, Item Value : {3}", new object[] { wordScanTargetBlock.Name.Substring(0, wordScanTargetBlock.Name.IndexOf("_")), wordScanTargetBlock.Name, item.Name, Convert.ToInt32(MProtocolUtils.CharRevcrse(str), 2) }));
                                }
                            }
                            this.m_dicScanData[wordScanTargetBlock.Name] = str;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (this.m_dicScanData.ContainsKey(wordScanTargetBlock.Name))
                {
                    this.m_dicScanData[wordScanTargetBlock.Name] = str;
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
            if (this.m_dicScanData != null)
            {
                lock (this.m_objDicScanDataLock)
                {
                    this.m_dicScanData.Clear();
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
                return this.m_plcBlockMap;
            }
            set
            {
                this.m_plcBlockMap = value;
            }
        }

        public override string ConnectionUnitName { get; set; }

        public override bool IsConnection { get; set; }

        public override string MultiBlockName { get; set; }

        public bool RunVirtualEQPScan
        {
            get
            {
                return this.m_bRunVirtualEQPScan;
            }
            set
            {
                this.m_bRunVirtualEQPScan = value;
            }
        }

        public override Dictionary<Block, string> ScanDataCollectDictionary { get; set; }
    }
}
