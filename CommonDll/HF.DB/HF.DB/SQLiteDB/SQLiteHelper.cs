using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using log4net;

namespace HF.DB.SQLiteDB
{
   public class SQLiteHelper:IDBHelper,IDisposable
    {
       private string _connInfo = "";
       private   SQLiteConnection Conn;
       private ILog logger = LogManager.GetLogger(typeof(SQLiteHelper));

       object ob = new object();

       public SQLiteHelper(string connInfo)
       {
           _connInfo = connInfo;
           Conn = new SQLiteConnection(connInfo);
       }
       
       ~SQLiteHelper()
       {
           Dispose();
           logger.Debug("SQLite DB dispose");
       }


       IDbCommand Cmd;

       public void Dispose()
       {
           CloseConnection();
           Conn.Dispose();
       }

       public IDbConnection OpenConnection()
       {
           lock(ob)
           { 
           if (Conn != null && Conn.State != ConnectionState.Open)
           {
               try
               {
                   Conn.Open();
                   logger.Debug("DB connection Open.");
                   
               }
               catch (Exception e)
               {
                   logger.Error(e.Message);
                   return null;
                  
               }

           }
           return Conn;
           }
       }

       public IDbCommand CreatCommand(string sql)
       {
           try
           {
               var cmd = Conn.CreateCommand();
               cmd.CommandTimeout = 60;
               cmd.CommandType = CommandType.Text;
               if(sql!=null&&sql.Trim().Length>1)
               {
                   cmd.CommandText = sql;
               }
               
               return cmd;
           }catch(Exception e)
           {
               logger.Error(e.Message);
               return null;
           }
       }

       public void CloseConnection()
       {
           lock(ob)
           {
               try { 
           if (Conn != null && Conn.State != ConnectionState.Closed)
           {
               try
               {
                   Conn.Close();
                   logger.Debug("DB connection Close.");
               }
               catch (Exception e)
               {
                   logger.Error(e.Message);
               }

           }}catch(Exception e)
               {
                   logger.Error(e.Message);
               }
           }
       }

       public DataTable ExecuteSqlGetDt(IDbCommand cmd, int topRows)
       {
           try
           {
               DataTable dt = new DataTable();
               logger.DebugFormat("Pre Excute sql [{0}]", cmd.CommandText);
               var reader = cmd.ExecuteReader();
             
               dt.Load(reader);
               return dt;

           }catch(Exception e)
           {
               logger.Error(e.Message);
           }
           return null;
       }

       public DataTable ExecuteSqlGetDt(IDbCommand cmd)
       {
           return ExecuteSqlGetDt(cmd, 1000);
       }

       public int ExecuteNonQuery(IDbCommand cmd, int timeOut)
       {
           int iR = -1;
           try
           {
               cmd.CommandTimeout = timeOut;
               logger.DebugFormat("Pre Excute sql [{0}]", cmd.CommandText);
               iR = cmd.ExecuteNonQuery();
               
           }
           catch (Exception e)
           {
               logger.Error(e.Message);
           }
           return iR;
       }

       public int ExecuteNonQuery(IDbCommand cmd)
       {
           return ExecuteNonQuery(cmd, 30);
       }
    }
}
