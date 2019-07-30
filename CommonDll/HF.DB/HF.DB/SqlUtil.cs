using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using log4net;
using System.IO;

namespace HF.DB
{
    public class SqlUtil
    {
        static ILog logger = LogManager.GetLogger(typeof(SqlUtil));
        public static string MakeQuerySql<T>(Dictionary<string, object> parameter,IList<string> orderBy,bool byAcs)
        {
            string sql = "SELECT ";

            string table = typeof(T).ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();

            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            //foreach( var pi in pis)
            //{
            //    string tem = " ," + pi.Name.ToUpper();
            //    sql += tem;
            //}
            for (int i = 0; i < pis.Length; i++)
            {
                var pi = pis[i];
                string tem = "";
                if (i != 0)
                {
                    tem += " , " + pi.Name.ToUpper();
                }
                else
                {
                    tem += " " + pi.Name.ToUpper();
                }
                sql += tem;
            }

            sql += " FROM " + table;
            if (parameter == null)
            {
                return sql;
            }
            sql += " WHERE ";


            int c = 0;
            foreach (var k in parameter.Keys)
            {
                string tem = "";
                if (c != 0)
                { tem += " AND "; }
                tem += " " + k.ToUpper() + "=";
                object v = parameter[k];
                if (v is string)
                {
                    tem += "'" + v.ToString() + "'";
                    sql += tem;
                    c++;
                    continue;
                }
                if (v is DateTime)
                {
                    tem += "'" + ((DateTime)v).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                    sql += tem;
                    c++;
                    continue;
                }
                if (v is bool)
                {
                    tem += "'" + v.ToString() + "'";
                    sql += tem;
                    c++;
                    continue;
                }
                if (IsNumberic(v))
                {
                    tem += v;
                    sql += tem;
                    c++;
                    continue;
                }

            }

            if(orderBy!=null)
            {
                string tem = " ";
            for (int i = 0; i < orderBy.Count;i++ )
            {
                
                if(i==0)
                {
                    tem += "ORDER BY ";
                }
                else
                {
                    tem += " , ";
                }
                tem += orderBy[i];
            }
            sql += tem;
                if(byAcs)
                {
                    sql += "ASC ";
                }else
                {
                    sql += " DESC ";
                }
            }

                return sql;
        }

        public static  string MakeInsertSql(Object obj)
        {

            if (obj == null)
            {
                return null;
            }
            Type t = obj.GetType();
            string table = t.ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            string sql = "INSERT INTO " + table + " ";

            
            PropertyInfo[] pis = t.GetProperties();

            string c = " ( ";
            string v = " VALUES ( ";
            int count = 0;
            foreach (PropertyInfo pi in pis)
            {
                Object o = pi.GetValue(obj, null);
                if (o != null)
                {
                    if (count != 0)
                    {
                        c += " , ";
                        v += " , ";
                    }
                    c += pi.Name.ToUpper();
                    v += toSqlString(o);
                    count++;
                    continue;
                }
            }

            if (count == 0)
            {
                return null;
            }
            c += " ) ";
            v += " ) ";

            sql = sql + c + v;

            return sql;

        }


        public static string MakeInsertSql<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            string table = typeof(T).ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            string sql = "INSERT INTO " + table + " ";

            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();

            string c = " ( ";
            string v = " VALUES ( ";
            int count = 0;
            foreach (PropertyInfo pi in pis)
            {
                Object o = pi.GetValue(obj, null);
                if (o != null)
                {
                    if (count != 0)
                    {
                        c += " , ";
                        v += " , ";
                    }
                    c += pi.Name.ToUpper();
                    v += toSqlString(o);
                    count++;
                    continue;
                }
            }

            if (count == 0)
            {
                return null;
            }
            c += " ) ";
            v += " ) ";

            sql = sql + c + v;

            return sql;

        }

        public static string MakeUpdateSql(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            Type t = obj.GetType();
            string table = t.ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            var cvs = new Dictionary<string, object>();
            var keys = new Dictionary<string, object>();
            
            PropertyInfo[] pis = t.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                Object o = pi.GetValue(obj, null);
                if (o == null)
                {
                    var attr = pi.GetCustomAttribute<ColummAttribute>();
                    if (attr != null)
                    {
                        if (attr.PrimaryKey)
                        {
                            return null;
                        }
                    }
                    continue;
                }

                cvs.Add(pi.Name.ToUpper(), o);

                var at = pi.GetCustomAttribute<ColummAttribute>();
                if (at != null && at.PrimaryKey == true)
                {
                    keys.Add(pi.Name.ToUpper(), o);
                }
            }

            int cvCnt = 0;
            string setString = " ";
            foreach (var k in cvs.Keys)
            {
                if (cvCnt != 0)
                {
                    setString += " , ";
                }
                else
                {
                    setString += " SET ";
                }

                setString += string.Format(" {0} = {1}", k.ToUpper(), toSqlString(cvs[k]));
                cvCnt++;
            }

            int keyCnt = 0;
            string keyString = " ";
            foreach (var k in keys.Keys)
            {
                if (keyCnt == 0)
                {
                    keyString += " WHERE ";
                }
                else
                {
                    keyString += " AND ";
                }

                keyString += string.Format(" {0} = {1}", k.ToUpper(), toSqlString(cvs[k]));
                cvCnt++;
            }

            string sql = string.Format("UPDATE  {0} {1} {2} ", new object[] { table, setString, keyString });
            return sql;
        }



        public static string MakeUpdateSql<T>(T obj)
        {
            if(obj==null)
            {
                return null;
            }
            string table = typeof(T).ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            var cvs = new Dictionary<string, object>();
            var keys = new Dictionary<string, object>();
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                Object o = pi.GetValue(obj, null);
                if (o == null)
                {
                    var attr = pi.GetCustomAttribute<ColummAttribute>();
                    if(attr!=null)
                    {
                        if( attr.PrimaryKey)
                        {
                            return null;
                        }
                    }
                    continue;
                 }

                cvs.Add(pi.Name.ToUpper(), o);

                var at = pi.GetCustomAttribute<ColummAttribute>();
                if (at != null&&at.PrimaryKey==true)
                {
                    keys.Add(pi.Name.ToUpper(), o);
                }
            }

            int cvCnt = 0;
            string setString = " ";
            foreach(var k in cvs.Keys)
            {
                if(cvCnt!=0)
                {
                    setString += " , ";
                }else
                {
                    setString+= " SET ";
                }
               
                setString+= string.Format(" {0} = {1}",k.ToUpper(),toSqlString(cvs[k]));
                cvCnt++;
            }

            int keyCnt = 0;
            string keyString = " ";
            foreach(var k in keys.Keys)
            {
                if(keyCnt==0)
                {
                    keyString += " WHERE ";
                }else
                {
                    keyString += " AND ";
                }

                keyString += string.Format(" {0} = {1}", k.ToUpper(), toSqlString(keys[k]));
                keyCnt++;
            }

            string sql = string.Format("UPDATE  {0} {1} {2} ", new object[] { table, setString, keyString });
            return sql;
        }

        //private static string getTableName<T>()
        //{

        //}


        public static string toSqlString(object v)
        {
            if (IsNumberic(v))
            {
                return v.ToString();
            }
            if (v is bool | v is string)
            {
                return "'" + v.ToString() + "'";
            }
            if (v is DateTime)
            {
                return "'" + ((DateTime)v).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }

            return null;
        }


        private static bool IsNumberic(object v)
        {
            var s = v.ToString();
            try
            {
                int var1 = Convert.ToInt32(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Object ColValueToObject(PropertyInfo pi,object obj)
        {
            if(pi.PropertyType == typeof(String))
            {
                if(obj is DateTime)
                {
                    return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:sss.fff");
                }
                return obj.ToString();
            }

            if (pi.PropertyType ==typeof( DateTime))
            {
                if(obj is DateTime)
                {
                    return obj;
                }
                try { 
                var rtn = DateTime.Parse(obj.ToString());
                    }
                catch(Exception e)
                {
                    logger.Error(e.Message);
                    return null;
                }

            }
            if(pi.PropertyType==typeof(Boolean))
            {
                try
                {
                    return Convert.ToBoolean(obj.ToString());
                }catch(Exception e)
                {
                    logger.Error(e.Message);
                    return false;
                }
            }

            if(IsNumberic(obj))
            {
                return Convert.ToInt32(obj.ToString());
            }

            return null;
        }



        public static string MakeDelteSql(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            Type t = obj.GetType();
            string table = t.ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            var keys = new Dictionary<string, object>();
            PropertyInfo[] pis = t.GetProperties();
            foreach( var pi in pis)
            {
                var attr = pi.GetCustomAttribute<ColummAttribute>();
                if(attr==null)
                {
                    continue;
                }
                if(!attr.PrimaryKey)
                {
                    continue;
                }
                object o = pi.GetValue(obj, null);
                if(o!=null)
                {
                    keys.Add(pi.Name, o);
                }else
                {
                    logger.ErrorFormat("Key[{0}] Value is Null", pi.Name.ToUpper());
                    return null;
                }
            }

            int kCnt = 0;
            string kString = " ";
            foreach(string  k in keys.Keys)
            {
                string tem = "";
                if(kCnt==0)
                {
                    tem += "WHERE ";
                }else
                {
                    tem += " AND ";
                }
                var tem2 = string.Format(" {0} = {1}", k, toSqlString(keys[k]));
                tem+=tem2;
                kString+=tem;
            }
            return string.Format("DELETE FROM {0} {1} ", table, kString);
        }

         public static string MakeHistoryCleanSql<T>(int remainDay)
        {

            Type t = typeof(T);
            string table = t.ToString().Split('.').LastOrDefault<string>().ToString().ToUpper();
            DateTime date = DateTime.Now.AddDays((0 - remainDay));
             //TODO
            return null;
        }

        public static T[] DataTableToObjectCollections<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
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


        public T[] GetCsvDocData<T>(string file)
        {
            FileStream fs = null;
            StreamReader sr = null;
            if (file == null || file.Trim().Length < 1)
            {
                file = "AlarmSpec.csv";
            }

            Dictionary<int, string> colmap = new Dictionary<int, string>();

            if(File.Exists(file))
            {
                try
                {
                    fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs, Encoding.UTF8);
                    bool firsLine = true;
                    string strLine = "";
                    List<T> datas = new List<T>();
                    Type t = typeof(T);
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if(firsLine)
                        {
                            strLine = strLine.Trim().ToUpper();
                            string[] cols = strLine.Split(',');
                            for(int i=0;i<cols.Length;i++)
                            {
                                colmap.Add(i,cols[i]);

                            }

                        }else
                        {

                            PropertyInfo[] pis = t.GetProperties();
                            var obj = Activator.CreateInstance(t, true);
                            strLine = strLine.Trim();
                            //TODO

                        }


                    }



                }catch(Exception e)
                {

                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                   
                }

            }




            return null;
        }

    }
}
