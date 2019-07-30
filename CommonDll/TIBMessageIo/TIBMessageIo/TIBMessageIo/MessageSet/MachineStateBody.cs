using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class MachineStateBody
    {

       [XmlElement]
       public string MACHINENAME;

       [XmlElement]
       public string MACHINESTATENAME;

       [XmlElement]
       public string ALARMCODE;

       [XmlElement]
       public string ALARMTEXT;

       [XmlElement]
       public string ALARMTIMESTAMP;

       [XmlElement]
       public string UNITNAME;

       [XmlElement]
       public string UNITSTATENAME;

       [XmlElement]
       public string SUBUNITNAME;

       [XmlElement]
       public string SUBUNITSTATENAME;


    }
}
