
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class LinkTest
    {
        [XmlElement]
        public EQPIO.Common.MultiBlock MultiBlock { get; set; }
    }
}
