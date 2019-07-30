
namespace HF.BC.Tool.EIPDriver.Driver.DataGathering
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using log4net;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SVDataTimer : IScanTimer
    {
        private HF.BC.Tool.EIPDriver.EIPDriver driver;
        private int interval = 200;
        private ILog logger = LogManager.GetLogger(typeof(ScanTimer));
        private bool running = true;
        private MultiTag scanMultiTag;
        private Thread scanThread;
        private TagFactory tagFactory;

        public event ScanEventHandler OnScanReceived;

        public SVDataTimer(TagFactory tagFactory, MultiTag scanMultiTag, HF.BC.Tool.EIPDriver.EIPDriver eipDriver)
        {
            this.interval = scanMultiTag.Interval;
            this.driver = eipDriver;
            this.tagFactory = tagFactory;
            this.scanMultiTag = (MultiTag)scanMultiTag.Clone();
        }

        private void ExecuteTrx(Tag currentTag)
        {
            Item item = currentTag.BlockCollection[0].ItemCollection[0];
            Trx data = this.tagFactory.CreateReceiveTrx(item);
            if (data == null)
            {
                this.logger.Info(string.Format("Cannot found Trx TagName={0}", currentTag.Name));
            }
            else
            {
                foreach (Tag tag in data.TagCollection.Values)
                {
                    if (tag.Name.Equals(currentTag.Name))
                    {
                        foreach (Block block in tag.BlockCollection.Values)
                        {
                            Block block2 = currentTag.BlockCollection[block.Name];
                            block.CopyItemValue(block2);
                        }
                        if (currentTag.RawData != null)
                        {
                            tag.RawData = new int[currentTag.RawData.Length];
                            Array.Copy(currentTag.RawData, 0, tag.RawData, 0, currentTag.RawData.Length);
                        }
                    }
                }
                if (this.OnScanReceived != null)
                {
                    this.OnScanReceived(this, data);
                }
            }
        }

        private void ScanRun()
        {
            while (this.running)
            {
                try
                {
                    lock (this.scanMultiTag)
                    {
                        foreach (Tag tag in this.scanMultiTag.TagCollection.Values)
                        {
                            Tag tag2 = (Tag)tag.Clone();
                            tag2.StartTime = DateTime.Now.Ticks;
                            this.driver.Read(tag2, false);
                            tag2.EndTime = DateTime.Now.Ticks;
                            this.ExecuteTrx(tag2);
                            Thread.Sleep(10);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Could not scan!!!", exception);
                }
                finally
                {
                    Thread.Sleep(this.interval);
                }
            }
        }

        public void Start()
        {
            this.running = true;
            this.scanThread = null;
            this.scanThread = new Thread(new ThreadStart(this.ScanRun));
            this.scanThread.IsBackground = true;
            this.scanThread.Name = "SVData";
            this.scanThread.Start();
        }

        public void Stop()
        {
            this.running = false;
            lock (this.scanMultiTag)
            {
                this.scanMultiTag.TagCollection.Clear();
            }
            this.scanThread.Join();
            this.scanThread = null;
        }

        public bool Running
        {
            get
            {
                return this.running;
            }
            set
            {
                this.running = value;
            }
        }

        public string ScanName
        {
            get
            {
                return this.scanMultiTag.Name;
            }
        }

        public delegate void ScanEventHandler(object sender, object data);
    }
}
