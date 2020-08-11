using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataLoader.DBSrv
{
   public interface IDBService
    {
       bool InertSql(string sql);
    }
}
