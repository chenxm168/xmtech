using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RS232Srv
{
   public class RS232Config
    {
       
       private int baudRate ;

       [XmlAttribute]
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }
        private int dataBits ;

       [XmlAttribute]
        public int DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }
        private string rsStopBits;

       [XmlAttribute]
        public string RsStopBits
        {
            get { return rsStopBits; }
            set { rsStopBits = value; }
        }
        private string rsParity ;

       [XmlAttribute]
        public string RsParity
        {
            get { return rsParity; }
            set { rsParity = value; }
        }
        private string portName ;

       [XmlAttribute]
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        private string flow = "none";

       [XmlAttribute]
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        private int rsRecvTimeout ;

       [XmlAttribute]
        public int RsRecvTimeout
        {
            get { return rsRecvTimeout; }
            set { rsRecvTimeout = value; }
        }

       public RS232Config()
        {
         BaudRate = 9600;
         DataBits = 8;
         RsStopBits = "1";
         RsParity = "None";
         PortName = "COM1";
         Flow = "none";
         RsRecvTimeout = 3000;
        }


       public RS232Config(string xmlpath)
       {

       }

    }
}
