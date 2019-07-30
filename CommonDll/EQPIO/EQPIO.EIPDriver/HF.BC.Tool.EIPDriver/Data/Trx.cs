
namespace HF.BC.Tool.EIPDriver.Data
{
    using HF.BC.Tool.EIPDriver.Enums;
    using System;

    [Serializable]
    public sealed class Trx
    {
        private bool bitOffEvent = false;
        private bool bitOffEventReport = false;
        private bool bitOffReadAction = false;
        private bool eventBit = false;
        private string key;
        private string name;
        private DictionaryList<string, Tag> tagCollection = new DictionaryList<string, Tag>();

        public void AddTag(Tag tag)
        {
            tag.ParentName = this.name;
            this.tagCollection.Add(tag.Name, tag);
        }

        public void ClearMultiTag()
        {
            this.tagCollection.Clear();
        }

        public void InitializeItemValue()
        {
            foreach (Tag tag in this.tagCollection.Values)
            {
                tag.InitializeItemValue();
            }
        }

        public Tag RemoveTag(string TagName)
        {
            Tag tag = this.tagCollection[TagName];
            this.tagCollection.Remove(TagName);
            return tag;
        }

        public bool BitOffEvent
        {
            get
            {
                return this.bitOffEvent;
            }
            set
            {
                this.bitOffEvent = value;
            }
        }

        public bool BitOffEventReport
        {
            get
            {
                return this.bitOffEventReport;
            }
            set
            {
                this.bitOffEventReport = value;
            }
        }

        public bool BitOffReadAction
        {
            get
            {
                return this.bitOffReadAction;
            }
            set
            {
                this.bitOffReadAction = value;
            }
        }

        public string BitSyncValue
        {
            set
            {
                foreach (Tag tag in this.tagCollection.Values)
                {
                    if (tag.Action == ActionEnum.W)
                    {
                        tag.BitSyncValue = value;
                    }
                }
            }
        }

        public bool EventBit
        {
            get
            {
                return this.eventBit;
            }
            set
            {
                this.eventBit = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
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
