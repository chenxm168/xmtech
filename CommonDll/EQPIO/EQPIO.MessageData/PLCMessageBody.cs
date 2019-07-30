
namespace EQPIO.MessageData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PLCMessageBody
    {
        private Dictionary<string, Dictionary<string, string>> readDataList = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, string>> writeDataList = new Dictionary<string, Dictionary<string, string>>();

        public string EventName { get; set; }

        public string EventValue { get; set; }

        public Dictionary<string, Dictionary<string, string>> ReadDataList
        {
            get
            {
                return this.readDataList;
            }
            set
            {
                this.readDataList = value;
            }
        }

        public Dictionary<string, Dictionary<string, string>> WriteDataList
        {
            get
            {
                return this.writeDataList;
            }
            set
            {
                this.writeDataList = value;
            }
        }
    }
}
