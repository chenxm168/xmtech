
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Objects
    {
        [XmlElement]
        public EQPIO.Common.Property[] Property { get; set; }
    }
}
