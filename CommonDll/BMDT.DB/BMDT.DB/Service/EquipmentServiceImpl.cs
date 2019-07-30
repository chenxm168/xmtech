using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.Service;
using BMDT.DB.Pojo;

namespace BMDT.DB.Service
{
    public class EquipmentServiceImpl:AbsService,IEquipmentService
    {
        public Pojo.Equipment FindByEQName(string eq_name)
        {
            Dictionary<string ,object> keys = new Dictionary<string,object>();
            keys.Add("EquipmentName",eq_name);
          return  FindAllbyKey<Equipment>(keys, null, false).FirstOrDefault<Equipment>();
        }

        public Pojo.Equipment FindOne()
        {
          return  FindAll().FirstOrDefault<Equipment>();
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
            return UpdateTable(eq);
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

        public int Update(Pojo.Equipment eq)
        {
            return UpdateTable(eq);
        }
    }
}
