
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class ConnectionInfo
    {
        [XmlAttribute]
        public int Channel { get; set; }

        [XmlAttribute]
        public string CodeType { get; set; }

        [XmlAttribute]
        public string CPUType { get; set; }

        [XmlAttribute]
        public string DirectAccess { get; set; }

        [XmlAttribute]
        public string DriverName { get; set; }

        [XmlAttribute]
        public string FixedBufferPort { get; set; }

        [XmlAttribute]
        public string IpAddress { get; set; }

        [XmlAttribute]
        public string IsFixedBufferEnabled { get; set; }

        [XmlAttribute]
        public string IsMelsecEnabled { get; set; }

        [XmlAttribute]
        public string IsVirtualEQPUsed { get; set; }

        [XmlAttribute]
        public string LocalName { get; set; }

        [XmlAttribute]
        public string MelsecPort { get; set; }

        [XmlAttribute]
        public string NetworkNo { get; set; }

        [XmlAttribute]
        public string PCNo { get; set; }

        [XmlAttribute]
        public string PLCMapFile { get; set; }

        [XmlAttribute]
        public string StationNo { get; set; }

        [XmlAttribute]
        public string TimeoutCheckLimit { get; set; }

        [XmlAttribute]
        public string VirtualEQPPLCMapFile { get; set; }

        [XmlAttribute]
        public string VirtualEQPPort { get; set; }
    }
}
