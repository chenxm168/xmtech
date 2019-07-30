using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.DB.Pojo;
using HF.DB.Service;
using HF.DB;
using System.Data;

namespace BMDT.DB.Service
{
   public class AlarmServiceImplProxy:AbsServiceProxy,IAlarmService
    {
       public   AlarmServiceImplProxy()
       {
           Service = new AlarmServiceImpl();
           InitDB();
       }

      // public EquipmentServiceImplProxy(IDBHelper hp):base(hp)
      //{
         
      //}

       public AlarmServiceImplProxy(IDBHelper hp, AbsService svc):base(hp,svc)
       {
           
       }

       public AlarmServiceImplProxy(AbsService svc)
           : base(svc)
       {
          
       }

        public int AddAlarm(string unitId, string alid, int alst)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).AddAlarm( unitId,  alid,  alst);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }

        public int AddAlarm(string alid, int alst)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).AddAlarm( alid, alst);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }

        public int DelAlarm(AlarmHistory his)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).DelAlarm(his);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }

        public int DelAlarm(AlarmHistory[] hiss)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).DelAlarm(hiss);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }

        public int HistoryDailyClean(int remainDay)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).HistoryDailyClean(remainDay);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }


        public AlarmHistory[] FindAllByUnitIdAndTime(string unitid, string frTime, string toTime)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);

            var rtn = ((IAlarmService)Service).FindAllByUnitIdAndTime( unitid,  frTime,  toTime);
            Service.Cmd.Dispose();
            return rtn;
        }

        System.Data.DataTable IAlarmService.FindAllByUnitIdAndTimeRtnDt(string unitid, string frTime, string toTime)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);

            var rtn = ((IAlarmService)Service).FindAllByUnitIdAndTimeRtnDt(unitid, frTime, toTime);
            Service.Cmd.Dispose();
            return rtn;
        }


        public AlarmSpec FindAlarmSpec(string unitid, string alid)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);

            var rtn = ((IAlarmService)Service).FindAlarmSpec(unitid, alid);
            Service.Cmd.Dispose();
            return rtn;
        }


        public int AddAlarm(string unitId, string alid, int alst, int alcd)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).AddAlarm(unitId, alid, alst,alcd);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }

        public int addAlarm(string unitId, string alid, int alst, int alcd, string text)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IAlarmService)Service).addAlarm(unitId, alid, alst, alcd, text);


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Commit();
                    Service.Cmd.Dispose();
                    trs.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            return r;
        }
    }
}
