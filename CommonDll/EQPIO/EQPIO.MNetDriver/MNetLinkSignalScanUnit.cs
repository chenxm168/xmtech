
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using log4net;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class MNetLinkSignalScanUnit
    {
        private ILog logger = LogManager.GetLogger(typeof(MNetScanUnit));
        private int m_iChannel = 0;
        private int m_iInterval = 0;
        private BlockMap m_plcBlockMap;
        private Block m_plcScanBlock;
        private string m_strName = string.Empty;
        private Thread m_tScan;
        private IMNetDriver plc;

        public event LinkSignalScanHandler OnLinkSignalScanReceived;

        public MNetLinkSignalScanUnit(int channel, BlockMap plcMap, Block block, int interval, string unitName, string multiBlockName)
        {
            this.m_iChannel = channel;
            this.m_plcBlockMap = plcMap;
            this.m_plcScanBlock = block;
            this.m_iInterval = interval;
            this.m_strName = unitName;
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
            }
            this.m_tScan = new Thread(new ThreadStart(this.ScanProc));
            this.m_tScan.Name = "ScanProc(LinkSignal)";
            this.m_tScan.IsBackground = true;
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
            if (!this.plc.Open(this.m_iChannel))
            {
                return false;
            }
            return true;
        }

        private void ScanLinkSignalBlock()
        {
            try
            {
                string data = string.Empty;
                int overCount = Convert.ToInt32(this.m_plcScanBlock.address.ToString().Substring(1), 0x10) % 8;
                byte[] buffer = this.plc.ReadBlock(this.m_plcScanBlock.address, (int)((this.m_plcScanBlock.Points / 0x10) + 1));
                if (buffer == null)
                {
                    this.logger.Error("LinkSignalBlock Read Error : buf is null");
                }
                else
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        data = Convert.ToString(buffer[i], 2).PadLeft(8, '0') + data;
                    }
                    this.OnLinkSignalScanReceived(this, MNetUtils.CharRevcrse(data), overCount, this.m_plcScanBlock);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void ScanProc()
        {
            try
            {
                bool flag;
                goto Label_003B;
            Label_0004:
                if (this.m_plcScanBlock.DeviceCode == "B")
                {
                    this.ScanLinkSignalBlock();
                }
                Thread.Sleep(this.m_iInterval);
            Label_003B:
                flag = true;
                goto Label_0004;
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("ScanLinkSignalBlock Error : {0}", exception.Message));
            }
        }

        public void Start()
        {
            this.m_tScan.Start();
            this.logger.Info(string.Format("LinkSignalScanUnit PLC Thread Start UnitName : {0}", this.m_strName));
        }

        public void Stop()
        {
            if ((this.m_tScan != null) && this.m_tScan.IsAlive)
            {
                this.m_tScan.Abort();
                this.logger.Info(string.Format("LinkSignalScanUnit PLC Thread Stop UnitName : {0}", this.m_strName));
            }
        }

        public delegate void LinkSignalScanHandler(object sender, string readBitString, int overCount, Block plcScanBlock);
    }
}
