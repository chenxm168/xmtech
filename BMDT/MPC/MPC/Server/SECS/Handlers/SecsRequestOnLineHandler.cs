using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDTEQP.Service;
using BMDT.SECS.Service;
using BMDT.DB.Service;
using BMDT.SECS;
using WinSECS.structure;
using BMDT.SECS.Message;

namespace MPC.Server.SECS.Handlers
{
    public class SecsRequestOnLineHandler : AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            SECSTransaction secsMsg = message as SECSTransaction;
            long systembyte = secsMsg.Systembyte;
            S1F17_RequestOnLine rMsg = new S1F17_RequestOnLine(secsMsg);
            var dbSrv = ServiceFactory.GetEquipmentService();
            var eq = dbSrv.FindOne();
            var secs = ObjectManager.getObject("SECService") as ISECSService;
            var cmdSrv = ObjectManager.getObject("commandService") as ICommandService ;
            try
            {
                if(!eq.IsConnected)
                {
                    secs.Reply_RequestOnLineAck_NoGood(systembyte);
                    return;
                }else
                {
                   switch( eq.GetControlStateCode())
                   {
                       case 0:
                           {
                               secs.Reply_RequestOnLineAck_Ok(systembyte);
                            if(   secs.Send_S6F11_ControlStateLocalChangeReport(eq.GetStateCode().ToString(), eq.EquipmentName))
                            {
                                cmdSrv.SendControlStateChangeCommand("L2", 2);
                                eq.SetControlState(2);
                                dbSrv.Update(eq);
                            }

                               break;
                           }
                       case 1:
                           {
                               secs.Reply_RequestOnLineAck_Ok(systembyte);
                               if (secs.Send_S6F11_ControlStateLocalChangeReport(eq.GetStateCode().ToString(), eq.EquipmentName))
                               {
                                   cmdSrv.SendControlStateChangeCommand("L2", 2);
                                   eq.SetControlState(2);
                                   dbSrv.Update(eq);
                               }
                               break;
                           }

                       case 2:
                           {
                               secs.Reply_RequestOnLineAck_AlreadyLocal(systembyte);
                               break;
                           }
                   }
                }

            }catch(Exception e)
            {
                
            }
        }
    }
}
