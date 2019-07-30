
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class PLCDriver
    {
        [XmlElement]
        public EQPIO.Common.BlockMap BlockMap { get; set; }

        [XmlElement]
        public EQPIO.Common.DataGathering DataGathering { get; set; }

        [XmlElement]
        public EQPIO.Common.ItemGroupCollection ItemGroupCollection { get; set; }

        [XmlElement]
        public EQPIO.Common.Transaction Transaction { get; set; }

        [XmlAttribute]
        public string version { get; set; }
    }
}
