using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
    public class PortDataMessage:AbstractMessage
    {
        [XmlElement]
        public PortDataBody Body;

        //public PortDataMessage():base()
        //{

        //}

        

        public static PortDataMessage getDafaultInstance()
        {

            PortDataMessage d = new PortDataMessage();

            d.Init();
            d.Body.MACHINENAME = StaticVarible.MachineID;
            return d;
            
        }

        private static PortDataMessage getInstance(PortBaseInfo[] infos,string messageName)
        {
            PortDataMessage data = new PortDataMessage();
            data.Init();
            if(messageName!=null)
            {
                data.Header.MESSAGENAME = messageName;
            }
            else
            {
                data.Header.MESSAGENAME = "PortDataUpdate";
            }

            PortBaseInfoList ls = new PortBaseInfoList(infos);
            PortDataBody body = new PortDataBody();
            body.PORTLIST = ls;
            body.MACHINENAME = StaticVarible.MachineID;
            data.Body = body;
            return data;

        }

        public static PortDataMessage getPortDataUpdateReportMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortDataUpdate");
        }

        public static PortDataMessage getPortEnableChangedMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortEnableChanged");
        }

        public static PortDataMessage getPortTransferStateChangedMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortTransferStateChanged");
        }

        public static PortDataMessage getPortTypeChangedMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortTypeChanged");
        }

        public static PortDataMessage getPortUseTypeChangedMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortUseTypeChanged");
        }

        public static PortDataMessage getPortAccessModeChangedMessage(PortBaseInfo[] infos)
        {
            return getInstance(infos, "PortAccessModeChanged");
        }

    }
}
