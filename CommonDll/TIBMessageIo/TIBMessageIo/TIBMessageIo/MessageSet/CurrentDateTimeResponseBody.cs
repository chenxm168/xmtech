using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    public class CurrentDateTimeResponseBody
    {
       [XmlElement(ElementName="MACHINENAME")]
        public string MachineName;

       [XmlElement(ElementName = "ACKNOWLEDGE")]
       public string Acknowledge;
    }
}
