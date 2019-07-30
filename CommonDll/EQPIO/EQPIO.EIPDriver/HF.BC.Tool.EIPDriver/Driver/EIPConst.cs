
namespace HF.BC.Tool.EIPDriver
{
    using System;
    using System.Reflection;

    public class EIPConst
    {
        public static readonly string ATTRIBUTE_ACTION = "Action";
        public static readonly string ATTRIBUTE_ACTIONSTEP = "ActionStep";
        public static readonly string ATTRIBUTE_AUTOOFFENABLE = "AutoOffEnable";
        public static readonly string ATTRIBUTE_AUTOOFFINTERVAL = "AutoOffInterval";
        public static readonly string ATTRIBUTE_BITOFFEVENT = "BitOffEvent";
        public static readonly string ATTRIBUTE_BITOFFREADACTION = "BitOffReadAction";
        public static readonly string ATTRIBUTE_CONNECTINTERVAL = "ConnectInterval";
        public static readonly string ATTRIBUTE_CONNECTMODE = "ConnectMode";
        public static readonly string ATTRIBUTE_ENABLE = "Enable";
        public static readonly string ATTRIBUTE_INTERVAL = "Interval";
        public static readonly string ATTRIBUTE_KEY = "Key";
        public static readonly string ATTRIBUTE_LOGMODE = "LogMode";
        public static readonly string ATTRIBUTE_NAME = "Name";
        public static readonly string ATTRIBUTE_OFFSET = "Offset";
        public static readonly string ATTRIBUTE_POINTS = "Points";
        public static readonly string ATTRIBUTE_REPRESENTATION = "Representation";
        public static readonly string ATTRIBUTE_STARTUP = "StartUp";
        public static readonly string ATTRIBUTE_SYNCVALUE = "SyncValue";
        public static readonly string ATTRIBUTE_TIMEOUT = "Timeout";
        public static readonly string ATTRIBUTE_TRIGGER = "Trigger";
        public static readonly string ATTRIBUTE_USED = "Used";
        public static readonly string ATTRIBUTE_VALUE = "Value";
        public static readonly string ELEMENT_BLOCK = "Block";
        public static readonly string ELEMENT_COMMUNICATION = "Communication";
        public static readonly string ELEMENT_DATAGATHERING = "DataGathering";
        public static readonly string ELEMENT_EIPDRIVER = "EIPDriver";
        public static readonly string ELEMENT_ENTITYREF = "EntityRef";
        public static readonly string ELEMENT_GROUP = "Group";
        public static readonly string ELEMENT_ITEM = "Item";
        public static readonly string ELEMENT_ITEMGROUP = "ItemGroup";
        public static readonly string ELEMENT_ITEMGROUPCOLLECTION = "ItemGroupCollection";
        public static readonly string ELEMENT_MULTIBLOCK = "MultiBlock";
        public static readonly string ELEMENT_MULTITAG = "MultiTag";
        public static readonly string ELEMENT_RECEIVE = "Receive";
        public static readonly string ELEMENT_SCAN = "Scan";
        public static readonly string ELEMENT_SEND = "Send";
        public static readonly string ELEMENT_TAG = "Tag";
        public static readonly string ELEMENT_TAGMAP = "TagMap";
        public static readonly string ELEMENT_TRANSACTION = "Transaction";
        public static readonly string ELEMENT_TRX = "Trx";
        public static readonly string IS_NOT_OPEN = "Is not open";
        public static readonly string TAG_LOG = "{0} ({1}) {2} ms";
        public static readonly string VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static readonly string WRITE_TAG_KEY_WORD = "SD";
        public static readonly string XPATH_BLOCKMAP = "/EIPDriver/BlockMap";
        public static readonly string XPATH_DATAGATHERING = "/EIPDriver/DataGathering";
        public static readonly string XPATH_DATAGATHERING_SCAN = "/EIPDriver/DataGathering/Scan";
        public static readonly string XPATH_EIPDRIVER = "/EIPDriver";
        public static readonly string XPATH_ITEMGROUPCOLLECTION = "/EIPDriver/ItemGroupCollection";
        public static readonly string XPATH_TAGMAP = "/EIPDriver/TagMap";
        public static readonly string XPATH_TRX = "/EIPDriver/Transaction";
        public static readonly string XPATH_TRX_RECEIVE = "/EIPDriver/Transaction/Receive";
        public static readonly string XPATH_TRX_SEND = "/EIPDriver/Transaction/Send";
    }
}
