using EQPIO.Controller;
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
    public class InitPortState : IEPQEventHandler
    {
        public void EQPEventProcess(object message)
        {
            var portSvr = ServiceManager.GetPortService();
            var keys = new Dictionary<string, object>();
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            Dictionary<string, string> states;

            string P2_CST=string.Empty;
            string P1_CST=string.Empty;
            Dictionary<string, string> P2_CST_blc;
            if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_Port#2UnloadRequestReportBlock", out P2_CST_blc))
            {
                P2_CST_blc.TryGetValue("CassetteId", out P2_CST);
            }

            Dictionary<string, string> P1_CST_blc;
            if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_Port#2UnloadRequestReportBlock", out P1_CST_blc))
            {
                P2_CST_blc.TryGetValue("CassetteId", out P1_CST);
            }

            if(msg.MessageBody.ReadDataList.TryGetValue("L2_B_LTM_H",out states))
            {
                string P2_LR;
                if (states.TryGetValue("Port#2LoadRequestReport", out P2_LR))
                {
                    
                }

                string P2_UR;
                if (states.TryGetValue("Port#2UnloadRequestReport", out P2_UR))
                {
                    if(P2_UR.Trim()=="1")
                    {
                        var ks1 = new Dictionary<string, object>();
                        ks1.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                        ks1.Add("PORTNAME", "PU01");
                        var p2 = portSvr.FindByKey(ks1, null, false);

                        if (p2!= null)
                        {
                            p2.PortStatus = "UnloadRequest";
                            p2.CarrierId = P2_CST;
                            if (portSvr.UpdatePort(p2, "UnloadRequest") > 0)
                            {
                                if (P2_CST != null && P2_CST != string.Empty && P2_CST.Trim().Length > 0)
                                {
                                    PortHandler.PortLoadComplateReport("PU01", P2_CST);
                                    System.Threading.Thread.Sleep(3000);
                                    PortHandler.PortUnloadRequestReport("PU01", P2_CST);
                                }
                            }
                        }



                    }else
                    {
                        if(P2_UR.Trim()=="0")
                        {

                        }
                    }

                }

                string P1_LR;
                if (states.TryGetValue("Port#1LoadRequestReport", out P1_LR))
                {
                    if (P1_LR.Trim() == "1")
                    {
                        var ks1 = new Dictionary<string, object>();
                        ks1.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                        ks1.Add("PORTNAME", "PL01");
                       var p1 = portSvr.FindByKey(ks1, null, false);
                      
                        if(p1!=null)
                        {
                            p1.PortStatus = "LoadRequest";
                            if (portSvr.UpdatePort(p1, "LoadRequest") > 0)
                            {
                                PortHandler.PortLoadRequestReport("PL01");
                            }
                        }                       
                    }
                }

                string P1_UR;
                if (states.TryGetValue("Port#1UnloadRequestReport", out P1_UR))
                {
                    if(P1_UR.Trim()=="1")
                    {

                        var ks1 = new Dictionary<string, object>();
                        ks1.Add("EQUIPMENTNAME", GlobalVariable.EQP_ID);
                        ks1.Add("PORTNAME", "PU01");
                        var p2 = portSvr.FindByKey(ks1, null, false);

                        if (p2 != null)
                        {
                            p2.PortStatus = "UnloadRequest";
                            if (portSvr.UpdatePort(p2, "UnloadRequest") > 0)
                            {

                                if (P1_CST != null && P1_CST != string.Empty && P1_CST.Trim().Length > 0)
                                {
                                    PortHandler.PortUnloadRequestReport("PL01", P1_CST);
                                }
                            }
                        }




                           
                    }
                }


            }


        }
    }
}
