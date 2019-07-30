using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using log4net;
using EQPIO.Common;
using EQPIO.MessageData;
using System.Threading;

namespace EQPIO.Handler
{
   public class EQPEventHandler:IEPQEventHandler,IEQPTraceDataHandler
    {
        private ControlManager mControlManager;

        ILog logger = LogManager.GetLogger(typeof(EQPEventHandler));
        public ControlManager MControlManager
        {
            get { return mControlManager; }
            set { mControlManager = value; }
        }



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


            //switch(msg.MessageType)
            //{
            //    case "Ethernet":

            //        Thread t1 = new Thread(mProtocol_OnEventReceived);
            //        t1.Name = "mProtocol_OnEventReceived";
            //        t1.Start(msg);

            //    //mProtocol_OnEventReceived(message);
            //    break;

            //    case "MNet":
            //    //mNet_OnEventReceived(message);

            //      Thread t2 = new Thread(mNet_OnEventReceived);
            //      t2.Name = "mNet_OnEventReceived";
            //        t2.Start(msg);
            //    break;

            //    default:
            //    logger.Error(String.Format("Not Found Message Type![{0}]", msg.MessageType));
            //    return;

            //}
        }

        private void mNet_OnEventReceived(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            //Utils.WritePLCLog(msg);
        }

        private void mProtocol_OnEventReceived(object message)
        {
            MessageData<PLCMessageBody> msg = (MessageData<PLCMessageBody>)message;
            //Utils.WritePLCLog(msg);
           
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
