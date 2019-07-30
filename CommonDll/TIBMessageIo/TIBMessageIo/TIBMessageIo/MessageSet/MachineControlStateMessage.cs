using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
     [XmlRoot(ElementName = "Message")]
   public class MachineControlStateMessage:AbstractMessage
    {
         [XmlElement]
         public MachineControlStateBody Body;

         private static MachineControlStateMessage getInstance(string msgName)
         {
             var msg = new MachineControlStateMessage();
             msg.Body = new MachineControlStateBody();
             msg.Init();
             msg.Header.MESSAGENAME = msgName;
             msg.Body.MACHINENAME = StaticVarible.MachineID;
             return msg;
         }

         public static MachineControlStateMessage getMachineControlStateChangedMessage()
         {
             return getInstance("MachineControlStateChanged");
         }
    }
}
