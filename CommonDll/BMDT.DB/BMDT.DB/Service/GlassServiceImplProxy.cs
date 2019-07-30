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
    public class GlassServiceImplProxy : AbsServiceProxy, IGlassService
    {


             public   GlassServiceImplProxy()
       {
           Service = new AlarmServiceImpl();
           InitDB();
       }

      // public EquipmentServiceImplProxy(IDBHelper hp):base(hp)
      //{
         
      //}

       public GlassServiceImplProxy(IDBHelper hp, AbsService svc):base(hp,svc)
       {
           
       }

       public GlassServiceImplProxy(AbsService svc)
           : base(svc)
       {
          
       }


        public int GlassStart(string unitId, string stage, string pnlId, string bluId, string pnlJudge)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IGlassService)Service).GlassStart(unitId, stage,pnlId,bluId,pnlJudge);


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

        public int GlassEnd(string unitId, string stage, string pnlId, string bluId, string pnlJudge)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IGlassService)Service).GlassEnd(unitId, stage, pnlId, bluId, pnlJudge);


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

        public DataTable FindHistoryByTimeRtnDt(string unitId, string frTime, string toTime)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IGlassService)Service).FindHistoryByTimeRtnDt(unitId,frTime,toTime);
            Service.Cmd.Dispose();
            return rtn;
        }

        public GlassHistory[] FindHistoryByTime(string unitId, string frTime, string toTime)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IGlassService)Service).FindHistoryByTime(unitId, frTime, toTime);
            Service.Cmd.Dispose();
            return rtn;
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
                r = ((IGlassService)Service).HistoryDailyClean(remainDay);


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
