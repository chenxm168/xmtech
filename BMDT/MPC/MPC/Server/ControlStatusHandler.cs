using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService;
using TIBMessageIo.MessageSet;
using TIBMessageIo;


namespace MPC.Server
{
   public  class ControlStatusHandler
    {
       private static void ControlStatusChange(string status)
       {
           //HSMS CASE TODO
           return;

           var s = ServiceManager.GetEquipmentService();
          int iR = s.UpdateEQControlStatus(GlobalVariable.EQP_ID, status);
          if (iR > 0)
          {
              var sender = ObjectManager.getObject("TibSender") as ISendable;
              var msg = MachineControlStateMessage.getMachineControlStateChangedMessage();
              msg.Body.TRACELEVEL = "M";
              msg.Body.MACHINECONTROLSTATENAME = status;
              sender.Send(msg);
          }
           switch(status.ToUpper())
           {
               case "OFFLINE":

                   break;

               case "REMOTE":
                   PortHandler.PortStatusReport();
                   break;

               case "LOCAL":
                   PortHandler.PortStatusReport();
                   break;

               default:

                   break;
           }
       }

       public static void ControlStatusChangeToOffline()
       {
           ControlStatusChange("Offline");
       }

       public static void ControlStatusChangeToOnline()
       {
           ControlStatusChange("Remote");
       }

       public static void ControlStatusChangeToLocal()
       {
           ControlStatusChange("Local");
       }
    }
}
