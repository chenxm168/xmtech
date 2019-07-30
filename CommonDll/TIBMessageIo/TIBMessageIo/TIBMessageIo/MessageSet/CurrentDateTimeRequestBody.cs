using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class CurrentDateTimeRequestBody
    {
       [XmlElement(ElementName="DATETIME")]
       public string DateTime;

       [XmlElement(ElementName = "MACHINENAME")]
       public string MACHINENAME;


       
       [XmlElement(ElementName = "ACKNOWLEDGE")]
       public string ACKNOWLEDGE;

    }


}
