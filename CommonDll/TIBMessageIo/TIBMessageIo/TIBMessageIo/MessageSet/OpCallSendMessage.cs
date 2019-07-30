using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
   public class OpCallSendMessage:AbstractMessage
    {
        [XmlElement]
        public OpCallSendBody Body;

        
        private static OpCallSendMessage getInstance(string text)
        {
            OpCallSendMessage msg = new OpCallSendMessage();
            msg.Init();
            msg.Header.MESSAGENAME = "OpCallSend";
            msg.Body = new OpCallSendBody();
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            msg.Body.OPCALLDESCRIPTION = text;
            return msg;
        }

        public static OpCallSendMessage getOpCallSendMessage(string text)
        {
            var msg = getInstance(text);
            return msg;
        }

    }
}
