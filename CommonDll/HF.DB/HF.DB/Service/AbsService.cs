using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Data;
using System.Reflection;

namespace HF.DB.Service
{
    public abstract class AbsService
    {
        protected ILog logger = LogManager.GetLogger(typeof(AbsService));

        public AbsSqlGenerator SqlUtil
        {
            get;
            set;
        }
        public IDBHelper Excutor
        {
            get;
            set;
        }

        public IDbCommand Cmd
        {
            get;
            set;
        }


        protected virtual T FindbyKey<T>(Dictionary<string, object> para, IList<string> orderBy, bool byAsc)
        {
            try
            {
                Type t = typeof(T);
                var dt = FindbyKeyRtnDt<T>(para, orderBy, byAsc);
                if (dt == null || dt.Rows.Count < 1)
                {
                    return default(T);
                }
                PropertyInfo[] pis = t.GetProperties();
                var obj = Activator.CreateInstance(t, true);
                foreach (PropertyInfo pi in pis)
                {
                    var cl = pi.Name.ToUpper().Trim();

                    try
                    {
                        if (dt.Rows[0][cl] == null)
                        {
                            pi.SetValue(obj, null);
                        }
                    }
                    catch (Exception e)
                    {
                        pi.SetValue(obj, null);
                    }
                    pi.SetValue(obj, SqlUtil.ColValueToObject(pi, (dt.Rows[0][cl])));

                }
                return (T)obj;

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return default(T);
            }
        }

        protected virtual DataTable FindbyKeyRtnDt<T>(Dictionary<string, object> para, IList<string> orderBy, bool byAsc)
        {


            try
            {
                Type t = typeof(T);
                string sql = SqlUtil.MakeQuerySql<T>(para, orderBy, byAsc);
                logger.Debug("pre excute sql:" + sql);
                Cmd.CommandText = sql;
                return Excutor.ExecuteSqlGetDt(Cmd, 1000);

            }
            catch (Exception e)
            {

                return null;

            }
        }

        protected virtual T[] FindAllbyKey<T>(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
        {
            IList<T> list = new List<T>();
            Type t = typeof(T);
            var dt = FindbyKeyRtnDt<T>(key, orderBy, byAsc);
            if (dt == null || dt.Rows.Count < 1)
            {
                return list.ToArray<T>();
            }

            return SqlUtil.DataTable2Objects<T>(dt);


        }


        protected virtual T[] FindAll<T>()
        {

            return FindAllbyKey<T>(null,null, false);
        }

        protected virtual T[] FindAll<T>(IList<string> orderBy, bool byAsc)
        {
            return FindAllbyKey<T>(null, orderBy, byAsc);
        }


        protected virtual DataTable FindAllRtnDt<T>()
        {
            return FindbyKeyRtnDt<T>(null, null, false);
        }

        protected virtual DataTable FindAllRtnDt<T>(IList<string> orderBy, bool byAsc)
        {
            return FindbyKeyRtnDt<T>(null, orderBy, byAsc);
        }


        protected virtual int UpdateTable<T>(T obj)
        {
            var sql = SqlUtil.MakeUpdateSql<T>(obj);
            //Ex.BeginTransaction();
            Cmd.CommandText = sql;
            int rtn = Excutor.ExecuteNonQuery(Cmd, 60);
            //Ex.CommitTransaction();
            return rtn;

        }

        protected virtual int UpdateTable(object obj)
        {
            var sql = SqlUtil.MakeUpdateSql(obj);
            Cmd.CommandText = sql;
            int rtn = Excutor.ExecuteNonQuery(Cmd, 60);
            return rtn;
        }

        protected virtual int InsertToTable(object obj)
        {
            string sql = SqlUtil.MakeInsertSql(obj);
            if (sql == null || sql.Trim().Length < 1)
            {
                return -1;
            }
            Cmd.CommandText = sql;
            int r = Excutor.ExecuteNonQuery(Cmd, 60);
            return r;
        }

        protected virtual int DelFromTable(object obj)
        {
            string sql = SqlUtil.MakeDelteSql(obj);
            if (sql == null || sql.Trim().Length < 1)
            {
                return -1;
            }
            Cmd.CommandText = sql;
            return Excutor.ExecuteNonQuery(Cmd, 60);
        }

        protected virtual T MakeHistoryObject<T>(object source, string etName)
        {

            Type t1 = source.GetType();
            Type t2 = typeof(T);

            List<PropertyInfo> t1_list = new List<PropertyInfo>();
            t1_list.AddRange(t1.GetProperties());

            T his = (T)Activator.CreateInstance(t2, true);
            PropertyInfo[] pis = t2.GetProperties();
            foreach (var pi in pis)
            {
                if (pi.Name.ToUpper().Trim().Equals("OBJECTNO"))
                {
                    string v = "S_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    pi.SetValue(his, v);
                    continue;
                }
                if (pi.Name.ToUpper().Trim().Equals("HISTORYTIME"))
                {
                    string v = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    pi.SetValue(his, v);
                    continue;
                } if (pi.Name.ToUpper().Trim().Equals("EVENTNAME"))
                {

                    pi.SetValue(his, etName);
                    continue;
                }
                for (int i = 0; i < t1_list.Count; i++)
                {
                    if (pi.Name.ToUpper().Trim() == t1_list[i].Name.ToUpper().Trim())
                    {
                        object v = t1_list[i].GetValue(source, null);
                        pi.SetValue(his, v);
                        t1_list.RemoveAt(i);
                        continue;
                    }
                }


            }
            return (T)his;

        }

        protected virtual int InsertHistory<T>(Object obj, string etName)
        {
            var his = MakeHistoryObject<T>(obj, etName);
            return InsertToTable(his);
        }


        protected virtual DataTable ExtQueryBySqlRtnDt<T>(string s, object[] param)
        {
            string sql = String.Format(s, param);
            Cmd.CommandText = sql;
            logger.DebugFormat("Pre Excute sql [{0}]", Cmd.CommandText);
            return  Excutor.ExecuteSqlGetDt(Cmd, 1000);
        }
        protected virtual T[] ExtQueryBySql<T>(string s, object[] param)
        {
            string sql = String.Format(s, param);
            Cmd.CommandText = sql;
            
            var dt = ExtQueryBySqlRtnDt<T>(s, param);

            return SqlUtil.DataTable2Objects<T>(dt);

        }

        protected virtual int ExtNoQueryBysql(string sql)
        {
            Cmd.CommandText = sql;
            logger.DebugFormat("Pre Excute sql [{0}]", Cmd.CommandText);
            return Excutor.ExecuteNonQuery(Cmd);
        }

        protected virtual int ExtNoQueryBysql(string s, Object[] param)
        {
            string sql = String.Format(s, param);
            Cmd.CommandText = sql;
            logger.DebugFormat("Pre Excute sql [{0}]", Cmd.CommandText);
            return Excutor.ExecuteNonQuery(Cmd);
        }

        protected virtual int HistoryDailyClean<T>(int remainDay)
        {
            var sql = SqlUtil.MakeHistoryDailyCleanSql<T>(remainDay);
            Cmd.CommandText = sql;
            logger.DebugFormat("Pre Excute sql [{0}]", Cmd.CommandText);
            return Excutor.ExecuteNonQuery(Cmd);

        }

    }//end class
}
