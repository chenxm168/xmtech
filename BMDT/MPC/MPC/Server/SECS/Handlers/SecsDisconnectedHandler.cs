using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.SECS;

namespace MPC.Server.SECS.Handlers
{
   public class SecsDisconnectedHandler:AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            Secs.IsConnected = false;
            logger.Debug("hsms disconnected!");
        }
    }
}
