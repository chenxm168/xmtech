using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net.Core;
using WinSECS.timeout;
using WinSECS.global;
using WinSECS.structure;
using WinSECS.connect;
using WinSECS.logger;
using WinSECS.MessageHandler;
using WinSECS.Utility;
using WinSECS.driver;


namespace WinSECS.manager
{
    internal class ManagerFactory : IManager
    {
        private WinSECS.callback.CallbackManager callbackManager;
        private SupportClass.ThreadClass ConnectionThread;
        private WinSECS.connect.connectManager connectManager;
        private bool isRunning = false;
        private WinSECS.logger.LoggerManager loggerManager;
        private SECSConfig m_config;
        private ModelingMessageFactory messageFactory;
        private WinSECS.timeout.TimerManager timerManager;

        public ManagerFactory()
        {
            this.InitBlock();
        }

        private void InitBlock()
        {
            this.timerManager = new WinSECS.timeout.TimerManager();
            this.callbackManager = new WinSECS.callback.CallbackManager();
            this.loggerManager = new WinSECS.logger.LoggerManager();
            this.messageFactory = new ModelingMessageFactory();
        }

        public virtual void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            if (this.isRunning)
            {
                returnObject.setError(0x67);
            }
            else
            {
                try
                {
                    this.m_config = config.Clone() as SECSConfig;
                }
                catch (Exception exception)
                {
                    returnObject.setError("Fail Clone SECSConfig : " + loggerHelper.getExceptionString(exception));
                    return;
                }
                rootHandle.Config = this.m_config;
                this.LoggerManager.Initialize(rootHandle, this.m_config, returnObject);
                if (returnObject.isSuccess())
                {
                    this.messageFactory.Initialize(rootHandle, this.m_config, returnObject);
                    if (returnObject.isSuccess())
                    {
                        this.callbackManager.Initialize(rootHandle, this.m_config, returnObject);
                        if (returnObject.isSuccess())
                        {
                            this.timerManager.Initialize(rootHandle, this.m_config, returnObject);
                            this.connectManager = new WinSECS.connect.connectManager(rootHandle, this.m_config, returnObject);
                            this.ConnectionThread = new SupportClass.ThreadClass(new ThreadStart(this.connectManager.Run));
                            this.ConnectionThread.Name = "ConnectionManager";
                            this.ConnectionThread.IsBackground = true;
                            this.ConnectionThread.Start();
                            this.isRunning = true;
                        }
                    }
                }
            }
        }

        public void ReloadConfig(SECSConfig newConfig, bool enforceReconnect, bool reloadSMD, ReturnObject returnObject)
        {
            try
            {
                this.m_config = newConfig.Clone() as SECSConfig;
            }
            catch (Exception exception)
            {
                returnObject.setError("Fail Clone SECSConfig : " + loggerHelper.getExceptionString(exception));
                return;
            }
            if (this.loggerManager != null)
            {
                this.loggerManager.ReloadConfig(this.m_config, returnObject);
            }
            if (returnObject.isSuccess())
            {
                if (reloadSMD)
                {
                    this.ReloadSMD(newConfig, returnObject);
                    if (!returnObject.isSuccess())
                    {
                        return;
                    }
                }
                if (this.callbackManager != null)
                {
                    this.callbackManager.ReloadConfig(this.m_config, enforceReconnect, reloadSMD, returnObject);
                }
                if (this.timerManager != null)
                {
                    this.timerManager.ReloadConfig(this.m_config, enforceReconnect, reloadSMD, returnObject);
                }
                if (this.connectManager != null)
                {
                    this.connectManager.ReloadConfig(this.m_config, enforceReconnect);
                }
            }
        }

        public void ReloadSMD(ReturnObject returnObject)
        {
            this.messageFactory.ReloadSMD(returnObject);
        }

        public void ReloadSMD(SECSConfig newConfig, ReturnObject returnObject)
        {
            this.messageFactory.ReloadSMD(newConfig, returnObject);
        }

        public virtual void Terminate(ReturnObject returnObject)
        {
            if (this.connectManager != null)
            {
                this.connectManager.Terminate(returnObject);
            }
            this.timerManager.Terminate(returnObject);
            Thread.Sleep(50);
            if (this.callbackManager != null)
            {
                this.callbackManager.Terminate(returnObject);
            }
            this.loggerManager.WriteConnnectionLog(Level.Info, this.m_config.DriverId + " TERMINATED \r\n", true);
            this.isRunning = false;
        }

        public virtual WinSECS.callback.CallbackManager CallbackManager
        {
            get
            {
                return this.callbackManager;
            }
            set
            {
                this.callbackManager = value;
            }
        }

        public virtual WinSECS.connect.connectManager ConnectManager
        {
            get
            {
                return this.connectManager;
            }
            set
            {
                this.connectManager = value;
            }
        }

        internal WinSECS.logger.LoggerManager LoggerManager
        {
            get
            {
                return this.loggerManager;
            }
            set
            {
                this.loggerManager = value;
            }
        }

        public virtual ModelingMessageFactory MessageFactory
        {
            get
            {
                return this.messageFactory;
            }
            set
            {
                this.messageFactory = value;
            }
        }

        public virtual WinSECS.timeout.TimerManager TimerManager
        {
            get
            {
                return this.timerManager;
            }
            set
            {
                this.timerManager = value;
            }
        }
    }
}
