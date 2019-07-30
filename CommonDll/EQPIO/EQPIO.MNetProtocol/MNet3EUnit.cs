
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MNet3EUnit : IMNetUnit
    {
        private BinaryReader _MelsecReader = null;
        private BinaryWriter _MelsecWriter = null;
        private Make3EProtocol _mProtocol;
        private TcpClient CommandClient;
        private ILog logger = LogManager.GetLogger(typeof(MNet3EUnit));
        private bool m_bReconnectFlag = false;
        private bool m_bTimeoutCheckFlag = false;
        private int m_iMelsecPortNo;
        private int m_iNetworkNo;
        private int m_iStationNo;
        private string m_strIPAddress;
        private Thread m_tReconnectionThread;
        private Thread m_tTimeoutCheckThread;
        private object readWriteObj = new object();

        public MNet3EUnit(ConnectionInfo conn, DataGathering dataGathering, BlockMap plcMap, Transaction trx, int melsecProtNo)
        {
            this.m_strIPAddress = conn.IpAddress;
            this.m_iMelsecPortNo = melsecProtNo;
            this.m_iNetworkNo = Convert.ToInt32(conn.NetworkNo);
            this.m_iStationNo = Convert.ToInt32(conn.PCNo);
            this.Name = conn.LocalName;
            this._mProtocol = new Make3EProtocol(this.m_iNetworkNo.ToString(), this.m_iStationNo.ToString());
            this._mProtocol.Init();
        }

        public override void Close()
        {
            if ((this.CommandClient != null) && this.CommandClient.Connected)
            {
                this.CommandClient.Client.Close();
            }
            if (this._mProtocol != null)
            {
                this._mProtocol.CloseMelsetReaderWriter();
            }
            if (this._MelsecReader != null)
            {
                this._MelsecReader.Close();
            }
            if (this._MelsecWriter != null)
            {
                this._MelsecWriter.Close();
            }
        }

        private bool Connect()
        {
            try
            {
                this.CommandClient = new TcpClient(this.m_strIPAddress, this.m_iMelsecPortNo);
                this.CommandClient.ReceiveTimeout = 0x1f40;
                this.CommandClient.Client.Blocking = true;
                this._MelsecReader = new BinaryReader(this.CommandClient.GetStream());
                this._MelsecWriter = new BinaryWriter(this.CommandClient.GetStream());
                if ((this._MelsecReader != null) && (this._MelsecWriter != null))
                {
                    this._mProtocol.SetMelsetReaderWriter(this._MelsecReader, this._MelsecWriter, this.Name);
                }
                this.logger.Info(string.Format("[Connection] Melsec Connect Address : {0} , PortNo : {1}", this.m_strIPAddress, this.m_iMelsecPortNo));
                return true;
            }
            catch (SocketException exception)
            {
                this.logger.Error(string.Format("[Connection] Ethernet SocketException : {0}", exception.Message));
                return false;
            }
            catch (Exception exception2)
            {
                this.logger.Error(string.Format("[Connection] Ethernet Error : {0}", exception2.Message));
                return false;
            }
        }

        public string GetAddress(string DeviceCode, string address, int offset)
        {
            if ((DeviceCode == "R") || (DeviceCode == "ZR"))
            {
                address = Convert.ToString((int)(int.Parse(address) + offset));
                return address;
            }
            address = Convert.ToString((int)(int.Parse(address, NumberStyles.HexNumber) + offset), 0x10).ToUpper();
            return address;
        }

        public override void Init()
        {
            if (!this.Connect())
            {
                this.ReConnect();
            }
            else
            {
                this.OnConnected(new ConnectedEventArgs(this));
            }
        }

        protected override void OnConnected(ConnectedEventArgs e)
        {
            base.OnConnected(e);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            base.OnDisconnected(e);
        }

        public override Dictionary<string, string> ReadBit(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Make3EProtocol.BlockType bit = Make3EProtocol.BlockType.Bit;
            string headDevice = block.HeadDevice;
            string name = block.Name;
            try
            {
                string str3 = this.ReadBitProtocol(block.DeviceCode, block.HeadDevice, block.Points, bit);
                if (str3 == null)
                {
                    return dictionary;
                }
                foreach (Item item in block.Item)
                {
                    string str4 = str3.Substring(int.Parse(item.Offset), 1);
                    dictionary.Add(item.Name, str4);
                    this.logger.Info(string.Format("[ReadBit] Item Name : {0}, Data : {1}", item.Name, str4));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Read Bit Error : {0}, address : {1}, blockname : {2} ", exception.Message, headDevice, name));
            }
            return dictionary;
        }

        private string ReadBitProtocol(string device, string address, int length, Make3EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetSingleRead(device, address, length, blockType);
            return this.ReadWriter(dataArry, ReceiveStatus.Read);
        }

        private string Reader(ReceiveStatus status)
        {
            try
            {
                int num;
                string str = string.Empty;
                string str2 = string.Empty;
                byte[] buffer = new byte[14];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[4];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                str2 = Encoding.ASCII.GetString(buffer);
                buffer = new byte[4];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                str = Encoding.ASCII.GetString(buffer);
                if (str != "0000")
                {
                    this.logger.Info(string.Format("[Receive] Receive NG : Plc Name : {0} , ReadData : {1}", this.Name, str));
                    num = Convert.ToInt32(str2, 0x10);
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        string str3 = Encoding.ASCII.GetString(buffer);
                        this.logger.Error(string.Format("[Receive] Receive NG : Garbage String : {0}", str3));
                        this._MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return null;
                }
                if (status == ReceiveStatus.Write)
                {
                    num = Convert.ToInt32(str2, 0x10);
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        this._MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return str;
                }
                buffer = new byte[Convert.ToInt32(str2, 0x10) - 4];
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

        private byte[] Reader(ReceiveStatus status, bool isCacheFun)
        {
            try
            {
                string str = string.Empty;
                string str2 = string.Empty;
                byte[] buffer = new byte[14];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[4];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                str2 = Encoding.ASCII.GetString(buffer);
                buffer = new byte[4];
                this._MelsecReader.Read(buffer, 0, buffer.Length);
                if (Encoding.ASCII.GetString(buffer) != "0000")
                {
                    return null;
                }
                if (status != ReceiveStatus.Write)
                {
                    int num = Convert.ToInt32(str2, 0x10);
                    if (num <= 4)
                    {
                        return null;
                    }
                    buffer = new byte[num - 4];
                    this._MelsecReader.Read(buffer, 0, buffer.Length);
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

        private string ReadRASCIIWord(string device, string address, int length, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, length, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRASCIIWord) address : {0}", address));
                return null;
            }
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + Convert.ToChar(int.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(str.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
            }
            return str2.TrimEnd(new char[1]).Trim();
        }

        private string ReadRBinaryWord(string device, string address, int length, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            int num2;
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, length, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRBinaryWord) address : {0}", address));
                return null;
            }
            int num = 0;
            string str2 = string.Empty;
            for (num2 = 0; num2 < (str.Length / 4); num2++)
            {
                num = Convert.ToInt32(str.Substring(num2 * 4, 4), 0x10);
                str2 = str2 + Convert.ToString(num, 2).PadLeft(0x10, '0');
            }
            string str3 = string.Empty;
            for (num2 = 0; num2 < (str2.Length / 0x10); num2++)
            {
                str3 = str2.Substring(num2 * 0x10, 0x10) + str3;
            }
            return str3;
        }

        public override Dictionary<string, string> ReadRBit(Block block, bool isHex, int networkNo, int pcNo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Make3EProtocol.BlockType bit = Make3EProtocol.BlockType.Bit;
            string headDevice = block.HeadDevice;
            string name = block.Name;
            try
            {
                string str3 = this.ReadRBitProtocol(block.DeviceCode, block.HeadDevice, block.Points, bit, networkNo, pcNo);
                if (str3 == null)
                {
                    return dictionary;
                }
                foreach (Item item in block.Item)
                {
                    string str4 = str3.Substring(int.Parse(item.Offset), 1);
                    dictionary.Add(item.Name, str4);
                    this.logger.Info(string.Format("[ReadRBit] Item Name : {0}, Data : {1}", item.Name, str4));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Read R Bit Error : {0}, address : {1}, blockname : {2} ", exception.Message, headDevice, name));
            }
            return dictionary;
        }

        private string ReadRBitProtocol(string device, string address, int length, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, length, blockType, networkNo, pcNo);
            return this.ReadWriter(dataArry, ReceiveStatus.Read);
        }

        private string ReadRBitWord(string device, string address, int length, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, length, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRBitWord) address : {0}", address));
                return null;
            }
            return Convert.ToString(Convert.ToInt32(str, 0x10), 2).PadLeft(0x10, '0');
        }

        private string ReadRHexWord(string device, string address, int length, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, length, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRHexWord) address : {0}", address));
                return null;
            }
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = (string.Empty + str.Substring(i * 4, 2)) + str.Substring((i * 4) + 2, 2) + str2;
            }
            return str2;
        }

        private ushort ReadRIntWord(string device, string address, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, 1, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRIntWord) address : {0}", address));
                return 0;
            }
            return Convert.ToUInt16(str, 0x10);
        }

        private short ReadRIntWord2(string device, string address, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRRead(device, address, 1, blockType, networkNo, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRIntWord2) address : {0}", address));
                return 0;
            }
            return Convert.ToInt16(str, 0x10);
        }

        public override Dictionary<string, string> ReadRWord(Block block, bool isHex, int networkNo, int pcNo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string address = string.Empty;
            string name = string.Empty;
            Make3EProtocol.BlockType word = Make3EProtocol.BlockType.Word;
            try
            {
                foreach (Item item in block.Item)
                {
                    string str4;
                    string str6;
                    string str8;
                    string str9;
                    string[] strArray = item.Offset.Split(new char[] { ':' });
                    string[] strArray2 = item.Points.Split(new char[] { ':' });
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                    name = item.Name;
                    string representation = item.Representation;
                    if (representation != null)
                    {
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_0146;
                            }
                            if (representation == "B")
                            {
                                goto Label_02F2;
                            }
                            if (representation == "H")
                            {
                                goto Label_03BA;
                            }
                            if (representation == "SI")
                            {
                                goto Label_0407;
                            }
                        }
                        else
                        {
                            string str3 = this.ReadRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), word, networkNo, pcNo);
                            dictionary.Add(item.Name, str3);
                            this.logger.Info(string.Format("[ReadRWord] ASCII Item Name : {0}, Data : {1}", item.Name, str3));
                        }
                    }
                    continue;
                Label_0146:
                    if (strArray.Length > 1)
                    {
                        str4 = this.ReadRBitWord(block.DeviceCode, address, int.Parse(strArray2[0]), word, networkNo, pcNo).Substring((0x10 - int.Parse(strArray[1])) - int.Parse(strArray2[1]), int.Parse(strArray2[1]));
                        dictionary.Add(item.Name, Convert.ToUInt16(str4, 2).ToString());
                        this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, Convert.ToInt32(str4, 2)));
                    }
                    else
                    {
                        representation = strArray2[0];
                        if (representation != null)
                        {
                            if (!(representation == "1"))
                            {
                                if (representation == "2")
                                {
                                    goto Label_0268;
                                }
                            }
                            else
                            {
                                string str5 = this.ReadRIntWord(block.DeviceCode, address, word, networkNo, pcNo).ToString();
                                dictionary.Add(item.Name, str5);
                                this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, str5));
                            }
                        }
                    }
                    continue;
                Label_0268:
                    str6 = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    uint num = MProtocolUtils.MakeDWord(this.ReadRIntWord(block.DeviceCode, str6, word, networkNo, pcNo), this.ReadRIntWord(block.DeviceCode, address, word, networkNo, pcNo));
                    dictionary.Add(item.Name, num.ToString());
                    this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, num));
                    continue;
                Label_02F2:
                    if (strArray.Length > 1)
                    {
                        str4 = this.ReadRBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), word, networkNo, pcNo).Substring(int.Parse(strArray[1]), int.Parse(strArray2[1]));
                        dictionary.Add(item.Name, str4);
                        this.logger.Info(string.Format("[ReadRWord] Bit Item Name : {0}, Data : {1}", item.Name, str4));
                    }
                    else
                    {
                        string str7 = this.ReadRBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), word, networkNo, pcNo);
                        dictionary.Add(item.Name, str7);
                        this.logger.Info(string.Format("[ReadRWord] Bit Item Name : {0}, Data : {1}", item.Name, str7));
                    }
                    continue;
                Label_03BA:
                    str8 = this.ReadRHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), word, networkNo, pcNo);
                    dictionary.Add(item.Name, str8);
                    this.logger.Info(string.Format("[ReadRWord] H Item Name : {0}, Data : {1}", item.Name, str8));
                    continue;
                Label_0407:
                    str9 = this.ReadRIntWord2(block.DeviceCode, address, word, networkNo, pcNo).ToString();
                    dictionary.Add(item.Name, str9);
                    this.logger.Info(string.Format("[ReadRWord] SI Item Name : {0}, Data : {1}", item.Name, str9));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Read R Word Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                return new Dictionary<string, string>();
            }
            return dictionary;
        }

        public override Dictionary<string, string> ReadWord(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                return this._mProtocol.ReadWord(block, isHex);
            }
            catch (Exception)
            {
            }
            return dictionary;
        }

        public override Dictionary<string, string> ReadWordOnce(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                return this._mProtocol.ReadWordOnce(block, isHex);
            }
            catch (Exception)
            {
            }
            return dictionary;
        }

        private string ReadWriter(byte[] dataArry, ReceiveStatus status)
        {
            string str;
            try
            {
                lock (this.readWriteObj)
                {
                    if (this._MelsecWriter == null)
                    {
                        return "0";
                    }
                    this._MelsecWriter.Write(dataArry, 0, dataArry.Length);
                    str = this.Reader(status);
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

        private byte[] ReadWriter(byte[] dataArry, ReceiveStatus status, bool isCacheFun)
        {
            byte[] buffer;
            try
            {
                if (this._MelsecWriter == null)
                {
                    buffer = null;
                }
                else
                {
                    lock (this.readWriteObj)
                    {
                        this._MelsecWriter.Write(dataArry, 0, dataArry.Length);
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

        public override void ReConnect()
        {
            this.m_bReconnectFlag = true;
            this.m_tReconnectionThread = new Thread(new System.Threading.ThreadStart(this.ReConnectThreadProc));
            this.m_tReconnectionThread.IsBackground = true;
            this.m_tReconnectionThread.Start();
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

        public override void StopConnect()
        {
            if (this.m_tReconnectionThread != null)
            {
                this.m_tReconnectionThread = null;
            }
        }

        public override void ThreadClose()
        {
            if ((this.m_tReconnectionThread != null) && this.m_tReconnectionThread.IsAlive)
            {
                this.m_tReconnectionThread.Abort();
            }
        }

        public override void ThreadStart()
        {
            this.m_tReconnectionThread = null;
        }

        public override void ThreadStop()
        {
            this.m_tReconnectionThread = null;
        }

        private void TimeOutCheck()
        {
            try
            {
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public override void TimeoutCheckStart()
        {
            throw new NotImplementedException();
        }

        public override void TimeoutCheckStop()
        {
            throw new NotImplementedException();
        }

        private void WriteASCIIWord(string device, string address, int length, string writeData, Make3EProtocol.BlockType blockType)
        {
            string str = string.Empty;
            writeData = ((writeData.Length % 2) != 0) ? writeData.PadRight(writeData.Length + 1, ' ') : writeData;
            for (int i = 0; i < (writeData.Length / 2); i++)
            {
                str = str + writeData.Substring((i * 2) + 1, 1) + writeData.Substring(i * 2, 1);
            }
            str = MProtocolUtils.StringToHex(str.PadRight(length * 2, ' '));
            byte[] dataArry = this._mProtocol.GetSingleWrite(device, address, str, length, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        public override bool WriteBit(Block block, Dictionary<string, string> data)
        {
            string address = string.Empty;
            string name = string.Empty;
            try
            {
                Make3EProtocol.BlockType bit = Make3EProtocol.BlockType.Bit;
                foreach (Item item in block.Item)
                {
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(item.Offset));
                        name = item.Name;
                        string writeData = string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name];
                        this.WriteBitProtocol(block.DeviceCode, address, int.Parse(item.Points), writeData, bit);
                        this.logger.Info(string.Format("[WriteBit] Item Name : {0}, Data : {1}", item.Name, writeData));
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write Bit Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                return false;
            }
        }

        private void WriteBitProtocol(string device, string address, int length, string writeData, Make3EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetSingleWrite(device, address, writeData, length, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        private void WriteHexWord(string device, string address, int length, string writeData, Make3EProtocol.BlockType blockType)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            if (writeData.Length < (length * 4))
            {
                str = writeData.PadLeft(length * 4, '0');
            }
            else if (writeData.Length > (length * 4))
            {
                str = writeData.Substring(writeData.Length - (length * 4), length * 4);
            }
            else
            {
                str = writeData;
            }
            for (int i = 0; i < length; i++)
            {
                str2 = str.Substring(i * 4, 4) + str2;
            }
            byte[] dataArry = this._mProtocol.GetSingleWrite(device, address, str2, length, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        private void WriteIntWord(string device, string address, string writeData, Make3EProtocol.BlockType blockType)
        {
            writeData = string.Format("{0:X}", Convert.ToInt32(writeData));
            byte[] dataArry = this._mProtocol.GetSingleWrite(device, address, writeData, 1, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        private void WriteRASCIIWord(string device, string address, int length, string writeRData, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            string writeData = string.Empty;
            writeRData = ((writeRData.Length % 2) != 0) ? writeRData.PadRight(writeRData.Length + 1, ' ') : writeRData;
            for (int i = 0; i < (writeRData.Length / 2); i++)
            {
                writeData = writeData + writeRData.Substring((i * 2) + 1, 1) + writeRData.Substring(i * 2, 1);
            }
            writeData = MProtocolUtils.StringToHex(writeData.PadRight(length * 2, ' '));
            byte[] dataArry = this._mProtocol.GetSingleRWrite(device, address, writeData, length, blockType, networkNo, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        public override bool WriteRBit(Block block, Dictionary<string, string> data, int networkNo, int pcNo)
        {
            string address = string.Empty;
            string name = string.Empty;
            try
            {
                Make3EProtocol.BlockType bit = Make3EProtocol.BlockType.Bit;
                foreach (Item item in block.Item)
                {
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(item.Offset));
                        name = item.Name;
                        string writeRData = string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name];
                        this.WriteRBitProtocol(block.DeviceCode, address, int.Parse(item.Points), writeRData, bit, networkNo, pcNo);
                        this.logger.Info(string.Format("[WriteRBit] Item Name : {0}, Data : {1}", item.Name, writeRData));
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write R Bit Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                return false;
            }
        }

        private void WriteRBitProtocol(string device, string address, int length, string writeRData, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetSingleRWrite(device, address, writeRData, length, blockType, networkNo, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        private void WriteRIntWord(string device, string address, string writeRData, Make3EProtocol.BlockType blockType, int networkNo, int pcNo)
        {
            if (writeRData == "None")
            {
                writeRData = "0";
            }
            writeRData = string.Format("{0:X}", Convert.ToInt32(writeRData));
            byte[] dataArry = this._mProtocol.GetSingleRWrite(device, address, writeRData, 1, blockType, networkNo, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write);
        }

        public override bool WriteRWord(Block block, Dictionary<string, string> data, int networkNo, int pcNo)
        {
            string address = string.Empty;
            string name = string.Empty;
            try
            {
                Make3EProtocol.BlockType word = Make3EProtocol.BlockType.Word;
                foreach (Item item in block.Item)
                {
                    uint num2;
                    int num3;
                    float num4;
                    string representation;
                    string[] strArray = item.Offset.Split(new char[] { ':' });
                    string[] strArray2 = item.Points.Split(new char[] { ':' });
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                        name = item.Name;
                        representation = item.Representation;
                        if (representation == null)
                        {
                            goto Label_0782;
                        }
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_018A;
                            }
                            if (representation == "B")
                            {
                                goto Label_039A;
                            }
                            if (representation == "H")
                            {
                                goto Label_059F;
                            }
                            if (representation == "F")
                            {
                                goto Label_0627;
                            }
                            goto Label_0782;
                        }
                        this.WriteRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name], word, networkNo, pcNo);
                        this.logger.Info(string.Format("[WriteRWord] ASCII Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    }
                    continue;
                Label_018A:
                    if (strArray.Length > 1)
                    {
                        ushort num = (ushort)(this.ReadRIntWord(block.DeviceCode, address, word, networkNo, pcNo) | (string.IsNullOrEmpty(data[item.Name]) ? ((ushort)0) : ((ushort)(Convert.ToInt32(data[item.Name]) << int.Parse(strArray2[1])))));
                        this.WriteRIntWord(block.DeviceCode, address, num.ToString(), word, networkNo, pcNo);
                        this.logger.Info(string.Format("[WriteRWord] Int Item Name : {0}, Data : {1}", item.Name, num));
                    }
                    else
                    {
                        representation = strArray2[0];
                        if (representation != null)
                        {
                            if (!(representation == "1"))
                            {
                                if (representation == "2")
                                {
                                    goto Label_02DF;
                                }
                            }
                            else
                            {
                                this.WriteRIntWord(block.DeviceCode, address, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name], word, networkNo, pcNo);
                                this.logger.Info(string.Format("[WriteRWord] Int Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name]));
                            }
                        }
                    }
                    continue;
                Label_02DF:
                    num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                    this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word, networkNo, pcNo);
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word, networkNo, pcNo);
                    this.logger.Info(string.Format("[WriteRWord] Int Item Name : {0}, Data : {1}", item.Name, num2));
                    continue;
                Label_039A:
                    if (strArray.Length > 1)
                    {
                        num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                        this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word, networkNo, pcNo);
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                        this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word, networkNo, pcNo);
                        this.logger.Info(string.Format("[WriteRWord] Bit Item Name : {0}, Data : {1}", item.Name, num2));
                    }
                    else
                    {
                        string[] strArray3 = new string[int.Parse(strArray2[0])];
                        string str3 = string.Empty;
                        if (string.IsNullOrEmpty(data[item.Name]))
                        {
                            str3 = str3.PadRight(int.Parse(strArray2[0]) * 0x10, '0');
                        }
                        else
                        {
                            str3 = str3.PadRight((data[item.Name].Length % 0x10) + ((int.Parse(strArray2[0]) * 0x10) - 0x10), '0');
                            str3 = MProtocolUtils.CharRevcrse(data[item.Name] + str3);
                        }
                        num3 = 0;
                        while (num3 < strArray3.Length)
                        {
                            address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + num3);
                            strArray3[num3] = Convert.ToInt32(str3.Substring(num3 * 0x10, 0x10), 2).ToString();
                            this.WriteRIntWord(block.DeviceCode, address, strArray3[num3], word, networkNo, pcNo);
                            this.logger.Info(string.Format("[WriteRWord] Bit Item Name : {0}, Data : {1}", item.Name, strArray3[num3]));
                            num3++;
                        }
                    }
                    continue;
                Label_059F:
                    this.WriteRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name], word, networkNo, pcNo);
                    this.logger.Info(string.Format("[WriteRWord] H Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_0627:
                    num4 = Convert.ToSingle(data[item.Name]);
                    byte[] bytes = new byte[10];
                    bytes = BitConverter.GetBytes(num4);
                    string str4 = string.Empty;
                    foreach (byte num5 in bytes)
                    {
                        str4 = num5.ToString("X").PadLeft(2, '0') + str4;
                    }
                    string str5 = string.Empty;
                    for (num3 = int.Parse(item.Points); num3 > 0; num3--)
                    {
                        string str6 = str4.Substring((num3 - 1) * 4, 4);
                        int num6 = int.Parse(str4.Substring((num3 - 1) * 4, 4), NumberStyles.HexNumber);
                        str5 = this.GetAddress(block.DeviceCode, address, int.Parse(item.Points) - num3);
                        this.WriteIntWord(block.DeviceCode, str5, string.IsNullOrEmpty(data[item.Name]) ? "0" : num6.ToString(), word);
                    }
                    this.logger.Info(string.Format("[WriteRWord] F Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_0782:
                    this.logger.Error(string.Format("Write R Word invalid Representation : {0} ", item.Representation));
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write R Word Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                return false;
            }
        }

        public override bool WriteWord(Block block, Dictionary<string, string> data)
        {
            string address = string.Empty;
            string name = string.Empty;
            bool flag = false;
            int num = 0x3c1;
            ushort num2 = 0;
            try
            {
                Make3EProtocol.BlockType word = Make3EProtocol.BlockType.Word;
                foreach (Item item in block.Item)
                {
                    uint num3;
                    int num4;
                    float num5;
                    string representation;
                    string[] strArray = item.Offset.Split(new char[] { ':' });
                    string[] strArray2 = item.Points.Split(new char[] { ':' });
                    if (strArray.Length > 1)
                    {
                        if (num == 0x3c1)
                        {
                            num = int.Parse(strArray[0]);
                            flag = true;
                        }
                        else if (num != int.Parse(strArray[0]))
                        {
                            this.WriteIntWord(block.DeviceCode, address, num2.ToString(), word);
                            num = int.Parse(strArray[0]);
                            flag = false;
                            num2 = 0;
                        }
                    }
                    else if (flag)
                    {
                        this.WriteIntWord(block.DeviceCode, address, num2.ToString(), word);
                        num = 0x3c1;
                        flag = false;
                        num2 = 0;
                    }
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                        name = item.Name;
                        representation = item.Representation;
                        if (representation == null)
                        {
                            goto Label_07FF;
                        }
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_024C;
                            }
                            if (representation == "B")
                            {
                                goto Label_040E;
                            }
                            if (representation == "H")
                            {
                                goto Label_0627;
                            }
                            if (representation == "F")
                            {
                                goto Label_06B2;
                            }
                            goto Label_07FF;
                        }
                        this.WriteASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name].Trim()) ? "  " : data[item.Name], word);
                        this.logger.Info(string.Format("[WriteWord] ASCII Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    }
                    continue;
                Label_024C:
                    if (strArray.Length > 1)
                    {
                        num2 = (ushort)(num2 | (string.IsNullOrEmpty(data[item.Name]) ? ((ushort)0) : ((ushort)(Convert.ToInt32(data[item.Name]) << int.Parse(strArray[1])))));
                    }
                    else
                    {
                        representation = strArray2[0];
                        if (representation != null)
                        {
                            if (!(representation == "1"))
                            {
                                if (representation == "2")
                                {
                                    goto Label_0354;
                                }
                            }
                            else
                            {
                                this.WriteIntWord(block.DeviceCode, address, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name], word);
                                this.logger.Info(string.Format("[WriteWord] Int Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name]));
                            }
                        }
                    }
                    continue;
                Label_0354:
                    num3 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                    this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num3).ToString(), word);
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num3).ToString(), word);
                    this.logger.Info(string.Format("[WriteWord] Int Item Name : {0}, Data : {1}", item.Name, num3));
                    continue;
                Label_040E:
                    if (strArray.Length > 1)
                    {
                        num2 = (ushort)(num2 | (string.IsNullOrEmpty(data[item.Name]) ? ((ushort)0) : ((ushort)(Convert.ToInt32(data[item.Name]) << int.Parse(strArray[1])))));
                    }
                    else
                    {
                        string[] strArray3 = new string[int.Parse(strArray2[0])];
                        string str3 = string.Empty;
                        if (string.IsNullOrEmpty(data[item.Name]))
                        {
                            str3 = str3.PadRight(int.Parse(strArray2[0]) * 0x10, '0');
                        }
                        else if (str3.Length == (int.Parse(strArray2[0]) * 0x10))
                        {
                            str3 = data[item.Name];
                        }
                        else if (str3.Length < (int.Parse(strArray2[0]) * 0x10))
                        {
                            string str4 = data[item.Name];
                            str4 = str4.PadLeft(int.Parse(strArray2[0]) * 0x10, '0');
                            num4 = 0;
                            while (num4 < int.Parse(strArray2[0]))
                            {
                                str3 = str4.Substring(num4 * 0x10, 0x10) + str3;
                                num4++;
                            }
                        }
                        else if (str3.Length > (int.Parse(strArray2[0]) * 0x10))
                        {
                        }
                        num4 = 0;
                        while (num4 < strArray3.Length)
                        {
                            address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + num4);
                            strArray3[num4] = Convert.ToInt32(str3.Substring(num4 * 0x10, 0x10), 2).ToString();
                            this.WriteIntWord(block.DeviceCode, address, strArray3[num4], word);
                            this.logger.Info(string.Format("[WriteWord] Bit Item Name : {0}, Data : {1}", item.Name, strArray3[num4]));
                            num4++;
                        }
                    }
                    continue;
                Label_0627:
                    this.WriteHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name], word);
                    this.logger.Info(string.Format("[WriteWord] H Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_06B2:
                    num5 = Convert.ToSingle(data[item.Name]);
                    byte[] bytes = new byte[10];
                    bytes = BitConverter.GetBytes(num5);
                    string str5 = string.Empty;
                    foreach (byte num6 in bytes)
                    {
                        str5 = num6.ToString("X").PadLeft(2, '0') + str5;
                    }
                    string str6 = string.Empty;
                    for (num4 = int.Parse(item.Points); num4 > 0; num4--)
                    {
                        int num7 = int.Parse(str5.Substring((num4 - 1) * 4, 4), NumberStyles.HexNumber);
                        str6 = this.GetAddress(block.DeviceCode, address, int.Parse(item.Points) - num4);
                        this.WriteIntWord(block.DeviceCode, str6, string.IsNullOrEmpty(data[item.Name]) ? "0" : num7.ToString(), word);
                    }
                    this.logger.Info(string.Format("[WriteWord] F Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_07FF:
                    this.logger.Error(string.Format("WriteWord Invalid Representation : {0} ", item.Representation));
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write Word Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                return false;
            }
        }

        public override string Name { get; set; }

        public enum ProtocolType
        {
            None,
            WriteIntWord,
            WriteASCIIWord,
            WriteBitProtocol,
            ReadBinaryWord,
            ReadBitWord,
            ReadIntWord,
            ReadIntWord2,
            ReadASCIIWord,
            ReadBitProtocol
        }

        public enum ReceiveStatus
        {
            Read,
            Write
        }
    }
}
