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
                case "L2_Port#1LoadRequestReport":

                    {
                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PL01");
                    var port = portSvr.FindByKey(keys, null, false);
                    if(port!=null)
                    {
                        port.PortStatus = "LoadRequest";

                    }

                  if(  portSvr.UpdatePort(port, "LoadRequest")>0)
                  {
                      PortHandler.PortLoadRequestReport("PL01");
                  }

                    //string status = data<



                    break;

                    }
                case "L2_Port#1UnloadRequestReport":
                    { 

                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PL01");
                    var port2 = portSvr.FindByKey(keys, null, false);
                    Dictionary<string, string> txValues;
                    string  cstid = String.Empty;
                   if( msg.MessageBody.ReadDataList.TryGetValue("L2_W_Port#1UnloadRequestReportBlock", out txValues))
                   {
                      if(! txValues.TryGetValue("CassetteId",out cstid))
                      {
                          return;
                      }
                          
                   }

                    if (port2 != null)
                    {
                        port2.PortStatus = "UnloadRequest";

                    }

                    if (portSvr.UpdatePort(port2, "UnloadRequest") > 0)
                    {
                        PortHandler.PortUnloadRequestReport("PL01",cstid);
                    }


                    //string status = data<



                    break;
                    }
                case "L2_Port#2UnloadRequestReport":

                    { 
                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PU01");
                    var port3 = portSvr.FindByKey(keys, null, false);
                    Dictionary<string, string> txValues3;
                    string cstid3 = String.Empty;
                    if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_Port#2UnloadRequestReportBlock", out txValues3))
                    {
                        if (!txValues3.TryGetValue("CassetteId", out cstid3))
                        {
                            return;
                        }

                    }

                    if (port3 != null)
                    {
                        port3.PortStatus = "UnloadRequest";

                    }

                    if (portSvr.UpdatePort(port3, "UnloadRequest") > 0)
                    {
                        PortHandler.PortUnloadRequestReport("PU01", cstid3);
                    }


                    //string status = data<



                    break;




                    }

                case "L2_Port#2PortStatusChangeReport":

                    { 
                    keys.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                    keys.Add("PORTNAME", "PU01");
                      var port4 = portSvr.FindByKey(keys, null, false);
                    Dictionary<string, string> txValues4;
                    string cstid4 = String.Empty;
                    if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_Port#2UnloadRequestReportBlock", out txValues4))
                    {
                        if (!txValues4.TryGetValue("CassetteId", out cstid4))
                        {
                            return;
                        }

                    }

                    if (port4 != null)
                    {
                        port4.PortStatus = "UnloadRequest";

                    }

                    if (portSvr.UpdatePort(port4, "UnloadRequest") > 0)
                    {
                        PortHandler.PortUnloadRequestReport("PU01", cstid4);
                    }


                    break;
                    }
                default:
                    break;
            }
        }
    }
}
