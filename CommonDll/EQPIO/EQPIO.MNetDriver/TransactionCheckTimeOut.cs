
namespace EQPIO.MNetDriver
{
    using EQPIO.Common;
    using System;

    public class TransactionCheckTimeOut
    {
        private MNetDev address;
        private int interval;
        private DateTime lastOnTime;
        private string name;
        private string value;

        public MNetDev Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
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

        public DateTime LastOnTime
        {
            get
            {
                return this.lastOnTime;
            }
            set
            {
                this.lastOnTime = value;
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

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
