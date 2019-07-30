
namespace HF.BC.Tool.EIPDriver.Data
{
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using HF.BC.Tool.EIPDriver.Enums;
    using System;
    using System.Text;
    using System.Threading;

    [Serializable]
    public class Item : ICloneable
    {
        private int b_offset = -1;
        private int b_points = -1;
        private long endTime;
        private bool isUpdate = false;
        private LogModeEnum logMode = LogModeEnum.NORMAL;
        private string name;
        private string offset;
        private string parentName;
        private string points;
        private HF.BC.Tool.EIPDriver.Data.Represent.Representation representation = null;
        private long startTime;
        private string strValue;
        private readonly object syncRoot = new object();
        private bool syncValue = false;
        private TriggerEnum trigger = TriggerEnum.C;
        private int w_offset;
        private int w_points;

        public object Clone()
        {
            return (Item)base.MemberwiseClone();
        }

        public int[] GetData()
        {
            return this.representation.GetData(this.name, this.strValue, this.w_points);
        }

        public void InitializeValue()
        {
            if (this.representation != null)
            {
                if (this.representation == HF.BC.Tool.EIPDriver.Data.Represent.Representation.A)
                {
                    if (this.Value == null)
                    {
                        this.Value = string.Empty;
                    }
                }
                else if (string.IsNullOrEmpty(this.Value))
                {
                    this.Value = "0";
                }
            }
        }

        public void Parse(int[] ints)
        {
            if (this.IsLikeBitMode)
            {
                if (this.representation == HF.BC.Tool.EIPDriver.Data.Represent.Representation.I)
                {
                    int num = ints[0];
                    string str2 = Convert.ToString(num, 2).PadLeft(0x10, '0').Substring((0x10 - this.b_offset) - this.b_points, this.b_points);
                    this.Value = Convert.ToUInt16(str2, 2).ToString();
                }
                else
                {
                    this.Value = this.representation.Parse(ints, this.w_points).Substring(((this.w_points * 0x10) - this.b_offset) - this.b_points, this.b_points);
                }
            }
            else
            {
                this.Value = this.representation.Parse(ints, this.w_points);
            }
        }

        public void PulseSyncComplete()
        {
            lock (this.syncRoot)
            {
                Monitor.PulseAll(this.syncRoot);
            }
        }

        public StringBuilder ToExternalLogStringBuilder()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            builder.Append(this.Representation.Expression);
            builder.Append(" ");
            builder.Append(this.Offset);
            builder.Append(" ");
            builder.Append(this.Points);
            builder.Append(" ");
            builder.Append(this.Name);
            builder.Append(" '");
            builder.Append(this.Value);
            builder.Append("'>");
            return builder;
        }

        public bool WaitSyncComplete(int timeout)
        {
            lock (this.syncRoot)
            {
                return Monitor.Wait(this.syncRoot, timeout);
            }
        }

        public int BitOffset
        {
            get
            {
                return this.b_offset;
            }
        }

        public int BitPoints
        {
            get
            {
                return this.b_points;
            }
        }

        public string BitSyncValue
        {
            set
            {
                if (this.syncValue)
                {
                    this.Value = value;
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

        public bool IsLikeBitMode
        {
            get
            {
                return (this.b_offset > -1);
            }
        }

        public bool IsUpdate
        {
            get
            {
                return this.isUpdate;
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

        public string Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
                int index = this.offset.IndexOf(":");
                if (index >= 0)
                {
                    this.w_offset = short.Parse(this.offset.Substring(0, index));
                    this.b_offset = short.Parse(this.offset.Substring(index + 1));
                }
                else
                {
                    this.w_offset = short.Parse(this.offset);
                }
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

        public string Points
        {
            get
            {
                return this.points;
            }
            set
            {
                this.points = value;
                int index = this.points.IndexOf(":");
                if (index >= 0)
                {
                    this.w_points = short.Parse(this.points.Substring(0, index));
                    this.b_points = short.Parse(this.points.Substring(index + 1));
                }
                else
                {
                    this.w_points = short.Parse(this.points);
                }
            }
        }

        public HF.BC.Tool.EIPDriver.Data.Represent.Representation Representation
        {
            get
            {
                return this.representation;
            }
            set
            {
                this.representation = value;
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

        public bool SyncValue
        {
            get
            {
                return this.syncValue;
            }
            set
            {
                this.syncValue = value;
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

        public string Value
        {
            get
            {
                return this.strValue;
            }
            set
            {
                this.strValue = value;
                if (this.strValue != value)
                {
                    this.isUpdate = true;
                }
            }
        }

        public int WordOffset
        {
            get
            {
                return this.w_offset;
            }
        }

        public int WordPoints
        {
            get
            {
                return this.w_points;
            }
        }
    }
}
