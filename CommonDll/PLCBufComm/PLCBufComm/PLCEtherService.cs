using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using log4net;

namespace PLCBufComm
{

   // public delegate void EventHandler(object sender, object args);
  public  class PLCEtherService:IDisposable
    {
      public event EventHandler<object> listenerOnMessage;
      public event EventHandler<object> listenerOnError;
      public event EventHandler<object> listenerClientOnConnected;
      public event EventHandler<object> listenerClientDisconnected;

      private ILog logger = LogManager.GetLogger(typeof(PLCEtherService));
      public PLCListener plcListenr
      { get; set; }

      public PLCEtherService()
      {

      }

      protected void listenerOnMessageEvent(object sender,object args)
      {
          try
          {
              TcpClient tc = (TcpClient)sender;

              logger.InfoFormat("Received Message [{0}] from [{1}]", (string)args, tc.Client.RemoteEndPoint.ToString());
          }catch(Exception  e)
          {
              logger.Error(e.Message);
          }


          
          if(listenerOnMessage!=null)
          {
              listenerOnMessage(sender, args);
          }
      }

      protected void listenerOnErrorEvent(object sender, object args)
      {
          if (listenerOnError != null)
          {
              listenerOnError(sender, args);
          }
      }

      protected void listenerClientOnConnectedEvent(object sender, object args)
      {
          if (listenerClientOnConnected != null)
          {
              listenerClientOnConnected(sender, args);
          }
      }

      protected void listenerClientDisconnectedEvent(object sender, object args)
      {
          if (listenerClientDisconnected != null)
          {
              listenerClientDisconnected(sender, args);
          }
      }


      public void Dispose()
      {
          plcListenr.Stop();
      }

      public void Init()
      {
          if(plcListenr!=null)
          {
              plcListenr.onError += listenerOnErrorEvent;
              plcListenr.onMessage += listenerOnMessageEvent;
              plcListenr.onConnected += listenerClientOnConnectedEvent;
              plcListenr.onDisconnected += listenerClientDisconnectedEvent;
              plcListenr.Start();
          }
      }
    }
}
