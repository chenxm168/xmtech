using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using log4net;

namespace HF.DB.ObjectService
{
   public class AbsServiceProxy:IDisposable
    {
       protected ILog logger = LogManager.GetLogger(typeof(AbsServiceProxy));
       public IDbCommand Cmd
       {
           get;
           set;
       }

       public AbsService2 Service
       {
           get;
           set;
       }

       public AbsServiceProxy()
       {
           //InitDB();
       }

       public AbsServiceProxy(AbsService2 service)
       {
           Service = service;
           InitDB();
          
       }

       public AbsServiceProxy(IDBHelper hp,AbsService2 service)
       {
           Service.Excutor = hp;
           Service = service;
       }

       protected bool InitDB()
       {
           
           if(Service.Excutor==null)
           {
               Service.Excutor = DBHelperManager.GetDBHelper();
           }
           if(Service.Excutor ==null)
           {
               logger.Error("DB Error");
               return false;
           }


           return true;

       }


       public void Dispose()
       {
           try
           {
               if(Cmd!=null)
               {
                   Cmd.Dispose();
               }
               Service.Excutor.CloseConnection();
               ((IDisposable)Service.Excutor).Dispose();

           }catch(Exception e )
           {
               logger.Error(e.Message);
           }

       }
    }
}
