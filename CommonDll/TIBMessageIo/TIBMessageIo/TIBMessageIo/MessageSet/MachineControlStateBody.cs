using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class MachineControlStateBody
    {
       [XmlElement]
       public string MACHINENAME;

       [XmlElement]
       public string UNITNAME;

       [XmlElement]
       public string MACHINECONTROLSTATENAME;

       [XmlElement]
       public string ACKNOWLEDGE;

       [XmlElement]
       public string TRACELEVEL;


       //[XmlElement]
       //public string TRACELEVEL;

       [XmlElement]
       public UnitList UNITLIST;


    }

    public class UintControlState
    {
        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public string UNITCONTROLSTATENAME;
    }

    public class UnitList
    {
        [XmlElement]
        public UintControlState[] UNIT;
    }
}
