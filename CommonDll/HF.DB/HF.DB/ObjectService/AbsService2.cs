using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using log4net;
using System.Reflection;


namespace HF.DB.ObjectService
{
   public class AbsService2
    {
       protected ILog logger = LogManager.GetLogger(typeof(AbsService2));
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

       protected T FindbyKey<T>(Dictionary<string, object> para, IList<string> orderBy, bool byAsc)
       {
           try
           {
               Type t = typeof(T);
               string sql = SqlUtil.MakeQuerySql<T>(para, orderBy, byAsc);
               Cmd.CommandText = sql;
               var dt = Excutor.ExecuteSqlGetDt(Cmd,1000);
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

           }catch (Exception e)
           {
               logger.Error(e.Message);
               return default(T);
           }
       }

       protected T FindbyKey<T>(Dictionary<string, object> para)
       {

           return FindbyKey<T>(para, null, false);
       }

       protected T[] FindAllbyKey<T>(Dictionary<string, object> key, IList<string> orderBy, bool byAsc)
       {
           IList<T> list = new List<T>();
           Type t = typeof(T);
           string sql = SqlUtil.MakeQuerySql<T>(key, orderBy, byAsc);
           Cmd.CommandText = sql;
           var dt = Excutor.ExecuteSqlGetDt(Cmd,1000);

           if (dt == null || dt.Rows.Count < 1)
           {
               return list.ToArray<T>();
           }

           foreach (DataRow row in dt.Rows)
           {

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
                   pi.SetValue(obj, SqlUtil.ColValueToObject(pi, (row[cl])));

               }
               list.Add((T)obj);
           }

           return list.ToArray<T>();

       }

       protected T[] FindAll<T>()
       {

           return FindAll<T>(null, false);
       }

       protected T[] FindAll<T>(IList<string> orderBy, bool byAsc)
       {
           return FindAllbyKey<T>(null, orderBy, byAsc);
       }

       protected int UpdateTable<T>(T obj)
       {
           var sql = SqlUtil.MakeUpdateSql<T>(obj);
           //Ex.BeginTransaction();
           Cmd.CommandText = sql;
           int rtn = Excutor.ExecuteNonQuery(Cmd, 60);
           //Ex.CommitTransaction();
           return rtn;

       }



       protected int UpdateTable(object obj)
       {
           var sql = SqlUtil.MakeUpdateSql(obj);
           Cmd.CommandText = sql;
           int rtn = Excutor.ExecuteNonQuery(Cmd, 60);
           return rtn;
       }


       protected int InsertToTable(object obj)
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

       protected int DelFromTable(object obj)
       {
           string sql = SqlUtil.MakeDelteSql(obj);
           if (sql == null || sql.Trim().Length < 1)
           {
               return -1;
           }
           Cmd.CommandText = sql;
           return Excutor.ExecuteNonQuery(Cmd, 60);
       }

       protected T MakeHistoryObject<T>(object source, string etName)
       {

           Type t1 = source.GetType();
           Type t2 = typeof(T);

           List<PropertyInfo> t1_list = new List<PropertyInfo>();
           t1_list.AddRange(t1.GetProperties());

           T his =(T) Activator.CreateInstance(t2, true);
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
                   string v =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
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



       protected int InsertHistory<T>(Object obj, string etName)
       {
           var his = MakeHistoryObject<T>(obj, etName);
           return InsertToTable(his);
       }

       protected T[] ExtQueryBySql<T>(string s, object[] param)
       {
           string sql = String.Format(s, param);
           Cmd.CommandText = sql;

           var dt = Excutor.ExecuteSqlGetDt(Cmd, 1000);

           return SqlUtil.DataTableToObjectCollections<T>(dt);

       }


       protected int ExtNoQueryBysql(string sql)
       {
           Cmd.CommandText = sql;
         return  Excutor.ExecuteNonQuery(Cmd);
       }

       protected int ExtNoQueryBysql(string s, Object[] param)
       {
           string sql = String.Format(s, param);
           Cmd.CommandText = sql;
           return Excutor.ExecuteNonQuery(Cmd);
       }

       protected T[] FindAllByKey<T>(Dictionary<string, object> keys, object FromTime, object ToTime, IList<string> orderBy, bool byAsc)
       {


           return null;

       }

       protected void HistoryDailyClean<T>(T[] obs)
       {
           string table = typeof(T).ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();




       }


    }
}
