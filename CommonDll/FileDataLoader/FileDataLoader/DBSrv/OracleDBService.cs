using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataLoader.DBSrv
{
   public class OracleDBService : IDBService,IDisposable
    {
       public string ConnString1
       {get;set;}

       public string ConnString2
       { get; set; }

       public bool IsOpen
       { get; set; }

       public ICustomizedLog Clog
       {
           get;
           set;
       }

       private OracleConnection conn=null;
       private bool isOpen = false;

       public OracleDBService()
       {
           ConnString1 = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.116.110.51)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=EDBDB)));Persist Security Info=True;User ID=fabedaadm;Password=fabedaadm1710;";
           ConnString2 = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.116.110.52)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=EDBDB)));Persist Security Info=True;User ID=fabedaadm;Password=fabedaadm1710;";

           openEnvironment();
       }

       public OracleDBService(string cs1,string cs2)
       {
           ConnString1 = cs1;
           ConnString2 = cs2;
           openEnvironment();
       }

       public OracleDBService(string configfile)
       {
           ConfigLoader cfg = ConfigLoader.getConfigInstance(configfile);
           ConnString1 = cfg.getAsciiParam("ConnString1");
           ConnString2 = cfg.getAsciiParam("Connstring2");
           string logname = cfg.getParam("Logger");
           Clog = LogFactory.getLogger(logname);
           openEnvironment();

       }

       private void oracleFailOver()
       {
           Clog.error("Fail Over!");
           if (conn.ConnectionString.Equals(ConnString1))
               conn = new OracleConnection(ConnString2);

           else if (conn.ConnectionString.Equals(ConnString2))
               conn = new OracleConnection(ConnString1);

           openEnvironment();
       }

       ~OracleDBService()
       {
           Dispose();
       }

       public bool InertSql(string sql)
       {
           bool bRtn = false;
           try
           {

               if(conn==null)
               {
                   Clog.error("DB connected fail");
                   oracleFailOver();
               }
               if(conn==null)
               {
                   Clog.error("DB Faill");
                   return bRtn;
               }
               OracleCommand insertCmd = new OracleCommand(sql, conn);
               int result = 0;
               if (connectState())
                   result = insertCmd.ExecuteNonQuery();

               if (result == 1)
               {
                  Clog.info(string.Format("Success insert,insertSql:{0}", sql));
                   //Form1.loginfo.Info(string.Format("Success insert,insertSql:{0}", sql));
                  bRtn = true;
               }
               else
               {
                   //  Form1.loginfo.Info(string.Format("Failed insert,insertSql:{0}", sql));
                   Clog.error(string.Format("Failed insert,insertSql:{0}", sql));
               }
           }
           catch (Exception ex)
           {
               Clog.error("Insert Exception:" + sql + "\n" + ex.Message);
           }

           return bRtn;
       }




       public void QuerySql(string sql)
       {
           try
           {
               OracleCommand queryCmd = new OracleCommand(sql, conn);
               int queryResult = 1;

               if (connectState())
                   queryResult = int.Parse(queryCmd.ExecuteScalar().ToString());
               if (queryResult == 0)
               {
                   //Exist
               }
               else
               {
                   //Not Exist
               }
           }
           catch (Exception ex)
           {
               Clog.error(ex.Message);
           }

       }




       public bool openEnvironment()
       {

           if (Clog == null)
           {
             Clog=  LogFactory.getSimpleLogger();
           }


           bool bRtn = false;
           if(Connect(ConnString1))
           {

           }else
           {
               Connect(ConnString2);
           }
           try{

               if (conn != null)
               {
                   conn.Open();
                   isOpen = true;
                   bRtn = true;
               }

           }catch(Exception e)
           {
               Clog.error(e.Message);
               isOpen = false;
           }


           return bRtn;
           
       }


       public bool Connect(string cs)
       {
           bool bRtn = false;
           

           try
           {
               if(conn==null)
               {
                   conn = new OracleConnection(cs);
                   bRtn = true;
               }

           }
           catch(Exception e)
           {
               conn = null;
               Clog.error(e.Message);
           }


           return bRtn;
       }




       private bool connectState()
       {
           if (conn.State.Equals(ConnectionState.Open))
           {
               isOpen = true;
               return true;
           }
           else
           {
               openEnvironment();
               return isOpen;
           }


       }


        public void Dispose()
        {
            try{
                if(conn!=null)
                {
                    conn.Close();
                }
            }catch(Exception e )
            {
                Clog.error(e.Message);
            }
            
        }
    }
}
