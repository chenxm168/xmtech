using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    public class MachineDataBody
    {
        [XmlElement]
        public String MACHINENAME;

        [XmlElement]
        public String MACHINESTATENAME;

        [XmlElement]
        public String OPERATIONMODENAME;

        [XmlElement]
        public String TRACELEVEL;

        [XmlElement]
        public UnitStateInfoList UNITLIST;


    }


    public class UnitStateInfoList
    {
        [XmlElement]
        public UnitStateInfo[] UNIT;
    }

    public class UnitStateInfo
    {
        [XmlElement]
        public String UNITNAME;

        [XmlElement]
        public String UNITSTATENAME;

        [XmlElement]
        public SubUnitStateInfoList SUBUNITLIST;

        
    }

    public class SubUnitStateInfoList
    {
        [XmlElement]
        public SubUnitStateInfo[] SUBUNIT;
    }

    public class SubUnitStateInfo
    {
        [XmlElement]
        public String SUBUNITNAME;

        [XmlElement]
        public String SUBUNITSTATENAME;
    }
}
