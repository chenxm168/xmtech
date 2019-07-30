using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class MachineOnlineParameterDataBody
    {
       [XmlElement]
       public string MACHINENAME;

       [XmlElement]
       public OnlieParameterUnitList UNITLIST;

    }

    public class OnlieParameterUnitList
    {
        [XmlElement]
        public OnlineParameterUnit UNIT;
    }

    public class OnlineParameterUnit
    {
        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public OnlineParameterList PARALIST;
    }

    public class OnlineParameter
    {
        [XmlElement]
        public string PARANAME;

        [XmlElement]
        public string PARAVALUE;
    }

    public class OnlineParameterList
    {
        [XmlElement]
        public OnlineParameter[] PARA;
    }

}
