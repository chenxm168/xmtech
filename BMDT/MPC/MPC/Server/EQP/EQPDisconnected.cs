using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using BMDT.DB.Service;
using BMDT.DB.Pojo;
using BMDT.SECS.Service;

namespace MPC.Server.EQP
{
    public class EQPDisconnected : IEPQEventHandler
    {



        public void EQPEventProcess(object message)
        {
            var srv = ServiceFactory.GetEquipmentService();
            var eq = srv.FindOne();
            eq.IsConnected = false;

            if(eq.OnlineControlStatus.ToUpper()!="OFFLINE")
            {
                eq.OnlineControlStatus = "Offline";
                var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
                secs.Send_S6F11_ControlStateOfflineChangeReport(eq.EquipmentStatus, eq.EquipmentName);
            }

           


            srv.Update(eq);
        }
    }
}
