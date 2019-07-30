using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HF.DB
{
   public  interface IDBHelper
    {
       IDbConnection OpenConnection();

       IDbCommand CreatCommand(string sql);

       void CloseConnection();


       DataTable ExecuteSqlGetDt(IDbCommand cmd,  int topRows);
       DataTable ExecuteSqlGetDt(IDbCommand cmd );
       int ExecuteNonQuery(IDbCommand cmd ,int timeOut);
       int ExecuteNonQuery(IDbCommand cmd);

      
    }
}
