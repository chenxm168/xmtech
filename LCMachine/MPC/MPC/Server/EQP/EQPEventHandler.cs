using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EQPIO.Controller;
using log4net;
using EQPIO.MessageData;

namespace MPC.Server.EQP
{
    public class EQPEventHandler : IEPQEventHandler,IEQPTraceDataHandler
    {

        private ControlManager mControlManager;

        ILog logger = LogManager.GetLogger(typeof(EQPEventHandler));
        public ControlManager MControlManager
        {
            get { return mControlManager; }
            set { mControlManager = value; }
        }
        public Dictionary<string, IEPQEventHandler> EventHandlers
        {
            get;
            set;
        }

        public event MessageEventHandler OnConnected;
        public event MessageEventHandler OnDisconnected;

        public void EQPEventProcess(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;

            if (msg.MessageType.ToUpper().Contains("MNET") || msg.MessageType.ToUpper().Contains("BOARD"))
            {
                Thread t2 = new Thread(mNet_OnEventReceived);
                t2.Name = "mNet_OnEventReceived";
                t2.Start(msg);
            }
            if (msg.MessageType.ToUpper().Contains("ETHERNET"))
            {
                Thread t1 = new Thread(mProtocol_OnEventReceived);
                t1.Name = "mProtocol_OnEventReceived";
                t1.Start(msg);
            }


        }

        private void mNet_OnEventReceived(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;

            if(msg.MessageName.ToUpper().Contains("CONNECTED"))
            {
                if(OnConnected!=null)
                {
                    OnConnected(null, null);
                }
                return;
            }
            else if (msg.MessageName.ToUpper().Contains("DISCONNECTED"))
            {
                if (OnDisconnected != null)
                {
                    OnDisconnected(null, null);
                }
                return;
            }

            foreach(var name in EventHandlers.Keys)
            {
               string  msgName = msg.MessageName.ToUpper();
                if(msgName==name.ToUpper()||msgName.Contains(name))
                {
                    EventHandlers[name].EQPEventProcess(message);
                }
            }
        }

        private void mProtocol_OnEventReceived(object message)
        {
          
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            if (msg.MessageName.ToUpper().Contains("CONNECTION"))
            {
                if (OnConnected != null)
                {
                    OnConnected(null, null);
                    
                }
                IEPQEventHandler handler;
               if( EventHandlers.TryGetValue(msg.MessageName,out handler))
                {
                    handler.EQPEventProcess(null);
                }
                
                return;
                
            }
            else if (msg.MessageName.ToUpper().Contains("DISCONNECTION"))
            {
                if (OnDisconnected != null)
                {
                    OnDisconnected(null, null);
                    
                }
                return;
                
            }

            foreach (var name in EventHandlers.Keys)
            {
                string msgName = msg.MessageBody.EventName;
                if (msgName == name.ToUpper() || msgName.Contains(name))
                {
                    EventHandlers[name].EQPEventProcess(message);
                }
            }
        }

        public void EIP_EQPEventProcess(object messsage)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)messsage;
            //Utils.WritePLCLog(msg);

        }


        public void EQPTraceDateProcess(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            switch (msg.MessageType)
            {
                case "MelsecEthernet":
                    //mProtocol_OnEventReceived(message);
                    Thread t1 = new Thread(mProtocol_OnEventReceived);
                    t1.Name = "mProtocol_OnTraceDataReceived";
                    t1.Start(msg);
                    break;

                case "MNet":
                    //mProtocol_OnTraceDataReceived(message);
                    Thread t2 = new Thread(mNet_OnTraceDataReceived);
                    t2.Name = "mNet_OnTraceDataReceived";
                    t2.Start(msg);
                    break;

                default:
                    logger.Error(String.Format("Not Found Message Type![{0}]", msg.MessageType));
                    return;

            }
        }


        private void mNet_OnTraceDataReceived(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            //Utils.WritePLCLog(msg);
        }

        private void mProtocol_OnTraceDataReceived(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            //Utils.WritePLCLog(msg);
        }
    }

}
