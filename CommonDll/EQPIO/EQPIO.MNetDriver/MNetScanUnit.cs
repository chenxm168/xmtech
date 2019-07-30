
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class MNetScanUnit
    {
        private ILog logger;
        private bool m_bScanStartUpCheck;
        private Dictionary<string, string> m_dicBitScanData;
        private Dictionary<string, string> m_dicWordScanData;
        private int m_iScanInterval;
        private MultiBlock m_ParentMultiBlock;
        private Thread m_tScanThread;
        private IMNetDriver plc;
        private BlockMap plcBlockMap;
        private Block plcScanBlock;

        public event ErrorMessageHandler OnErrorMessage;

        public event ScanEventHandler OnScanReceived;

        public MNetScanUnit(int channel, BlockMap plcMap, Block block, MultiBlock multiBlock)
        {
            this.logger = LogManager.GetLogger(typeof(MNetScanUnit));
            this.m_dicBitScanData = new Dictionary<string, string>();
            this.m_dicWordScanData = new Dictionary<string, string>();
            this.m_ParentMultiBlock = null;
            this.m_bScanStartUpCheck = true;
            this.Channel = channel;
            this.plcBlockMap = plcMap;
            this.plcScanBlock = block;
            this.m_iScanInterval = multiBlock.Interval;
            this.MultiBlockName = multiBlock.Name;
            this.Name = block.Name;
            this.m_ParentMultiBlock = multiBlock;
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
            this.m_tScanThread.Name = "ScanThreadProc-" + block.Name;
            this.m_tScanThread.IsBackground = true;
        }

        public MNetScanUnit(int channel, BlockMap plcMap, Block block, int interval, string unitName, string multiBlockName)
        {
            this.logger = LogManager.GetLogger(typeof(MNetScanUnit));
            this.m_dicBitScanData = new Dictionary<string, string>();
            this.m_dicWordScanData = new Dictionary<string, string>();
            this.m_ParentMultiBlock = null;
            this.m_bScanStartUpCheck = true;
            this.Channel = channel;
            this.plcBlockMap = plcMap;
            this.plcScanBlock = block;
            this.m_iScanInterval = interval;
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
            this.m_tScanThread.Name = "ScanThreadProc-" + block.Name;
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
            this.OnErrorMessage(message);
        }

        private void Scan()
        {
            if (this.m_bScanStartUpCheck)
            {
                Globalproperties.Instance.UpdateScanStatus(enumConnectionType.PLCBoard, "PLCBoard", this.m_ParentMultiBlock, this.m_ParentMultiBlock.StartUp.ToUpper() == "TRUE");
                if (this.m_ParentMultiBlock.StartUp.ToLower() == "false")
                {
                    return;
                }
                this.m_bScanStartUpCheck = false;
            }
            else if (!Globalproperties.Instance.ScanStatusPLCBoard.MultiBlockOnOff["PLCBoard"][this.m_ParentMultiBlock])
            {
                return;
            }
            if (this.plcScanBlock.DeviceCode == "B")
            {
                this.ScanBit();
            }
            else if (this.plcScanBlock.DeviceCode == "W")
            {
                this.ScanWord();
            }
        }

        private void ScanBit()
        {
            try
            {
                string data = string.Empty;
                int num = 0;
                int num2 = Convert.ToInt32(this.plcScanBlock.address.ToString().Substring(1), 0x10) % 8;
                byte[] buffer = this.plc.ReadBlock(this.plcScanBlock.address, (int)((this.plcScanBlock.Points / 0x10) + 1));
                if (buffer == null)
                {
                    this.logger.Error("Scan Bit Error : buf is null");
                }
                else
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        data = Convert.ToString(buffer[i], 2).PadLeft(8, '0') + data;
                    }
                    data = MNetUtils.CharRevcrse(data);
                    if (!this.m_dicBitScanData.ContainsKey(this.plcScanBlock.Name))
                    {
                        this.m_dicBitScanData.Add(this.plcScanBlock.Name, data);
                    }
                    else if (!this.m_dicBitScanData[this.plcScanBlock.Name].Equals(data))
                    {
                        foreach (Item item in this.plcScanBlock.Item)
                        {
                            if (num2 > 0)
                            {
                                num = int.Parse(item.Offset) + num2;
                            }
                            else
                            {
                                num = int.Parse(item.Offset);
                            }
                            char ch = this.m_dicBitScanData[this.plcScanBlock.Name][num];
                            if (!ch.Equals(data[num]))
                            {
                                if (((item.Name.ToUpper() != "MACHINEHEARTBEATSIGNAL") && (item.Name.ToUpper() != "EQUIPMENTALIVE")) && !Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(item.Name.ToUpper()))
                                {
                                    this.logger.Info(string.Format("ScanBit BlockName : {0} ItemName : {1} , Value : {2}", this.plcScanBlock.Name, item.Name, data[num]));
                                }
                                this.OnScanReceived(this, this.plcScanBlock.Name, item.Name, data[num] == '1');
                            }
                        }
                        this.m_dicBitScanData[this.plcScanBlock.Name] = data;
                    }
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
                bool flag;
                goto Label_0019;
            Label_0004:
                this.Scan();
                Thread.Sleep(this.m_iScanInterval);
            Label_0019:
                flag = true;
                goto Label_0004;
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Scan unit Error : {0}", exception.Message));
                //logger.Error(exception.StackTrace);
            }
        }

        private void ScanWord()
        {
            try
            {
                string data = string.Empty;
                int num = 0;
                byte[] buffer = this.plc.ReadBlock(this.plcScanBlock.address, this.plcScanBlock.Points);
                if (buffer == null)
                {
                    this.logger.Error("Scan Word Error : buf is null");
                }
                else
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        data = Convert.ToString(buffer[i], 2).PadLeft(8, '0') + data;
                    }
                    data = MNetUtils.CharRevcrse(data);
                    if (!this.m_dicBitScanData.ContainsKey(this.plcScanBlock.Name))
                    {
                        this.m_dicBitScanData.Add(this.plcScanBlock.Name, data);
                    }
                    else if (!this.m_dicBitScanData[this.plcScanBlock.Name].Equals(data))
                    {
                        if (this.m_dicBitScanData[this.plcScanBlock.Name].Length != data.Length)
                        {
                            this.logger.Error(string.Format("ReadData Index difference, Pre Length : [{0}], Now Length: [{1}]", this.m_dicBitScanData[this.plcScanBlock.Name].Length, data.Length));
                        }
                        else
                        {
                            foreach (Item item in this.plcScanBlock.Item)
                            {
                                string[] strArray = item.Offset.Split(new char[] { ':' });
                                num = (int.Parse(strArray[0]) * 0x10) + int.Parse(strArray[1]);
                                char ch = this.m_dicBitScanData[this.plcScanBlock.Name][num];
                                if (!ch.Equals(data[num]))
                                {
                                    this.logger.Info(string.Format("ScanWord BlockName : {0} ItemName : {1} , Value : {2}", this.plcScanBlock.Name, item.Name, data[num]));
                                    this.OnScanReceived(this, this.plcScanBlock.Name, item.Name, data[num] == '1');
                                }
                            }
                            this.m_dicBitScanData[this.plcScanBlock.Name] = data;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        public void Start()
        {
            if ((this.plc != null) && (this.m_tScanThread != null))
            {
                this.m_tScanThread.Start();
                this.logger.Info(string.Format("ScanUnit PLC Thread Start UnitName : {0}", this.Name));
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

        public int Channel { get; set; }

        public string MultiBlockName { get; set; }

        public string Name { get; set; }

        public delegate void ErrorMessageHandler(string message);

        public delegate void ScanEventHandler(object sender, string blockName, string itemName, bool flag);
    }
}
