using BMDT.DB.Pojo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDT.DB.Service
{
  public  interface IGlassService
    {
       int GlassStart(string unitId, string stage, string pnlId, string bluId, string pnlJudge);
       int GlassEnd(string unitId, string stage, string pnlId, string bluId, string pnlJudge);

       DataTable FindHistoryByTimeRtnDt(string unitId, string frTime,string toTime);
       GlassHistory[] FindHistoryByTime(string unitId, string frTime, string toTime);

      int HistoryDailyClean(int remainDay);

    }
}
