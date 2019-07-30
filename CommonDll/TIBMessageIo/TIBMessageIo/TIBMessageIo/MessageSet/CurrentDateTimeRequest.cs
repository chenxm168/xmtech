using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
    public  class CurrentDateTimeRequest:AbstractMessage
    {
        [XmlElement(ElementName="Body")]
        public CurrentDateTimeRequestBody Body;

        public CurrentDateTimeRequest GetInstance(string xmlString)
        {
            return Util.XmlToObj<CurrentDateTimeRequest>(xmlString);
        }
    }
}
