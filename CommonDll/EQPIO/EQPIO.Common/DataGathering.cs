using System.Xml.Serialization;

namespace EQPIO.Common
{
    public class DataGathering
    {
        [XmlElement]
        public Scan Scan
        {
            get;
            set;
        }

        [XmlElement]
        public OnDemand OnDemand
        {
            get;
            set;
        }

        [XmlElement]
        public LinkTest LinkTest
        {
            get;
            set;
        }
    }
}
