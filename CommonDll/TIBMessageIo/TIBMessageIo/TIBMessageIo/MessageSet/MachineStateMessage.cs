using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
  public  class MachineStateMessage:AbstractMessage
    {

        [XmlElement]
        public MachineStateBody Body;

        private static MachineStateMessage getInstance(string messageName)
        {
            MachineStateMessage msg = new MachineStateMessage();
            msg.Body = new MachineStateBody();
            msg.Init();
            msg.Header.MESSAGENAME = messageName;
            return msg;
        }

        public static MachineStateMessage getMachineStateChangedMessage(ConstantDef.MACHINE_STATE state)
        {
            var msg = getInstance("MachineStateChanged");
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            msg.Body.MACHINESTATENAME = state.ToString();
            return msg;
        }

        public static MachineStateMessage getUnitStateChangedMessage(string unit,ConstantDef.MACHINE_STATE state)
        {
            var msg = getInstance("UnitStateChanged");
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            msg.Body.UNITNAME = unit;
            msg.Body.UNITSTATENAME = state.ToString();
            return msg;
        }

        public static MachineStateMessage getSubUnitStateChangedMessage(string unit, ConstantDef.MACHINE_STATE state)
        {
            var msg = getInstance("SubUnitStateChanged");
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            msg.Body.SUBUNITNAME = unit;
            msg.Body.SUBUNITSTATENAME = state.ToString();
            return msg;
        }

        
    }
}
