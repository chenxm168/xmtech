using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.DB.Pojo;
using System.Data;

namespace BMDT.DB.Service
{
   public interface IOpCallService
    {
       int Add(OpCallHistory call);

       OpCallHistory[] FindByTime(string fTime, string tTime);

       DataTable FindByTimeRtnDt(string fTime, string tTime);

       int Del(OpCallHistory call);

       int Del(OpCallHistory[] calls);

       int HistoryDailyClean(int remainDay);

    }
}
