using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HF.DB;
using log4net;

namespace HF.DB.ObjectService.Type1.Service
{
   public  class EquipmentServiceImplProxy  :AbsServiceProxy ,IEquipmentService
    {
       
     

       //public  IEquipmentService Service
       //{
       //    get;
       //    set;
       //}

      public   EquipmentServiceImplProxy()
       {
           Service = new EquipmentServiceImpl();
           InitDB();
       }

      // public EquipmentServiceImplProxy(IDBHelper hp):base(hp)
      //{
         
      //}

       public EquipmentServiceImplProxy(IDBHelper hp, AbsService2 svc):base(hp,svc)
       {
           
       }

       public EquipmentServiceImplProxy(AbsService2 svc):base(svc)
       {
          
       }

       public Pojo.Equipment FindByEQName(string eq_name)
       {

           Service.Excutor.OpenConnection();
           Service.Cmd = Service.Excutor.CreatCommand(null);

           var rtn = ((IEquipmentService)Service).FindByEQName(eq_name);
           Service.Cmd.Dispose();
           return rtn;
       }

       public Pojo.Equipment[] FindAll()
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
               

           }catch(Exception e)
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

       //private void Init()
       //{
       //    if(Excutor==null)
       //    {
       //        Excutor = DBHelperManager.GetDBHelper();
       //        if(Excutor!=null)
       //        {
       //            logger.Error("DB Error");
       //            return;
       //        }
       //    }
           
       //}
       



       public new  void Dispose()
       {
           try { 
                
               }
           catch(Exception e)
           {
               return;
           }
          
       }






       //public  static EquipmentServiceImplProxy GetEquipmentServiceImplProxy(ISQLExcutable2 ex)
       //{
       //    var proxy = new EquipmentServiceImplProxy(ex);
       //    proxy.Service = new EquipmentServiceImpl(proxy.Excutor);
       //    return proxy;
       //}
          
    }
}
