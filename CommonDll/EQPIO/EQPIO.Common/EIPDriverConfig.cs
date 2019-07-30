
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class EIPDriverConfig
    {
        [XmlElement]
        public string DriverName { get; set; }

        [XmlElement]
        public string EIPMapFile { get; set; }

        [XmlElement]
        public string Log4NetPath { get; set; }

        [XmlElement]
        public string LogRootDir { get; set; }

        [XmlElement]
        public string TimeOutCheckList { get; set; }
    }
}
