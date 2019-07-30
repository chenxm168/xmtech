using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using BMDT.DB.Service;
using BMDT.DB.Pojo;
using BMDT.SECS.Service;
using EQPIO.MessageData;

namespace MPC.Server.EQP
{
   public  class L2ControlStateChangeRequest:IEPQEventHandler
    {
       Object obj = new object();
        public void EQPEventProcess(object message)
        {
            var srv = ServiceFactory.GetEquipmentService();
            var eq = srv.FindOne();
            var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            Dictionary<string,string> d1 ;
           if(  msg.MessageBody.ReadDataList.TryGetValue("L2_W_ControlStateChangeRequestBlock", out d1))
           {
               string state = String.Empty;

               if (d1.TryGetValue("ControlState", out state))
               {
                   int iState = Convert.ToInt16(state);
                   switch (iState)
                   {
                       case 0:
                           {

                               if(eq.OnlineControlStatus.ToUpper()=="OFFLINE")
                               {
                                   return;
                               }else
                               {
                                   eq.SetControlState(iState);
                                   srv.Update(eq);
                                   secs.Send_S6F11_ControlStateOfflineChangeReport(eq.GetStateCode().ToString(), eq.EquipmentName);
                               }
                               break;
                           }

                       case 1:
                           {
                               break;
                           }

                       case 2:
                           {
                               if (eq.OnlineControlStatus.ToUpper() == "LOCAL")
                               {
                                   return;
                               }
                               else
                               {
                                   
                                  if( secs.Send_S6F11_ControlStateLocalChangeReport(eq.GetStateCode().ToString(), eq.EquipmentName))
                                  {
                                      eq.SetControlState(iState);
                                      srv.Update(eq);
                                  }
                               }
                               break;
                           }

                   }
               }
           }

        }

       
    }
}
