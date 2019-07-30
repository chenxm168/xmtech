using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBMessageIo;
using TIBMessageIo.MessageSet;
using HF.DB.ObjectService;

namespace MPC.Server.TIB
{
   public class MachineControlStateRequestHandler:IMessageHandler
    {
        public void doWork(object ob)
        {
            var msg = MessageUtils.Convert<MachineControlStateMessage>((string)ob);
            if(msg!=null)
            {
                msg.Header.MESSAGENAME = "MachineControlStateResponse";
                msg.Body.TRACELEVEL = "M";
                msg.Body.ACKNOWLEDGE = "Y";
                var os = ServiceManager.GetEquipmentService();
                var eq = os.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
                if(eq==null)
                {
                    return;
                }
                msg.Body.MACHINECONTROLSTATENAME = eq.OnlineControlStatus;

                var sender = ObjectManager.getObject("TibSender") as ISendable;
                sender.Send(msg);
            }
        }
    }
}
