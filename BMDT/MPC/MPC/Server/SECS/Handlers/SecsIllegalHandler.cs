using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.SECS.Message;
using BMDT.SECS;

namespace MPC.Server.SECS.Handlers
{
    public class SecsIllegalHandler:AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            var ts = IllegalData.makeTransaction(false, "0 0 0 0 0 0 0 0 0 0");
            //Secs.s
        }
    }
}
