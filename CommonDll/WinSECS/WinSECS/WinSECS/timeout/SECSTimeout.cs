using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.structure;

namespace WinSECS.timeout
{
    public class SECSTimeout : ISECSTimout
    {
        private int id;
        public const int LINK = -100;
        private SECSTransaction message;
        public const int T3 = -3;
        public const int T4 = -4;
        public const int T5 = -5;
        public const int T6 = -6;
        public const int T7 = -7;
        public const int T8 = -8;
        private long timeoutTime = 0L;

        public SECSTimeout(int id)
        {
            this.id = id;
        }

        public object Clone()
        {
            return new SECSTimeout(this.Id) { Message = this.Message };
        }

        public override string ToString()
        {
            return ((-3 == this.Id) ? (this.Type + "SystemByte=" + this.message.Systembyte) : this.Type);
        }

        public virtual int Id
        {
            get
            {
                return this.id;
            }
        }

        public virtual ISECSTransaction Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value as SECSTransaction;
            }
        }

        public virtual long TimeoutTime
        {
            get
            {
                return this.timeoutTime;
            }
            set
            {
                this.timeoutTime = value;
            }
        }

        public virtual string Type
        {
            get
            {
                switch (this.id)
                {
                    case -8:
                        return "T8 Timeout";

                    case -7:
                        return "T7 Timeout";

                    case -6:
                        return "T6 Timeout";

                    case -5:
                        return "T5 Timeout";

                    case -3:
                        return "T3 Timeout";
                }
                return "";
            }
        }
    }
}
