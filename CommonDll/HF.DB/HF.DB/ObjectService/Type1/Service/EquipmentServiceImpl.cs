using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;
using HF.DB;
using System.Data;
using System.Reflection;

namespace HF.DB.ObjectService.Type1.Service
{
   public class EquipmentServiceImpl:AbsService2,IEquipmentService
    {






       public Pojo.Equipment FindByEQName(string eq_name)
       {
           var key = new Dictionary<string, object>();
           key.Add("EQUIPMENTNAME", eq_name);
           string sql = SqlUtil.MakeQuerySql<Equipment>(key,null,false);
           return   FindbyKey<Equipment>(key);
       }

       public Pojo.Equipment[] FindAll()
       {
           return FindAll<Equipment>();
       }

       public int UpdateEQStatus(string eq_name, string status)
       {
           var eq = FindByEQName(eq_name);
           if(eq==null)
           {
               return -1;
           }
           eq.OldEquipmentStatus = eq.EquipmentStatus;
           eq.EquipmentStatus = status;
           eq.CurrentStatusTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        return    UpdateTable(eq);
       }

       public int UpdateEQControlStatus(string eq_name, string status)
       {
           var eq = FindByEQName(eq_name);
           if (eq == null)
           {
               return -1;
           }
           eq.OnlineControlStatus = status;
           return UpdateTable(eq);
       }
    }
}
