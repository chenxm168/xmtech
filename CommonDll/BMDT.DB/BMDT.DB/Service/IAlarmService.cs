using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.DB.Pojo;
using System.Data;

namespace BMDT.DB.Service
{
   public  interface IAlarmService
    {
       int AddAlarm(string unitId, string alid,int alst);
       int AddAlarm(string unitId, string alid, int alst,int alcd);
       int addAlarm(string unitId, string alid, int alst, int alcd, string text);
       int AddAlarm(string alid,int alst);

       int DelAlarm(AlarmHistory his);
       int DelAlarm(AlarmHistory[] hiss);

       int HistoryDailyClean(int remainDay);

       AlarmSpec FindAlarmSpec(string unitid, string alid);

       AlarmHistory[] FindAllByUnitIdAndTime(string unitid, string frTime, string toTime);
       DataTable FindAllByUnitIdAndTimeRtnDt(string unitid, string frTime, string toTime);

    }
}
