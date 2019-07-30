using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    public  class RecipeParameterBody
    {
        [XmlElement]
        public string MACHINENAME;

        [XmlElement]
        public string MACHINERECIPENAME;
    }

    public class ParameterUnitList
    {
        [XmlElement]
        public ParameterUnit[] UNIT;
    }

    public class ParameterUnit
    {
        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public string LOCALRECIPENAME;

        [XmlElement]
        public ParameterCodeList CCODELIST;

       
    }

    public class ParameterCodeList
    {
        [XmlElement]
        public ParameterCode[] CCODE;
    }
    public class ParameterCode
    {
        [XmlElement]
        public string CCODENAME;

        [XmlElement]
        public ParameterInfoList PARALIST;
    }

    public class ParameterInfoList
    {
        [XmlElement]
        public ParameterInfo[] PARA;
    }

    public class ParameterInfo
    {
        [XmlElement]
        public string PARANAME;

        [XmlElement]
        public string PARAVALUE;
    }
}
