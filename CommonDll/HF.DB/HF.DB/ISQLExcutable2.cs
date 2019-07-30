using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HF.DB
{
   public interface ISQLExcutable2
    {
       void BeginTransaction();
       void CommitTransaction();
       void RollbackTransaction();

       //DataSet ExecuteSqlGetDs(string sql);
       DataTable ExecuteSqlGetDt(string sql, int topRows);
       int ExecuteNonQuery(string sql, int timeOut);
    }
}
