using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.SECS.Message;
using WinSECS.structure;
using BMDT.SECS;


namespace MPC.Server.SECS.Handlers
{
   public  class DateTimeRequestHandler:AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            var msg = message as SECSTransaction;
            long b = msg.Systembyte; 
            String t = DateTime.Now.ToString("yyyyMMddHHmmss");
            var ts = S2F18_DateTimeDataRequestAck.makeTransaction(false, t,b);
            ts.Systembyte = b;
            Secs.Reply(ts);
            
        }
    }
}
