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
    public class SecsRequestOffineHandler : AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            SECSTransaction secsMsg = message as SECSTransaction;
            long systembyte = secsMsg.Systembyte;

            S1F15_RequestOffLine rMsg = new S1F15_RequestOffLine(secsMsg);

            try
            {
                var dbSrv = ServiceFactory.GetEquipmentService();
                var eq = dbSrv.FindOne();
                int iState = eq.GetControlStateCode();
                if(iState==0||(!eq.IsConnected))
                {
                    var secsSrv = ObjectManager.getObject("SECService") as SECSServiceImpl;
                    secsSrv.Send_S1F0(systembyte);
                }else
                {
                    
                        var secsSrv = ObjectManager.getObject("SECService") as SECSServiceImpl;
                        var trx = S1F16_RequestOffLineAck.makeTransaction(false, "0", systembyte);
                        secsSrv.Secs.Reply(trx);

                        eq.SetControlState(0);
                        dbSrv.Update(eq);
                        var cmdSrv = ObjectManager.getObject("commandService") as ICommandService;
                        cmdSrv.SendControlStateChangeCommand("L2", 0);

                        secsSrv.Send_S6F11_ControlStateOfflineChangeReport(eq.GetStateCode().ToString(), eq.EquipmentName);
                       
                       
                   
                    
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
            }

        }
    }
}
