using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.DB.Pojo;
using HF.DB.Service;

namespace BMDT.DB.Service
{
   public class OpCallServiceImpl:AbsService,IOpCallService
    {
        public int Add(OpCallHistory call)
        {
          return   InsertToTable(call);
        }

        public OpCallHistory[] FindByTime(string fTime, string tTime)
        {
            string sql = "SELECT * FROM OPCALLHISTORY WHERE HISTORYTIME BETWEEN '{0}' AND '{1}' ";
            return  ExtQueryBySql<OpCallHistory>(sql,new object[]{fTime,tTime});
        }

        public System.Data.DataTable FindByTimeRtnDt(string fTime, string tTime)
        {
            string sql = "SELECT * FROM OPCALLHISTORY WHERE HISTORYTIME BETWEEN '{0}' AND '{1}' ";
            return ExtQueryBySqlRtnDt<OpCallHistory>(sql, new object[] { fTime, tTime });
        }

        public int Del(OpCallHistory call)
        {
            throw new NotImplementedException();
        }

        public int Del(OpCallHistory[] calls)
        {
            throw new NotImplementedException();
        }

        public int HistoryDailyClean(int remainDay)
        {
            throw new NotImplementedException();
        }
    }
}
