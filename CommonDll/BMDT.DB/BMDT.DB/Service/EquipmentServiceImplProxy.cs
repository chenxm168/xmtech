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
  public  class EquipmentServiceImplProxy:AbsServiceProxy,IEquipmentService
    {

             public   EquipmentServiceImplProxy()
       {
           Service = new EquipmentServiceImpl();
           InitDB();
       }

      // public EquipmentServiceImplProxy(IDBHelper hp):base(hp)
      //{
         
      //}

       public EquipmentServiceImplProxy(IDBHelper hp, AbsService svc):base(hp,svc)
       {
           
       }

       public EquipmentServiceImplProxy(AbsService svc)
           : base(svc)
       {
           
       }


        public Equipment FindByEQName(string eq_name)
        {
            Service.Excutor.OpenConnection();
           Service.Cmd = Service.Excutor.CreatCommand(null);
           var rtn= ((IEquipmentService)Service).FindByEQName(eq_name);
           Service.Cmd.Dispose();
           return rtn;
        }

        public Equipment FindOne()
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IEquipmentService)Service).FindOne();
            Service.Cmd.Dispose();
            return rtn;
        }

        public Equipment[] FindAll()
        {
            Service.Excutor.OpenConnection();
            Service.Cmd = Service.Excutor.CreatCommand(null);
            var rtn = ((IEquipmentService)Service).FindAll();
            Service.Cmd.Dispose();
            return rtn;
        }

        public int UpdateEQStatus(string eq_name, string status)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IEquipmentService)Service).UpdateEQStatus(eq_name, status);


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

        public int UpdateEQControlStatus(string eq_name, string status)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IEquipmentService)Service).UpdateEQControlStatus(eq_name, status);


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

        public int Update(Equipment eq)
        {
            IDbConnection conn = Service.Excutor.OpenConnection();
            IDbTransaction trs = conn.BeginTransaction();
            IDbCommand cmd = Service.Excutor.CreatCommand(null);
            cmd.Transaction = trs;
            Service.Cmd = cmd;

            int r = -1;
            try
            {
                r = ((IEquipmentService)Service).Update(eq);


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
