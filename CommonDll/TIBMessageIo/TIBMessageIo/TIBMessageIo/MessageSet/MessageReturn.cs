using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class MessageReturn
    {

       [XmlElement]
       public string RETURNCODE;

       [XmlElement]
       public string RETURNMESSAGE;
    }
}
