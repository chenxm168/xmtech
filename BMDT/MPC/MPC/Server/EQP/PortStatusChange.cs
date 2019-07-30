using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.MessageData;
using EQPIO.Controller;
using HF.DB.ObjectService;

namespace MPC.Server.EQP
{
    public class PortStatusChange:IEPQEventHandler
    {


        public void EQPEventProcess(object message)
        {
            var portSvr = ServiceManager.GetPortService();
            var keys = new Dictionary<string, object>();
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            switch (msg.MessageBody.EventName)
            {
                case "L2_Port#1PortStatusChangeReport":


                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PL01");
                    var port = portSvr.FindByKey(keys, null, false);
                    if(port!=null)
                    {
                        port.PortStatus = "LoadRequest";

                    }

                  if(  portSvr.UpdatePort(port, "LoadRequest")>0)
                  {
                      PortHandler.PortStatusReport();
                  }

                    //string status = data<



                    break;


                case "L2_Port#2PortStatusChangeReport":


                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PU01");
                    var p = portSvr.FindByKey(keys, null, false);
                    if (p != null)
                    {
                        p.PortStatus = "UnloadRequest";
                        

                    }

                    if (portSvr.UpdatePort(p, "UnloadRequest") > 0)
                  {
                      PortHandler.PortStatusReport();
                  }


                    break;

                default:
                    break;
            }
        }
    }
}
