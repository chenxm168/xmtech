using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;
using HF.DB;

namespace HF.DB.ObjectService.Type1.Service
{
   public class MachineServiceImpl:AbsService2,IMachineService
    {
        public Pojo.Machine FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
          return  FindbyKey<Machine>(key,orderBy,byAsc);
        }

        public Pojo.Machine[] FindAll()
        {
            return FindAll<Machine>();
        }

        public Pojo.MachineHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            IList<string> list = null;
            if(orderBy==null)
            { 
            list = new List<string>();
            list.Add("HISTORYTIME");
            }
            return FindAllbyKey<MachineHistory>(key, list, byAsc);
            
        }

        public Pojo.MachineHistory[] FindAllHistory()
        {
            IList<string> list = new List<string>();
            list.Add("HISTORYTIME");
            return FindAllbyKey<MachineHistory>(null, list, false);
        }

        public int UpdateMachine(Pojo.Machine machine)
        {
            int r= UpdateTable(machine);
            return r;

        }

       

        public int UpateMachine(Pojo.Machine machine, string EventName)
        {
            int r = UpdateTable(machine);
            return r;
        }

        public int DeleteMachine(Pojo.Machine machine)
        {
            return DelFromTable(machine);
        }

        public int AddMachine(Pojo.Machine machine)
        {
            //TODO;
            return InsertToTable(machine);

           
        }

        public int InsertHistory(Object obj,string etName)
        {
            return InsertHistory<MachineHistory>(obj,etName);
        }

    }
}
