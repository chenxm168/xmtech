using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HF.DB.ObjectService.Type1.Pojo;

namespace HF.DB.ObjectService.Type1.Service
{
    public  class PortServiceImplProxy:AbsServiceProxy,IPortService
    {
              public PortServiceImplProxy(AbsService2 service):base(service)
      {
      }

       public PortServiceImplProxy(IDBHelper hp,AbsService2 svc):base(hp,svc)
       {
           
       }

       public PortServiceImplProxy()
           : base()
       {
           base.Service = new MachineServiceImpl();
           InitDB();
       }

        public Pojo.Port FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn= ((IPortService)Service).FindByKey(key, orderBy, byAsc);
            Service.Cmd.Dispose();
            return rtn;
        }

        public Pojo.Port[] FindAll()
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IPortService)Service).FindAll();
            Service.Cmd.Dispose();
            return rtn;
          
        }

        public Pojo.PortHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IPortService)Service).FindHistoryByKey(key, orderBy, byAsc);
            Service.Cmd.Dispose();
            return rtn;
            
        }

        public Pojo.PortHistory[] FindAllHistory()
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IPortService)Service).FindAllHistory();
            Service.Cmd.Dispose();
            return rtn;
          
            
        }

        public int UpdatePort(Pojo.Port port)
        {
            //beginTransaction();
            //int iR = -1;
            //try
            //{
            //    iR = ((IPortService)Service).UpdatePort(port);
            //}catch(Exception e)
            //{
            //    RollbackTransaction();
            //    logger.Error(e.Message);
            //}

            //if(iR>0)
            //{
            //    int iR2 = -1;
            //    try
            //    {
            //        iR2 = InsertHistory(port, null);
            //    }catch(Exception e)
            //    {
            //        logger.Error(e);
            //    }
            //    CommitTransaction();
            //}

            //return iR;
            return UpdatePort(port, null);
        }

        public int UpdatePort(Pojo.Port port, string EventName)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;
            int iR = -1;

            int r = -1;
            try
            {
                r = ((IPortService)Service).UpdatePort(port, EventName);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
                trs.Dispose();
                Service.Cmd.Dispose();

                return r;
            }
            if (r < 1)
            {
                trs.Rollback();
                Service.Cmd.Dispose();
                return r;
            }
            int r2 = -1;
            try
            {
                r2 = ((IPortService)Service).InsertHistory(port, EventName);
                if (r2 < 1)
                {
                    logger.Error("Insert History Error");
                }
            }
            catch (Exception e2)
            {
                logger.Error(e2.Message);
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



            //try
            //{
            //    iR = ((IPortService)Service).UpdatePort(port);
            //}
            //catch (Exception e)
            //{
            //    trs.Rollback();
            //    logger.Error(e.Message);
            //}

            //if (iR > 0)
            //{
            //    int iR2 = -1;
            //    try
            //    {
                
            //        iR2 = InsertHistory(port, EventName);
            //        trs.Commit();
            //    }
            //    catch (Exception e)
            //    {
            //        logger.Error(e);
            //    }
             
            //}
        }

        public int DeletePort(Port port)
        {
            int r = -1;
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;
            try
            {
                r = ((IPortService)Service).DeletePort(port);
                trs.Commit();

            }
            catch (Exception e)
            {
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Dispose();
                    Service.Cmd.Dispose();
                }
                catch (Exception e)
                { }
            }

            return r;
        }

        public int AddPort(Port port)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;
            int r = -1;
            try
            {
                r = ((IPortService)Service).AddPort(port);
                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Rollback();
            }
            finally
            {
                try
                {
                    trs.Dispose();
                    Service.Cmd.Dispose();
                }
                catch (Exception e)
                { }
            }

            return r;
        }

        public int InsertHistory(object obj, string etName)
        {
            //IDbConnection conn = Service.Excutor.OpenConnection();
            //IDbTransaction trs = conn.BeginTransaction();
            //IDbCommand cmd = Service.Excutor.CreatCommand(null);
            //cmd.Transaction = trs;
            //Service.Cmd = cmd;

            int iR = -1;
            try
            {
                iR = ((IPortService)Service).InsertHistory(obj, etName);
                //trs.Commit();
            }
            catch (Exception e)
            {
                //trs.Rollback();


            }
            finally
            {
                try
                {
                    //trs.Dispose();
                    //Service.Cmd.Dispose();
                }
                catch (Exception e)
                { }
            }
            return iR;
        }
    }
}
