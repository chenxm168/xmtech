using System;
using System.Collections.Generic;
using System.Net;
using TIBCO.Rendezvous;
using System.Xml;
using Common.TIB;


namespace Common.TIB
{
    public class MessageService : IMessage
    {
        MessageInfo messageInfo = new MessageInfo();


        private Dispatcher dispatcher = null;
        private TIBCO.Rendezvous.Queue queue = null;

        // Create Network transport
        public static Transport transport;
        public static Transport fmbtransport;
        private Dictionary<string, Listener> listenerList;
        private Dictionary<string, Transport> transportList = new Dictionary<string, Transport>();
        //private List<Transport> transportList;
        private Listener ownListener;
        private Listener fmbListener;
        private bool isRun = false;

        public delegate void EventHandlerForTIBMessageReceived(string subjectName, object listener, string message);
        public event EventHandlerForTIBMessageReceived OwnListenerReceived;
        public event EventHandlerForTIBMessageReceived ListenerReceived;
        public event EventHandlerForTIBMessageReceived FMCListenerReceived;

        // JXM 
        public event EventHandlerForTIBMessageReceived ALARMTEST;
        private Listener alarmListener;

        string OwnSubjectName = "";
        string FMBListenSubjectName = "";

        //mwahn start
        public static Transport querytransport;
        private Listener queryListener;
        public event EventHandlerForTIBMessageReceived QueryListenerReceived;
        string QueryListenSubjectName = "";
        string GetQueryResult = "GetQueryResult";
        public static string Manual = "";
        //mwahn end



        private static MessageService m_This;
        static MessageService()
        {
            m_This = new MessageService();
        }

        public static MessageService This
        {
            get
            {
                return m_This;
            }
        }

        public MessageService()
        {
            messageInfo.InitialzeConfig(Common.TIB.Config.configFile, "DEFAULT");
        }


        public void Initialize()
        {

            transportList = new Dictionary<string, Transport>();
            try
            {
                if (isRun)
                {
                    Terminate();
                }

                openTibEnvironment();
                //createTransport();
                //createQueryTransport();
                //createFMBTransport();

                string oldDaemon = messageInfo.Daemon;

                createTransport(messageInfo.Daemon, messageInfo.Daemon);
                createTransport("QRY", messageInfo.Daemon);
                createTransport("FMC", messageInfo.Daemon);

                transport = transportList[oldDaemon];
                querytransport = transportList["QRY"];
                fmbtransport = transportList["FMC"];
                Manual = messageInfo.Manual;
                
                createQueue();
                createDispatcher();

                createOwnListener();
                createListener();
                //    createFMBListener1("ARRAY", "BAY11");

                isRun = true;
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);

                throw ex;
            }
        }

        public void Initialize(string strFilePath, string Target)
        {
            messageInfo.InitialzeConfig(strFilePath, Target);
            transportList = new Dictionary<string, Transport>();
            try
            {
                if (isRun)
                {
                    Terminate();
                }

                openTibEnvironment();
                //createTransport();
                //createQueryTransport();
                //createFMBTransport();

                string oldDaemon = messageInfo.Daemon;
                createTransport(messageInfo.Daemon, messageInfo.Daemon);
                createTransport("QRY", messageInfo.Daemon);
                createTransport("FMC", messageInfo.Daemon);

                transport = transportList[oldDaemon];
                querytransport = transportList["QRY"];
                fmbtransport = transportList["FMC"];

                createQueue();
                createDispatcher();

                createOwnListener();
                createListener();

                isRun = true;
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        // Tibrv Terminate.
        public void Terminate()
        {
            if (!isRun) return;

            try
            {
                destroyQueue();
                destroyListener();
                destroyTransfort();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
            finally
            {
                closeTibEnvironment();
                isRun = false;
            }
        }

        // Tibrv Terminate.
        public void TerminateQueueAndTransport()
        {
            if (!isRun) return;

            try
            {
                destroyQueue();
                destroyTransfort();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
            finally
            {
                closeTibEnvironment();
                isRun = false;
            }
        }

        // Open Tibrv Environment.
        private void openTibEnvironment()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        private void createTransport(string type, string currentDeamon)
        {
            //string currentDeamon = messageInfo.Daemon;

            try
            {
                if (type.Equals("QRY"))
                {
                    NetTransport transport = new NetTransport(messageInfo.QueryService, messageInfo.QueryNetwork, currentDeamon);
                    transportList.Add(type, transport);

                }
                else if (type.Equals("FMC"))
                {
                    NetTransport transport = new NetTransport(messageInfo.QueryService, messageInfo.QueryNetwork, currentDeamon);
                    transportList.Add(type, transport);
                }
                else
                {
                    NetTransport transport = new NetTransport(messageInfo.Service, messageInfo.Network, currentDeamon);
                    //transportList.Add(messageInfo.Daemon, transport);
                    //transportList.Add(messageInfo.QueryNetwork, querytransport);
                    //transportList.Add("FMC", fmbtransport);
                    transportList.Add(type, transport);
                }
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);

                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    messageInfo.UnableDaemonList.Add(currentDeamon);
                    string newDeamon = messageInfo.resetDeamon(currentDeamon);

                    if (string.IsNullOrEmpty(newDeamon))
                        throw new Exception("No available deamon router");
                    else
                        createTransport(type, newDeamon);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void reconnect(string currentDeamon)
        {
            try
            {
                if (isRun)
                {
                    //purge all communication environment
                    Terminate();
                }

                string newDeamon = messageInfo.resetDeamon(currentDeamon);

                if (string.IsNullOrEmpty(newDeamon))
                    throw new Exception("No available deamon router");

                TIBCO.Rendezvous.Environment.Open();

                transportList.Clear();

                createTransport("WIP", newDeamon);
                createTransport("QRY", newDeamon);
                createTransport("FMC", newDeamon);

                transport = transportList["WIP"];
                querytransport = transportList["QRY"];
                fmbtransport = transportList["FMC"];

                createQueue();
                createDispatcher();
                createOwnListener();
                createListener();

                isRun = true;
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        // Create Transport.
        private void createTransport()
        {
            try
            {
                try
                {
                    transport = new NetTransport(messageInfo.Service, messageInfo.Network, messageInfo.Daemon);
                    transportList.Add(messageInfo.Daemon, transport);
                }
                catch
                {
                    for (int i = 0; i < messageInfo.DaemonList.Count; i++)
                    {
                        try
                        {
                            if (messageInfo.Daemon != messageInfo.DaemonList[i])
                            {
                                transport = new NetTransport(messageInfo.Service, messageInfo.Network, messageInfo.DaemonList[i]);
                                transportList.Add(messageInfo.Daemon, transport);
                                messageInfo.Daemon = messageInfo.DaemonList[i];
                                break;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        // Create QueryTransport. mwahn
        private void createQueryTransport()
        {
            try
            {
                try
                {
                    querytransport = new NetTransport(messageInfo.QueryService, messageInfo.QueryNetwork, messageInfo.Daemon);
                    transportList.Add(messageInfo.QueryNetwork, querytransport);
                }
                catch
                {
                    for (int i = 0; i < messageInfo.DaemonList.Count; i++)
                    {
                        try
                        {
                            if (messageInfo.Daemon != messageInfo.DaemonList[i])
                            {
                                querytransport = new NetTransport(messageInfo.QueryService, messageInfo.QueryNetwork, messageInfo.DaemonList[i]);
                                transportList.Add(messageInfo.QueryNetwork, querytransport);
                                messageInfo.Daemon = messageInfo.DaemonList[i];
                                break;
                            }
                        }
                        catch { }

                    }
                }
            }
            catch (RendezvousException ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        // Create FMBTransport.
        private void createFMBTransport()
        {
            try
            {
                try
                {
                    fmbtransport = new NetTransport(messageInfo.FMCService, messageInfo.FMCNetwork, messageInfo.Daemon);
                    //transportList.Add(messageInfo.FMBNetwork, fmbtransport);
                    transportList.Add("FMC", fmbtransport);
                }
                catch
                {
                    for (int i = 0; i < messageInfo.DaemonList.Count; i++)
                    {
                        try
                        {
                            if (messageInfo.Daemon != messageInfo.DaemonList[i])
                            {
                                fmbtransport = new NetTransport(messageInfo.FMCService, messageInfo.FMCNetwork, messageInfo.DaemonList[i]);
                                transportList.Add(messageInfo.FMCNetwork, fmbtransport);
                                messageInfo.Daemon = messageInfo.DaemonList[i];
                                break;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        // Create Queue.
        private void createQueue()
        {
            try
            {
                queue = new TIBCO.Rendezvous.Queue();
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        // Create Dispatcher.
        private void createDispatcher()
        {
            try
            {
                dispatcher = new Dispatcher(queue);
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        // Create Listener.
        private void createListener()
        {
            listenerList = new Dictionary<string, Listener>();
            foreach (string listenSubjectName in messageInfo.ListenSubjectList)
            {
                Listener listener = new Listener(queue, transport, listenSubjectName, null);
                listener.MessageReceived +=
                    new MessageReceivedEventHandler(OnMessageReceived);
                listenerList.Add(listenSubjectName, listener);
            }
        }

        // Create Listener.
        public void createListener(string listenSubject)
        {
            //listenerList = new Dictionary<string, Listener>();
            //Listener listener = new Listener(queue, transport, listenSubject, null);
            //listener.MessageReceived +=new MessageReceivedEventHandler(OnMessageReceived);
            //listenerList.Add(listenSubject, listener);

            listenerList = new Dictionary<string, Listener>();
            foreach (string listenSubjectName in messageInfo.ListenSubjectList)
            {
                //Listener listener = new Listener(queue, transport, listenSubjectName, null);
                //listener.MessageReceived +=
                //    new MessageReceivedEventHandler(OnMessageReceived);
                //listenerList.Add(listenSubjectName, listener);

                if (listenSubjectName.Contains("OIC"))
                {
                    string tempsubject = listenSubjectName + "." + listenSubject;
                    Listener listener = new Listener(queue, transport, tempsubject, null);
                    listener.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
                    listenerList.Add(tempsubject, listener);
                }
                else
                {
                    Listener listener = new Listener(queue, fmbtransport, listenSubjectName, null);
                    listener.MessageReceived += new MessageReceivedEventHandler(OnFmbMessageReceived);
                    listenerList.Add(listenSubjectName, listener);
                    this.FMBListenSubjectName = listenSubjectName;
                }
            }
        }

        // Create Own Listener.
        public void createOwnListener()
        {
            try
            {
                ownListener = new Listener(queue, transport, messageInfo.OwnSubject, null);
                ownListener.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public void createOwnListener(string userid)
        {
            try
            {
                OwnSubjectName = messageInfo.OwnSubject + "." + userid;
                ownListener = new Listener(queue, transport, OwnSubjectName, null);
                ownListener.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public void createOwnListener(string userid, ref string daemon)
        {
            try
            {
                OwnSubjectName = messageInfo.OwnSubject + "." + userid;
                ownListener = new Listener(queue, transport, OwnSubjectName, null);
                ownListener.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
                daemon = messageInfo.Daemon;
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        #region Create FMB Listener By Shop, Bay
        public void createFMBListener1(string shopid, string bayid)
        {
            try
            {
                FMBListenSubjectName = messageInfo.FMCListenSubJectName;//+ "." + shopid + "." + bayid;
                fmbListener = new Listener(queue, fmbtransport, FMBListenSubjectName, null);
                fmbListener.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        // Destoroy Queue.
        private void destroyQueue()
        {
            if (queue != null)
            {
                queue.Destroy();
            }
        }

        // Destroy Listener.
        public void destroyListener()
        {
            //ownListener.Destroy();
            //foreach (Listener listener in listenerList.Values)
            //    listener.Destroy();
            if (ownListener != null)
            {
                ownListener.Destroy();
            }

            if (fmbListener != null)
            {
                fmbListener.Destroy();
            }

            foreach (Listener listener in listenerList.Values)
            {
                if (listener != null)
                {
                    listener.Destroy();
                }
            }
        }

        #region Destroy FMB Listener
        public void destroyFMBListener1()
        {
            if (fmbListener != null)
            {
                fmbListener.Destroy();
            }
        }
        #endregion

        #region Destroy Own Listener
        public void destroyOwnListener()
        {
            if (ownListener != null)
            {
                ownListener.Destroy();
            }
        }
        #endregion

        #region Destroy Bulletin, FMB Listener
        public void destroyBFListener()
        {
            if (fmbListener != null)
            {
                fmbListener.Destroy();
            }

            foreach (Listener listener in listenerList.Values)
            {
                if (listener != null)
                {
                    listener.Destroy();
                }
            }
        }
        #endregion

        #region Destroy Bulletin Listener
        public void destroyBulletinListener()
        {
            foreach (Listener listener in listenerList.Values)
            {
                if (listener != null)
                {
                    listener.Destroy();
                }
            }
        }
        #endregion



        // Destroy Transport.
        private void destroyTransfort()
        {
            //fmbtransport.Destroy();
            //transport.Destroy();
            foreach (Transport transport in transportList.Values)
            {
                if (transport != null)
                {
                    transport.Destroy();
                }
            }
        }


        // Tibrv Environment Close.
        private void closeTibEnvironment()
        {
            try
            {
                // Force optimizer to keep alive listeners up to this point.
                GC.KeepAlive(listenerList);
                TIBCO.Rendezvous.Environment.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        // Create Tib Message.
        private TIBCO.Rendezvous.Message createTibMessage(string aMsg)
        {
            TIBCO.Rendezvous.Message message = new TIBCO.Rendezvous.Message();
            message.SendSubject = messageInfo.TargetSubject;
            message.ReplySubject = messageInfo.SourceSubject;
            message.AddField("DATA", aMsg);

            return message;
        }

        private void OnMessageReceived(object listener, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            try
            {

                TIBCO.Rendezvous.Message message = messageReceivedEventArgs.Message;
                string a = message.GetField("xmlData").Value.ToString();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(a);

                string messageName = xDoc.SelectNodes("Message/Header/MESSAGENAME")[0].InnerText.ToString();
                //System.Diagnostics.Debug.WriteLine(message.ToString());
                //System.Diagnostics.Debug.WriteLine(message.GetField("xmlData").Value.ToString());

                //if (message.SendSubject.Equals(messageInfo.OwnSubject))
                if (message.SendSubject.Contains(OwnSubjectName))
                {
                    if (OwnListenerReceived != null)
                    {
                        //OwnListenerReceived(message.SendSubject, listener, message.ToString());
                        OwnListenerReceived(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                    }
                }
                else if (message.SendSubject.Contains(this.fmbListener.Subject))
                {

                    //Common.TIB.MessageService.This.FMBListenerReceived -= new MessageService.EventHandlerForTIBMessageReceived(theFMBManager.FMBMachineManager_OnMessageReceived);
                    if (FMCListenerReceived != null)
                    {
                        FMCListenerReceived(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                    }
                }
                // JXM
                if (messageName == "FdcAlarmReport" || messageName == "SPCAlarmReport")  //.Name.ToString()//.SendSubject.Contains(messageInfo.OwnSubject))
                {
                    //Common.TIB.MessageService.This.ALARMTEST -= new MessageService.ALARMDELEGATE(this.ALARMTEST);
                    ALARMTEST(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                    messageName = "";
                }

                else
                {
                    if (ListenerReceived != null)
                    {
                        //ListenerReceived(message.SendSubject, listener, message.ToString());
                        ListenerReceived(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        private void OnFmbMessageReceived(object listener, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            try
            {

                TIBCO.Rendezvous.Message message = messageReceivedEventArgs.Message;

                if (FMCListenerReceived != null)
                {
                    FMCListenerReceived(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public void Send(string sendMessage)
        {
            // Create the message
            Message message = new Message();
            message.SendSubject = messageInfo.TargetSubject;

            try
            {
                message.AddField(messageInfo.FieldName, sendMessage, 0);

                transport.Send(message);
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon(messageInfo.Daemon);

                    if (!string.IsNullOrEmpty(newDeamon))
                        reconnect(messageInfo.Daemon);
                }

                Console.Error.WriteLine(ex.StackTrace);
                return;
            }
        }

        public void Send(string sendMessage, string userID)
        {
            // Create the message
            Message message = new Message();
            message.SendSubject = messageInfo.OwnSubject + "." + userID;

            try
            {
                message.AddField(messageInfo.FieldName, sendMessage, 0);

                transport.Send(message);
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon(messageInfo.Daemon);

                    if (!string.IsNullOrEmpty(newDeamon))
                        reconnect(messageInfo.Daemon);
                }

                Console.Error.WriteLine(ex.StackTrace);
                return;
            }
        }

        public string SendRequest(string sendMessage)
        {
            try
            {
                Message message;
                if (sendMessage.Contains(GetQueryResult))
                {
                    message = new Message() { SendSubject = messageInfo.QueryTargetSubject };
                    message.AddField(messageInfo.FieldName, sendMessage, 0);
                    Message received = querytransport.SendRequest(message, Double.Parse(messageInfo.TimeOut));

                    return (received == null) ? null : received.GetField(messageInfo.FieldName).Value.ToString();
                }
                else
                {
                    message = new Message() { SendSubject = messageInfo.TargetSubject };
                    message.AddField(messageInfo.FieldName, sendMessage, 0);

                    Message received = transport.SendRequest(message, Double.Parse(messageInfo.TimeOut));

                    return (received == null) ? null : received.GetField(messageInfo.FieldName).Value.ToString();
                }
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon(messageInfo.Daemon);

                    if (!string.IsNullOrEmpty(newDeamon))
                    {
                        reconnect(messageInfo.Daemon);
                        return string.Empty;
                    }
                    else
                        throw ex;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public string SendRequest(string sendMessage, double RequestTimeout)
        {
            try
            {
                Message message;
                if (sendMessage.Contains(GetQueryResult))
                {
                    message = new Message() { SendSubject = messageInfo.QueryTargetSubject };
                    message.AddField(messageInfo.FieldName, sendMessage, 0);
                    Message received = querytransport.SendRequest(message, RequestTimeout);

                    return (received == null) ? null : received.GetField(messageInfo.FieldName).Value.ToString();
                }
                else
                {
                    message = new Message() { SendSubject = messageInfo.TargetSubject };
                    message.AddField(messageInfo.FieldName, sendMessage, 0);
                    Message received = transport.SendRequest(message, RequestTimeout);

                    return (received == null) ? null : received.GetField(messageInfo.FieldName).Value.ToString();
                }
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon(messageInfo.Daemon);

                    if (!string.IsNullOrEmpty(newDeamon))
                    {
                        reconnect(messageInfo.Daemon);
                        return string.Empty;
                    }
                    else
                        throw ex;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public bool isInitiated()
        {
            return isRun;
        }
    }
}
