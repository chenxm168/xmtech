using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;
using System.Data;

namespace HF.DB.ObjectService.Type1.Service
{
    public interface IMachineService
    {
        Machine FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc);
        Machine[] FindAll();
        MachineHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc);
        MachineHistory[] FindAllHistory();

        int UpdateMachine(Machine machine);

        int UpateMachine(Machine machine, string EventName);
        int DeleteMachine(Machine machine);
        int AddMachine(Machine machine);
        int InsertHistory(Object obj, string etName);

        //DataTable QueryHistByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc);

    }
}
