using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TCPSrv.Reader
{
   public  class VCRConfigTCP
    {
       [XmlAttribute]
       public bool Enable
       {
           get;
           set;
       }
       [XmlAttribute]
       public string RemoteIp
       {
       get;set;

       }
       [XmlAttribute]
       public int TcpPort
       {
           get;
           set;
       }
       [XmlAttribute]
       public int ReadInterval
       { get; set; }
       [XmlAttribute]
       public int ReveiveTimeout
       {
           get;
           set;
       }

       [XmlAttribute]
       public byte Terminator
       {
           get;
           set;
       }

       [XmlAttribute]
       public string NGString
       {
           get;
           set;
       }

       public VCRConfigTCP()
       {
          
           
          // xs.Deserialize
       }
    }
}
