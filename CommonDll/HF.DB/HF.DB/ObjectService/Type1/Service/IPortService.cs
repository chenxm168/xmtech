using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;

namespace HF.DB.ObjectService.Type1.Service
{
   public interface IPortService
    {
        Port FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc);
        Port[] FindAll();
        PortHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc);
        PortHistory[] FindAllHistory();

        int UpdatePort(Port port);

        int UpdatePort(Port port, string EventName);
        int DeletePort(Port port);
        int AddPort(Port port);
        int InsertHistory(Object obj, string etName);
    }
}
