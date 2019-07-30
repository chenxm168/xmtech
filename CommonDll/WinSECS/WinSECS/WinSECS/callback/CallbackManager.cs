using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using log4net;
using WinSECS.driver;
using WinSECS.global;
using WinSECS.manager;
using WinSECS.structure;
using WinSECS.Utility;
using WinSECS.timeout;
using WinSECS.connect;


namespace WinSECS.callback
{
    [ComVisible(false)]
    internal class CallbackManager : abstractManager, ISECSListener
    {
        private bool bActorNone = false;
        private bool bActorRun = true;
        private bool bFirstConnected = true;
        internal SupportClass.ThreadClass callbackactor;
        private WinSECS.Utility.Queue<Callback> callbackQueue;
        private SECSConfig config = null;
        private LinkTestActor linkTestActor;
        private SupportClass.ThreadClass linktestactorContainer;
        private List<ISECSListener> listeners = new List<ISECSListener>();
        private ILog logger;

        public CallbackManager()
        {
            this.InitBlock();
        }

        public virtual void addSECSListener(ISECSListener listener)
        {
            if (!this.getListeners().Contains(listener))
            {
                this.getListeners().Add(listener);
            }
        }

        public WinSECS.Utility.Queue<Callback> getCallbackQueue()
        {
            return this.callbackQueue;
        }

        public List<ISECSListener> getListeners()
        {
            return this.listeners;
        }

        private void InitBlock()
        {
            this.callbackQueue = new WinSECS.Utility.Queue<Callback>();
        }

        public override void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            this.logger = rootHandle.ManagerFactory.LoggerManager.Logger;
            base.Initialize(rootHandle, config, returnObject);
            this.bActorRun = true;
            callbackActor actor1 = new callbackActor(this, this.logger);
            this.callbackactor = new SupportClass.ThreadClass(new ThreadStart(actor1.Run));
            this.callbackactor.Name = "CallBackActor";
            this.callbackactor.IsBackground = true;
            this.callbackactor.Start();
            this.config = config;
            if (config.LinktestDuration > 0)
            {
                this.StartLinkTestActor(this.config.LinktestDuration);
            }
        }

        private void LinkTestReload(SECSConfig newConfig)
        {
            int linktestDuration = this.config.LinktestDuration;
            int duration = newConfig.LinktestDuration;
            if (linktestDuration != duration)
            {
                if ((linktestDuration > 0) && (duration <= 0))
                {
                    this.StopLinkTestActor();
                }
                if ((linktestDuration <= 0) && (duration > 0))
                {
                    this.startLinkTestActorWithoutTrigger(duration);
                }
                if ((linktestDuration > 0) && (duration > 0))
                {
                    this.linkTestActor.LinkTestDuation = duration;
                }
            }
        }

        public virtual void onConnected(string driverID)
        {
            this.callbackQueue.Enqueue(new Connected(driverID));
            if (this.bFirstConnected)
            {
                this.triggerLinkTestActor();
            }
        }

        public virtual void onDisconnected(string driverID)
        {
            this.callbackQueue.Enqueue(new Disconnected(driverID));
        }

        public virtual void onIllegal(string driverID, SECSTransaction transaction, string illegalMessage)
        {
            ArrayList list = new ArrayList();
            list.Add(transaction);
            list.Add(illegalMessage);
            this.callbackQueue.Enqueue(new Illegal(list));
        }

        public virtual void onLog(string driverID, string log)
        {
            this.callbackQueue.Enqueue(new Logging(log));
        }

        public virtual void onReceived(string driverID, SECSTransaction transaction)
        {
            this.callbackQueue.Enqueue(new Received(transaction));
        }

        public virtual void onSendComplete(string driverID, SECSTransaction transaction)
        {
            this.callbackQueue.Enqueue(new SendCompleted(transaction));
        }

        public virtual void onSendFailed(string driverID, SECSTransaction transaction)
        {
            this.callbackQueue.Enqueue(new SendFailed(transaction));
        }

        public virtual void onTimeout(string driverID, SECSTimeout timeout)
        {
            this.callbackQueue.Enqueue(new WinSECS.callback.Timeout(timeout));
        }

        public void onUnknownReceived(string driverID, SECSTransaction transaction)
        {
            this.callbackQueue.Enqueue(new UnknownReceived(transaction));
        }

        public void ReloadConfig(SECSConfig newConfig, bool forceReconnect)
        {
            this.LinkTestReload(newConfig);
            this.config = newConfig;
        }

        public virtual void removeSECSListener(ISECSListener listener)
        {
            this.getListeners().Remove(listener);
        }

        public void setCallbackQueue(WinSECS.Utility.Queue<Callback> callbackQueue)
        {
            this.callbackQueue = callbackQueue;
        }

        private void StartLinkTestActor(int duration)
        {
            if (this.linkTestActor == null)
            {
                this.linkTestActor = new LinkTestActor(base.rootHandle, duration);
                this.linktestactorContainer = new SupportClass.ThreadClass(new ThreadStart(this.linkTestActor.Run));
                this.linktestactorContainer.Name = "LinkTestActor";
                this.linktestactorContainer.IsBackground = true;
                this.linktestactorContainer.Start();
            }
        }

        private void startLinkTestActorWithoutTrigger(int duration)
        {
            if (this.linkTestActor == null)
            {
                this.linkTestActor = new LinkTestActor(base.rootHandle, duration);
                this.linkTestActor.Pause = false;
                this.linktestactorContainer = new SupportClass.ThreadClass(new ThreadStart(this.linkTestActor.Run));
                this.linktestactorContainer.Name = "LinkTestActor";
                this.linktestactorContainer.IsBackground = true;
                this.linktestactorContainer.Start();
            }
        }

        private void StopLinkTestActor()
        {
            if (this.linkTestActor != null)
            {
                this.linkTestActor.IsIntendedStop = true;
                this.linktestactorContainer.Interrupt();
            }
        }

        public virtual void Terminate(ReturnObject returnObject)
        {
            this.bActorRun = false;
            this.StopLinkTestActor();
            if (this.callbackactor != null)
            {
                this.callbackactor.Interrupt();
            }
        }

        private void triggerLinkTestActor()
        {
            if (this.config.LinktestDuration > 0)
            {
                this.bFirstConnected = false;
                base.rootHandle.ManagerFactory.ConnectManager.InitializeLastActionTime();
                this.linkTestActor.Pause = false;
                this.linkTestActor.releaseMutex();
            }
        }

        internal class callbackActor : IThreadRunnable
        {
            public ILog callbackLogger;
            private CallbackManager enclosingInstance;

            public callbackActor(CallbackManager enclosingInstance, ILog logger)
            {
                this.callbackLogger = logger;
                this.InitBlock(enclosingInstance);
            }

            private void InitBlock(CallbackManager enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public virtual void Run()
            {
                while (this.Enclosing_Instance.bActorRun)
                {
                    Callback callback = this.Enclosing_Instance.callbackQueue.Dequeue();
                    if ((callback != null) && !this.Enclosing_Instance.bActorNone)
                    {
                        Exception exception;
                        switch (callback.getType())
                        {
                            case 0:
                                {
                                    SECSTransaction transaction = (SECSTransaction)callback.getObject();
                                    foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                    {
                                        try
                                        {
                                            this.callbackLogger.Info(string.Concat(new object[] { "[CallbackManager][run] Start Invoke onReceive MsgName= ", transaction.Name, " SB= ", transaction.Systembyte }));
                                            listener.onReceived(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                            this.callbackLogger.Info(string.Concat(new object[] { "[CallbackManager][run] End Invoke onReceive MsgName= ", transaction.Name, " SB= ", transaction.Systembyte }));
                                        }
                                        catch (Exception exception1)
                                        {
                                            exception = exception1;
                                            this.callbackLogger.Error("[CallbackManager][run] - During callback OnReceived Exception ", exception);
                                        }
                                    }
                                    try
                                    {
                                        this.enclosingInstance.rootHandle.onReceivedEvent(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                    }
                                    catch (Exception exception2)
                                    {
                                        exception = exception2;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback OnReceived Exception ", exception);
                                    }
                                    break;
                                }
                            case 1:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    listener.onConnected((string)callback.getObject());
                                }
                                this.enclosingInstance.rootHandle.onConnectedEvent(this.enclosingInstance.config.DriverId);
                                break;

                            case 2:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    listener.onDisconnected((string)callback.getObject());
                                }
                                this.enclosingInstance.rootHandle.onDisconnectedEvent(this.enclosingInstance.config.DriverId);
                                break;

                            case 3:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    ArrayList list = (ArrayList)callback.getObject();
                                    listener.onIllegal(this.enclosingInstance.config.DriverId, (SECSTransaction)list[0], (string)list[1]);
                                }
                                this.enclosingInstance.rootHandle.onIllegalEvent(this.enclosingInstance.config.DriverId, (SECSTransaction)((ArrayList)callback.getObject())[0]);
                                break;

                            case 4:
                                {
                                    WinSECS.callback.Timeout timeout = (WinSECS.callback.Timeout)callback;
                                    SECSTimeout timeout2 = (SECSTimeout)timeout.getObject();
                                    foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                    {
                                        try
                                        {
                                            listener.onTimeout(this.enclosingInstance.config.DriverId, timeout2);
                                        }
                                        catch (Exception exception3)
                                        {
                                            exception = exception3;
                                            this.callbackLogger.Error("[CallbackManager][run] - During callback onTimeout Exception", exception);
                                        }
                                    }
                                    try
                                    {
                                        this.enclosingInstance.rootHandle.onTimeoutEvent(this.enclosingInstance.config.DriverId, timeout2);
                                    }
                                    catch (Exception exception4)
                                    {
                                        exception = exception4;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback onTimeout Exception", exception);
                                    }
                                    break;
                                }
                            case 5:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    try
                                    {
                                        listener.onSendComplete(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                    }
                                    catch (Exception exception5)
                                    {
                                        exception = exception5;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback onSendComplete Exception", exception);
                                    }
                                }
                                try
                                {
                                    this.enclosingInstance.rootHandle.onSendCompleteEvent(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                }
                                catch (Exception exception6)
                                {
                                    exception = exception6;
                                    this.callbackLogger.Error("[CallbackManager][run] - During callback onSendComplete Exception", exception);
                                }
                                break;

                            case 6:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    try
                                    {
                                        listener.onLog(this.enclosingInstance.config.DriverId, (string)callback.getObject());
                                    }
                                    catch (Exception exception7)
                                    {
                                        exception = exception7;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback OnLog Exception", exception);
                                    }
                                }
                                try
                                {
                                    this.enclosingInstance.rootHandle.onLogEvent(this.enclosingInstance.config.DriverId, (string)callback.getObject());
                                }
                                catch (Exception exception8)
                                {
                                    exception = exception8;
                                    this.callbackLogger.Error("[CallbackManager][run] - During callback OnLog Exception", exception);
                                }
                                break;

                            case 7:
                                {
                                    SECSTransaction transaction2 = (SECSTransaction)callback.getObject();
                                    foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                    {
                                        try
                                        {
                                            this.callbackLogger.Info(string.Concat(new object[] { "[CallbackManager][run] Start Invoke onUnknownReceived MsgName= ", transaction2.Name, " SB= ", transaction2.Systembyte }));
                                            listener.onUnknownReceived(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                            this.callbackLogger.Info(string.Concat(new object[] { "[CallbackManager][run] End Invoke onUnknownReceived MsgName= ", transaction2.Name, " SB= ", transaction2.Systembyte }));
                                        }
                                        catch (Exception exception9)
                                        {
                                            exception = exception9;
                                            this.callbackLogger.Error("[CallbackManager][run] - During callback onUnknownReceived Exception", exception);
                                        }
                                    }
                                    try
                                    {
                                        this.enclosingInstance.rootHandle.onUnknownReceivedEvent(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                    }
                                    catch (Exception exception10)
                                    {
                                        exception = exception10;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback onUnknownReceived Exception", exception);
                                    }
                                    break;
                                }
                            case 8:
                                foreach (ISECSListener listener in this.enclosingInstance.listeners)
                                {
                                    try
                                    {
                                        listener.onSendFailed(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                    }
                                    catch (Exception exception11)
                                    {
                                        exception = exception11;
                                        this.callbackLogger.Error("[CallbackManager][run] - During callback onSendFailed Exception", exception);
                                    }
                                }
                                try
                                {
                                    this.enclosingInstance.rootHandle.onSendCompleteEvent(this.enclosingInstance.config.DriverId, (SECSTransaction)callback.getObject());
                                }
                                catch (Exception exception12)
                                {
                                    exception = exception12;
                                    this.callbackLogger.Error("[CallbackManager][run] - During callback onSendFailed Exception", exception);
                                }
                                break;
                        }
                    }
                }
            }

            public CallbackManager Enclosing_Instance
            {
                get
                {
                    return this.enclosingInstance;
                }
            }
        }
    }
}
