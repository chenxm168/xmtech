using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
   public class MachineOnlineParameterDataMessage:AbstractMessage
    {
        [XmlElement]
        public MachineOnlineParameterDataBody Body;


    }
}
