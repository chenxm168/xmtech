using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{

    [XmlRoot(ElementName = "Message")]
   public  class ProcessDataMessage:AbstractMessage
    {
        [XmlElement]
        public ProcessDataBody Body;

        private static  ProcessDataMessage getInstance(string messageName)
        {
            ProcessDataMessage msg = new ProcessDataMessage();
            msg.Body = new ProcessDataBody();
            msg.Init();
            msg.Header.MESSAGENAME = messageName;
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            return msg;
        }

        public static  ProcessDataMessage getLotProcessDataMessage()
        {
            return getInstance("LotProcessData");
        }

        public static ProcessDataMessage getProductProcessDataMessage()
        {
            return getInstance("ProductProcessData");
        }
    }

    

    public class ProcessDataBody
    {
        [XmlElement]
        public string MACHINENAME;

        [XmlElement]
        public string LOTNAME;

        [XmlElement]
        public string CARRIERNAME;

        [XmlElement]
        public string MACHINERECIPENAME;

        [XmlElement]
        public string PROCESSOPERATIONNAME;

        [XmlElement]
        public string PRODUCTSPECNAME;

        [XmlElement]
        public string PRODUCTSPECVERSION;

        [XmlElement]
        public ProcessUnitInfoList UNITLIST;

        [XmlElement]
        public processGlassInfoList PRODUCTLIST;
    }
    public class ProcessUnitInfoList
    {
        [XmlElement]
        public ProcessUnitInfo[] UNIT;
    }
    public class ProcessUnitInfo
    {
        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public string SUBUNITNAME;

        [XmlElement]
        public ProcessItemInfoList ITEMLIST;
    }

    public class ProcessItemInfoList
    {
        [XmlElement]
        public ProcessItemInfo[] ITEM;
    }

    public class ProcessItemInfo
    {
        [XmlElement]
        public string ITEMNAME;

        [XmlElement]
        public string ITEMVALUE;
    }

    public class processGlassInfoList
    {
        [XmlElement]
        public processGlassInfo[] PRODUCT;
    }

    public class processGlassInfo
    {
        [XmlElement]
        public string PRODUCTNAME;

        [XmlElement]
        public string PRODUCTIONTYPE;

        [XmlElement]
        public string PROCESSINGSTATE;


        [XmlElement]
        public ProcessUnitInfoList UNITLIST;


    }
}
