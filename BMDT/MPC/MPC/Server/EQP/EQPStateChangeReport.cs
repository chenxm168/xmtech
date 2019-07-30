using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using EQPIO.MessageData;
namespace MPC.Server.EQP
{
   public  class EQPStateChangeReport:IEPQEventHandler
    {


        public void EQPEventProcess(object message)
        {
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;



        }
    }
}
