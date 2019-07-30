using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
   public class MachineDataMessage:AbstractMessage
    {
       [ XmlElement]
        public MachineDataBody Body;



       private static MachineDataMessage getMachineDataMessage(string msgName)
       {
           MachineDataMessage msg = new MachineDataMessage();
           msg.Init();
           msg.Body = new MachineDataBody();
           msg.Header.MESSAGENAME = msgName;
           return msg;
       }

        public static MachineDataMessage getMachineDataUpdateMessage()
       {
           return getMachineDataMessage("MachineDataUpdate");
       }

        public static MachineDataMessage getMachineDataResponseMessage()
        {
            return getMachineDataMessage("MachineDataResponse");
        }

        public static MachineDataMessage getMachineOnlineParameterChangeResponseMessage()
        {
            return getMachineDataMessage("MachineOnlineParameterChangeResponse");
        }

        
    }
}
