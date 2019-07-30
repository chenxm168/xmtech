
namespace HF.BC.Tool.EIPDriver
{
    using HF.BC.Tool.EIPDriver.Config;
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Parser;
    using HF.BC.Tool.EIPDriver.Utils;
    using log4net;
    using OMRON.Compolet.Variable;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class EIPDriver
    {
        private ILog _logger = LogManager.GetLogger(typeof(EIPClient));
        private TagFactory _tagFactory;
        private TimeOutChecker _timeOutChecker;
        private VariableCompolet _variableCompolet = new VariableCompolet();
        private static uint eventID = 1;

        public EIPDriver(TagFactory tagFactory, EIPConfig config)
        {
            _variableCompolet.PlcEncoding = Encoding.UTF8;
            _variableCompolet.Active = true;
            _tagFactory = tagFactory;
            _timeOutChecker = new TimeOutChecker();
            _timeOutChecker.OnTimeOut += _timeOutChecker_OnTimeOut;
        }

        private void _timeOutChecker_OnTimeOut(Item item)
        {
            Block block = this._tagFactory.CreateBlock(item.ParentName);
            block.ItemCollection.Clear();
            block.AddItem(item);
            this.Write(block);
        }

        public void AddTimeOutCheckBlock(Block block)
        {
            this._timeOutChecker.Add(block);
        }

        public Tag Read(Tag tag)
        {
            return this.Read(tag, false);
        }

        private object Read(string tagName)
        {
            if (!_variableCompolet.Active)
            {
                _variableCompolet.Active = true;
            }
            return _variableCompolet.ReadVariable(tagName);
        }
        public Tag Read(Tag tag, bool isLazyParser)
        {
            tag.RawData = (int[])this.Read(tag.Name);
            if (!isLazyParser)
            {
                WordTypeParser.ConvertObjectToTag(tag, false);
            }
            return tag;
        }

        public Block Read(Block block, bool writeLog)
        {
            block.StartTime = DateTime.Now.Ticks;
            Tag tag = this._tagFactory.CreateTag(block.ParentName);
            tag.RawData = (int[])this.Read(block.ParentName);
            block.RawData = ArrayUtils<int>.GetSubInt(tag.RawData, block.Offset, block.Points);
            WordTypeParser.ConvertObjectToBlock(block, false);
            block.EndTime = DateTime.Now.Ticks;
            if (writeLog)
            {
                StringBuilder builder = new StringBuilder(0x7d0);
                builder.Append(string.Empty);
                builder.Append(string.Format(EIPConst.TAG_LOG, tag.Name, tag.Points, block.Duration));
                builder.Append("\r\n");
                builder.Append(block.ToExternalLogStringBuilder());
                this._logger.Info(builder.ToString());
            }
            return block;
        }

        public void Stop()
        {
            _timeOutChecker.Stop();
            _variableCompolet.Changed -= variableCompolet_Changed;
            _variableCompolet.Active = false;
            _variableCompolet.Dispose();
        }

        private void variableCompolet_Changed(object sender, EventArgs e)
        {
            try
            {
                string str = string.Empty;
                int num = 0;
                this._variableCompolet.ReciveEvent(out str, out num, 0);
                this._logger.Info(string.Format("Change EventID={0}, TagName={1}", num, str));
                this._variableCompolet.ClearEvent(str);
            }
            catch (Exception exception)
            {
                this._logger.Error(exception);
            }
        }

        public void Write(Tag tag)
        {
            lock (this._tagFactory.getSyncTagObject(tag.Name))
            {
                if (tag.Name.Equals("SD_CIMToFeeder_CMD_01_AL_00"))
                {
                    this._logger.Info(string.Format("++++++++++ Start(EventID={0}, TagName={1}) ++++++++++", eventID, tag.Name));
                }
                else
                {
                    this._logger.Info(string.Format("++++++++++ Start(TagName={0}) ++++++++++", tag.Name));
                }
                tag.RawData = (int[])this.Read(tag.Name);
                foreach (Block block in tag.BlockCollection.Values)
                {
                    WordTypeParser.ConvertBlockToWriteData(tag.RawData, block);
                    this._timeOutChecker.Add(block);
                }
                this.Write(tag.Name, tag.RawData);
            }
        }

        public void Write(Trx trx)
        {
            foreach (Tag tag in trx.TagCollection.Values)
            {
                foreach (Block block in tag.BlockCollection.Values)
                {
                    this.Write(block);
                }
            }
        }

        public void Write(Block block)
        {
            lock (this._tagFactory.getSyncTagObject(block.ParentName))
            {
                if (block.ParentName.Equals("SD_CIMToFeeder_CMD_01_AL_00"))
                {
                    this._logger.Info(string.Format("++++++++++ Start(EventID={0}, TagName={1}, BlockName={2}) ++++++++++", eventID, block.ParentName, block.Name));
                }
                else
                {
                    this._logger.Info(string.Format("++++++++++ Start(TagName={0}, BlockName={1}) ++++++++++", block.ParentName, block.Name));
                }
                block.StartTime = DateTime.Now.Ticks;
                Tag tag = this._tagFactory.CreateTag(block.ParentName);
                tag.RawData = (int[])this.Read(block.ParentName);
                WordTypeParser.ConvertBlockToWriteData(tag.RawData, block);
                this.Write(tag.Name, tag.RawData);
                this._timeOutChecker.Add(block);
                block.EndTime = DateTime.Now.Ticks;
                StringBuilder builder = new StringBuilder(0x7d0);
                builder.Append(string.Empty);
                builder.Append(string.Format(EIPConst.TAG_LOG, tag.Name, tag.Points, block.Duration));
                builder.Append("\r\n");
                builder.Append(block.ToExternalLogStringBuilder());
                this._logger.Info(builder.ToString());
            }
        }

        private void Write(string tagName, int[] values)
        {
            if (!_variableCompolet.Active)
            {
                _variableCompolet.Active = true;
            }
            if (_variableCompolet.WindowHandle == IntPtr.Zero)
            {
                _variableCompolet.WindowHandle = Process.GetCurrentProcess().MainWindowHandle;
                _variableCompolet.Changed += variableCompolet_Changed;
            }
            Thread.Sleep(2);
            if (tagName.Equals("SD_CIMToFeeder_CMD_01_AL_00"))
            {
                this._variableCompolet.ClearEvent("SD_CIMToFeeder_CMD_01_AL_00");
                this._variableCompolet.SetEvent("SD_CIMToFeeder_CMD_01_AL_00", (int)eventID);
                eventID++;
            }
            this._variableCompolet.WriteVariable(tagName, values);
        }

        public bool IsOpen
        {
            get
            {
                return _variableCompolet.Active;
            }
            set
            {
                _variableCompolet.Active = value;
            }
        }

        public List<string> SysmacGatewayTagNames
        {
            get
            {
                if (!this.IsOpen)
                {
                    throw new Exception("is not Active.");
                }
                string[] source = _variableCompolet.VariableNames;
                if (source.Length < 1)
                {
                    throw new Exception("Tag is not set.");
                }
                return source.ToList<string>();
            }
        }
    }
}
