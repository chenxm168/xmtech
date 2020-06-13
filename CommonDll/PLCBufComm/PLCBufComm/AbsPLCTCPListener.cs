using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using log4net;
using System.Collections.Concurrent;
using System.Threading;

namespace PLCBufComm
{
   public abstract  class AbsPLCTCPListener:IDisposable
    {
       protected ILog logger = LogManager.GetLogger(typeof(AbsPLCTCPListener));
       protected TcpListener listener;

       protected ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();

        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        private int tcpPort;

        public int TcpPort
        {
            get { return tcpPort; }
            set { tcpPort = value; }
        }

        protected abstract void receivedMessage(byte[] bMessage);

       public AbsPLCTCPListener(string ip,int port)
        {
            this.IpAddress = ip;
            this.TcpPort = port;
        }

       public AbsPLCTCPListener(int port)
       {
           this.IpAddress = "127.0.0.1";
           this.TcpPort = port;

       }

       public AbsPLCTCPListener()
       {
           this.IpAddress = "127.0.0.1";
           this.TcpPort = 9800;
       }


       protected virtual void Init()
       {
           if(listener==null)
           {

           }
       }




       public void Dispose()
       {
           try
           {
               listener.Stop();
           }
           catch(Exception e)
           {
               logger.ErrorFormat("Stop Listen Error! [Error:{0}]", e.Message);
           }
           
       }
    }
}
