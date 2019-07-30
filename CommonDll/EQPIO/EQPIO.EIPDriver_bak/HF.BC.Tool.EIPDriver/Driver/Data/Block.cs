
namespace HF.BC.Tool.EIPDriver.Driver.Data
{
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using HF.BC.Tool.EIPDriver.Enums;
    using System;
    using System.Text;

    [Serializable]
    public class Block : ICloneable
    {
        private bool actionStep = false;
        private long endTime;
        private DictionaryList<string, Item> itemCollection = new DictionaryList<string, Item>();
        private LogModeEnum logMode = LogModeEnum.NORMAL;
        private string name;
        private int offset;
        private string parentName;
        private bool parseCompleted;
        private int points;
        private int[] rawData = null;
        private long startTime;
        private TriggerEnum trigger = TriggerEnum.N;

        public void AddItem(Item item)
        {
            item.ParentName = this.name;
            this.itemCollection.Add(item.Name, item);
        }

        public void AddItem(string itemName, string itemValue)
        {
            Item item = new Item
            {
                Name = itemName,
                Value = itemValue
            };
            this.AddItem(item);
        }

        public void ClearItem()
        {
            this.itemCollection.Clear();
        }

        public object Clone()
        {
            Block block = (Block)base.MemberwiseClone();
            if (this.RawData != null)
            {
                block.RawData = new int[this.RawData.Length];
                Array.Copy(this.RawData, 0, block.RawData, 0, this.RawData.Length);
            }
            block.itemCollection = new DictionaryList<string, Item>();
            foreach (Item item in this.ItemCollection.Values)
            {
                block.AddItem((Item)item.Clone());
            }
            return block;
        }

        public Block CloneWithOutItem()
        {
            Block block = (Block)base.MemberwiseClone();
            if (this.RawData != null)
            {
                block.RawData = new int[this.RawData.Length];
                Array.Copy(this.RawData, 0, block.RawData, 0, this.RawData.Length);
            }
            block.itemCollection = new DictionaryList<string, Item>();
            return block;
        }

        public bool ContainsItemTrigger(TriggerEnum trigger)
        {
            foreach (Item item in this.itemCollection.Values)
            {
                if (item.Trigger == trigger)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyItemValue(Block block)
        {
            foreach (Item item in this.itemCollection.Values)
            {
                item.Value = block.itemCollection[item.Name].Value;
            }
            if (block.RawData != null)
            {
                this.RawData = new int[block.RawData.Length];
                Array.Copy(block.RawData, 0, this.rawData, 0, block.rawData.Length);
            }
        }

        public void InitializeValue()
        {
            foreach (Item item in this.itemCollection.Values)
            {
                item.InitializeValue();
            }
        }

        public Item RemoveItem(string itemName)
        {
            if (this.itemCollection.ContainsKey(itemName))
            {
                Item item = this.itemCollection[itemName];
                this.itemCollection.Remove(itemName);
                return item;
            }
            return null;
        }

        public StringBuilder ToExternalLogStringBuilder()
        {
            StringBuilder builder = new StringBuilder(0x7d0);
            builder.Append("");
            builder.Append("<");
            builder.Append(this.name);
            builder.Append(" (");
            builder.Append(this.offset);
            builder.Append(", ");
            builder.Append(this.points);
            builder.Append(")");
            builder.Append("\r\n");
            foreach (Item item in this.itemCollection.Values)
            {
                builder.Append("\t");
                builder.Append(item.ToExternalLogStringBuilder());
                builder.Append("\r\n");
            }
            builder.Append(">");
            return builder;
        }

        public bool ActionStep
        {
            get
            {
                return this.actionStep;
            }
            set
            {
                this.actionStep = value;
            }
        }

        public string BitSyncValue
        {
            set
            {
                foreach (Item item in this.itemCollection.Values)
                {
                    if (item.Representation == Representation.BIT)
                    {
                        item.BitSyncValue = value;
                    }
                }
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

        public bool IsBitBlock
        {
            get
            {
                foreach (Item item in this.itemCollection.Values)
                {
                    if (item.Representation != Representation.BIT)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsUpdate
        {
            get
            {
                foreach (Item item in this.itemCollection.Values)
                {
                    if (item.IsUpdate)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public DictionaryList<string, Item> ItemCollection
        {
            get
            {
                return this.itemCollection;
            }
        }

        public int ItemCount
        {
            get
            {
                return this.itemCollection.Count;
            }
        }

        public LogModeEnum LogMode
        {
            get
            {
                return this.logMode;
            }
            set
            {
                this.logMode = value;
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

        public int Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
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
                return this.parseCompleted;
            }
            set
            {
                this.parseCompleted = value;
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

        public TriggerEnum Trigger
        {
            get
            {
                return this.trigger;
            }
            set
            {
                this.trigger = value;
            }
        }
    }
}
