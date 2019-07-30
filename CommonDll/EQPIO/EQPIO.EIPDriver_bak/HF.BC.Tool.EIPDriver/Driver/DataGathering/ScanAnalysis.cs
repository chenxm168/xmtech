
namespace HF.BC.Tool.EIPDriver.Driver.DataGathering
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Enums;
    using HF.BC.Tool.EIPDriver.Parser;
    using HF.BC.Tool.EIPDriver.Utils;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class ScanAnalysis
    {
        private HF.BC.Tool.EIPDriver.EIPDriver _driver;
        private Queue<Tag> _queue = new Queue<Tag>();
        private bool _running = true;
        private Dictionary<string, Tag> _scanTagHash = new Dictionary<string, Tag>();
        private TagFactory _tagFactory;
        private Thread _thread;
        private ILog logger = LogManager.GetLogger(typeof(ScanAnalysis));

        public event AnalysisEventHandler OnAnalysisReceived;

        public ScanAnalysis(TagFactory tagFactory, HF.BC.Tool.EIPDriver.EIPDriver eipDriver, string multiTagName)
        {
            this._tagFactory = tagFactory;
            this._driver = eipDriver;
            this.Init(multiTagName);
        }

        public void AddTag(Tag tag)
        {
            lock (((ICollection)this._queue).SyncRoot)
            {
                this._queue.Enqueue(tag);
            }
        }

        private void CompareItemTrigger(Tag currentTag, Block currentBlock, Block prevBlock)
        {
            if (ArrayUtils<int>.ArrayEqual(currentBlock.RawData, prevBlock.RawData))
            {
                this.CopyItemValue(prevBlock, currentBlock);
                if (!currentBlock.ContainsItemTrigger(TriggerEnum.A))
                {
                    return;
                }
            }
            else
            {
                WordTypeParser.ConvertObjectToBlock(currentBlock, false);
            }
            foreach (Item item in currentBlock.ItemCollection.Values)
            {
                bool flag = false;
                if (item.Trigger == TriggerEnum.A)
                {
                    flag = true;
                }
                else if ((item.Trigger == TriggerEnum.C) || (item.Trigger == TriggerEnum.I))
                {
                    if (!prevBlock.ItemCollection.ContainsKey(item.Name))
                    {
                        throw new Exception("Prev Item is NULL.");
                    }
                    Item item2 = prevBlock.ItemCollection[item.Name];
                    if (item2.Value == null)
                    {
                        throw new Exception("Prev Item Value is NULL.");
                    }
                    if (item.Value == null)
                    {
                        throw new Exception("Curr Item Value is NULL.");
                    }
                    if (item.Value != item2.Value)
                    {
                        flag = true;
                        if (item.LogMode == LogModeEnum.NORMAL)
                        {
                            this.logger.Info(this.ToItemLog(currentTag, currentBlock, item));
                        }
                        else if (item.LogMode == LogModeEnum.NONE)
                        {
                            this.logger.Info(this.ToItemLog(currentTag, currentBlock, item));
                        }
                    }
                    else
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                }
                if (flag)
                {
                    Trx trx = this._tagFactory.CreateReceiveTrx(item);
                    if (trx != null)
                    {
                        this.ExecuteTrx(trx, currentTag);
                    }
                }
            }
        }

        private void CompareTrigger(Tag currTag, Tag prevTag)
        {
            if (!currTag.ParseCompleted)
            {
                WordTypeParser.ConvertObjectToTag(currTag, true);
            }
            foreach (Block block in currTag.BlockCollection.Values)
            {
                if (block.Trigger == TriggerEnum.N)
                {
                    DateTime now = DateTime.Now;
                    Thread.Sleep(0);
                    TimeSpan span = (TimeSpan)(DateTime.Now - now);
                    if (span.TotalSeconds > 10.0)
                    {
                        this.logger.Error(string.Format("CompareTrigger Delay(Sleep(0)) : {0}", span.TotalSeconds));
                    }
                }
                else
                {
                    Block prevBlock = prevTag.BlockCollection[block.Name];
                    this.CompareItemTrigger(currTag, block, prevBlock);
                }
            }
        }

        private void CopyItemValue(Block sourceBlock, Block targetBlock)
        {
            foreach (Item item in targetBlock.ItemCollection.Values)
            {
                Item item2 = sourceBlock.ItemCollection[item.Name];
                item.Value = item2.Value;
            }
        }

        private void ExecuteTrx(Trx trx, Tag currentTag)
        {
            if (trx.TagCount > 0)
            {
                foreach (Tag tag in trx.TagCollection.Values)
                {
                    if (tag.Action == ActionEnum.R)
                    {
                        if (tag.Name.Equals(currentTag.Name))
                        {
                            if (!currentTag.ParseCompleted)
                            {
                                WordTypeParser.ConvertObjectToTag(currentTag, false);
                            }
                            StringBuilder builder = new StringBuilder();
                            builder.Append(string.Empty);
                            builder.Append(string.Format(EIPConst.TAG_LOG, tag.Name, tag.Points, tag.Duration));
                            builder.Append("\r\n");
                            foreach (Block block in tag.BlockCollection.Values)
                            {
                                Block block2 = currentTag.BlockCollection[block.Name];
                                block.CopyItemValue(block2);
                                builder.Append(block.ToExternalLogStringBuilder());
                                builder.Append("\r\n");
                            }
                            this.PrintLog(tag);
                        }
                        else
                        {
                            this._driver.Read(tag);
                            this.PrintLog(tag);
                        }
                    }
                    else
                    {
                        foreach (Block block in tag.BlockCollection.Values)
                        {
                            this._driver.Write(block);
                        }
                    }
                }
            }
            if (this.OnAnalysisReceived != null)
            {
                this.OnAnalysisReceived(this, trx);
            }
        }

        private void Init(string multiTagName)
        {
            this._thread = new Thread(new ThreadStart(this.Run));
            this._thread.IsBackground = true;
            this._thread.Name = string.Format("{0}-ScanAnalysis", multiTagName);
            this._thread.Start();
        }

        private void InitialCompareTrigger(Tag currentTag)
        {
            if (!currentTag.ParseCompleted)
            {
                WordTypeParser.ConvertObjectToTag(currentTag, false);
            }
            foreach (Block block in currentTag.BlockCollection.Values)
            {
                if (block.Trigger == TriggerEnum.N)
                {
                    Thread.Sleep(0);
                }
                else
                {
                    foreach (Item item in block.ItemCollection.Values)
                    {
                        if ((item.Trigger == TriggerEnum.A) || (item.Trigger == TriggerEnum.I))
                        {
                            Trx trx = this._tagFactory.CreateReceiveTrx(item);
                            if (trx == null)
                            {
                                this.logger.Error(string.Format("Trx is Null itemName={0}", item.Name));
                            }
                            else
                            {
                                this.ExecuteTrx(trx, currentTag);
                            }
                        }
                        Thread.Sleep(0);
                    }
                }
            }
        }

        private void PrintLog(Tag tag)
        {
            string message = string.Empty;
            if (tag.Name.IndexOf("_DVData_") > -1)
            {
                message = string.Format(EIPConst.TAG_LOG, tag.Name, tag.Points, tag.Duration);
            }
            else
            {
                message = tag.ToLogString();
            }
            if (tag.LogMode == LogModeEnum.NONE)
            {
                this.logger.Info(message);
            }
            else
            {
                this.logger.Info(message);
            }
        }

        private void Run()
        {
            while (this._running)
            {
                try
                {
                    Tag prevTag = null;
                    Tag currentTag = null;
                    lock (((ICollection)this._queue).SyncRoot)
                    {
                        if (this._queue.Count < 1)
                        {
                            continue;
                        }
                        currentTag = this._queue.Dequeue();
                        if (this._queue.Count > 0x3e8)
                        {
                            this.logger.Info(this._queue.Count);
                        }
                    }
                    if (currentTag == null)
                    {
                        continue;
                    }
                    if (this._scanTagHash.ContainsKey(currentTag.Name))
                    {
                        prevTag = this._scanTagHash[currentTag.Name];
                        if (ArrayUtils<int>.ArrayEqual(currentTag.RawData, prevTag.RawData) && !currentTag.ContainsTrigger(TriggerEnum.A))
                        {
                            continue;
                        }
                    }
                    if (prevTag == null)
                    {
                        this.logger.Info(string.Format("{0} is initial.", currentTag.Name));
                        this.InitialCompareTrigger(currentTag);
                        this._scanTagHash.Add(currentTag.Name, (Tag)currentTag.Clone());
                    }
                    else
                    {
                        this.CompareTrigger(currentTag, prevTag);
                        this._scanTagHash[currentTag.Name] = (Tag)currentTag.Clone();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error(exception);
                }
                finally
                {
                    Thread.Sleep(3);
                }
            }
        }

        public void Stop()
        {
            this._running = false;
            lock (((ICollection)this._queue).SyncRoot)
            {
                this._queue.Clear();
            }
            this._thread.Join();
            this._thread = null;
        }

        private string ToItemLog(Tag tag, Block block, Item item)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("");
            builder.Append(string.Format(EIPConst.TAG_LOG, tag.Name, tag.Points, item.Duration));
            builder.Append("\r\n");
            builder.Append("");
            builder.Append("<");
            builder.Append(block.Name);
            builder.Append(" (");
            builder.Append(block.Offset);
            builder.Append(", ");
            builder.Append(block.Points);
            builder.Append(")");
            builder.Append("\r\n");
            builder.Append("\t");
            builder.Append(item.ToExternalLogStringBuilder());
            return builder.ToString();
        }

        public delegate void AnalysisEventHandler(object sender, Trx trx);
    }
}
