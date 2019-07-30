
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class MultiBlock
    {
        [XmlAttribute]
        public string Action { get; set; }

        [XmlAttribute]
        public string AutoOffInterval { get; set; }

        [XmlElement]
        public EQPIO.Common.Block[] Block { get; set; }

        [XmlAttribute]
        public bool DirectAccess { get; set; }

        [XmlAttribute]
        public int Interval { get; set; }

        [XmlAttribute]
        public bool IsFDC { get; set; }

        [XmlAttribute]
        public bool IsRGA { get; set; }

        [XmlAttribute]
        public bool IsTRACE { get; set; }

        [XmlAttribute]
        public string LogMode { get; set; }

        [XmlAttribute]
        public bool MultiBlockScan { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string NetworkNo { get; set; }

        [XmlAttribute]
        public string PCNo { get; set; }

        [XmlAttribute]
        public string Priority { get; set; }

        [XmlAttribute]
        public string StartUp { get; set; }
    }
}
