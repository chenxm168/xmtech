using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using BMDT.DB.Service;
using BMDT.DB.Pojo;

namespace MPC.Server.EQP
{
    public class EQPConnected : IEPQEventHandler
    {
        object obj = new object();


        public void EQPEventProcess(object message)
        {
            lock(obj)
            {
                var srv = ServiceFactory.GetEquipmentService();
                var eq = srv.FindOne();
                eq.IsConnected = true;
                srv.Update(eq);
            }
            
        }
    }
}
