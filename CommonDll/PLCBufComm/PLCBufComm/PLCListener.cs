using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PLCBufComm
{
   public abstract class PLCListener:IDisposable
    {

       protected ILog logger = LogManager.GetLogger(typeof(PLCListener));
       public event EventHandler<object> onMessage;
       public event EventHandler<object> onError;
       public event EventHandler<object> onConnected;
       public event EventHandler<object> onDisconnected;

       protected bool isStart = false;
       protected void onMessageEvent(object sender,object args)
       {
           if(onMessage!=null)
           {
               onMessage(sender, args);
           }
       }


       protected void onErrorEvent(object sender, object args)
       {
           if (onError != null)
           {
               onError(sender, args);
           }
       }

       protected void onConnectedEvent(object sender, object args)
       {
           if (onConnected != null)
           {
               onConnected(sender, args);
           }
       }

       protected void onDisconnectedEvent(object sender, object args)
       {
           if (onDisconnected != null)
           {
               onDisconnected(sender, args);
           }
       }

       public abstract void Start();
       public abstract void Stop();




       public abstract void Dispose();

    }
}
