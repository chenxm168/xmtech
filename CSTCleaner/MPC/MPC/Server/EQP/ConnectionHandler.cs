using EQPIO.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Server.EQP
{
    public class ConnectionHandler : IEPQEventHandler
    {
        public void EQPEventProcess(object message)
        {
            var rs = ObjectManager.getObject("plcRequest") as PLCRequest;

            rs.SendRequest("L2_BIT_AllPortState", "R");
        }
    }
}
