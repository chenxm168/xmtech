using BMDT.SECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDTEQP.Service;
using BMDT.SECS.Service;
using BMDT.DB.Service;

namespace MPC.Server.SECS.Handlers
{
    public class SecsControlLocalChangeReportAckHandler : AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            var eqCmd = ObjectManager.getObject("commandService") as CommandServiceImpl;

            eqCmd.SendControlStateChangeCommand("L2", 2);

            var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
            var dbSrv = ServiceFactory.GetEquipmentService();
            var eq = dbSrv.FindOne();
            eq.OnlineControlStatus = "Local";
            dbSrv.Update(eq);
            secs.Send_S2F17_DateTimeRequest();
            

        }
    }
}
