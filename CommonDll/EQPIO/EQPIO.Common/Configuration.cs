
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Configuration
    {
        [XmlElement]
        public EQPIO.Common.ConnectionInfo[] ConnectionInfo { get; set; }
    }
}
