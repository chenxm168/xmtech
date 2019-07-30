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
    public class L2ProcessStartReport : IEPQEventHandler
    {
        public void EQPEventProcess(object message)
        {
            var srv = ServiceFactory.GetEquipmentService();
            var eq = srv.FindOne();
            var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            Dictionary<string, string> d1;
        }
    }
}
