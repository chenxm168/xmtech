
namespace HF.BC.Tool.EIPDriver.Data
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Enums;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public class Tag : ICloneable
    {
        private ActionEnum action;
        private DictionaryList<string, Block> blockCollection;
        private long endTime;
        private string name;
        private string parentName;
        private int points;
        private int[] rawData;
        private long startTime;
        private string[] triggerCollection;

        public Tag()
        {
            this.rawData = null;
            this.blockCollection = new DictionaryList<string, Block>();
        }

        public Tag(string name)
        {
            this.rawData = null;
            this.blockCollection = new DictionaryList<string, Block>();
            this.name = name;
        }

        public void AddBlock(Block block)
        {
            block.ParentName = this.name;
            this.blockCollection.Add(block.Name, block);
        }

        public object Clone()
        {
            Tag tag = (Tag)base.MemberwiseClone();
            if (this.RawData != null)
            {
                tag.RawData = new int[this.RawData.Length];
                Array.Copy(this.RawData, 0, tag.RawData, 0, this.RawData.Length);
            }
            if (this.triggerCollection != null)
            {
                tag.triggerCollection = new string[this.triggerCollection.Length];
                Array.Copy(this.triggerCollection, 0, tag.triggerCollection, 0, this.triggerCollection.Length);
            }
            tag.blockCollection = new DictionaryList<string, Block>();
            foreach (Block block in this.blockCollection.Values)
            {
                tag.AddBlock((Block)block.Clone());
            }
            return tag;
        }

        public Tag CloneWithOutBlock()
        {
            Tag tag = (Tag)base.MemberwiseClone();
            if (this.RawData != null)
            {
                tag.RawData = new int[this.RawData.Length];
                Array.Copy(this.RawData, 0, tag.RawData, 0, this.RawData.Length);
            }
            if (this.triggerCollection != null)
            {
                tag.triggerCollection = new string[this.triggerCollection.Length];
                Array.Copy(this.triggerCollection, 0, tag.triggerCollection, 0, this.triggerCollection.Length);
            }
            tag.blockCollection = new DictionaryList<string, Block>();
            return tag;
        }

        public bool ContainsTrigger(TriggerEnum trigger)
        {
            if (this.triggerCollection == null)
            {
                List<string> list = new List<string>();
                foreach (Block block in this.blockCollection.Values)
                {
                    if (list.IndexOf(block.Trigger.ToString()) < 0)
                    {
                        list.Add(block.Trigger.ToString());
                    }
                    foreach (Item item in block.ItemCollection.Values)
                    {
                        if (list.IndexOf(item.Trigger.ToString()) < 0)
                        {
                            list.Add(item.Trigger.ToString());
                        }
                    }
                }
                this.triggerCollection = list.ToArray();
                Array.Sort<string>(this.triggerCollection);
            }
            return (Array.BinarySearch<string>(this.triggerCollection, trigger.ToString()) > -1);
        }

        public void CopyBlock(Tag currTag)
        {
            foreach (Block block in this.blockCollection.Values)
            {
                block.CopyItemValue(currTag.blockCollection[block.Name]);
            }
            this.RawData = currTag.RawData;
        }

        private List<Block> GetBitBlock()
        {
            List<Block> list = new List<Block>();
            foreach (Block block in this.blockCollection.Values)
            {
                if (block.IsBitBlock)
                {
                    list.Add(block);
                }
            }
            return list;
        }

        private List<Block> GetBlockList()
        {
            List<Block> list = new List<Block>();
            list.AddRange(this.GetWordBlock());
            list.AddRange(this.GetBitBlock());
            return list;
        }

        private List<Block> GetWordBlock()
        {
            List<Block> list = new List<Block>();
            foreach (Block block in this.blockCollection.Values)
            {
                if (!block.IsBitBlock)
                {
                    list.Add(block);
                }
            }
            return list;
        }

        public void InitializeItemValue()
        {
            foreach (Block block in this.blockCollection.Values)
            {
                block.InitializeValue();
            }
        }

        public string ToLogString()
        {
            StringBuilder builder = new StringBuilder(50);
            builder.Append(string.Format(EIPConst.TAG_LOG, this.name, this.points, this.Duration));
            builder.Append("\r\n");
            foreach (Block block in this.GetBlockList())
            {
                builder.Append(block.ToExternalLogStringBuilder());
                builder.Append("\r\n");
            }
            return builder.ToString();
        }

        public ActionEnum Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        public string BitSyncValue
        {
            set
            {
                foreach (Block block in this.blockCollection.Values)
                {
                    if (block.IsBitBlock)
                    {
                        block.BitSyncValue = value;
                    }
                }
            }
        }

        public DictionaryList<string, Block> BlockCollection
        {
            get
            {
                return this.blockCollection;
            }
        }

        public int BlockCount
        {
            get
            {
                return this.blockCollection.Count;
            }
        }

        public long Duration
        {
            get
            {
                return ((this.endTime - this.startTime) / 0x2710L);
            }
        }

        public long EndTime
        {
            get
            {
                return this.endTime;
            }
            set
            {
                this.endTime = value;
            }
        }

        public bool IsUpdate
        {
            get
            {
                foreach (Block block in this.blockCollection.Values)
                {
                    if (block.IsUpdate)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public LogModeEnum LogMode
        {
            get
            {
                LogModeEnum logMode = this.blockCollection[0].LogMode;
                foreach (Block block in this.blockCollection.Values)
                {
                    if (block.LogMode != logMode)
                    {
                        return block.LogMode;
                    }
                }
                return logMode;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string ParentName
        {
            get
            {
                return this.parentName;
            }
            set
            {
                this.parentName = value;
            }
        }

        public bool ParseCompleted
        {
            get
            {
                foreach (Block block in this.blockCollection.Values)
                {
                    if (!block.ParseCompleted)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public int Points
        {
            get
            {
                return this.points;
            }
            set
            {
                this.points = value;
            }
        }

        public int[] RawData
        {
            get
            {
                return this.rawData;
            }
            set
            {
                this.rawData = value;
            }
        }

        public long StartTime
        {
            get
            {
                return this.startTime;
            }
            set
            {
                this.startTime = value;
            }
        }
    }
}
