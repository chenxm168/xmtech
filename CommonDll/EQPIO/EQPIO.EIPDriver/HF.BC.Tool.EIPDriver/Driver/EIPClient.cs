
namespace HF.BC.Tool.EIPDriver
{
    using HF.BC.Tool.EIPDriver.Config;
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Driver.DataGathering;
    using HF.BC.Tool.EIPDriver.Driver.EventHandler;
    using HF.BC.Tool.EIPDriver.Driver.Threading;
    using HF.BC.Tool.EIPDriver.Enums;
    using log4net;
    using log4net.Config;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class EIPClient
    {
        private EIPConfig _eipConfig = null;
        private HF.BC.Tool.EIPDriver.EIPDriver _eipDriver = null;
        private bool _isOpen = false;
        private Dictionary<string, IScanTimer> _scanTimer = new Dictionary<string, IScanTimer>();
        private TagFactory _tagFactory = null;
        private AbstractEventInvoker eventInvoker = null;
        private ILog logger = LogManager.GetLogger(typeof(EIPClient));
        private AbstractEventInvoker svDataEventInvoker = null;

        public event EIPEventHandler.ReceiveCompleteEventHandler OnReceived;

        public event EIPEventHandler.SVDataEventHandler OnSVData;

        private void CheckBeforehand(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                string message = "Name is Null or Empty";
                this.logger.Error(message);
                throw new Exception(message);
            }
            if (!this._isOpen)
            {
                throw new Exception(EIPConst.IS_NOT_OPEN);
            }
        }

        public void Close()
        {
            this.StopScanAll();
            this._isOpen = false;
            if (this._eipDriver != null)
            {
                this._eipDriver.Stop();
            }
            this.logger.Info("Terminate");
        }

        private void CompareMapTagAndSysmacTag()
        {
            List<string> sysmacGatewayTagNames = this._eipDriver.SysmacGatewayTagNames;
            foreach (string str in this._tagFactory.GetTagNames())
            {
                if (!sysmacGatewayTagNames.Contains(str))
                {
                    throw new Exception(string.Format("The tag does not exist in Sysmac Gateway. tag name={0}", str));
                }
            }
        }

        public ConfigureError Configure(EIPConfig config)
        {
            Exception exception;
            string loggerName = "EIPDriver." + config.DriverName;
            try
            {
                XmlConfigurator.Configure(new FileInfo(config.Log4NetPath));
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.logger.Error(exception);
                return ConfigureError.InvalidLogFile;
            }
            try
            {
                this.WriteStartUpLog();
                this.CreateTagFactory(config.EIPMapFile);
                this._eipDriver = new HF.BC.Tool.EIPDriver.EIPDriver(this._tagFactory, config);
                foreach (Tag tag in this._tagFactory.GetTags())
                {
                    if (tag.Name.IndexOf("SD_") > -1)
                    {
                        this._eipDriver.Read(tag);
                        foreach (Block block in tag.BlockCollection.Values)
                        {
                            this._eipDriver.AddTimeOutCheckBlock(block);
                        }
                    }
                }
                this.CreateEventInvoker(loggerName, config.DriverName);
                this._eipConfig = (EIPConfig)config.Clone();
                this.logger.Info("Finish configuration.");
            }
            catch (Exception exception2)
            {
                exception = exception2;
                this.logger.Error(exception);
                return ConfigureError.InvalidMapFile;
            }
            return ConfigureError.None;
        }

        public Block CreateBlock(string blockName)
        {
            this.CheckBeforehand(blockName);
            Block block = this._tagFactory.CreateBlock(blockName);
            if (block == null)
            {
                throw new Exception(string.Format("Could not found Block [{0}]", blockName));
            }
            this._eipDriver.Read(block, false);
            return block;
        }

        private void CreateEventInvoker(string loggerName, string driverName)
        {
            if (this.eventInvoker != null)
            {
                this.eventInvoker.LoggerName = loggerName;
            }
            else if (this.svDataEventInvoker != null)
            {
                this.svDataEventInvoker.LoggerName = loggerName;
            }
            else
            {
                EIPEventInvoker invoker = new EIPEventInvoker(loggerName, driverName + "-EVT");
                invoker.OnReceived += new EIPEventHandler.ReceiveCompleteEventHandler(this.OnEventReceived);
                EIPSVDataEventInvoker invoker2 = new EIPSVDataEventInvoker(loggerName, driverName + "-SVDataEvt");
                invoker2.OnReceived += new EIPEventHandler.ReceiveCompleteEventHandler(this.svDataInvoker_OnReceived);
                this.eventInvoker = invoker;
                this.eventInvoker.Start();
                this.svDataEventInvoker = invoker2;
                this.svDataEventInvoker.Start();
            }
        }

        public Trx CreateSendTransaction(string trxName)
        {
            this.CheckBeforehand(trxName);
            Trx trx = this._tagFactory.CreateSendTrx(trxName);
            if (trx == null)
            {
                throw new Exception(string.Format("Could not found transaction [{0}]", trxName));
            }
            return trx;
        }

        public Tag CreateTag(string tagName)
        {
            this.CheckBeforehand(tagName);
            Tag tag = this._tagFactory.CreateTag(tagName);
            if (tag == null)
            {
                throw new Exception(string.Format("Could not found Tag [{0}]", tagName));
            }
            this._eipDriver.Read(tag);
            return tag;
        }

        private void CreateTagFactory(string mapFile)
        {
            if (this._tagFactory == null)
            {
                this._tagFactory = new TagFactory();
            }
            this._tagFactory.Configure(mapFile, true);
        }

        public List<string> GetConfigTagNames()
        {
            return this._tagFactory.GetTagNames();
        }

        private void OnEventReceived(object sender, Trx trx)
        {
            if (this.OnReceived != null)
            {
                this.OnReceived(sender, trx);
            }
            else
            {
                this.logger.Warn("HF.BC.Tool.EIPDriver.OnReceived Event is not used.");
            }
        }

        public bool Open()
        {
            bool flag;
            try
            {
                this.CompareMapTagAndSysmacTag();
                this.StartScanAll();
                this._isOpen = true;
                this.logger.Info("###################### Open ########################");
                flag = true;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
            return flag;
        }

        public void ReadBlock(Block block)
        {
            try
            {
                if (!this._isOpen)
                {
                    throw new Exception(EIPConst.IS_NOT_OPEN);
                }
                this._eipDriver.Read(block, true);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        public void ReadTag(Tag tag)
        {
            try
            {
                if (!this._isOpen)
                {
                    throw new Exception(EIPConst.IS_NOT_OPEN);
                }
                tag.StartTime = DateTime.Now.Ticks;
                this._eipDriver.Read(tag);
                tag.EndTime = DateTime.Now.Ticks;
                this.logger.Info(string.Format("{0} ({1}) {2} ms", tag.Name, tag.Points, tag.Duration));
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        private void scanTimer_OnScanReceived(object sender, object data)
        {
            this.eventInvoker.FireReceivedEvent(sender, data);
        }

        private void StartScanAll()
        {
            this._scanTimer.Clear();
            foreach (MultiTag tag in this._tagFactory.CreateScans())
            {
                if (tag.StartUp)
                {
                    if (tag.SVData)
                    {
                        SVDataTimer timer = new SVDataTimer(this._tagFactory, tag, this._eipDriver);
                        timer.OnScanReceived += new SVDataTimer.ScanEventHandler(this.svDataTime_OnScanReceived);
                        timer.Start();
                        this._scanTimer.Add(tag.Name, timer);
                    }
                    else
                    {
                        ScanTimer timer2 = new ScanTimer(this._tagFactory, tag, this._eipDriver);
                        timer2.OnScanReceived += new ScanTimer.ScanEventHandler(this.scanTimer_OnScanReceived);
                        timer2.Start();
                        this._scanTimer.Add(tag.Name, timer2);
                    }
                    this.logger.Info("Start Scan -" + this.ToDataGatherLogString(tag));
                }
            }
        }

        private void StopScanAll()
        {
            try
            {
                foreach (string str in this._scanTimer.Keys)
                {
                    this.logger.Info(string.Format("Stop Scan - {0}", str));
                    this._scanTimer[str].Stop();
                }
                this._scanTimer.Clear();
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void svDataInvoker_OnReceived(object sender, Trx trx)
        {
            if (this.OnSVData != null)
            {
                this.OnSVData(sender, trx);
            }
        }

        private void svDataTime_OnScanReceived(object sender, object data)
        {
            this.svDataEventInvoker.FireReceivedEvent(sender, data);
        }

        private string ToDataGatherLogString(MultiTag multiTag)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(multiTag.Name);
            builder.Append(string.Format(" ({0})ms ", multiTag.Interval));
            builder.Append(" [");
            foreach (Tag tag in multiTag.TagCollection.Values)
            {
                builder.Append(" ");
                builder.Append(tag.Name);
                builder.Append(", ");
                builder.Append("\r\n");
            }
            builder.Append("]");
            return builder.ToString();
        }

        public void WriteBlock(Block block)
        {
            try
            {
                if (!this._isOpen)
                {
                    throw new Exception(EIPConst.IS_NOT_OPEN);
                }
                this._eipDriver.Write(block);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        private void WriteStartUpLog()
        {
            this.logger.Info(string.Empty);
            this.logger.Info(string.Empty);
            this.logger.Info("####################################################");
            this.logger.Info("#   HF Systems, Inc. EIPDriver " + EIPConst.VERSION.PadRight(11) + "   #");
            this.logger.Info("####################################################");
            this.logger.Info(string.Empty);
            this.logger.Info(string.Empty);
            this.logger.Info("Start configuration.");
        }

        public void WriteTag(Tag tag)
        {
            try
            {
                if (!this._isOpen)
                {
                    throw new Exception(EIPConst.IS_NOT_OPEN);
                }
                tag.StartTime = DateTime.Now.Ticks;
                this._eipDriver.Write(tag);
                tag.EndTime = DateTime.Now.Ticks;
                this.logger.Info(string.Format("{0} ({1}) {2} ms", tag.Name, tag.Points, tag.Duration));
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        public void WriteTrx(Trx trx)
        {
            try
            {
                if (!this._isOpen)
                {
                    throw new Exception(EIPConst.IS_NOT_OPEN);
                }
                this._eipDriver.Write(trx);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        public bool IsOpen
        {
            get
            {
                return this._isOpen;
            }
        }

        internal class EIPEventInvoker : AbstractEventInvoker
        {
            private string name;

            public event EIPEventHandler.ReceiveCompleteEventHandler OnReceived;

            internal EIPEventInvoker(string loggerName, string name)
            {
                if (!string.IsNullOrEmpty(loggerName))
                {
                    base.logger = LogManager.GetLogger(loggerName);
                }
                this.name = name;
            }

            protected override void Run()
            {
                while (base.running)
                {
                    try
                    {
                        EIPEvent event2 = base.eventQueue.Dequeue();
                        base.logger.Info(string.Format("Queue Count = {0}", base.eventQueue.Count));
                        if (event2 == null)
                        {
                            continue;
                        }
                        if (this.OnReceived != null)
                        {
                            Trx data = event2.Data as Trx;
                            if (!data.BitOffEventReport && !data.EventBit)
                            {
                                continue;
                            }
                            base.logger.Info("RECEIVE EVENT BEGIN INVOKE.");
                            this.OnReceived(event2.Sender, data);
                            base.logger.Info("RECEIVE EVENT END INVOKE.");
                        }
                    }
                    catch (Exception exception)
                    {
                        base.logger.Error(exception);
                    }
                }
            }

            public override string Name
            {
                get
                {
                    return this.name;
                }
            }
        }

        internal class EIPSVDataEventInvoker : AbstractEventInvoker
        {
            private string name;

            public event EIPEventHandler.ReceiveCompleteEventHandler OnReceived;

            internal EIPSVDataEventInvoker(string loggerName, string name)
            {
                if (!string.IsNullOrEmpty(loggerName))
                {
                    base.logger = LogManager.GetLogger(loggerName);
                }
                this.name = name;
            }

            protected override void Run()
            {
                while (base.running)
                {
                    try
                    {
                        EIPEvent event2 = base.eventQueue.Dequeue();
                        if (base.eventQueue.Count > 0x3e8)
                        {
                            base.logger.Info(string.Format("SVDATA Queue Count = {0}", base.eventQueue.Count));
                        }
                        if (event2 == null)
                        {
                            continue;
                        }
                        if (this.OnReceived != null)
                        {
                            Trx data = event2.Data as Trx;
                            this.OnReceived(event2.Sender, data);
                        }
                    }
                    catch (Exception exception)
                    {
                        base.logger.Error(exception);
                    }
                }
            }

            public override string Name
            {
                get
                {
                    return this.name;
                }
            }
        }
    }
}
