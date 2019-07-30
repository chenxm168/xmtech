using System;
using System.Xml;
using System.Collections.Generic;
using System.Net;

namespace Common.TIB
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

         /*   XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + this.configXml);
                //xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "configTEST.xml");
              
                Service = xn.SelectNodes("ConnectionInfo/Service")[0].InnerText.ToString();
        
                Network = xn.SelectNodes("ConnectionInfo/Network")[0].InnerText.ToString();
    
                Service = xDoc.SelectNodes("ConnectionInfo/Service")[0].InnerText.ToString();
                Network = xDoc.SelectNodes("ConnectionInfo/Network")[0].InnerText.ToString();

                //mwahn start
                QueryService = xDoc.SelectNodes("ConnectionInfo/QueryService")[0].InnerText.ToString();
                QueryNetwork = xDoc.SelectNodes("ConnectionInfo/QueryNetwork")[0].InnerText.ToString();
                QueryTargetSubject = xDoc.SelectNodes("ConnectionInfo/QueryTargetSubject")[0].InnerText.ToString();
                //mwahn end

                #region FMC
                FMCService = xDoc.SelectNodes("ConnectionInfo/FMCService")[0].InnerText.ToString();
                FMCNetwork = xDoc.SelectNodes("ConnectionInfo/FMCNetwork")[0].InnerText.ToString();
                FMCListenSubJectName = xDoc.SelectNodes("ConnectionInfo/FMCListenSubJectName")[0].InnerText.ToString();
                #endregion

                daemonlist.Clear();
                XmlNode DaemonNodeList = xDoc.SelectSingleNode("ConnectionInfo/DaemonList");
                if (DaemonNodeList != null)
                {
                    foreach (XmlNode daemonNode in DaemonNodeList.ChildNodes)
                    {
                        daemonlist.Insert(daemonlist.Count, daemonNode.InnerText.ToString());
                    }
                }
                else
                {
                    throw new Exception("No daemon-list in config file.");
                }

                IPAddress[] address = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (address == null || address.Length <= 0)
                {
                    this.daemon = daemonlist[0];
                }
                else
                {
                    foreach (IPAddress addr in address)
                    {
                        bool found = false;
                        string ip = addr.ToString();
                        string[] ips = null;
                        if (!ip.Contains(".") || (ips = ip.Split('.')) == null || ips.Length <= 0)
                        {
                            continue;
                        }

                        for (int i = 0; i < daemonlist.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(daemonlist[i]))
                            {
                                string endIPDaemon = daemonlist[i].Substring(daemonlist[i].Length - 6, 1);

                                int endIPNo = 0;
                                int ipNo = 0;
                                if (!int.TryParse(endIPDaemon, out endIPNo) || endIPNo <= 0 ||
                                    !int.TryParse(ips[ips.Length - 1], out ipNo) || ipNo <= 0)
                                {
                                    continue;
                                }

                                if (int.Parse(endIPDaemon) % daemonlist.Count == int.Parse(ips[ips.Length - 1]) % daemonlist.Count)
                                {
                                    this.daemon = daemonlist[i];
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (found)
                        {
                            break;
                        }
                        else
                        {
                            this.daemon = daemonlist[0];
                        }
                    }
                }

                //SourceSubject = xDoc.SelectNodes("ConnectionInfo/SourceSubject")[0].InnerText.ToString();
                TargetSubject = xDoc.SelectNodes("ConnectionInfo/TargetSubject")[0].InnerText.ToString();
                FieldName = xDoc.SelectNodes("ConnectionInfo/FieldName")[0].InnerText.ToString();
                TimeOut = xDoc.SelectNodes("ConnectionInfo/TimeOut")[0].InnerText.ToString();

                ListenSubjectList.Clear();
                XmlNode listenSubjectNodeList = xDoc.SelectSingleNode("ConnectionInfo/ListenSubjectList");
                if (listenSubjectNodeList != null)
                {
                    foreach (XmlNode subjectNode in listenSubjectNodeList.ChildNodes)
                    {
                        ListenSubjectList.Insert(ListenSubjectList.Count, subjectNode.InnerText.ToString());
                    }
                }

                //OwnSubject = string.Format("OIC.{0}", ip);
                OwnSubject = xDoc.SelectNodes("ConnectionInfo/OwnSubject")[0].InnerText.ToString();

                //ConnectionInfo.LOG_DIR = xDoc.SelectNodes("ConnectionInfo/FieldName")[0].InnerText.ToString();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }  */
        }
        //--------------------------------------------------------------------
        public XmlNode getXmlNode(string configXml,string version)
        {

            XmlDocument xDoc = new XmlDocument();

          
                xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + configXml);
                //xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "configTEST.xml");

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

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + strFilePath);
                //xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "configTEST.xml");
                XmlNode xn = this.getXmlNode(strFilePath,Target);

                if (xn == null)
                {
                    xn = this.getXmlNode(strFilePath, "DEFAULT");
                }

             //   XmlNode xn = this.getXmlNode(Target);

               
                Service = (xn.SelectSingleNode("Service")).InnerText.ToString();
                Network = (xn.SelectSingleNode("Network")).InnerText.ToString();
                try
                {
                    Manual = (xn.SelectSingleNode("Manual")).InnerText.ToString();
                }
                catch (Exception e) { }
             
              //  Service = xDoc.SelectNodes("ConnectionInfo/Service")[0].InnerText.ToString();
               // Network = xDoc.SelectNodes("ConnectionInfo/Network")[0].InnerText.ToString();
                Daemon = string.Empty;


                //mwahn start
                QueryService = (xn.SelectSingleNode("QueryService")).InnerText.ToString();
                QueryNetwork = (xn.SelectSingleNode("QueryNetwork")).InnerText.ToString();
                QueryTargetSubject = (xn.SelectSingleNode("QueryTargetSubject")).InnerText.ToString();
                ServiceIp = (xn.SelectSingleNode("AppLogServiceIp")).InnerText.ToString();
                path = (xn.SelectSingleNode("AppLogPath")).InnerText.ToString();
                userName = (xn.SelectSingleNode("AppLogUserName")).InnerText.ToString();
                passWord = (xn.SelectSingleNode("AppLogPassWord")).InnerText.ToString();
              //  QueryService = xDoc.SelectNodes("ConnectionInfo/QueryService")[0].InnerText.ToString();
              //  QueryNetwork = xDoc.SelectNodes("ConnectionInfo/QueryNetwork")[0].InnerText.ToString();
              //  QueryTargetSubject = xDoc.SelectNodes("ConnectionInfo/QueryTargetSubject")[0].InnerText.ToString();
                //mwahn end

             /*   #region FMC

                FMCService = xDoc.SelectNodes("ConnectionInfo/FMCService")[0].InnerText.ToString();
                FMCNetwork = xDoc.SelectNodes("ConnectionInfo/FMCNetwork")[0].InnerText.ToString();
                FMCListenSubJectName = xDoc.SelectNodes("ConnectionInfo/FMCListenSubJectName")[0].InnerText.ToString();
                #endregion
                */
                daemonlist.Clear();
                XmlNode DaemonNodeList = xn.SelectSingleNode("DaemonList");
               // XmlNode DaemonNodeList = xDoc.SelectSingleNode("ConnectionInfo/DaemonList");
                if (DaemonNodeList != null)
                {
                    foreach (XmlNode daemonNode in DaemonNodeList.ChildNodes)
                    {
                        daemonlist.Insert(daemonlist.Count, daemonNode.InnerText.ToString());
                    }
                }
                else
                {
                    throw new Exception("No daemon-list in config file.");
                }

                //Get Random Deamon From Deamon List
                Random ran = new Random();
                int n = ran.Next(daemonlist.Count);
                this.daemon = daemonlist[n];
                //this.daemon = daemonlist[0];

                //IPAddress[] address = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                //if (address == null || address.Length <= 0)
                //{
                //    this.daemon = daemonlist[0];
                //}
                //else
                //{
                //    foreach (IPAddress addr in address)
                //    {
                //        bool found = false;
                //        string ip = addr.ToString();
                //        string[] ips = null;
                //        if (!ip.Contains(".") || (ips = ip.Split('.')) == null || ips.Length <= 0)
                //        {
                //            continue;
                //        }

                //        for (int i = 0; i < daemonlist.Count; i++)
                //        {
                //            if (!String.IsNullOrEmpty(daemonlist[i]))
                //            {
                //                string endIPDaemon = daemonlist[i].Substring(daemonlist[i].Length - 6, 1);

                //                int endIPNo = 0;
                //                int ipNo = 0;
                //                if (!int.TryParse(endIPDaemon, out endIPNo) || endIPNo <= 0 ||
                //                    !int.TryParse(ips[ips.Length - 1], out ipNo) || ipNo <= 0)
                //                {
                //                    continue;
                //                }

                //                if (int.Parse(endIPDaemon) % daemonlist.Count == int.Parse(ips[ips.Length - 1]) % daemonlist.Count)
                //                {
                //                    this.daemon = daemonlist[i];
                //                    found = true;
                //                    break;
                //                }
                //            }
                //        }

                //        if (found)
                //        {
                //            break;
                //        }
                //        else
                //        {
                //            this.daemon = daemonlist[0];
                //        }
                //    }
                //}

                //SourceSubject = xDoc.SelectNodes("ConnectionInfo/SourceSubject")[0].InnerText.ToString();

                TargetSubject = (xn.SelectSingleNode("TargetSubject")).InnerText.ToString();
                FieldName = (xn.SelectSingleNode("FieldName")).InnerText.ToString();
                TimeOut = (xn.SelectSingleNode("TimeOut")).InnerText.ToString();

                //TargetSubject = xDoc.SelectNodes("ConnectionInfo/TargetSubject")[0].InnerText.ToString();
              //  FieldName = xDoc.SelectNodes("ConnectionInfo/FieldName")[0].InnerText.ToString();
               // TimeOut = xDoc.SelectNodes("ConnectionInfo/TimeOut")[0].InnerText.ToString();

                ListenSubjectList.Clear();
                XmlNode listenSubjectNodeList = xn.SelectSingleNode("ListenSubjectList");
             //   XmlNode listenSubjectNodeList = xDoc.SelectSingleNode("ConnectionInfo/ListenSubjectList");
                if (listenSubjectNodeList != null)
                {
                    foreach (XmlNode subjectNode in listenSubjectNodeList.ChildNodes)
                    {
                        ListenSubjectList.Insert(ListenSubjectList.Count, subjectNode.InnerText.ToString());
                    }
                }

                //OwnSubject = string.Format("OIC.{0}", ip);
                OwnSubject = (xn.SelectSingleNode("OwnSubject")).InnerText.ToString();
             
                //OwnSubject = xDoc.SelectNodes("ConnectionInfo/OwnSubject")[0].InnerText.ToString();

                //ConnectionInfo.LOG_DIR = xDoc.SelectNodes("ConnectionInfo/FieldName")[0].InnerText.ToString();
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
    }
}
