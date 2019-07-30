
namespace HF.BC.Tool.EIPDriver.Driver.DataGathering
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ScanTimer : IScanTimer
    {
        private int _analysisCount = 0;
        private Dictionary<int, ScanAnalysis> _analysisMap;
        private readonly int _denominator = 20;
        private HF.BC.Tool.EIPDriver.EIPDriver _driver;
        private volatile int _index = 0;
        private int _interval = 200;
        private ILog _logger = LogManager.GetLogger(typeof(ScanTimer));
        private bool _running = true;
        private MultiTag _scanMultiTag;
        private Thread _scanThread;
        private readonly object _SyncRoot = new object();
        private TagFactory _tagFactory;

        public event ScanEventHandler OnScanReceived;

        public ScanTimer(TagFactory tagFactory, MultiTag scanMultiTag, HF.BC.Tool.EIPDriver.EIPDriver eipDriver)
        {
            this._interval = scanMultiTag.Interval;
            this._driver = eipDriver;
            this._tagFactory = tagFactory;
            this._scanMultiTag = (MultiTag)scanMultiTag.Clone();
            this.Init();
        }

        private void analysis_OnAnalysisReceived(object sender, Trx trx)
        {
            if (this.OnScanReceived != null)
            {
                this.OnScanReceived(this, trx);
            }
        }

        private void Init()
        {
            int num = this._scanMultiTag.TagCollection.Count / this._denominator;
            int num2 = this._scanMultiTag.TagCollection.Count % this._denominator;
            if (num2 != 0)
            {
                this._analysisCount = num + 1;
            }
            else
            {
                this._analysisCount = num;
            }
            this._analysisMap = new Dictionary<int, ScanAnalysis>();
            for (int i = 0; i < this._analysisCount; i++)
            {
                ScanAnalysis analysis = new ScanAnalysis(this._tagFactory, this._driver, this._scanMultiTag.Name);
                analysis.OnAnalysisReceived += new ScanAnalysis.AnalysisEventHandler(this.analysis_OnAnalysisReceived);
                this._analysisMap.Add(i, analysis);
            }
        }

        private void ScanRun()
        {
            while (this._running)
            {
                object obj2;
                try
                {
                    lock (this._scanMultiTag)
                    {
                        foreach (Tag tag in this._scanMultiTag.TagCollection.Values)
                        {
                            Tag tag2 = (Tag)tag.Clone();
                            tag2.StartTime = DateTime.Now.Ticks;
                            this._driver.Read(tag2, true);
                            tag2.EndTime = DateTime.Now.Ticks;
                            lock ((obj2 = this._SyncRoot))
                            {
                                this._analysisMap[this._index].AddTag(tag2);
                                this._index++;
                                if (this._index >= this._analysisMap.Count)
                                {
                                    this._index = 0;
                                }
                            }
                            Thread.Sleep(5);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this._logger.Error("Could not scan!!!", exception);
                }
                finally
                {
                    lock ((obj2 = this._SyncRoot))
                    {
                        this._index = 0;
                    }
                    Thread.Sleep(this._interval);
                }
            }
        }

        public void Start()
        {
            this._running = true;
            this._scanThread = null;
            this._scanThread = new Thread(new ThreadStart(this.ScanRun));
            this._scanThread.IsBackground = true;
            this._scanThread.Start();
        }

        public void Stop()
        {
            this._running = false;
            foreach (ScanAnalysis analysis in this._analysisMap.Values)
            {
                analysis.Stop();
            }
            lock (this._scanMultiTag)
            {
                this._scanMultiTag.TagCollection.Clear();
            }
            this._scanThread.Join();
            this._scanThread = null;
        }

        public bool Running
        {
            get
            {
                return this._running;
            }
            set
            {
                this._running = value;
            }
        }

        public string ScanName
        {
            get
            {
                return this._scanMultiTag.Name;
            }
        }

        public delegate void ScanEventHandler(object sender, object data);
    }
}
