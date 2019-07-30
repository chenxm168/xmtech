using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace HF.DB.FirebirdDB
{
   public class FirebirdConnectionString
    {
       private FbConnectionStringBuilder cs = null;
       private FbServerType _serverType = FbServerType.Default;

        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string pwd;

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string dbPath;

        public string DbPath
        {
            get { return dbPath; }
            set { dbPath = value; }
        }

        private string dbServer;

        public string DbServer
        {
            get { return dbServer; }
            set { dbServer = value; }
        }

        private string dbChartSet;

        public string DbChartSet
        {
            get { return dbChartSet; }
            set { dbChartSet = value; }
        }

        private int servicePort;

        public int  ServicePort
        {
            get { return servicePort; }
            set { servicePort = value; }
        }

        private int  dialect;

        public int Dialect
        {
            get { return dialect; }
            set { dialect = value; }
        }

        private string dbRole;

        public string DbRole
        {
            get { return dbRole; }
            set { dbRole = value; }
        }

        private int connectionLifeTime;

        public int  ConnectionLifeTime
        {
            get { return connectionLifeTime; }
            set { connectionLifeTime = value; }
        }

        private bool dbPooling;

        public bool DbPooling
        {
            get { return dbPooling; }
            set { dbPooling = value; }
        }

        private int minPoolSize;

        public int MinPoolSize
        {
            get { return minPoolSize; }
            set { minPoolSize = value; }
        }

        private int maxPoolSize;

        public int MaxPoolSize
        {
            get { return maxPoolSize; }
            set { maxPoolSize = value; }
        }

        private int packetSize;

        public int PacketSize
        {
            get { return packetSize; }
            set { packetSize = value; }
        }

        private string serverType;

        public string ServerType
        {
            get { return serverType; }
            set { serverType = value; }
        }

        private string clientLibrary;

        public string ClientLibrary
        {
            get { return clientLibrary; }
            set { clientLibrary = value; }
        }

       public string GetConnetionString()
        {
           if(serverType==null||serverType==""||serverType.ToUpper().Trim()=="SERVER")
           {
               _serverType = FbServerType.Default;

           }else
           {
               _serverType = FbServerType.Embedded;
           }
           if(pwd==null||pwd.Length<1)
           {
               pwd="masterkey";
           }



           if(cs==null)
           {
               cs = new FbConnectionStringBuilder();
           }

           if (dbRole == null || dbRole.Length < 1)
           {
               cs.Role = "";
           }
           cs.UserID = userID;

           cs.Password = pwd;
           if (_serverType == FbServerType.Embedded)
           {
               cs.ServerType = _serverType;
               cs.ClientLibrary = clientLibrary;
           }else
           {
           cs.Pooling = dbPooling;
           cs.DataSource = dbServer;
           cs.Port = servicePort;
           cs.Dialect = dialect;
           cs.Charset = dbChartSet;
           cs.MinPoolSize = minPoolSize;
           cs.MinPoolSize = maxPoolSize;
           cs.ConnectionLifeTime = connectionLifeTime;
           cs.Database = dbPath;
           cs.PacketSize = packetSize;
           }
           return cs.ToString();
        }

    }
}
