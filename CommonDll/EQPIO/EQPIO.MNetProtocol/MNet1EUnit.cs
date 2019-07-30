
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

    public class MNet1EUnit : IMNetUnit
    {
        private string _address;
        private BinaryReader _MelsecReader = null;
        private BinaryWriter _MelsecWriter = null;
        private Make1EProtocol _mProtocol;
        private int _networkNo;
        private int _portNo;
        private bool _reConnectFlag = false;
        private int _stationNo;
        private TcpClient CommandClient;
        private ILog logger = LogManager.GetLogger(typeof(MNet1EUnit));
        private object readWriteObj = new object();
        private Thread reConnectionThread;

        public MNet1EUnit(ConnectionInfo conn, DataGathering dataGathering, BlockMap plcMap, Transaction trx, int melsecProtNo)
        {
            this._address = conn.IpAddress;
            this._portNo = melsecProtNo;
            this._networkNo = Convert.ToInt32(conn.NetworkNo);
            this._stationNo = Convert.ToInt32(conn.PCNo);
            this.Name = conn.LocalName;
            this._mProtocol = new Make1EProtocol();
        }

        public override void Close()
        {
            if ((this.CommandClient != null) && this.CommandClient.Connected)
            {
                this.CommandClient.Client.Close();
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
                this.CommandClient = new TcpClient(this._address, this._portNo);
                this.CommandClient.ReceiveTimeout = 0x1f40;
                this.CommandClient.Client.Blocking = true;
                this._MelsecReader = new BinaryReader(this.CommandClient.GetStream());
                this._MelsecWriter = new BinaryWriter(this.CommandClient.GetStream());
                this.logger.Info(string.Format("[Connection] Melsec Connect Address : {0} , PortNo : {1}", this._address, this._portNo));
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

        private string ReadASCIIWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, isHex, Make1EProtocol.BlockType.Word);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadASCIIWord) address : {0}", address));
                return null;
            }
            str = str.TrimEnd(new char[1]);
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + Convert.ToChar(int.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(str.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
            }
            return str2.TrimEnd(new char[1]).Trim();
        }

        private string ReadBinaryWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType)
        {
            int num2;
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, isHex, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
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

        public override Dictionary<string, string> ReadBit(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Make1EProtocol.BlockType bit = Make1EProtocol.BlockType.Bit;
            string str = this.ReadBitProtocol(block.DeviceCode, block.HeadDevice, block.Points, isHex, bit);
            if (str != null)
            {
                foreach (Item item in block.Item)
                {
                    string str2 = str.Substring(int.Parse(item.Offset), 1);
                    dictionary.Add(item.Name, str2);
                    this.logger.Info(string.Format("[ReadBit] Item Name : {0}, Data : {1}", item.Name, str2));
                }
            }
            return dictionary;
        }

        private string ReadBitProtocol(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, isHex, Make1EProtocol.BlockType.Bit);
            return this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Bit);
        }

        private string ReadBitWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, isHex, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadBitWord) address : {0}", address));
                return null;
            }
            return Convert.ToString(Convert.ToInt32(str.TrimEnd(new char[1]), 0x10), 2).PadLeft(0x10, '0');
        }

        private string Reader(ReceiveStatus status, int length, ProtocolType type)
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
                    this.logger.Info(string.Format("[Receive] Receive NG : Plc Name : {0} , ReadData : {1}", this.Name, str));
                    return null;
                }
                if (status == ReceiveStatus.Write)
                {
                    return str;
                }
                if (type == ProtocolType.Bit)
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
                this.OnDisconnected(new DisconnectedEventArgs(this));
                return null;
            }
            catch (Exception exception2)
            {
                this.logger.Error(string.Format("[Error] Message : {0}", exception2.Message));
                this.OnDisconnected(new DisconnectedEventArgs(this));
                return null;
            }
        }

        private string ReadHexWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, length, isHex, Make1EProtocol.BlockType.Word);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadHexWord) address : {0}", address));
                return null;
            }
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + str.Substring((i * 4) + 2, 2) + str.Substring(i * 4, 2);
            }
            return str2;
        }

        private ushort ReadIntWord(string device, string address, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, 1, isHex, Make1EProtocol.BlockType.Word);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, 1, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadIntWord) address : {0}", address));
                return 0;
            }
            return Convert.ToUInt16(str.TrimEnd(new char[1]), 0x10);
        }

        private short ReadIntWord2(string device, string address, bool isHex, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetRead(device, address, 1, isHex, Make1EProtocol.BlockType.Word);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, 1, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadIntWord2) address : {0}", address));
                return 0;
            }
            return Convert.ToInt16(str.TrimEnd(new char[1]), 0x10);
        }

        private string ReadRASCIIWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, length, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRASCIIWord) address : {0}", address));
                return null;
            }
            str = str.TrimEnd(new char[1]);
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + Convert.ToChar(int.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(str.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
            }
            return str2.TrimEnd(new char[1]).Trim();
        }

        private string ReadRBinaryWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            int num2;
            byte[] dataArry = this._mProtocol.GetRRead(device, address, length, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
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
            Make1EProtocol.BlockType bit = Make1EProtocol.BlockType.Bit;
            string str = this.ReadRBitProtocol(block.DeviceCode, block.HeadDevice, block.Points, isHex, bit, pcNo);
            if (str != null)
            {
                foreach (Item item in block.Item)
                {
                    string str2 = str.Substring(int.Parse(item.Offset), 1);
                    dictionary.Add(item.Name, str2);
                    this.logger.Info(string.Format("[ReadRBit] Item Name : {0}, Data : {1}", item.Name, str2));
                }
            }
            return dictionary;
        }

        private string ReadRBitProtocol(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, length, isHex, blockType, pcNo);
            return this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Bit);
        }

        private string ReadRBitWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, length, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRBitWord) address : {0}", address));
                return null;
            }
            return Convert.ToString(Convert.ToInt32(str, 0x10), 2).PadLeft(0x10, '0');
        }

        private string ReadRHexWord(string device, string address, int length, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, length, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, length, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRHexWord) address : {0}", address));
                return null;
            }
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + str.Substring((i * 4) + 2, 2) + str.Substring(i * 4, 2);
            }
            return str2;
        }

        private ushort ReadRIntWord(string device, string address, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, 1, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, 1, ProtocolType.Word);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadRIntWord) address : {0}", address));
                return 0;
            }
            return Convert.ToUInt16(str, 0x10);
        }

        private short ReadRIntWord2(string device, string address, bool isHex, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRRead(device, address, 1, isHex, blockType, pcNo);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read, 1, ProtocolType.Word);
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
            Make1EProtocol.BlockType word = Make1EProtocol.BlockType.Word;
            try
            {
                foreach (Item item in block.Item)
                {
                    string str3;
                    string str5;
                    string str7;
                    string str8;
                    string[] strArray = item.Offset.Split(new char[] { ':' });
                    string[] strArray2 = item.Points.Split(new char[] { ':' });
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                    string representation = item.Representation;
                    if (representation != null)
                    {
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_0132;
                            }
                            if (representation == "B")
                            {
                                goto Label_02D8;
                            }
                            if (representation == "H")
                            {
                                goto Label_039C;
                            }
                            if (representation == "SI")
                            {
                                goto Label_03E7;
                            }
                        }
                        else
                        {
                            string str2 = this.ReadRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word, pcNo);
                            dictionary.Add(item.Name, str2);
                            this.logger.Info(string.Format("[ReadRWord] ASCII Item Name : {0}, Data : {1}", item.Name, str2));
                        }
                    }
                    continue;
                Label_0132:
                    if (strArray.Length > 1)
                    {
                        str3 = this.ReadRBitWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word, pcNo).Substring((0x10 - int.Parse(strArray[1])) - int.Parse(strArray2[1]), int.Parse(strArray2[1]));
                        dictionary.Add(item.Name, Convert.ToUInt16(str3, 2).ToString());
                        this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, Convert.ToInt32(str3, 2)));
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
                                    goto Label_0250;
                                }
                            }
                            else
                            {
                                string str4 = this.ReadRIntWord(block.DeviceCode, address, isHex, word, pcNo).ToString();
                                dictionary.Add(item.Name, str4);
                                this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, str4));
                            }
                        }
                    }
                    continue;
                Label_0250:
                    str5 = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    uint num = MProtocolUtils.MakeDWord(this.ReadRIntWord(block.DeviceCode, str5, isHex, word, pcNo), this.ReadRIntWord(block.DeviceCode, address, isHex, word, pcNo));
                    dictionary.Add(item.Name, num.ToString());
                    this.logger.Info(string.Format("[ReadRWord] Int Item Name : {0}, Data : {1}", item.Name, num));
                    continue;
                Label_02D8:
                    if (strArray.Length > 1)
                    {
                        str3 = this.ReadRBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word, pcNo).Substring(int.Parse(strArray[1]), int.Parse(strArray2[1]));
                        dictionary.Add(item.Name, str3);
                        this.logger.Info(string.Format("[ReadRWord] Bit Item Name : {0}, Data : {1}", item.Name, str3));
                    }
                    else
                    {
                        string str6 = this.ReadRBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word, pcNo);
                        dictionary.Add(item.Name, str6);
                        this.logger.Info(string.Format("[ReadRWord] Bit Item Name : {0}, Data : {1}", item.Name, str6));
                    }
                    continue;
                Label_039C:
                    str7 = this.ReadRHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word, pcNo);
                    dictionary.Add(item.Name, str7);
                    this.logger.Info(string.Format("[ReadRWord] H Item Name : {0}, Data : {1}", item.Name, str7));
                    continue;
                Label_03E7:
                    str8 = this.ReadRIntWord2(block.DeviceCode, address, isHex, word, pcNo).ToString();
                    dictionary.Add(item.Name, str8);
                    this.logger.Info(string.Format("[ReadRWord] SI Item Name : {0}, Data : {1}", item.Name, str8));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("ReadRWord Error : {0}", exception));
                return new Dictionary<string, string>();
            }
            return dictionary;
        }

        public override Dictionary<string, string> ReadWord(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string address = string.Empty;
            Make1EProtocol.BlockType word = Make1EProtocol.BlockType.Word;
            try
            {
                foreach (Item item in block.Item)
                {
                    string str3;
                    string str5;
                    string str6;
                    string str9;
                    string[] strArray = item.Offset.Split(new char[] { ':' });
                    string[] strArray2 = item.Points.Split(new char[] { ':' });
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                    string representation = item.Representation;
                    switch (representation)
                    {
                        case "A":
                            {
                                string str2 = this.ReadASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word);
                                dictionary.Add(item.Name, str2);
                                this.logger.Info(string.Format("[ReadWord] ASCII Item Name : {0}, Data : {1}", item.Name, str2));
                                continue;
                            }
                        case "I":
                            {
                                if (strArray.Length <= 1)
                                {
                                    break;
                                }
                                str3 = this.ReadBitWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word).Substring((0x10 - int.Parse(strArray[1])) - int.Parse(strArray2[1]), int.Parse(strArray2[1]));
                                dictionary.Add(item.Name, Convert.ToUInt16(str3, 2).ToString());
                                this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, Convert.ToInt32(str3, 2)));
                                continue;
                            }
                        case "B":
                            {
                                if (strArray.Length <= 1)
                                {
                                    goto Label_037D;
                                }
                                str3 = this.ReadBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word).Substring(int.Parse(strArray[1]), int.Parse(strArray2[1]));
                                dictionary.Add(item.Name, str3);
                                this.logger.Info(string.Format("[ReadWord] Bit Item Name : {0}, Data : {1}", item.Name, str3));
                                continue;
                            }
                        case "H":
                            {
                                string str7 = this.ReadHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word);
                                dictionary.Add(item.Name, str7);
                                this.logger.Info(string.Format("[ReadWord] H Item Name : {0}, Data : {1}", item.Name, str7));
                                continue;
                            }
                        case "SI":
                            {
                                string str8 = this.ReadIntWord2(block.DeviceCode, address, isHex, word).ToString();
                                dictionary.Add(item.Name, str8);
                                this.logger.Info(string.Format("[ReadWord] SI Item Name : {0}, Data : {1}", item.Name, str8));
                                continue;
                            }
                        case "F":
                            {
                                if (int.Parse(strArray2[0]) == 2)
                                {
                                    goto Label_047B;
                                }
                                continue;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                    representation = strArray2[0];
                    if (representation != null)
                    {
                        if (!(representation == "1"))
                        {
                            if (representation == "2")
                            {
                                goto Label_0287;
                            }
                        }
                        else
                        {
                            string str4 = this.ReadIntWord(block.DeviceCode, address, isHex, word).ToString();
                            dictionary.Add(item.Name, str4);
                            this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, str4));
                        }
                    }
                    continue;
                Label_0287:
                    str5 = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    uint num = MProtocolUtils.MakeDWord(this.ReadIntWord(block.DeviceCode, str5, isHex, word), this.ReadIntWord(block.DeviceCode, address, isHex, word));
                    dictionary.Add(item.Name, num.ToString());
                    this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, num));
                    continue;
                Label_037D:
                    str6 = this.ReadBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word);
                    dictionary.Add(item.Name, str6);
                    this.logger.Info(string.Format("[ReadWord] Bit Item Name : {0}, Data : {1}", item.Name, str6));
                    continue;
                Label_047B:
                    str9 = this.ReadHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), isHex, word);
                    byte[] buffer = new byte[4];
                    buffer[3] = Convert.ToByte(Convert.ToInt32(str9.Substring(0, 2), 0x10));
                    buffer[2] = Convert.ToByte(Convert.ToInt32(str9.Substring(2, 2), 0x10));
                    buffer[1] = Convert.ToByte(Convert.ToInt32(str9.Substring(4, 2), 0x10));
                    buffer[0] = Convert.ToByte(Convert.ToInt32(str9.Substring(6, 2), 0x10));
                    float num2 = BitConverter.ToSingle(buffer, 0);
                    dictionary.Add(item.Name, num2.ToString());
                    this.logger.Info(string.Format("[ReadWord] F Item Name : {0}, Data : {1}", item.Name, num2.ToString()));
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("ReadWord Error : {0}", exception));
                return new Dictionary<string, string>();
            }
            return dictionary;
        }

        public override Dictionary<string, string> ReadWordOnce(Block block, bool isHex)
        {
            throw new NotImplementedException();
        }

        private string ReadWriter(byte[] dataArry, ReceiveStatus status, int length, ProtocolType type)
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
                    str = this.Reader(status, length, type);
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

        public override void ReConnect()
        {
            this._reConnectFlag = true;
            this.reConnectionThread = new Thread(new System.Threading.ThreadStart(this.ReConnectThreadProc));
            this.reConnectionThread.IsBackground = true;
            this.reConnectionThread.Start();
        }

        private void ReConnectThreadProc()
        {
            try
            {
                while (this._reConnectFlag)
                {
                    if (this.Connect())
                    {
                        this._reConnectFlag = false;
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
            if (this.reConnectionThread != null)
            {
                this.reConnectionThread = null;
            }
        }

        public override void ThreadClose()
        {
            if ((this.reConnectionThread != null) && this.reConnectionThread.IsAlive)
            {
                this.reConnectionThread.Abort();
            }
        }

        public override void ThreadStart()
        {
            this.reConnectionThread = null;
        }

        public override void ThreadStop()
        {
            this.reConnectionThread = null;
        }

        public override void TimeoutCheckStart()
        {
            throw new NotImplementedException();
        }

        public override void TimeoutCheckStop()
        {
            throw new NotImplementedException();
        }

        private void WriteASCIIWord(string device, string address, int length, string writeData, Make1EProtocol.BlockType blockType)
        {
            string str = string.Empty;
            writeData = ((writeData.Length % 2) != 0) ? writeData.PadRight(writeData.Length + 1, ' ') : writeData;
            for (int i = 0; i < (writeData.Length / 2); i++)
            {
                str = str + writeData.Substring((i * 2) + 1, 1) + writeData.Substring(i * 2, 1);
            }
            str = MProtocolUtils.StringToHex(str.PadRight(length * 2, ' '));
            byte[] dataArry = this._mProtocol.GetWrite(device, address, str, length, true, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write, length, ProtocolType.Word);
        }

        public override bool WriteBit(Block block, Dictionary<string, string> data)
        {
            try
            {
                string address = string.Empty;
                Make1EProtocol.BlockType bit = Make1EProtocol.BlockType.Bit;
                foreach (Item item in block.Item)
                {
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(item.Offset));
                        string writeData = string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name];
                        this.WriteBitProtocol(block.DeviceCode, address, int.Parse(item.Points), writeData, bit);
                        this.logger.Info(string.Format("[WriteBit] Item Name : {0}, Data : {1}", item.Name, writeData));
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write Bit Error : {0} ", exception.Message));
                return false;
            }
        }

        private void WriteBitProtocol(string device, string address, int length, string writeData, Make1EProtocol.BlockType blockType)
        {
            byte[] dataArry = this._mProtocol.GetWrite(device, address, writeData, length, true, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write, length, ProtocolType.Word);
        }

        private void WriteIntWord(string device, string address, string writeData, Make1EProtocol.BlockType blockType)
        {
            writeData = string.Format("{0:X}", Convert.ToInt32(writeData));
            if (writeData.Length < 4)
            {
                writeData = writeData.PadLeft(4, '0');
            }
            else if (writeData.Length > 4)
            {
                writeData = writeData.Substring(writeData.Length - 4);
            }
            byte[] dataArry = this._mProtocol.GetWrite(device, address, writeData, 1, true, blockType);
            this.ReadWriter(dataArry, ReceiveStatus.Write, 1, ProtocolType.Word);
        }

        private void WriteRASCIIWord(string device, string address, int length, string writeData, Make1EProtocol.BlockType blockType, int pcNo)
        {
            string str = string.Empty;
            writeData = ((writeData.Length % 2) != 0) ? writeData.PadRight(writeData.Length + 1, ' ') : writeData;
            for (int i = 0; i < (writeData.Length / 2); i++)
            {
                str = str + writeData.Substring((i * 2) + 1, 1) + writeData.Substring(i * 2, 1);
            }
            str = MProtocolUtils.StringToHex(str.PadRight(length * 2, ' '));
            byte[] dataArry = this._mProtocol.GetRWrite(device, address, str, length, false, blockType, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write, length, ProtocolType.Word);
        }

        public override bool WriteRBit(Block block, Dictionary<string, string> data, int networkNo, int pcNo)
        {
            try
            {
                string address = string.Empty;
                Make1EProtocol.BlockType bit = Make1EProtocol.BlockType.Bit;
                foreach (Item item in block.Item)
                {
                    if (data.ContainsKey(item.Name))
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(item.Offset));
                        string writeData = string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name];
                        this.WriteBitProtocol(block.DeviceCode, address, int.Parse(item.Points), writeData, bit);
                        this.logger.Info(string.Format("[WriteBit] Item Name : {0}, Data : {1}", item.Name, writeData));
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write Bit Error : {0} ", exception.Message));
                return false;
            }
        }

        private void WriteRBitProtocol(string device, string address, int length, string writeData, Make1EProtocol.BlockType blockType, int pcNo)
        {
            byte[] dataArry = this._mProtocol.GetRWrite(device, address, writeData, length, true, blockType, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write, length, ProtocolType.Word);
        }

        private void WriteRIntWord(string device, string address, string writeData, Make1EProtocol.BlockType blockType, int pcNo)
        {
            writeData = string.Format("{0:X}", Convert.ToInt32(writeData));
            byte[] dataArry = this._mProtocol.GetRWrite(device, address, writeData, 1, true, blockType, pcNo);
            this.ReadWriter(dataArry, ReceiveStatus.Write, 1, ProtocolType.Word);
        }

        public override bool WriteRWord(Block block, Dictionary<string, string> data, int networkNo, int pcNo)
        {
            try
            {
                string address = string.Empty;
                Make1EProtocol.BlockType word = Make1EProtocol.BlockType.Word;
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
                        representation = item.Representation;
                        if (representation == null)
                        {
                            goto Label_073E;
                        }
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_017A;
                            }
                            if (representation == "B")
                            {
                                goto Label_0384;
                            }
                            if (representation == "H")
                            {
                                goto Label_0580;
                            }
                            if (representation == "F")
                            {
                                goto Label_0607;
                            }
                            goto Label_073E;
                        }
                        this.WriteRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name], word, pcNo);
                        this.logger.Info(string.Format("[WriteRWord] ASCII Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    }
                    continue;
                Label_017A:
                    if (strArray.Length > 1)
                    {
                        ushort num = (ushort)(this.ReadRIntWord(block.DeviceCode, address, true, word, pcNo) | (string.IsNullOrEmpty(data[item.Name]) ? ((ushort)0) : ((ushort)(Convert.ToInt32(data[item.Name]) << int.Parse(strArray2[1])))));
                        this.WriteRIntWord(block.DeviceCode, address, num.ToString(), word, pcNo);
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
                                    goto Label_02CC;
                                }
                            }
                            else
                            {
                                this.WriteRIntWord(block.DeviceCode, address, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name], word, pcNo);
                                this.logger.Info(string.Format("[WriteRWord] Int Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? "0" : data[item.Name]));
                            }
                        }
                    }
                    continue;
                Label_02CC:
                    num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                    this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word, pcNo);
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word, pcNo);
                    this.logger.Info(string.Format("[WriteRWord] Int Item Name : {0}, Data : {1}", item.Name, num2));
                    continue;
                Label_0384:
                    if (strArray.Length > 1)
                    {
                        num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                        this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word, pcNo);
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                        this.WriteRIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word, pcNo);
                        this.logger.Info(string.Format("[WriteRWord] Bit Item Name : {0}, Data : {1}", item.Name, num2));
                    }
                    else
                    {
                        string[] strArray3 = new string[int.Parse(strArray2[0])];
                        string str2 = string.Empty;
                        if (string.IsNullOrEmpty(data[item.Name]))
                        {
                            str2 = str2.PadRight(int.Parse(strArray2[0]) * 0x10, '0');
                        }
                        else
                        {
                            str2 = str2.PadRight((data[item.Name].Length % 0x10) + ((int.Parse(strArray2[0]) * 0x10) - 0x10), '0');
                            str2 = MProtocolUtils.CharRevcrse(data[item.Name] + str2);
                        }
                        num3 = 0;
                        while (num3 < strArray3.Length)
                        {
                            address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + num3);
                            strArray3[num3] = Convert.ToInt32(str2.Substring(num3 * 0x10, 0x10), 2).ToString();
                            this.WriteRIntWord(block.DeviceCode, address, strArray3[num3], word, pcNo);
                            this.logger.Info(string.Format("[WriteRWord] Bit Item Name : {0}, Data : {1}", item.Name, strArray3[num3]));
                            num3++;
                        }
                    }
                    continue;
                Label_0580:
                    this.WriteRASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name], word, pcNo);
                    this.logger.Info(string.Format("[WriteRWord] H Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_0607:
                    num4 = Convert.ToSingle(data[item.Name]);
                    byte[] bytes = new byte[10];
                    bytes = BitConverter.GetBytes(num4);
                    string str3 = string.Empty;
                    foreach (byte num5 in bytes)
                    {
                        str3 = num5.ToString("X").PadLeft(2, '0') + str3;
                    }
                    string str4 = string.Empty;
                    for (num3 = 0; num3 < int.Parse(item.Points); num3++)
                    {
                        int num6 = int.Parse(str3.Substring(num3 * 4, 4), NumberStyles.HexNumber);
                        str4 = this.GetAddress(block.DeviceCode, address, num3);
                        this.WriteIntWord(block.DeviceCode, str4, string.IsNullOrEmpty(data[item.Name]) ? "0" : num6.ToString(), word);
                    }
                    this.logger.Info(string.Format("[WriteRWord] F Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_073E:
                    this.logger.Error(string.Format("Write R Word invalid Representation : {0} ", item.Representation));
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Write R Word Error : {0} ", exception.Message));
                return false;
            }
        }

        public override bool WriteWord(Block block, Dictionary<string, string> data)
        {
            try
            {
                string address = string.Empty;
                Make1EProtocol.BlockType word = Make1EProtocol.BlockType.Word;
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
                        representation = item.Representation;
                        if (representation == null)
                        {
                            goto Label_072F;
                        }
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_017D;
                            }
                            if (representation == "B")
                            {
                                goto Label_037D;
                            }
                            if (representation == "H")
                            {
                                goto Label_0573;
                            }
                            if (representation == "F")
                            {
                                goto Label_05F8;
                            }
                            goto Label_072F;
                        }
                        this.WriteASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name].Trim()) ? "  " : data[item.Name], word);
                        this.logger.Info(string.Format("[WriteWord] ASCII Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    }
                    continue;
                Label_017D:
                    if (strArray.Length > 1)
                    {
                        ushort num = (ushort)(this.ReadIntWord(block.DeviceCode, address, true, word) | (string.IsNullOrEmpty(data[item.Name]) ? ((ushort)0) : ((ushort)(Convert.ToInt32(data[item.Name]) << int.Parse(strArray2[1])))));
                        this.WriteIntWord(block.DeviceCode, address, num.ToString(), word);
                        this.logger.Info(string.Format("[WriteWord] Int Item Name : {0}, Data : {1}", item.Name, num));
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
                                    goto Label_02C9;
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
                Label_02C9:
                    num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                    this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word);
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                    this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word);
                    this.logger.Info(string.Format("[WriteWord] Int Item Name : {0}, Data : {1}", item.Name, num2));
                    continue;
                Label_037D:
                    if (strArray.Length > 1)
                    {
                        num2 = string.IsNullOrEmpty(data[item.Name]) ? 0 : Convert.ToUInt32(data[item.Name].ToString());
                        this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.LoWord(num2).ToString(), word);
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                        this.WriteIntWord(block.DeviceCode, address, MProtocolUtils.HiWord(num2).ToString(), word);
                        this.logger.Info(string.Format("[WriteWord] Bit Item Name : {0}, Data : {1}", item.Name, num2));
                    }
                    else
                    {
                        string[] strArray3 = new string[int.Parse(strArray2[0])];
                        string str2 = string.Empty;
                        if (string.IsNullOrEmpty(data[item.Name]))
                        {
                            str2 = str2.PadRight(int.Parse(strArray2[0]) * 0x10, '0');
                        }
                        else
                        {
                            str2 = str2.PadRight((data[item.Name].Length % 0x10) + ((int.Parse(strArray2[0]) * 0x10) - 0x10), '0');
                            str2 = MProtocolUtils.CharRevcrse(data[item.Name] + str2);
                        }
                        num3 = 0;
                        while (num3 < strArray3.Length)
                        {
                            address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + num3);
                            strArray3[num3] = Convert.ToInt32(str2.Substring(num3 * 0x10, 0x10), 2).ToString();
                            this.WriteIntWord(block.DeviceCode, address, strArray3[num3], word);
                            this.logger.Info(string.Format("[WriteWord] Bit Item Name : {0}, Data : {1}", item.Name, strArray3[num3]));
                            num3++;
                        }
                    }
                    continue;
                Label_0573:
                    this.WriteASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name], word);
                    this.logger.Info(string.Format("[WriteWord] H Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_05F8:
                    num4 = Convert.ToSingle(data[item.Name]);
                    byte[] bytes = new byte[10];
                    bytes = BitConverter.GetBytes(num4);
                    string str3 = string.Empty;
                    foreach (byte num5 in bytes)
                    {
                        str3 = num5.ToString("X").PadLeft(2, '0') + str3;
                    }
                    string str4 = string.Empty;
                    for (num3 = 0; num3 < int.Parse(item.Points); num3++)
                    {
                        int num6 = int.Parse(str3.Substring(num3 * 4, 4), NumberStyles.HexNumber);
                        str4 = this.GetAddress(block.DeviceCode, address, num3);
                        this.WriteIntWord(block.DeviceCode, str4, string.IsNullOrEmpty(data[item.Name]) ? "0" : num6.ToString(), word);
                    }
                    this.logger.Info(string.Format("[WriteWord] F Item Name : {0}, Data : {1}", item.Name, string.IsNullOrEmpty(data[item.Name]) ? " " : data[item.Name]));
                    continue;
                Label_072F:
                    this.logger.Error(string.Format("WriteWord Invalid Representation : {0} ", item.Representation));
                }
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("WriteWord Error : {0}", exception.Message));
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
            ReadBitProtocol,
            Word,
            Bit
        }

        public enum ReceiveStatus
        {
            Read,
            Write
        }
    }
}
