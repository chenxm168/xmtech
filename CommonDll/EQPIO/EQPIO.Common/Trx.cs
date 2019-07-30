
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Trx
    {
        [XmlAttribute]
        public bool BitOffEvent { get; set; }

        [XmlAttribute]
        public bool BitOffEventReport { get; set; }

        [XmlAttribute]
        public bool BitOffReadAction { get; set; }

        [XmlAttribute]
        public string Key { get; set; }

        [XmlElement]
        public EQPIO.Common.MultiBlock[] MultiBlock { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}
