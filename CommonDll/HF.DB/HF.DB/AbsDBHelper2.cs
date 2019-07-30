using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using log4net;
using System.Data;
using System.Data.Common;

namespace HF.DB
{
    public abstract class AbsDBHelper2 : IDisposable, ISQLExcutable2
    {
        protected ILog logger = LogManager.GetLogger(typeof(AbsDBHelper2));

        public DbTransaction Trs
        {
            get;
            set;
        }

        public DbConnection Conn
        {
            get;
            set;
        }

        public DbCommand Cmd
        {
            get;
            set;
        }
        protected DataTable ExtSql(IDbCommand cmd)
        {
            DataTable dt = new DataTable();
            var reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        protected int ExtNoQuery(IDbCommand cmd)
        {
            logger.DebugFormat("Pre Excute sql [{0}]", cmd.CommandText);
            return cmd.ExecuteNonQuery();
        }


        public void Dispose()
        {
            Trs.Dispose();
            CloseConnection();
            Conn.Dispose();
        }

        public void OpenConnection()
        {
            if (Conn != null && Conn.State != ConnectionState.Open)
            {
                try
                {
                    Conn.Open();
                    logger.Debug("DB connection Open.");
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }

            }
        }

        public void CloseConnection()
        {
            if (Conn != null && Conn.State != ConnectionState.Closed)
            {
                try
                {
                    Conn.Close();
                    logger.Debug("DB connection Close.");
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }

            }
        }

        public void BeginTransaction()
        {
            //Conn.Open();
            OpenConnection();
            if(Trs==null)
            { 
            Trs = Conn.BeginTransaction();
            }
            if(Cmd==null)
            { 
            Cmd = Conn.CreateCommand();
            }
            //if(Cmd.Transaction==null)
            //{ 
            //Cmd.Transaction = Trs;
            //}
            Trs = Conn.BeginTransaction();
            Cmd.Transaction = Trs;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandTimeout = 60;
        }

        public void CommitTransaction()
        {
            if (Trs != null)
            {
                Trs.Commit();
            }
            //if(Cmd!=null)
            //{ 
            //Cmd.Dispose();
            //}
            //Trs.Dispose();
        }




        public abstract System.Data.DataTable ExecuteSqlGetDt(string sql, int topRows);


        public abstract int ExecuteNonQuery(string sql, int timeOut);



        public void RollbackTransaction()
        {
            if(Trs==null)
            {
                return;
            }
            try
            { 
            Trs.Rollback();
                }catch(Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }

}