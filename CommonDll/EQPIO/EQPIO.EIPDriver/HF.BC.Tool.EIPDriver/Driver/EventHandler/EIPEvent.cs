
namespace HF.BC.Tool.EIPDriver.Driver.EventHandler
{
    using System;

    public class EIPEvent
    {
        private object data;
        private object sender;

        public object Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        public object Sender
        {
            get
            {
                return this.sender;
            }
            set
            {
                this.sender = value;
            }
        }
    }
}
