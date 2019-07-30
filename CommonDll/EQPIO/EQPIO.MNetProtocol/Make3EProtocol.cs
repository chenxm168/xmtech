
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class Make3EProtocol
    {
        private const string BlockRead = "0406";
        private const string BlockWrite = "1406";
        private const int commonBlockProtoclLength = 0x10;
        private const int commonSingleProtoclLength = 0x18;
        private string CpuTimer = "0010";
        private const int dataLengthPosition = 14;
        private byte[] headerByteArray = new byte[0x15];
        private const int headerByteLength = 0x15;
        private ILog logger = LogManager.GetLogger(typeof(Make3EProtocol));
        private bool m_bLoggingFlag = true;
        private bool m_bMelsecOpen = false;
        private const int m_iOnceReadMaxSize = 960;
        private BinaryReader m_MelsecReader = null;
        private BinaryWriter m_MelsecWriter = null;
        private string m_strName = string.Empty;
        private string m_strNetworkNo;
        private string m_strStationNo;
        private const string ModuleIONo = "03FF";
        private const string ModuleStationNo = "00";
        private byte[] otherLocalheaderArray = new byte[0x15];
        private const string ProtocolLength = "0000";
        private object readWriteObj = new object();
        private const string SingleRead = "0401";
        private const string SingleWrite = "1401";
        private const int singProtocolLength = 0x29;
        private const string SubCommandBit = "0001";
        private const string SubCommandBlock = "0000";
        private const string SubCommandWord = "0000";
        private const string SubHeader = "5000";

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public Make3EProtocol(string networkNo, string stationNo)
        {
            this.m_strNetworkNo = networkNo.PadLeft(2, '0');
            this.m_strStationNo = stationNo.PadLeft(2, '0');
        }

        private string CalRequestBlockLength(string deviceCode, int point)
        {
            string str = "0000";
            if (deviceCode == "B")
            {
                return Convert.ToString((int)Math.Ceiling((double)(((double)point) / 16.0)), 0x10).PadLeft(4, '0');
            }
            if ((deviceCode == "W") || (deviceCode == "R"))
            {
                str = Convert.ToString(point, 0x10).PadLeft(4, '0');
            }
            return str;
        }

        public void CloseMelsetReaderWriter()
        {
            this.m_bMelsecOpen = false;
            this.m_MelsecReader = null;
            this.m_MelsecWriter = null;
        }

        private string GetAddress(string DeviceCode, string address, int offset)
        {
            if ((DeviceCode == "R") || (DeviceCode == "ZR"))
            {
                address = Convert.ToString((int)(int.Parse(address) + offset));
                return address;
            }
            address = Convert.ToString((int)(int.Parse(address, NumberStyles.HexNumber) + offset), 0x10).ToUpper();
            return address;
        }

        private Dictionary<string, string> GetItemReadWord(Block block, byte[] readWord)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string name = string.Empty;
            string str2 = string.Empty;
            int sourceIndex = 0;
            int length = 0;
            try
            {
                if (readWord != null)
                {
                    foreach (Item item in block.Item)
                    {
                        string str4;
                        string str5;
                        string str9;
                        string representation;
                        string[] strArray = item.Offset.Split(new char[] { ':' });
                        string[] strArray2 = item.Points.Split(new char[] { ':' });
                        name = item.Name;
                        length = int.Parse(strArray2[0]) * 4;
                        byte[] destinationArray = new byte[length];
                        sourceIndex = int.Parse(strArray[0]) * 4;
                        Array.Copy(readWord, sourceIndex, destinationArray, 0, length);
                        str2 = Encoding.ASCII.GetString(destinationArray);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            representation = item.Representation;
                            switch (representation)
                            {
                                case "A":
                                    {
                                        string str3 = this.ReadASCIIWordNew(str2);
                                        dictionary.Add(item.Name, str3);
                                        if (this.m_bLoggingFlag)
                                        {
                                            this.logger.Info(string.Format("[ASCII] Item Name : {0}, Data : {1}", item.Name, str3));
                                        }
                                        break;
                                    }
                                case "I":
                                    if (strArray.Length <= 1)
                                    {
                                        goto Label_026F;
                                    }
                                    str4 = this.ReadBitWordNew(str2).Substring((0x10 - int.Parse(strArray[1])) - int.Parse(strArray2[1]), int.Parse(strArray2[1]));
                                    dictionary.Add(item.Name, Convert.ToUInt16(str4, 2).ToString());
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[Int:offset > 1] Item Name : {0}, Data : {1}", item.Name, Convert.ToUInt16(str4, 2).ToString()));
                                    }
                                    break;

                                case "B":
                                    {
                                        string str6 = this.ReadBinaryWordNew(str2);
                                        dictionary.Add(item.Name, str6);
                                        if (this.m_bLoggingFlag)
                                        {
                                            this.logger.Info(string.Format("[BIT] Item Name : {0}, Data : {1}", item.Name, str6));
                                        }
                                        break;
                                    }
                                case "H":
                                    {
                                        string str7 = this.ReadHexWordNew(str2);
                                        dictionary.Add(item.Name, str7);
                                        if (this.m_bLoggingFlag)
                                        {
                                            this.logger.Info(string.Format("[Hex] Item Name : {0}, Data : {1}", item.Name, str7));
                                        }
                                        break;
                                    }
                                case "SI":
                                    {
                                        string str8 = Convert.ToInt32(str2, 0x10).ToString();
                                        dictionary.Add(item.Name, str8);
                                        if (this.m_bLoggingFlag)
                                        {
                                            this.logger.Info(string.Format("[Signed Int] Item Name : {0}, Data : {1}", item.Name, str8));
                                        }
                                        break;
                                    }
                                case "F":
                                    if (int.Parse(strArray2[0]) == 2)
                                    {
                                        goto Label_048A;
                                    }
                                    break;
                            }
                        }
                        continue;
                    Label_026F:
                        representation = strArray2[0];
                        if (representation != null)
                        {
                            if (!(representation == "1"))
                            {
                                if (representation == "2")
                                {
                                    goto Label_02F3;
                                }
                            }
                            else
                            {
                                str4 = this.ReadIntWordNew(str2).ToString();
                                dictionary.Add(item.Name, str4);
                                if (this.m_bLoggingFlag)
                                {
                                    this.logger.Info(string.Format("[Int:offset <= 1, point =1] Item Name : {0}, Data : {1}", item.Name, str4));
                                }
                            }
                        }
                        continue;
                    Label_02F3:
                        str5 = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                        uint num3 = MProtocolUtils.MakeDWord(this.ReadIntWordNew(str2.Substring(4)), this.ReadIntWordNew(str2.Substring(0, 4)));
                        dictionary.Add(item.Name, num3.ToString());
                        if (this.m_bLoggingFlag)
                        {
                            this.logger.Info(string.Format("[Int:offset <= 1, point =2] Item Name : {0}, Data : {1}", item.Name, num3.ToString()));
                        }
                        continue;
                    Label_048A:
                        str9 = this.ReadHexWordNew(str2);
                        byte[] buffer2 = new byte[4];
                        buffer2[1] = Convert.ToByte(Convert.ToInt32(str9.Substring(0, 2), 0x10));
                        buffer2[0] = Convert.ToByte(Convert.ToInt32(str9.Substring(2, 2), 0x10));
                        buffer2[3] = Convert.ToByte(Convert.ToInt32(str9.Substring(4, 2), 0x10));
                        buffer2[2] = Convert.ToByte(Convert.ToInt32(str9.Substring(6, 2), 0x10));
                        float num4 = BitConverter.ToSingle(buffer2, 0);
                        dictionary.Add(item.Name, num4.ToString());
                        if (this.m_bLoggingFlag)
                        {
                            this.logger.Info(string.Format("[Float] F Item Name : {0}, Data : {1}", item.Name, num4.ToString()));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                this.logger.Error(string.Format("ItemName : [{0}], String : [{1}]", name, str2));
                return new Dictionary<string, string>();
            }
            return dictionary;
        }

        public byte[] GetMultiblockRead(MultiBlock multiblock)
        {
            string s = string.Empty;
            string str2 = string.Empty;
            int num = 0;
            int num2 = 0;
            byte[] multiProtocolLength = this.GetMultiProtocolLength(multiblock);
            Array.Copy(multiProtocolLength, 0, this.headerByteArray, 14, multiProtocolLength.Length);
            s = Encoding.ASCII.GetString(this.headerByteArray) + "0406" + "0000";
            foreach (Block block in multiblock.Block)
            {
                if (block.DeviceCode == "B")
                {
                    num2++;
                }
                else if ((block.DeviceCode == "W") || (block.DeviceCode == "R"))
                {
                    num++;
                }
                str2 = (str2 + (block.DeviceCode + "*").PadLeft(2, '0')) + block.HeadDevice.PadLeft(6, '0') + this.CalRequestBlockLength(block.DeviceCode, block.Points);
            }
            s = (s + Convert.ToString(num, 0x10).PadLeft(2, '0')) + Convert.ToString(num2, 0x10).PadLeft(2, '0') + str2;
            return Encoding.ASCII.GetBytes(s);
        }

        private byte[] GetMultiProtocolLength(MultiBlock multiblock)
        {
            string s = string.Empty;
            int num = 0;
            if (multiblock.Block.Length > 0)
            {
                num = 0x10;
                num += multiblock.Block.Length * 12;
                s = Convert.ToString(num, 0x10).ToUpper().PadLeft("0000".Length, '0');
            }
            return Encoding.ASCII.GetBytes(s);
        }

        private byte[] GetSingleProtocolLength(string writeData, BlockType blockType, int point)
        {
            string s = string.Empty;
            if (string.IsNullOrEmpty(writeData))
            {
                s = Convert.ToString(0x18, 0x10).ToUpper().PadLeft("0000".Length, '0');
            }
            else if (blockType == BlockType.Bit)
            {
                s = Convert.ToString((int)(0x18 + point), 0x10).ToUpper().PadLeft("0000".Length, '0');
            }
            else if (blockType == BlockType.Word)
            {
                s = Convert.ToString((int)(0x18 + (point * 4)), 0x10).ToUpper().PadLeft("0000".Length, '0');
            }
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetSingleRead(string deviceCode, string address, int length, BlockType blockType)
        {
            if (length >= 960)
            {
            }
            byte[] sourceArray = this.GetSingleProtocolLength(null, blockType, length);
            Array.Copy(sourceArray, 0, this.headerByteArray, 14, sourceArray.Length);
            string s = Encoding.ASCII.GetString(this.headerByteArray) + "0401" + ((blockType == BlockType.Bit) ? "0001" : "0000");
            if (deviceCode == "ZR")
            {
                s = s + "ZR";
            }
            else
            {
                s = s + MProtocolUtils.PadLeft(deviceCode + "*", 2, '0');
            }
            s = s + MProtocolUtils.PadLeft(address, 6, '0') + MProtocolUtils.PadLeft(Convert.ToString(length, 0x10).ToUpper(), 4, '0');
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetSingleRRead(string deviceCode, string address, int length, BlockType blockType, int networkNo, int pcNo)
        {
            this.MakePLCheader(networkNo, pcNo);
            byte[] sourceArray = this.GetSingleProtocolLength(null, blockType, length);
            Array.Copy(sourceArray, 0, this.otherLocalheaderArray, 14, sourceArray.Length);
            string s = Encoding.ASCII.GetString(this.otherLocalheaderArray) + "0401" + ((blockType == BlockType.Bit) ? "0001" : "0000");
            string str2 = string.Empty;
            if (deviceCode == "ZR")
            {
                s = s + "ZR";
                str2 = Convert.ToString(Convert.ToInt32(address), 0x10);
            }
            else
            {
                s = s + (deviceCode + "*").PadLeft(2, '0');
                str2 = address;
            }
            s = s + str2.PadLeft(6, '0') + Convert.ToString(length, 0x10).ToUpper().PadLeft(4, '0');
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetSingleRWrite(string deviceCode, string address, string writeData, int length, BlockType blockType, int networkNo, int pcNo)
        {
            this.MakePLCheader(networkNo, pcNo);
            byte[] sourceArray = this.GetSingleProtocolLength(writeData, blockType, length);
            Array.Copy(sourceArray, 0, this.otherLocalheaderArray, 14, sourceArray.Length);
            string s = Encoding.ASCII.GetString(this.otherLocalheaderArray) + "1401" + ((blockType == BlockType.Bit) ? "0001" : "0000");
            string str2 = string.Empty;
            if (deviceCode == "ZR")
            {
                s = s + "ZR";
                str2 = Convert.ToString(Convert.ToInt32(address), 0x10);
            }
            else
            {
                s = s + (deviceCode + "*").PadLeft(2, '0');
                str2 = address;
            }
            s = s + str2.PadLeft(6, '0') + Convert.ToString(length, 0x10).ToUpper().PadLeft(4, '0');
            if (blockType != BlockType.Bit)
            {
                for (int i = 0; i < (writeData.Length / 4); i++)
                {
                    s = s + writeData.Substring(i * 4, 4);
                }
                if ((writeData.Length % 4) != 0)
                {
                    s = s + writeData.Substring(writeData.Length - (writeData.Length % 4)).PadLeft(4, '0');
                }
            }
            else
            {
                s = s + writeData;
            }
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetSingleWrite(string deviceCode, string address, string writeData, int length, BlockType blockType)
        {
            byte[] sourceArray = this.GetSingleProtocolLength(writeData, blockType, length);
            Array.Copy(sourceArray, 0, this.headerByteArray, 14, sourceArray.Length);
            string s = ((Encoding.ASCII.GetString(this.headerByteArray) + "1401") + ((blockType == BlockType.Bit) ? "0001" : "0000") + deviceCode.PadRight(2, '*')) + MProtocolUtils.PadLeft(address, 6, '0') + MProtocolUtils.PadLeft(Convert.ToString(length, 0x10).ToUpper(), 4, '0');
            if (blockType != BlockType.Bit)
            {
                for (int i = 0; i < (writeData.Length / 4); i++)
                {
                    s = s + writeData.Substring(i * 4, 4);
                }
                if ((writeData.Length % 4) != 0)
                {
                    s = s + MProtocolUtils.PadLeft(writeData.Substring(writeData.Length - (writeData.Length % 4)), 4, '0');
                }
            }
            else
            {
                s = s + writeData;
            }
            return Encoding.ASCII.GetBytes(s);
        }

        public void Init()
        {
            string s = "5000" + Convert.ToString(int.Parse(this.m_strNetworkNo), 0x10).PadLeft(2, '0') + Convert.ToString(int.Parse(this.m_strStationNo), 0x10).PadLeft(2, '0') + "03FF000000" + this.CpuTimer;
            this.headerByteArray = Encoding.ASCII.GetBytes(s);
        }

        private void MakePLCheader(int networkNo, int pcNo)
        {
            string s = "5000" + Convert.ToString(networkNo, 0x10).PadLeft(2, '0') + Convert.ToString(pcNo, 0x10).PadLeft(2, '0') + "03FF000000" + this.CpuTimer;
            this.otherLocalheaderArray = Encoding.ASCII.GetBytes(s);
        }

        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            EventHandler<DisconnectedEventArgs> disconnected = this.Disconnected;
            if (disconnected != null)
            {
                disconnected(this, e);
            }
        }

        private string ReadASCIIWord(string device, string address, int length, BlockType blockType)
        {
            byte[] dataArry = this.GetSingleRead(device, address, length, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadASCIIWord) address : {0}", address));
                return null;
            }
            string str2 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str2 = str2 + Convert.ToChar(int.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(str.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
            }
            return str2.TrimEnd(new char[1]).Trim();
        }

        private string ReadASCIIWordNew(string readWord)
        {
            string str = string.Empty;
            for (int i = 0; i < (readWord.Length / 4); i++)
            {
                str = str + Convert.ToChar(int.Parse(readWord.Substring((i * 4) + 2, 2), NumberStyles.HexNumber)).ToString() + Convert.ToChar(int.Parse(readWord.Substring(i * 4, 2), NumberStyles.HexNumber)).ToString();
            }
            return str.TrimEnd(new char[1]).Trim();
        }

        public string ReadBinaryWord(string device, string address, int length, BlockType blockType)
        {
            int num2;
            byte[] dataArry = this.GetSingleRead(device, address, length, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadBinaryWord) address : {0}", address));
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

        private string ReadBinaryWordNew(string readWord)
        {
            int num2;
            int num = 0;
            string str = string.Empty;
            for (num2 = 0; num2 < (readWord.Length / 4); num2++)
            {
                num = Convert.ToInt32(readWord.Substring(num2 * 4, 4), 0x10);
                str = str + Convert.ToString(num, 2).PadLeft(0x10, '0');
            }
            string str2 = string.Empty;
            for (num2 = 0; num2 < (str.Length / 0x10); num2++)
            {
                str2 = str.Substring(num2 * 0x10, 0x10) + str2;
            }
            return str2;
        }

        private string ReadBitWord(string device, string address, int length, BlockType blockType)
        {
            byte[] dataArry = this.GetSingleRead(device, address, length, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadBitWord) address : {0}", address));
                return null;
            }
            return Convert.ToString(Convert.ToInt32(str, 0x10), 2).PadLeft(0x10, '0');
        }

        private string ReadBitWordNew(string readWord)
        {
            return Convert.ToString(Convert.ToInt32(readWord, 0x10), 2).PadLeft(0x10, '0');
        }

        private string Reader(ReceiveStatus status)
        {
            try
            {
                int num;
                string str = string.Empty;
                string str2 = string.Empty;
                byte[] buffer = new byte[14];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                str2 = Encoding.ASCII.GetString(buffer);
                buffer = new byte[4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                str = Encoding.ASCII.GetString(buffer);
                if (str != "0000")
                {
                    this.logger.Error(string.Format("[Receive] Receive NG : Plc Name : {0} , ReadData : {1}", this.m_strName, str));
                    num = Convert.ToInt32(str2, 0x10);
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        string str3 = Encoding.ASCII.GetString(buffer);
                        this.logger.Error(string.Format("[Receive] Receive NG : Garbage String : {0}", str3));
                        this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return null;
                }
                if (status == ReceiveStatus.Write)
                {
                    num = Convert.ToInt32(str2, 0x10);
                    if (num > 4)
                    {
                        buffer = new byte[num - 4];
                        this.m_MelsecReader.Read(buffer, 0, buffer.Length);
                    }
                    return str;
                }
                buffer = new byte[Convert.ToInt32(str2, 0x10) - 4];
                this.m_MelsecReader.Read(buffer, 0, buffer.Length);
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
                if (status != ReceiveStatus.Write)
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

        private string ReadHexWord(string device, string address, int length, BlockType blockType)
        {
            byte[] dataArry = this.GetSingleRead(device, address, length, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadHexWord) address : {0}", address));
                return null;
            }
            string str3 = string.Empty;
            for (int i = 0; i < (str.Length / 4); i++)
            {
                str3 = (string.Empty + str.Substring(i * 4, 2)) + str.Substring((i * 4) + 2, 2) + str3;
            }
            return str3;
        }

        private string ReadHexWordNew(string readWord)
        {
            string str2 = string.Empty;
            for (int i = 0; i < (readWord.Length / 4); i++)
            {
                str2 = (string.Empty + readWord.Substring(i * 4, 2)) + readWord.Substring((i * 4) + 2, 2) + str2;
            }
            return str2;
        }

        private ushort ReadIntWord(string device, string address, BlockType blockType)
        {
            byte[] dataArry = this.GetSingleRead(device, address, 1, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if ((str == null) || string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Convert.ToUInt16(str, 0x10);
        }

        private short ReadIntWord2(string device, string address, BlockType blockType)
        {
            byte[] dataArry = this.GetSingleRead(device, address, 1, blockType);
            string str = this.ReadWriter(dataArry, ReceiveStatus.Read);
            if (str == null)
            {
                this.logger.Error(string.Format("[ReadWord Error] (ReadIntWord2) address : {0}", address));
                return 0;
            }
            return Convert.ToInt16(str, 0x10);
        }

        private short ReadIntWord2New(string readWord)
        {
            return Convert.ToInt16(readWord, 0x10);
        }

        private ushort ReadIntWordNew(string readWord)
        {
            return Convert.ToUInt16(readWord, 0x10);
        }

        public Dictionary<string, string> ReadRWordOnce(Block block, bool isHex, string networkNo, string pcNo)
        {
            Dictionary<string, string> itemReadWord = new Dictionary<string, string>();
            string address = string.Empty;
            BlockType word = BlockType.Word;
            try
            {
                byte[] buffer2;
                byte[] src = null;
                if (block.Points <= 960)
                {
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, 0);
                    buffer2 = this.GetSingleRRead(block.DeviceCode, address, block.Points, word, Convert.ToInt32(networkNo), Convert.ToInt32(pcNo));
                    src = this.ReadWriter(buffer2, ReceiveStatus.Read, false);
                }
                else if (block.Points > 960)
                {
                    int num = (block.Points / 960) + (((block.Points % 960) > 0) ? 1 : 0);
                    int points = block.Points;
                    for (int i = 0; i < num; i++)
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, 960 * i);
                        buffer2 = null;
                        if (points >= 960)
                        {
                            buffer2 = this.GetSingleRRead(block.DeviceCode, address, 960, word, Convert.ToInt32(networkNo), Convert.ToInt32(pcNo));
                            points -= 960;
                        }
                        else
                        {
                            buffer2 = this.GetSingleRRead(block.DeviceCode, address, points, word, Convert.ToInt32(networkNo), Convert.ToInt32(pcNo));
                            points = 0;
                        }
                        byte[] buffer3 = this.ReadWriter(buffer2, ReceiveStatus.Read, false);
                        if (src == null)
                        {
                            src = buffer3;
                        }
                        else
                        {
                            byte[] dst = new byte[src.Length + buffer3.Length];
                            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
                            Buffer.BlockCopy(buffer3, 0, dst, src.Length, buffer3.Length);
                            src = dst;
                        }
                    }
                }
                itemReadWord = this.GetItemReadWord(block, src);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return new Dictionary<string, string>();
            }
            return itemReadWord;
        }

        public Dictionary<string, string> ReadWord(Block block, bool isHex)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string address = string.Empty;
            string name = string.Empty;
            BlockType word = BlockType.Word;
            if (this.m_bMelsecOpen)
            {
                try
                {
                    foreach (Item item in block.Item)
                    {
                        string str4;
                        string str6;
                        string str7;
                        string str10;
                        string[] strArray = item.Offset.Split(new char[] { ':' });
                        string[] strArray2 = item.Points.Split(new char[] { ':' });
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]));
                        name = item.Name;
                        string representation = item.Representation;
                        switch (representation)
                        {
                            case "A":
                                {
                                    string str3 = this.ReadASCIIWord(block.DeviceCode, address, int.Parse(strArray2[0]), word);
                                    dictionary.Add(item.Name, str3);
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[ReadWord] ASCII Item Name : {0}, Data : {1}", item.Name, str3));
                                    }
                                    continue;
                                }
                            case "I":
                                {
                                    if (strArray.Length <= 1)
                                    {
                                        break;
                                    }
                                    str4 = this.ReadBitWord(block.DeviceCode, address, int.Parse(strArray2[0]), word).Substring((0x10 - int.Parse(strArray[1])) - int.Parse(strArray2[1]), int.Parse(strArray2[1]));
                                    dictionary.Add(item.Name, Convert.ToUInt16(str4, 2).ToString());
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, Convert.ToInt32(str4, 2)));
                                    }
                                    continue;
                                }
                            case "B":
                                {
                                    if (strArray.Length <= 1)
                                    {
                                        goto Label_03F3;
                                    }
                                    str4 = this.ReadBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), word).Substring((0x10 - int.Parse(strArray[1])) - 1, 1);
                                    dictionary.Add(item.Name, str4);
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[ReadWord] Bit Item Name : {0}, Data : {1}", item.Name, str4));
                                    }
                                    continue;
                                }
                            case "H":
                                {
                                    string str8 = this.ReadHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), word);
                                    dictionary.Add(item.Name, str8);
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[ReadWord] H Item Name : {0}, Data : {1}", item.Name, str8));
                                    }
                                    continue;
                                }
                            case "SI":
                                {
                                    string str9 = this.ReadIntWord2(block.DeviceCode, address, word).ToString();
                                    dictionary.Add(item.Name, str9);
                                    if (this.m_bLoggingFlag)
                                    {
                                        this.logger.Info(string.Format("[ReadWord] SI Item Name : {0}, Data : {1}", item.Name, str9));
                                    }
                                    continue;
                                }
                            case "F":
                                {
                                    if (int.Parse(strArray2[0]) == 2)
                                    {
                                        goto Label_0521;
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
                                    goto Label_02E1;
                                }
                            }
                            else
                            {
                                string str5 = this.ReadIntWord(block.DeviceCode, address, word).ToString();
                                dictionary.Add(item.Name, str5);
                                if (this.m_bLoggingFlag)
                                {
                                    this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, str5));
                                }
                            }
                        }
                        continue;
                    Label_02E1:
                        str6 = this.GetAddress(block.DeviceCode, block.HeadDevice, int.Parse(strArray[0]) + 1);
                        uint num = MProtocolUtils.MakeDWord(this.ReadIntWord(block.DeviceCode, str6, word), this.ReadIntWord(block.DeviceCode, address, word));
                        dictionary.Add(item.Name, num.ToString());
                        if (this.m_bLoggingFlag)
                        {
                            this.logger.Info(string.Format("[ReadWord] Int Item Name : {0}, Data : {1}", item.Name, num));
                        }
                        continue;
                    Label_03F3:
                        str7 = this.ReadBinaryWord(block.DeviceCode, address, int.Parse(strArray2[0]), word);
                        dictionary.Add(item.Name, str7);
                        if (this.m_bLoggingFlag)
                        {
                            this.logger.Info(string.Format("[ReadWord] Bit Item Name : {0}, Data : {1}", item.Name, str7));
                        }
                        continue;
                    Label_0521:
                        str10 = this.ReadHexWord(block.DeviceCode, address, int.Parse(strArray2[0]), word);
                        byte[] buffer = new byte[4];
                        buffer[1] = Convert.ToByte(Convert.ToInt32(str10.Substring(0, 2), 0x10));
                        buffer[0] = Convert.ToByte(Convert.ToInt32(str10.Substring(2, 2), 0x10));
                        buffer[3] = Convert.ToByte(Convert.ToInt32(str10.Substring(4, 2), 0x10));
                        buffer[2] = Convert.ToByte(Convert.ToInt32(str10.Substring(6, 2), 0x10));
                        float num2 = BitConverter.ToSingle(buffer, 0);
                        dictionary.Add(item.Name, num2.ToString());
                        if (this.m_bLoggingFlag)
                        {
                            this.logger.Info(string.Format("[ReadWord] F Item Name : {0}, Data : {1}", item.Name, num2.ToString()));
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error(string.Format("Read Word Error : {0}, address : {1}, itemname : {2} ", exception.Message, address, name));
                    return new Dictionary<string, string>();
                }
            }
            return dictionary;
        }

        public Dictionary<string, string> ReadWordOnce(Block block, bool isHex)
        {
            Dictionary<string, string> itemReadWord = new Dictionary<string, string>();
            string address = string.Empty;
            BlockType word = BlockType.Word;
            try
            {
                byte[] buffer2;
                byte[] src = null;
                if (block.Points <= 960)
                {
                    address = this.GetAddress(block.DeviceCode, block.HeadDevice, 0);
                    buffer2 = this.GetSingleRead(block.DeviceCode, address, block.Points, word);
                    src = this.ReadWriter(buffer2, ReceiveStatus.Read, false);
                }
                else if (block.Points > 960)
                {
                    int num = (block.Points / 960) + (((block.Points % 960) > 0) ? 1 : 0);
                    int points = block.Points;
                    for (int i = 0; i < num; i++)
                    {
                        address = this.GetAddress(block.DeviceCode, block.HeadDevice, 960 * i);
                        buffer2 = null;
                        if (points >= 960)
                        {
                            buffer2 = this.GetSingleRead(block.DeviceCode, address, 960, word);
                            points -= 960;
                        }
                        else
                        {
                            buffer2 = this.GetSingleRead(block.DeviceCode, address, points, word);
                        }
                        byte[] buffer3 = this.ReadWriter(buffer2, ReceiveStatus.Read, false);
                        if (src == null)
                        {
                            src = buffer3;
                        }
                        else
                        {
                            byte[] dst = new byte[src.Length + buffer3.Length];
                            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
                            Buffer.BlockCopy(buffer3, 0, dst, src.Length, buffer3.Length);
                            src = dst;
                        }
                    }
                }
                itemReadWord = this.GetItemReadWord(block, src);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return new Dictionary<string, string>();
            }
            return itemReadWord;
        }

        private string ReadWriter(byte[] dataArry, ReceiveStatus status)
        {
            string str;
            try
            {
                lock (this.readWriteObj)
                {
                    if (this.m_MelsecWriter == null)
                    {
                        return "0";
                    }
                    this.m_MelsecWriter.Write(dataArry, 0, dataArry.Length);
                    str = this.Reader(status);
                }
            }
            catch (IOException)
            {
                str = null;
            }
            catch (ObjectDisposedException)
            {
                str = null;
            }
            return str;
        }

        private byte[] ReadWriter(byte[] dataArry, ReceiveStatus status, bool isCacheFun)
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
                    lock (this.readWriteObj)
                    {
                        this.m_MelsecWriter.Write(dataArry, 0, dataArry.Length);
                        buffer = this.Reader(status, isCacheFun);
                    }
                }
            }
            catch (IOException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                buffer = null;
            }
            catch (ObjectDisposedException)
            {
                this.OnDisconnected(new DisconnectedEventArgs(this));
                buffer = null;
            }
            return buffer;
        }

        public void SetMelsetReaderWriter(BinaryReader reader, BinaryWriter writer, string name)
        {
            this.m_strName = name;
            this.m_bMelsecOpen = true;
            this.m_MelsecReader = reader;
            this.m_MelsecWriter = writer;
        }

        public bool LoggingFlag
        {
            set
            {
                this.m_bLoggingFlag = value;
            }
        }

        public enum BlockType
        {
            Bit,
            Word
        }

        public enum ReceiveStatus
        {
            Read,
            Write
        }
    }
}
