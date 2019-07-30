
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class EQPIOOption
    {
        [XmlElement]
        public EQPIO.Common.LoggingFilter LoggingFilter { get; set; }

        [XmlElement]
        public EQPIO.Common.LoggingFilterText LoggingFilterText { get; set; }

        [XmlElement]
        public EQPIO.Common.TimeoutCheck TimeoutCheck { get; set; }
    }
}
