using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;

namespace HF.DB.ObjectService.Type1.Service
{
   public interface IAlarmService
    {
       int AddAlarm(string unitid, string alid, int alst);
       int AddAlarm(string alid, int alst);

       AlarmSpec[] FindHistoryByUnitAndTime(string unitId, object fromTime, object toTime, IList<string> orderBy, bool byAsc);

       AlarmSpec[] FindHistoryByTime( object fromTime, object toTime, IList<string> orderBy, bool byAsc);

       void HistoryDailyClean(int remainDay);

       void DeleteHistoryByList(AlarmHistory[] his);

    }
}
