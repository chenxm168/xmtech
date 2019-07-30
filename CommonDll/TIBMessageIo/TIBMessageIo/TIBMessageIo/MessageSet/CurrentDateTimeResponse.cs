using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class CurrentDateTimeResponse:AbstractMessage
    {
       [XmlElement]
       public CurrentDateTimeResponseBody Body;

       public CurrentDateTimeResponse GetInstance(string xmlString)
       {
           return Util.XmlToObj<CurrentDateTimeResponse>(xmlString);
       }
    }
}
