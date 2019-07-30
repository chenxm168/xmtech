using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.SQLiteDB;
using HF.DB.FirebirdDB;

namespace HF.DB
{
   public  class DBHelperManager
    {
       private static IDBHelper hp = null;
       private static object obj = new object();
       private static DBType dbType;
       public static IDBHelper GetDBHelper()
       {


           return GetDBHelper(DBType.SQLite);
       }

       public static IDBHelper GetDBHelper(DBType type)
       {
           lock(obj)
           {
           if(hp==null)
           {
               if(type==DBType.SQLite)
               {
                  if( GenerateSQLiteDB())
                  {
                      dbType=type;
                  }
               }
           }else
           {
               if(dbType!=type)
               {
                   ((IDisposable)hp).Dispose();
                   if (GenerateSQLiteDB())
                   {
                       dbType = type;
                   }
               }
           }
           }
           return hp;
       }


       public static bool GenerateSQLiteDB()
       {
           lock(obj)
           {
           if(hp!=null)
           {
               return false;
           }
            dbType= DBType.SQLite;
            string path = @"ECS.DB";
            hp = new HF.DB.SQLiteDB.SQLiteHelper("Data Source=" + path);
           if(hp!=null)
           {
               return true;
           }

           return false;
           }

       }

       

       public static bool GenerateFireBirdDB()
       {
           lock (obj)
           {
               if (hp != null)
               {
                   return false;
               }
               var ci = new FirebirdConnectionString();
               ci.UserID = "SYSDBA";
               ci.Pwd = "masterkey";
               ci.DbPath = @"D:\CIMPC.FDB";
               ci.DbServer = "Localhost";
               ci.DbChartSet = "UTF8";
               ci.ServicePort = 3050;
               ci.Dialect = 3;
               ci.ConnectionLifeTime = 15;
               ci.DbPooling = true;
               ci.MaxPoolSize = 15;
               ci.MinPoolSize = 0;
               ci.PacketSize = 8192;

               hp = new FirebirdHelper(ci);
               if (hp == null)
               {
                   return false;
               }
               return true;
           }

       }

       public static DBType GetDBType()
       {
           return dbType;
       }
    }



    public enum DBType
    {
        Firebird,SQLite,MySQL,SQLServer,Oracle
    }
}
