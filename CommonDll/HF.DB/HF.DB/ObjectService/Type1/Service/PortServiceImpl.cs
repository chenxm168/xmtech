using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;

namespace HF.DB.ObjectService.Type1.Service
{
   public  class PortServiceImpl:AbsService2,IPortService
    {

        public Pojo.Port FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            return FindAllbyKey<Port>(key, orderBy, byAsc).FirstOrDefault<Port>();
        }

        public Pojo.Port[] FindAll()
        {
            return FindAll<Port>();
        }

        public Pojo.PortHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
             IList<string> list = null;
            if(orderBy==null)
            {
                list = new List<string>();
                 list.Add("HISTORYTIME");
            }else
            {
                list = orderBy;
            }

            return FindHistoryByKey(key, list, byAsc);
        }

        public Pojo.PortHistory[] FindAllHistory()
        {
            return FindHistoryByKey(null, null, false);
        }

        public int UpdatePort(Pojo.Port port)
        {
            return UpdateTable(port);
        }

        public int UpdatePort(Pojo.Port port, string EventName)
        {
            return UpdateTable(port);
        }

        public int DeletePort(Port port)
        {
            return DelFromTable(port);
        }

        public int AddPort(Port port)
        {
            return InsertToTable(port);
        }

        public int InsertHistory(object obj, string etName)
        {
                              
                  Port port = (Port) obj;
            
                    var his = new PortHistory();
                    his.ObjectNo = "S_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    his.HistoryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    his.EventName = etName;
                    his.PortEnableMode = port.PortEnableMode;
                    his.PortGrade = port.PortGrade;
                    his.PortMode = port.PortMode;
                    his.PortName = port.PortName;
                    his.PortNo = port.PortNo;
                    his.PortQtime = port.PortQtime;
                    his.PortStatus = port.PortStatus;
                    his.PortTransferMode = port.PortTransferMode;
                    his.PortType = port.PortType;
                    his.PortTypeAutoMode = port.PortTypeAutoMode;
                    his.PortUseType = port.PortUseType;
                    his.BCRExist = port.BCRExist;
                    his.Capacity = port.Capacity;
                    his.CassetteSeqNo = port.CassetteSeqNo;
                    his.CassetteType = port.CassetteType;
                    his.CurrentStatusTime = port.CurrentStatusTime;
                    his.GradePriority = port.GradePriority;
                    his.PortGrade = port.PortGrade;
                    his.MachineName = port.EquipmentName;
                    //his.MachineName =port.m
                    //var ms = ServiceManager.GetMachineService();
                    //if(ms ==null)
                    //{
                    //    return -1;
                    //}
                    //Dictionary<string, object> keys = new Dictionary<string, object>();
                    //keys.Add("EQUIPMENTNAME", port.EquipmentName);
                    //keys.Add("LOCALNO",port.)

          return   InsertToTable(his);

         //return   InsertHistory<Port>(obj, etName);

           //base.AD
        }
    }
}
