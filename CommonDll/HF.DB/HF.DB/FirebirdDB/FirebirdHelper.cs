using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using System.Data;
using HF.DB;


namespace HF.DB.FirebirdDB
{
   public class FirebirdHelper:IDBHelper, IDisposable
    {
        

        private FirebirdConnectionString connString;
        ILog logger = LogManager.GetLogger(typeof(FirebirdHelper));
        private FbConnection Conn;
        object ob = new object();
        public FirebirdConnectionString ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        //public FbConnection FbConn
        //{
        //    get { return fbConn; }
        //    set { fbConn = value; }
        //}

       public FirebirdHelper()
        {
            logger.Error("Not Connection String!");
        }

       public FirebirdHelper(FirebirdConnectionString cs)
       {
          var  fbConn = new FbConnection(cs.GetConnetionString());
          Conn = fbConn;
       }





       /*

       public System.Data.DataSet ExecuteSqlGetDs(string sql)
       {
           try {



               OpenConnection();
              FbCommand cmd  = new FbCommand(sql,(FbConnection) Conn);
             
              cmd.CommandType=  CommandType.Text;
              //cmd.CommandText
              cmd.CommandTimeout=60;
              
              FbDataAdapter fda = new FbDataAdapter(cmd);
              DataSet ds = new DataSet();
              logger.DebugFormat("Pre Excute SQL[{0}].", sql);
              fda.Fill(ds);
              cmd.Dispose();
              fda.Dispose();
              return ds;
          
           }catch (Exception e)
           {
               logger.Error(e.Message);
           }

           return null;

       }

       public override System.Data.DataTable ExecuteSqlGetDt(string sql, int topRows)
       {
           try
           {
               OpenConnection();

               FbCommand cmd = new FbCommand(sql, (FbConnection)Conn);
            
               
               cmd.CommandType = CommandType.Text;
               //cmd.CommandText
               cmd.CommandTimeout = 60;
               FbDataAdapter fda = new FbDataAdapter(cmd);
               DataTable dt = new DataTable();
               logger.DebugFormat("Pre Excute SQL[{0}].", sql);
               fda.Fill(0,topRows,dt);
               cmd.Dispose();
               fda.Dispose();
               return dt;

           }catch(Exception e)
           {
               logger.Error(e.Message);
           }
           return null;
       }

       public override int ExecuteNonQuery(string sql, int timeOut)
       {
           try
           {
              // OpenConnetion();
              // FbTransaction trs = fbConn.BeginTransaction();
              // FbCommand cmd = new FbCommand(sql, fbConn);
              // cmd.CommandType = CommandType.Text;
              // //cmd.CommandText
              // cmd.CommandTimeout = 60;
              // logger.DebugFormat("Pre Excute SQL[{0}].", sql);
              //int res = cmd.ExecuteNonQuery();
              //  trs.Commit();
              //  trs.Dispose();
              // cmd.Dispose();

              // CloseConnection();
              // return res;
               Cmd.CommandText = sql;
               return ExtNoQuery(Cmd);

           }catch(Exception e)
           {
               logger.Error(e.Message);
           }
           return 0;
       }*/

       public IDbConnection OpenConnection()
       {
           lock (ob)
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
               FbCommand cmd=null;
               if(sql==null)
               {
                    cmd = new FbCommand(" ",Conn);
               }else
               {
                cmd = new FbCommand(sql,Conn);
                }
               cmd.CommandType = CommandType.Text;
               cmd.CommandTimeout = 60;

               return cmd;

           }
           catch (Exception e)
           {
               logger.Error(e.Message);
               return null;

           }
       }

       public void CloseConnection()
       {
          try
          {
              Conn.Close();

          }catch(Exception e)
          {
              logger.Error(e.Message);
          }
       }

       public DataTable ExecuteSqlGetDt(IDbCommand cmd, int topRows)
       {
           try
           { 
           FbDataAdapter fda = new FbDataAdapter((FbCommand)cmd);
           DataTable dt = new DataTable();
           logger.DebugFormat("Pre Excute SQL[{0}].", cmd.CommandText);
           fda.Fill(0, topRows, dt);
           fda.Dispose();
           return dt;
               }catch (Exception e)
           {
               logger.Error(e.Message);
               return null;
           }
       }

       public DataTable ExecuteSqlGetDt(IDbCommand cmd)
       {
           return ExecuteSqlGetDt(cmd, 1000);
       }

       public int ExecuteNonQuery(IDbCommand cmd, int timeOut)
       {
           int iR =-1;
           try
           {
               iR = cmd.ExecuteNonQuery(); // ((FbCommand)cmd).ExecuteNonQuery();
           }catch (Exception e)
           {
               logger.Error(e.Message);
               
           }

           return iR;
       }

       public int ExecuteNonQuery(IDbCommand cmd)
       {
           return ExecuteNonQuery(cmd, 30);
       }

       public void Dispose()
       {
           CloseConnection();
           Conn.Dispose();
       }
    }
}
