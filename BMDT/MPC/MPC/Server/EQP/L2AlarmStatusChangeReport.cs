using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using BMDT.DB.Service;
using BMDT.DB.Pojo;
using BMDT.SECS.Service;
using EQPIO.MessageData;


namespace MPC.Server.EQP
{
    public class L2AlarmStatusChangeReport : IEPQEventHandler
    {
        Dictionary<int,string> uMap = new Dictionary<int,string>();

        public Dictionary<int, string> UnitMap
        {
          get { return uMap; }
          set { uMap = value; }
        }

        public void EQPEventProcess(object message)
        {
            var alSrv = ServiceFactory.GetAlarmService();
            var eqSrv = ServiceFactory.GetEquipmentService();
            var eq = eqSrv.FindOne();
            var secs = ObjectManager.getObject("SECService") as SECSServiceImpl;
            MessageData<PLCMessageBody> msg = message as MessageData<PLCMessageBody>;
            Dictionary<string, string> d1;
            string unitid = String.Empty;
            string alid = String.Empty;
            string alcd = String.Empty;
            string alst = string.Empty;
            string altx = "Unknow Alarm";
            if (msg.MessageBody.ReadDataList.TryGetValue("L2_W_AlarmStatusChangeReportBlock", out d1))
            {
                if (d1.TryGetValue("AlarmStatus", out alst))
                {                    
                        d1.TryGetValue("Unit", out unitid) ;
                        d1.TryGetValue("AlarmID", out alid) ;
                        d1.TryGetValue("AlarmCode", out alcd)   ;  
                        d1.TryGetValue("AlarmText", out altx)   ;               
                   
                    if(unitid !=null||unitid!=String.Empty)
                    {
                        int iu = Convert.ToInt16(unitid);
                        string u =String.Empty;
                       if( UnitMap.TryGetValue(iu,out u))
                       {
                           unitid = u;
                       }else
                       {
                           return;
                       }
                      
                        
                    }

                    alSrv.addAlarm(unitid, alid, Convert.ToInt16(alst), Convert.ToInt16(alcd), altx);
                            secs.Send_S5F1_AlarmReport(alst, alcd, alid, altx, unitid);

                        }                   

                }

               
            }
        }
    }


