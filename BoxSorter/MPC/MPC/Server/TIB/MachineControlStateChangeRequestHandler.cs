using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBMessageIo.MessageSet;
using HF.DB.ObjectService;
using HF.DB.ObjectService.Type1.Pojo;
using MPC;

namespace MPC.Server.TIB
{
   public class MachineControlStateChangeRequestHandler:IMessageHandler
    {
       public event MessageEventHandler OnMachineControlStateChange;

        public void doWork(object ob)
        {
            var msg = MessageUtils.Convert<MachineControlStateMessage>((string)ob);
            var s = ServiceManager.GetEquipmentService();
            int iR = s.UpdateEQControlStatus(MPC.GlobalVariable.EQP_ID,msg.Body.MACHINECONTROLSTATENAME);
            if(iR>0)
            {
                if(OnMachineControlStateChange!=null)
                {
                    OnMachineControlStateChange(this, new object[] { ob });
                }
            }
            replyMessage(msg);

            PortHandler.PortStatusReport();
        }

        private void replyMessage(object ob)
        {
            var o = ob as MachineControlStateMessage;
            o.Body.ACKNOWLEDGE = "Y";
            o.Header.MESSAGENAME = "MachineControlStateChangeResponse";
            var s = ObjectManager.getObject("TibSender") as TIBMessageIo.ISendable;
            //string m =(( AbstractMessage) ob).ToString();
            //s.Send(m);
            s.Send(ob);
        }
    }
}
