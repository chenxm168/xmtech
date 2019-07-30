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
    public class L2EQPStateChangeReport:IEPQEventHandler
    {
        public void EQPEventProcess(object message)
        {
            var srv = ServiceFactory.GetEquipmentService();
            var eq = srv.FindOne();
            var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            Dictionary<string, string> d1;
            if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_EquipmentStatusChangeReportBlock", out d1))
            {
                string state=String.Empty;
                d1.TryGetValue("EquipmentStatus", out state);
                int iState = Convert.ToInt16(state);
                if(eq.GetStateCode()!=iState)
                {
                    eq.SetState(iState);
                    srv.Update(eq);
                    if(eq.GetControlStateCode()!=0)
                    {
                        secs.Send_S6F11_EQPStateChangeReport(eq.GetControlStateCode().ToString(), iState.ToString(), eq.EquipmentName);
                    }
                    

                }else
                {
                    return;
                }
            }
        }
    }
}
