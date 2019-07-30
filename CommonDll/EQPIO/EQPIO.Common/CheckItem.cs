
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class CheckItem
    {
        [XmlAttribute]
        public int ExtensionInterval { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Skip { get; set; }
    }
}
