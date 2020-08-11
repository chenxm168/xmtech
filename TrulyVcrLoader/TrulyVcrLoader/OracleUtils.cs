
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrulyVcrLoader
{
  public  class OracleUtils
    {
        //private static string connString = "Provider=OraOLEDB.Oracle.1;User ID=fabedaadm;Password=fabedaadm1710;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST =10.116.110.51)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = EDBDB)))";
      private static string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.116.110.51)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=EDBDB)));Persist Security Info=True;User ID=fabedaadm;Password=fabedaadm1710;";
      private static string connString1 = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.116.110.52)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=EDBDB)));Persist Security Info=True;User ID=fabedaadm;Password=fabedaadm1710;";
      private static OracleConnection conn = new OracleConnection(connString);
        public static bool isOpen = false;
        public void openEnvironment()
        {
            try
            {
                conn.Open();
                isOpen = true;
                AnalysizeVcrFile.addLog("Oracle DataSource=" +conn.DataSource);
                
            }
            catch(Exception ex)
            {
                isOpen = false;
                AnalysizeVcrFile.addLog("OpenOracle Exception:"+conn.DataSource+"\n" + ex.Message);
                oracleFailOver();
            }
        }

        private void oracleFailOver()
        {
            AnalysizeVcrFile.addLog("Fail Over!");
            if (conn.ConnectionString.Equals(connString))
            conn = new OracleConnection(connString1);

            else if(conn.ConnectionString.Equals(connString1))
                conn = new OracleConnection(connString1);

            openEnvironment();
        }

        public void inertSql(string sql)
        {
            try
            {

          
            OracleCommand insertCmd = new OracleCommand(sql, conn);
            int result=0;
            if(connectState())
                result = insertCmd.ExecuteNonQuery();

            if (result == 1)
            {
                AnalysizeVcrFile.addLog(string.Format("Success insert,insertSql:{0}", sql));
                //Form1.loginfo.Info(string.Format("Success insert,insertSql:{0}", sql));
            }
            else
            {
              //  Form1.loginfo.Info(string.Format("Failed insert,insertSql:{0}", sql));
                AnalysizeVcrFile.addLog(string.Format("Failed insert,insertSql:{0}", sql));
            }
                  }
            catch (Exception ex)
            {
                AnalysizeVcrFile.addLog("Insert Exception:" + sql+"\n" + ex.Message);
            }
        }
        public void querySql(string sql)
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

            }
           
        }

        private bool connectState()
        {
            if (conn.State.Equals(ConnectionState.Open))
            {
                return true;
            }
            else
            {
                openEnvironment();
                return isOpen;
            }

                
        }
    }
}
