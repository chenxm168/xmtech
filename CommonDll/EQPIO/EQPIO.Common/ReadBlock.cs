
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class ReadBlock
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Reprot { get; set; }
    }
}
