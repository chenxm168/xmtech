using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EQPIO.Controller.Proxy;

namespace EQPIO.Controller
{
   public class ControlManagerFactory
    {
       private ILog logger = LogManager.GetLogger(typeof(ControlManagerFactory));
        private ControlManager mControlManager;

        public ControlManager MControlManager
        {
            get { return mControlManager; }
            set { mControlManager = value; }
        }

        private string configFile;

        public string ConfigFile
        {
            get { return configFile; }
            set { configFile = value; }
        }

        public IEPQEventHandler EQPEventHandler
        {
            get;
            set;
        }

        public IEQPTraceDataHandler EQPTraceDataHandler
        {
            get;
            set;
        }

       public ControlManagerFactory(string config)
        {

        }

       public ControlManagerFactory()
       {

       }


       public ControlManager getControlManager ()
       {
           if (mControlManager == null)
           {
               mControlManager = new ControlManager();
               mControlManager.EQPEventHandler = EQPEventHandler;
               mControlManager.EQPTraceDataHandler = EQPTraceDataHandler;
               if (configFile == null || configFile.Trim().Length < 1)
               {
                   mControlManager.Init();
               }
               else
               {
                   mControlManager.Init(configFile);
               }
               if(mControlManager.UseMQ)
               { 
               mControlManager.InitMQ();
               }
               string errorMsg = string.Empty;
               if (mControlManager.UseBoard && !mControlManager.InitMNet(ref errorMsg))
               {
                   logger.Error(string.Format("ControlManager Init MNet Error : {0}", errorMsg));

               }

               if (mControlManager.UseEthernet && !mControlManager.InitMEthernet(ref errorMsg))
               {
                   logger.Error(string.Format("ControlManager Init MEthernet Error : {0}", errorMsg));

               }

               if (mControlManager.UseEIP && !mControlManager.InitEIP(ref errorMsg))
               {
                   logger.Error(string.Format("ControlManager Init EIP Error : {0}", errorMsg));
               }
           }
           
           
           return mControlManager;

       }

       public MNetProxy getMNetProxy()
       {
           if(mControlManager!=null)
           { return mControlManager.getMNetProxy(); }
           return null;
           
       }

       public MProtocolProxy getMProtocolProxy()
       {
           if (mControlManager != null)
           { return mControlManager.getMProtocolProxy(); }
           return null;
           
       }

       public EIPProxy getEIPProxy()
       {
           if (mControlManager != null)
           { return mControlManager.getEIPProxy(); }
           return null;
           
       }

       public MQProxy getMQProxy()
       {
           if (mControlManager != null)
           { return mControlManager.getMQProxy(); }
           return null;
           
       }

    }
}
