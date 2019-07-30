using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.SECS;

namespace MPC.Server.SECS.Handlers
{
   public  class SecsConnectedHandler:AbsSecsHandler
    {

       public SecsConnectedHandler()
       {

       }

        protected override void proc(string driverId, object message)
        {
            Secs.IsConnected = true;

            logger.Debug("hsms connected!");
        }
    }
}
