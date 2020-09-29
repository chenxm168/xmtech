﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FileDataLoader.DBSrv
{
   public interface IDBService
    {
       bool ExcNonQuerySql(string sql);
       DataTable QueryData(string sql);
    }
}
