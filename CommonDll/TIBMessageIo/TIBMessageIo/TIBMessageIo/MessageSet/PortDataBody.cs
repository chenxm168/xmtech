using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public  class PortDataBody
    {
       [XmlElement]
       public string MACHINENAME;

       [XmlElement]
       public PortBaseInfoList PORTLIST;
    }


    public class PortBaseInfoList
    {
      [XmlElement]
        public PortBaseInfo[] PORT;

      public PortBaseInfoList()
        {

        }

        public PortBaseInfoList(PortBaseInfo[] portList)
      {
          PORT = portList;
      }
    }

    public class  PortBaseInfo
    {
        [XmlElement]
        public string PORTNAME;

        [XmlElement]
        public string PORTTRANSFERSTATENAME;

        [XmlElement]
        public string PORTSTATENAME;

        [XmlElement]
        public string PORTTYPE;

        [XmlElement]
        public string PORTUSETYPE;

        [XmlElement]
        public string PORTACCESSMODE;

        [XmlElement]
        public string CARRIERNAME;

        [XmlElement]
        public string CARRIERTYPE;

        [XmlElement]
        public string PORTENABLEMODE;

        [XmlElement]
        public string PORTOPERMODE;

        [XmlElement]
        public string MACHINECONTROLSTATENAME;

        [XmlElement]
        public string MACHINESTATENAME;

    }
}
