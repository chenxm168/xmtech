
namespace EQPIO.MessageData
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class MessageData<T>
    {
        public string EventTime { get; set; }

        public string MachineName { get; set; }

        public T MessageBody { get; set; }

        public string MessageName { get; set; }

        public string MessageType { get; set; }

        public int ReturnCode { get; set; }

        public string ReturnMessage { get; set; }

        public string Transaction { get; set; }
    }
}
