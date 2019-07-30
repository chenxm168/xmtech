using System;
using System.Xml;
using System.Collections.Generic;
using System.Net;
using System.IO;
using log4net;

namespace TIBMessageIo
{
    public class MessageInfo
    {
        /// <summary>
        /// TIBrv Connection Info : Service, Network, Daemon, Subject
        /// </summary>

        #region compile access production/dev server according to parameter (DEVMODE )
        /// <summary>
        /// PRD Server
        /// </summary>
        private const string CONFIG_XML = "config.xml";
        /// <summary>
        /// Dev Server
        /// </summary>
        private const string CONFIG_XML_DEV = "configDEV.xml";
        /// <summary>
        /// Test Server 
        /// </summary>
        private const string CONFIG_XML_TEST = "configTEST.xml";

        private string configXml = "config.xml";
        #endregion

        ILog logger = LogManager.GetLogger(typeof(MessageInfo));

        private string service;
        public virtual string Service
        {
            get { return service; }
            set { service = value; }
        }

        private string network;
        public virtual string Network
        {
            get { return network; }
            set { network = value; }
        }

        private string daemon;
        public virtual string Daemon
        {
            get { return daemon; }
            set { daemon = value; }
        }
        private List<string> daemonlist = new List<string>();
        public virtual List<string> DaemonList
        {
            get { return daemonlist; }
        }

        private string sourcesubject;
        public virtual string SourceSubject
        {
            get { return sourcesubject; }
            set { sourcesubject = value; }
        }

        private List<string> listensubjectlist = new List<string>();
        public virtual List<string> ListenSubjectList
        {
            get { return listensubjectlist; }
        }

        private string targetsubject;
        public virtual string TargetSubject
        {
            get { return targetsubject; }
            set { targetsubject = value; }
        }

        private string fieldname;
        public virtual string FieldName
        {
            get { return fieldname; }
            set { fieldname = value; }
        }

        private string timeout;
        public virtual string TimeOut
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private string ownsubject;
        public virtual string OwnSubject
        {
            get { return ownsubject; }
            set { ownsubject = value; }
        }

        //mwahn start
        private string querytargetsubject;
        public virtual string QueryTargetSubject
        {
            get { return querytargetsubject; }
            set { querytargetsubject = value; }
        }

        private string queryService;
        public virtual string QueryService
        {
            get { return queryService; }
            set { queryService = value; }
        }


        private string queryNetwork;
        public virtual string QueryNetwork
        {
            get { return queryNetwork; }
            set { queryNetwork = value; }
        }

        private string manual;
        public virtual string Manual
        {
            get { return manual; }
            set { manual = value; }
        }
        //mwahn end

        #region FMC
        private string fmcservice;
        public virtual string FMCService
        {
            get { return fmcservice; }
            set { fmcservice = value; }
        }

        private string fmcnetwork;
        public virtual string FMCNetwork
        {
            get { return fmcnetwork; }
            set { fmcnetwork = value; }
        }

        private string fmclistensubjectname;
        public virtual string FMCListenSubJectName
        {
            get { return fmclistensubjectname; }
            set { fmclistensubjectname = value; }
        }
        #endregion

        private List<string> unableDaemonList = new List<string>();
        public virtual List<string> UnableDaemonList
        {
            get { return unableDaemonList; }
        }

        public MessageInfo()
   
        {
           /*
#if DEVMODE
            configXml = CONFIG_XML_DEV;
#elif TESTMODE
			configXml = CONFIG_XML_TEST;
#else
            configXml = CONFIG_XML;
#endif
           */
        }

        public MessageInfo(string tibXml,string field)
        {
            InitialzeConfig(tibXml, field);
        }

        #region AppLogApplication
        private string servierIp;
        public virtual string ServiceIp
        {
            get { return servierIp; }
            set { servierIp = value; }
        }
        private string path;
        public virtual string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string userName;
        public virtual string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string passWord;
        public virtual string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        #endregion


        public void InitialzeConfig()
        {
            InitialzeConfig(@"tibrv.xml", "configuration");
        
        }
        //--------------------------------------------------------------------
        public XmlNode getXmlNode(string configXml,string version)
        {

            XmlDocument xDoc = new XmlDocument();

          
                //xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + configXml);
                //xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "configTEST.xml");
            if(!File.Exists(configXml))
            {
                return  null;
            }

              xDoc.Load(configXml);

                XmlNodeList xnList = xDoc.SelectNodes("//ConnectionInfo");

                foreach (XmlNode xn in xnList)
                {
                    string ConnectionInfoID = xn.SelectSingleNode("ConnectionInfoID").InnerText;
                    if (ConnectionInfoID == version)
                    {
                        return xn;
                    }
            

                }

                return null;

        }

        //---------------------------------------------------------------------
        public void InitialzeConfig(string strFilePath,string Target)
        {

            if(!File.Exists(strFilePath))
            {
                return;
            }
            XmlDocument xDoc = new XmlDocument();

            try
            {

                xDoc.Load(strFilePath);

                XmlNode xn = xDoc.SelectSingleNode(Target);


                if (xn == null)
                {

                    return;
                }



                Service = xn.SelectSingleNode("tibrvProxy/service").InnerText.ToString();
                Network = xn.SelectSingleNode("tibrvProxy/network").InnerText.ToString();

                Daemon = xn.SelectSingleNode("tibrvProxy/daemon").InnerText.ToString();

                SourceSubject = xn.SelectSingleNode("tibrvProxy/sourceSubject").InnerText.ToString();
                TargetSubject = xn.SelectSingleNode("tibrvProxy/targetSubject").InnerText.ToString();
                FieldName = xn.SelectSingleNode("tibrvProxy/fieldName").InnerText.ToString();
                try
                {
                    TimeOut = xn.SelectSingleNode("tibrvProxy/timeout").InnerText.ToString();
                }catch (Exception e )
                {
                    TimeOut = "20";
                    //logger.Error(e.StackTrace);
                }

                daemonlist.Clear();
                XmlNode DaemonNodeList = xn.SelectSingleNode("daemonList");
                if (DaemonNodeList != null)
                {
                    foreach (XmlNode daemonNode in DaemonNodeList.ChildNodes)
                    {
                        daemonlist.Insert(daemonlist.Count, daemonNode.InnerText.ToString());
                    }
                }
                else
                {
                    //throw new Exception("No daemon-list in config file.");
                }

                //Random ran = new Random();
                //int n = ran.Next(daemonlist.Count);
                //this.daemon = daemonlist[n];
                


                ListenSubjectList.Clear();
                listensubjectlist.Add(SourceSubject);
                //XmlNode listenSubjectNodeList = xn.SelectSingleNode("ListenSubjectList");
                //if (listenSubjectNodeList != null)
                //{
                //    foreach (XmlNode subjectNode in listenSubjectNodeList.ChildNodes)
                //    {
                //        ListenSubjectList.Insert(ListenSubjectList.Count, subjectNode.InnerText.ToString());
                //    }
                //}

                //OwnSubject = (xn.SelectSingleNode("OwnSubject")).InnerText.ToString();
             
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public string resetDeamon(string currentDeamon)
        {
            string result = string.Empty;

            foreach (string deamon in this.DaemonList)
            {
                if (currentDeamon.Equals(deamon))
                {
                    continue;
                }
                else
                {
                    result = deamon;

                    foreach (string unableDeamon in this.unableDaemonList)
                    {
                        if (deamon.Equals(unableDeamon))
                        {
                            result = "";
                            break;
                            //continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        break;
                    }
                }
            }

            this.daemon = result;
            return result;
        }

        public string resetDeamon()
        {
            unableDaemonList.Add(Daemon);
            if (daemonlist!=null&&daemonlist.Count>0)
            {
            while(true)
            { 
            Random ran = new Random();
            int n = ran.Next(daemonlist.Count);
                foreach(string s in unableDaemonList)
                {
                    if (s.Trim().Equals(DaemonList[n].Trim()))
                    {
                        DaemonList.RemoveAt(n);
                        if (DaemonList.Count < 1)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        Daemon = DaemonList[n].Trim();
                        return Daemon;
                    }
                }
            }
            }else
            {
                return null;
            }


        }
                    
    }
}
