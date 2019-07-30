
namespace HF.BC.Tool.EIPDriver.Driver.Data
{
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TimeOutChecker
    {
        private Dictionary<string, Item> _itemDictionary = new Dictionary<string, Item>();
        private ILog _logger = LogManager.GetLogger(typeof(TimeOutChecker));
        private BlockingQueue<Item> _queue = new BlockingQueue<Item>();
        private volatile bool _running = true;
        private readonly object _syncRoot = new object();
        private Thread _thread;
        private volatile int _timeOut = 0x7530;
        private readonly string Key = "{0}.{1}";

        public event TimeOutHandler OnTimeOut;

        public TimeOutChecker()
        {
            this._thread = new Thread(new ThreadStart(this.DequeueRun));
            this._thread.IsBackground = true;
            this._thread.Start();
            this._logger.Info(string.Format("Time Out Checker Start [{0}]", (int)this._timeOut));
        }

        public void Add(Block block)
        {
            if (block.Name.IndexOf("_RV_") <= -1)
            {
                foreach (Item item in block.ItemCollection.Values)
                {
                    if (item.Representation == Representation.BIT)
                    {
                        if (item.Value.Equals("1"))
                        {
                            if (this.checkNoTimeOutItem(block.ParentName, block.Name, item))
                            {
                                this._queue.Enqueue(item);
                            }
                        }
                        else
                        {
                            lock (this._syncRoot)
                            {
                                string key = string.Format(this.Key, block.Name, item.Name);
                                if (this._itemDictionary.ContainsKey(key))
                                {
                                    this._itemDictionary[key].PulseSyncComplete();
                                    this._itemDictionary.Remove(key);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool checkNoTimeOutItem(string tagName, string blockName, Item item)
        {
            if (tagName.Equals("SD_CIMToMa_RelayData_01_AL_00"))
            {
                string str = blockName;
                if ((str != null) && (((str == "L1_SD_ProductStopSignalRelayData") || (str == "L1_SD_BufferStopSignalRelayData")) || (str == "L1_SD_SpecialStopSignalRelayData")))
                {
                    return false;
                }
            }
            return true;
        }

        private void DequeueRun()
        {
            while (this._running)
            {
                try
                {
                    Item item = this._queue.Dequeue();
                    if (item == null)
                    {
                        continue;
                    }
                    lock (this._syncRoot)
                    {
                        string key = string.Format(this.Key, item.ParentName, item.Name);
                        if (!this._itemDictionary.ContainsKey(key))
                        {
                            this._itemDictionary.Add(key, item);
                        }
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.Run), item);
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception);
                }
            }
        }

        private void Run(object obj)
        {
            Item item = obj as Item;
            if (item != null)
            {
                string str;
                if (!item.WaitSyncComplete(this._timeOut))
                {
                    str = string.Format(this.Key, item.ParentName, item.Name);
                    this._logger.Info(string.Format("Time Out Item Name={0}", str));
                    if (this.OnTimeOut != null)
                    {
                        Item item2 = (Item)item.Clone();
                        item2.Value = "0";
                        this.OnTimeOut(item2);
                    }
                }
                lock (this._syncRoot)
                {
                    str = string.Format(this.Key, item.ParentName, item.Name);
                    if (this._itemDictionary.ContainsKey(str))
                    {
                        this._itemDictionary.Remove(str);
                    }
                }
            }
        }

        public void Stop()
        {
            this._running = false;
            this._queue.Clear();
            this._thread.Join();
            this._thread = null;
        }

        public delegate void TimeOutHandler(Item item);
    }
}
