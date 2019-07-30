using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class MessageHead
    {
       [XmlElement]
       public string MESSAGENAME;

       [XmlElement]
       public string TRANSACTIONID;

       [XmlElement]
       public string ORIGINALTRANSACTIONID;

       [XmlElement]
       public string ORIGINALSOURCESUBJECTNAME;

       [XmlElement]
       public string EVENTUSER;

       [XmlElement]
       public string EVENTCOMMENT;

       [XmlElement]
       public string TOOLID;

    }
}
