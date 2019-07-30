using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.Service;
using System.Collections;
using BMDT.DB.Pojo;

namespace BMDT.DB.Service
{
   public class AlarmServiceImpl:AbsService,IAlarmService
    {
        public int AddAlarm(string unitId, string alid, int alst)
        {
            Dictionary<string, Object> keys = new Dictionary<string, object>();
            keys.Add("UNITID", unitId);
            keys.Add("ALID", alid);

            var al = FindbyKey<AlarmSpec>(keys, null, false);
            AlarmHistory his;
            
                his = new AlarmHistory(al, alst);
         

            return InsertToTable(his);

        }

        public int AddAlarm(string alid, int alst)
        {
            Dictionary<string, Object> keys = new Dictionary<string, object>();
            keys.Add("ALID", alid);

            var al = FindbyKey<AlarmSpec>(keys, null, false);
             AlarmHistory his;
            
                his= new AlarmHistory(al, alst);
          
                AlarmSpec alsp = new AlarmSpec();

            
            

            return InsertToTable(his);
        }

        public int DelAlarm(Pojo.AlarmHistory his)
        {
          return  DelFromTable(his);
        }

        public int DelAlarm(Pojo.AlarmHistory[] hiss)
        {
            foreach(var al in hiss)
            {
                DelAlarm(al);
            }
            return 0;
        }

        public int HistoryDailyClean(int remainDay)
        {
          return  HistoryDailyClean<AlarmHistory>(remainDay);
        }


        public AlarmHistory[] FindAllByUnitIdAndTime(string unitid, string frTime, string toTime)
        {
            string sql = "SELECT * FROM ALARMHISTORY WHERE UNITID='{0}' AND ( HISTORYTIME BETWEEN '{1}' AND '{2}' )" ;
            
          return  ExtQueryBySql<AlarmHistory>(sql,new object[]{unitid,frTime,toTime});
        }

        System.Data.DataTable IAlarmService.FindAllByUnitIdAndTimeRtnDt(string unitid, string frTime, string toTime)
        {

            string sql = "SELECT * FROM ALARMHISTORY WHERE UNITID='{0}' AND ( HISTORYTIME BETWEEN '{1}' AND '{2}' )";

            return ExtQueryBySqlRtnDt<AlarmHistory>(sql, new object[] { unitid, frTime, toTime });
        }


        public AlarmSpec FindAlarmSpec(string unitid, string alid)
        {
            Dictionary<string,object> keys = new Dictionary<string,object>();
            keys.Add("UNITID",unitid);
            keys.Add("ALID",alid);
          return  FindbyKey<AlarmSpec>(keys, null, false);
        }


        public int AddAlarm(string unitId, string alid, int alst, int alcd)
        {
            Dictionary<string, Object> keys = new Dictionary<string, object>();
            keys.Add("UNITID", unitId);
            keys.Add("ALID", alid);

            var al = FindbyKey<AlarmSpec>(keys, null, false);
            AlarmHistory his;
            if(al!=null)
            {
                his = new AlarmHistory(al, alst);
            }else
            {
                AlarmSpec sp = new AlarmSpec(unitId, alid, alcd, "Unknow Alarm Occur");
                his = new AlarmHistory(sp, alst);

            }
            


            return InsertToTable(his);
        }


        public int addAlarm(string unitId, string alid, int alst, int alcd, string text)
        {
            AlarmSpec sp = new AlarmSpec(unitId, alid, alcd, text);
            AlarmHistory his = new AlarmHistory(sp,alst);
            return InsertToTable(his);
            
        }
    }
}
