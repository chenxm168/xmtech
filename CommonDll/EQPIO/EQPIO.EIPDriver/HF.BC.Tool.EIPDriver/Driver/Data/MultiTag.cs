
namespace HF.BC.Tool.EIPDriver.Driver.Data
{
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Enums;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class MultiTag : ICloneable
    {
        private ActionEnum action;
        private int interval = 0;
        private LogModeEnum logMode = LogModeEnum.NORMAL;
        private string name;
        private bool startUp = true;
        private bool svData = false;
        private DictionaryList<string, Tag> tagCollection = new DictionaryList<string, Tag>();

        public void AddTag(Tag tag)
        {
            tag.ParentName = this.name;
            this.tagCollection.Add(tag.Name, tag);
        }

        public void ClearTag()
        {
            this.tagCollection.Clear();
        }

        public object Clone()
        {
            MultiTag tag = (MultiTag)base.MemberwiseClone();
            tag.tagCollection = new DictionaryList<string, Tag>();
            foreach (Tag tag2 in this.tagCollection.Values)
            {
                tag.AddTag((Tag)tag2.Clone());
            }
            return tag;
        }

        public MultiTag CloneWithOutTag()
        {
            MultiTag tag = (MultiTag)base.MemberwiseClone();
            tag.tagCollection = new DictionaryList<string, Tag>();
            return tag;
        }

        public bool ContainsTag(string tagName)
        {
            return this.tagCollection.ContainsKey(tagName);
        }

        public Tag[] GetExistTags(string[] tagNames)
        {
            List<Tag> list = new List<Tag>();
            foreach (string str in tagNames)
            {
                if (this.tagCollection.ContainsKey(str))
                {
                    list.Add(this.tagCollection[str]);
                }
            }
            return list.ToArray();
        }

        public void InitializeItemValue()
        {
            foreach (Tag tag in this.tagCollection.Values)
            {
                tag.InitializeItemValue();
            }
        }

        public Tag RemoveTag(string tagName)
        {
            if (this.tagCollection.ContainsKey(tagName))
            {
                Tag tag = this.tagCollection[tagName];
                this.tagCollection.Remove(tagName);
                return tag;
            }
            return null;
        }

        public void UpdateTag(Tag tag)
        {
            string name = tag.Name;
            if (this.tagCollection.ContainsKey(name))
            {
                Tag tag2 = this.tagCollection[name];
                tag2 = tag;
            }
            else
            {
                this.AddTag(tag);
            }
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
                foreach (Tag tag in this.tagCollection.Values)
                {
                    tag.BitSyncValue = value;
                }
            }
        }

        public int Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public int ItemGroupCount
        {
            get
            {
                int num = 0;
                foreach (Tag tag in this.tagCollection.Values)
                {
                    num += tag.BlockCount;
                }
                return num;
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

        public bool StartUp
        {
            get
            {
                return this.startUp;
            }
            set
            {
                this.startUp = value;
            }
        }

        public bool SVData
        {
            get
            {
                return this.svData;
            }
            set
            {
                this.svData = value;
            }
        }

        public DictionaryList<string, Tag> TagCollection
        {
            get
            {
                return this.tagCollection;
            }
        }

        public int TagCount
        {
            get
            {
                return this.tagCollection.Count;
            }
        }
    }
}
