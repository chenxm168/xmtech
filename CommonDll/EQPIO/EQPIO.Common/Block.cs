
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Block
    {
        public MNetDev address { get; set; }

        [XmlAttribute]
        public string DeviceCode { get; set; }

        [XmlAttribute]
        public string HeadDevice { get; set; }

        [XmlElement]
        public EQPIO.Common.Item[] Item { get; set; }

        [XmlElement]
        public EQPIO.Common.ItemGroup ItemGroup { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Points { get; set; }

        [XmlAttribute]
        public string Trigger { get; set; }
    }
}
