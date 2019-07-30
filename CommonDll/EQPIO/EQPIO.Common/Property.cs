
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Property
    {
        [XmlAttribute]
        public string ConsumerExchange { get; set; }

        [XmlAttribute]
        public string ConsumerQueue { get; set; }

        [XmlAttribute]
        public string ConsumerRoutingKey { get; set; }

        [XmlAttribute]
        public string HostIP { get; set; }

        [XmlAttribute]
        public string HostName { get; set; }

        [XmlAttribute]
        public string MessageType { get; set; }

        [XmlAttribute]
        public string ProducerExchange { get; set; }

        [XmlAttribute]
        public string ProducerRoutingKey { get; set; }

        [XmlElement]
        public EQPIO.Common.ReadBlock[] ReadBlock { get; set; }

        [XmlAttribute]
        public string Timeout { get; set; }

        [XmlAttribute]
        public bool Value { get; set; }
    }
}
