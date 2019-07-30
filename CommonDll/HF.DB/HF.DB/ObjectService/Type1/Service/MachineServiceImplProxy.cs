using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HF.DB.ObjectService.Type1.Pojo;
using HF.DB;

namespace HF.DB.ObjectService.Type1.Service
{
   public  class MachineServiceImplProxy:AbsServiceProxy,IMachineService
    {

       public MachineServiceImplProxy(AbsService2 service):base(service)
      {
      }

       public MachineServiceImplProxy(IDBHelper hp,AbsService2 svc):base(hp,svc)
       {
           
       }

       public MachineServiceImplProxy( ):base()
       {
           base.Service = new MachineServiceImpl();
           InitDB();
       }



        public Pojo.Machine FindByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);

            var rtn= ((IMachineService)Service).FindByKey(key, orderBy, byAsc);
            Service.Cmd.Dispose();
            return rtn;
           
        }

        public Pojo.Machine[] FindAll()
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var  rtn = ((IMachineService)Service).FindAll();
            Service.Cmd.Dispose();
            return rtn;
        }

        public Pojo.MachineHistory[] FindHistoryByKey(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
          Service.Cmd = Service.Excutor.CreatCommand(null);
          var rtn=  ((IMachineService)Service).FindHistoryByKey(key, orderBy, byAsc);
          Service.Cmd.Dispose();
          return rtn;
        }

        public Pojo.MachineHistory[] FindAllHistory()
        {
            //return ((IMachineService)Service).FindAllHistory();
            return FindHistoryByKey(null, null, false);
        }

        public int UpdateMachine(Pojo.Machine machine)
        {

            return UpateMachine(machine, null);
        }
        

        public int UpateMachine(Pojo.Machine machine, string etName)
        {
            IDbConnection  conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r =-1;
            try {
             r    = ((IMachineService)Service).UpateMachine(machine,etName);
                 }
            catch(Exception e)
            {
                logger.Error(e.Message);
                trs.Rollback();
                trs.Dispose();
                Service.Cmd.Dispose();

                return r;
            }
            if(r<1)
            {
                trs.Rollback();
                Service.Cmd.Dispose();
                return r;
            }
            int r2 = -1;
            try
            { 
             r2 =((IMachineService)Service).InsertHistory(machine,etName);
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
                try {
                trs.Commit();
                Service.Cmd.Dispose();
                trs.Dispose();
                    }catch(Exception e)
                {

                }
            }
           
            return r;
        }

        public int DeleteMachine(Pojo.Machine machine)
        {
            int r = -1;
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;
            try
            {
              r=  ((IMachineService)Service).DeleteMachine(machine);
              trs.Commit();

            }catch(Exception e)
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

        public int AddMachine(Pojo.Machine machine)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;
            int r = -1;
            try
            { 
              r= ((IMachineService)Service).AddMachine(machine);
              trs.Commit();
                }
            catch(Exception e)
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

        public int InsertHistory(object obj)
        {
            return InsertHistory(obj,null);
        }


        public int InsertHistory(object obj, string etName)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int iR = -1;
            try
            { 
             iR=((IMachineService)Service).InsertHistory(obj, etName);
             trs.Commit();
                }catch (Exception e )
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
            return iR;
        }
    }
}
