
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MNetSVScanUnit
    {
        private ILog logger = LogManager.GetLogger(typeof(MNetScanUnit));
        private bool m_bisRGA = false;
        private Thread m_tScanThread;
        private IMNetDriver plc;
        private BlockMap plcBlockMap;
        private Block plcScanBlock;

        public event ErrorMessageHandler OnErrorMessage;

        public event ScanEventHandler OnRGAScanReceived;

        public event ScanEventHandler OnScanReceived;

        public MNetSVScanUnit(int channel, BlockMap plcMap, Block block, int interval, string unitName, string multiBlockName)
        {
            this.Channel = channel;
            this.plcBlockMap = plcMap;
            this.plcScanBlock = block;
            this.ScanInterval = interval;
            this.Name = unitName;
            this.MultiBlockName = multiBlockName;
            switch (channel)
            {
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                    this.plc = new MNetDriverH();
                    break;

                case 0x97:
                case 0x98:
                case 0x99:
                case 0x9a:
                    this.plc = new MNetDriverG();
                    break;

                default:
                    this.plc = new MNetDriverG();
                    break;
            }
            this.plc.OnErrorMessage += new IMNetDriver.ErrorMessage(this.plc_OnErrorMessage);
            this.m_tScanThread = new Thread(new ThreadStart(this.ScanThreadProc));
            this.m_tScanThread.Name = "ScanThreadProc(SV)-" + block.Name;
            this.m_tScanThread.IsBackground = true;
        }

        public void Close()
        {
            if ((this.plc != null) && this.plc.IsOpened)
            {
                this.plc.Close();
            }
        }

        public bool Open()
        {
            if (!this.plc.Open(this.Channel))
            {
                return false;
            }
            return true;
        }

        private void plc_OnErrorMessage(string message)
        {
            message = "[MNetSVScanUnit] " + message;
            this.OnErrorMessage(message);
        }

        private void ScanThreadProc()
        {
            try
            {
                bool flag;
                goto Label_004E;
            Label_0004:
                if (this.plcScanBlock.DeviceCode == "B")
                {
                    this.SVBitScan(this.plcScanBlock);
                }
                else
                {
                    this.SVScan(this.plcScanBlock);
                }
                Thread.Sleep(this.ScanInterval);
            Label_004E:
                flag = true;
                goto Label_0004;
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("ScanSVBit Error : {0}", exception.Message));
            }
        }

        public void Start()
        {
            if ((this.plc != null) && (this.m_tScanThread != null))
            {
                this.m_tScanThread.Start();
                this.logger.Info(string.Format("ScanSVUnit PLC Thread Start UnitName : {0}", this.Name));
            }
        }

        public void Stop()
        {
            if ((this.m_tScanThread != null) && this.m_tScanThread.IsAlive)
            {
                this.m_tScanThread.Abort();
                this.logger.Info(string.Format("ScanUnit PLC Thread Stop UnitName : {0}", this.Name));
                this.m_tScanThread.Join(0x3e8);
            }
        }

        private void SVBitScan(Block plcBlock)
        {
            Func<Block, bool> predicate = null;
            try
            {
                string str = string.Empty;
                int num = 0;
                if (predicate == null)
                {
                    predicate = blockName => blockName.Name == plcBlock.Name;
                }
                Block block = this.plcBlockMap.Block.Where<Block>(predicate).FirstOrDefault<Block>();
                if (block != null)
                {
                    num = (short)(Convert.ToInt32(block.address.ToString().Substring(1), 0x10) % 8);
                    str = this.plc.ReadBitString(block.address - num, (short)(block.Points + num));
                    foreach (Item item in plcBlock.Item)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void SVScan(Block block)
        {
            try
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                byte[] bytes = this.plc.ReadBlock(block.address, block.Points);
                if (bytes == null)
                {
                    this.logger.Error("SVScan Word Error : buf is null");
                }
                else
                {
                    foreach (Item item in block.Item)
                    {
                        byte[] buffer2;
                        byte[] buffer3;
                        int num3;
                        ushort[] numArray2;
                        string[] strArray = item.Offset.Split(new char[] { ':' });
                        string[] strArray2 = item.Points.Split(new char[] { ':' });
                        string representation = item.Representation;
                        if (representation == null)
                        {
                            continue;
                        }
                        if (!(representation == "A"))
                        {
                            if (representation == "I")
                            {
                                goto Label_016F;
                            }
                            if (representation == "B")
                            {
                                goto Label_023A;
                            }
                            if (representation == "H")
                            {
                                goto Label_0369;
                            }
                            if (representation == "SI")
                            {
                                goto Label_0425;
                            }
                            continue;
                        }
                        int count = 0;
                        count = 0;
                        while (count < (int.Parse(strArray2[0]) * 2))
                        {
                            if (bytes[(int.Parse(strArray[0]) * 2) + count] == 0)
                            {
                                break;
                            }
                            count++;
                        }
                        string str = Encoding.Default.GetString(bytes, int.Parse(strArray[0]) * 2, count).Trim();
                        list.Add(item.Name, str);
                        continue;
                    Label_016F:
                        if (strArray.Length <= 1)
                        {
                            representation = strArray2[0];
                            if (representation != null)
                            {
                                if (!(representation == "1"))
                                {
                                    if (representation == "2")
                                    {
                                        goto Label_01F4;
                                    }
                                }
                                else
                                {
                                    buffer2 = new byte[4];
                                    Buffer.BlockCopy(bytes, int.Parse(item.Offset) * 2, buffer2, 0, 2);
                                    list.Add(item.Name, BitConverter.ToUInt16(buffer2, 0).ToString());
                                }
                            }
                        }
                        continue;
                    Label_01F4:
                        buffer3 = new byte[4];
                        Buffer.BlockCopy(bytes, int.Parse(item.Offset) * 2, buffer3, 0, 4);
                        list.Add(item.Name, BitConverter.ToUInt32(buffer3, 0).ToString());
                        continue;
                    Label_023A:
                        if (strArray.Length > 1)
                        {
                            string str2 = this.plc.ReadBitString(block.address + int.Parse(strArray[0]), int.Parse(strArray2[0])).Substring(int.Parse(strArray[1]), int.Parse(strArray2[1]));
                            list.Add(item.Name, str2);
                        }
                        else
                        {
                            ushort[] dst = new ushort[int.Parse(item.Points)];
                            Buffer.BlockCopy(bytes, int.Parse(item.Offset) * 2, dst, 0, int.Parse(item.Points) * 2);
                            string data = string.Empty;
                            foreach (short num2 in dst)
                            {
                                num3 = 0;
                                while (num3 < 0x10)
                                {
                                    data = data + (((num2 & (((int)1) << num3)) == 0) ? '0' : '1');
                                    num3++;
                                }
                            }
                            string str4 = string.Empty;
                            str4 = MNetUtils.CharRevcrse(data) + str4;
                            list.Add(item.Name, str4);
                        }
                        continue;
                    Label_0369:
                        numArray2 = this.plc.ReadWordBlock(block.address + int.Parse(strArray[0]), int.Parse(strArray2[0]));
                        string str5 = string.Empty;
                        string str6 = string.Empty;
                        string str7 = string.Empty;
                        for (num3 = 0; num3 < numArray2.Length; num3++)
                        {
                            str5 = string.Empty;
                            str6 = Convert.ToString((int)numArray2[num3], 0x10).PadLeft(4, '0');
                            str7 = (str5 + str6.Substring(0, 2)) + str6.Substring(2) + str7;
                        }
                        list.Add(item.Name, str7);
                        continue;
                    Label_0425:
                        if (strArray.Length <= 1)
                        {
                            representation = strArray2[0];
                            if (representation != null)
                            {
                                if (!(representation == "1"))
                                {
                                    if (representation == "2")
                                    {
                                        goto Label_04AA;
                                    }
                                }
                                else
                                {
                                    buffer2 = new byte[4];
                                    Buffer.BlockCopy(bytes, int.Parse(item.Offset) * 2, buffer2, 0, 2);
                                    list.Add(item.Name, BitConverter.ToInt16(buffer2, 0).ToString());
                                }
                            }
                        }
                        continue;
                    Label_04AA:
                        buffer3 = new byte[4];
                        Buffer.BlockCopy(bytes, int.Parse(item.Offset) * 2, buffer3, 0, 4);
                        list.Add(item.Name, BitConverter.ToInt32(buffer3, 0).ToString());
                    }
                    if (this.IsRGA)
                    {
                        this.OnRGAScanReceived(this, block.Name, list);
                    }
                    else
                    {
                        this.OnScanReceived(this, block.Name, list);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public int Channel { get; set; }

        public bool IsRGA
        {
            get
            {
                return this.m_bisRGA;
            }
            set
            {
                this.m_bisRGA = value;
            }
        }

        public string MultiBlockName { get; set; }

        public string Name { get; set; }

        public int ScanInterval { get; set; }

        public delegate void ErrorMessageHandler(string message);

        public delegate void ScanEventHandler(object sender, string blockName, Dictionary<string, string> list);
    }
}
