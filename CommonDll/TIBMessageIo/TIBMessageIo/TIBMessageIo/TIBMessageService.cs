using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;
using log4net;
using System.Xml;

namespace TIBMessageIo
{
  public  class TIBMessageService:IMessage , IDisposable
    {
      ILog logger = LogManager.GetLogger(typeof(TIBMessageService));

      MessageInfo messageInfo;


      private Dispatcher dispatcher = null;
      private TIBCO.Rendezvous.Queue queue = null;


      public static Transport transport;
      private Dictionary<string, Listener> listenerList;
      private Dictionary<string, Transport> transportList = new Dictionary<string, Transport>();
      private Listener ownListener;
      private bool isRun = false;

      public delegate void EventHandlerForTIBMessageReceived(string subjectName, object listener, string message);
//      public event EventHandlerForTIBMessageReceived OwnListenerReceived;
      public event EventHandlerForTIBMessageReceived ListenerReceived;

       private static TIBMessageService m_This =null;
        //static TIBMessageService()
        //{
        //    m_This = new TIBMessageService();
        //}

        //public static TIBMessageService This
        //{
        //    get
        //    {
        //        return m_This;
        //    }
        //}

       public static TIBMessageService getInstance()
        {
           if(m_This==null)
           {
               m_This = new TIBMessageService();
           }
           return m_This;
        }

        public TIBMessageService()
        {
            if(messageInfo==null)
            {
                messageInfo = new MessageInfo(); 
            }
            if (isRun == false)
            {
                messageInfo.InitialzeConfig();
                connection();
            }
        }

        public TIBMessageService (MessageInfo msgInfo)
        {
            
            if(isRun==false)
            { 
            messageInfo = msgInfo;
            //messageInfo.InitialzeConfig();
            connection();
            }
        }


       public void Initialize()
        {
           
            connection();
        }


        public void Initialize(string strFilePath, string Target)
        {
            messageInfo.InitialzeConfig(strFilePath, Target);
            connection();

        }

        private void connection()
        {
            transportList = new Dictionary<string, Transport>();
            try
            {
                if (isRun)
                {
                    Terminate();
                }

                openTibEnvironment();
                transportList.Clear();
                string oldDaemon = messageInfo.Daemon;
                createTransport(messageInfo.Daemon);

                transport = transportList[oldDaemon];
                //logger.Debug("Send Transport:" + transport.CreateInbox());

                createQueue();
                createDispatcher();

                createListener();

                isRun = true;
            }
            catch (RendezvousException ex)
            {
                logger.Error(ex.StackTrace);
            }
        }

        private void openTibEnvironment()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();
            }
            catch (RendezvousException ex)
            {
               logger.Error(ex.StackTrace);
                throw ex;
            }
        }

        private void createQueue()
        {
            try
            {
                queue = new TIBCO.Rendezvous.Queue();
            }
            catch (RendezvousException ex)
            {
                logger.Error(ex.StackTrace);
                throw ex;
            }
        }

        private void createDispatcher()
        {
            try
            {
                dispatcher = new Dispatcher(queue);
            }
            catch (RendezvousException ex)
            {
                logger.Error(ex.StackTrace);
                throw ex;
            }
        }

        private void createTransport(string currentDeamon)
        {
            //string currentDeamon = messageInfo.Daemon;

            try
            {
    
                    NetTransport transport = new NetTransport(messageInfo.Service, messageInfo.Network, currentDeamon);
                    transportList.Add(currentDeamon, transport);
                    logger.DebugFormat("Create Transport Deamon[{0}], serverice[{1}],network[{2}]",currentDeamon,messageInfo.Service,messageInfo.Network);

            }
            catch (RendezvousException ex)
            {
                logger.Error(ex.StackTrace);

                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    messageInfo.UnableDaemonList.Add(currentDeamon);
                    string newDeamon = messageInfo.resetDeamon();

                    if (string.IsNullOrEmpty(newDeamon))
                        throw new Exception("No available deamon router");
                    else
                        createTransport( newDeamon);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void createListener()
        {
            listenerList = new Dictionary<string, Listener>();
            foreach (string listenSubjectName in messageInfo.ListenSubjectList)
            {
                Listener listener = new Listener(queue, transport, listenSubjectName, null);
                listener.MessageReceived +=
                    new MessageReceivedEventHandler(OnMessageReceived);
                listenerList.Add(listenSubjectName, listener);
                logger.DebugFormat("Creat Listen subject[{0}]", listenSubjectName);
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

                connection();

                isRun = true;
            }
            catch (RendezvousException ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }


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

            foreach (Listener listener in listenerList.Values)
            {
                if (listener != null)
                {
                    listener.Destroy();
                }
            }
        }

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

        private TIBCO.Rendezvous.Message createTibMessage(string aMsg)
        {
            TIBCO.Rendezvous.Message message = new TIBCO.Rendezvous.Message();
            message.SendSubject = messageInfo.TargetSubject;
            message.ReplySubject = messageInfo.SourceSubject;
            message.AddField(messageInfo.FieldName, aMsg);

            return message;
        }



        public void Send(string sendMssage)
        {
            Message message = new Message();
            message.SendSubject = messageInfo.TargetSubject;
            message.ReplySubject = messageInfo.SourceSubject;

            try
            {
                //message.AddField(messageInfo.FieldName, sendMssage, 0);
                message.AddField(messageInfo.FieldName, sendMssage,0);

                transport.Send(message);
                logger.Info("Send Message:\n" + sendMssage);
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon();

                    if (!string.IsNullOrEmpty(newDeamon))
                        reconnect(messageInfo.Daemon);
                }

                logger.Error(ex.StackTrace);
                return;
            }
        }

        public string SendRequest(string sendMessage)
        {
            try
            {
                Message message;

                    message = new Message() { SendSubject = messageInfo.TargetSubject };
                    message.AddField(messageInfo.FieldName, sendMessage, 0);

                    Message received = transport.SendRequest(message, Double.Parse(messageInfo.TimeOut));

                    return (received == null) ? null : received.GetField(messageInfo.FieldName).Value.ToString();
                
            }
            catch (RendezvousException ex)
            {
                if (ex.Status == Status.InvalidTransport || ex.Status == Status.DaemonNotConnected)
                {
                    string newDeamon = messageInfo.resetDeamon();

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

        public void Dispose()
        {
            this.Terminate();
        }

        #region Event

        private void OnMessageReceived(object listener, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            try
            {

                TIBCO.Rendezvous.Message message = messageReceivedEventArgs.Message;
                string a = message.GetField("xmlData").Value.ToString();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(a);

                string messageName = xDoc.SelectNodes("Message/Header/MESSAGENAME")[0].InnerText.ToString();
                logger.Info("Recieved:\n" + a);
                    if (ListenerReceived != null)
                    {
                       
                        ListenerReceived(message.SendSubject, listener, message.GetField("xmlData").Value.ToString());
                    }
                }
            
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
        }

        #endregion


    }
}
