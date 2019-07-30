using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinSECS.driver;
using System.IO;

namespace WinSECS.connect
{
    internal abstract class HSMSInterface
    {
        internal bool bRun = true;
        public static int gettableMutex = 100;
        internal SinglePlugIn rootHandle;
        protected internal int status;
        public static int ungettableMutex = 0x65;

        protected HSMSInterface()
        {
        }

        public virtual void AcquireMutext()
        {
            HSMSInterface interface2;
            Monitor.Enter(interface2 = this);
            try
            {
                Monitor.Wait(this);
            }
            catch (ThreadInterruptedException)
            {
            }
            finally
            {
                Monitor.Exit(interface2);
            }
        }

        public abstract void doRun();
        protected internal virtual void preready()
        {
            this.rootHandle.ManagerFactory.TimerManager.ClearTimerList();
            long disconnectToken = this.rootHandle.ManagerFactory.ConnectManager.GetDisconnectToken();
            if (disconnectToken != 0L)
            {
                this.rootHandle.ManagerFactory.ConnectManager.releaseMutext();
                this.rootHandle.ManagerFactory.LoggerManager.Logger.Info("HSMSxxx Release Mutex fot Disconnection");
                try
                {
                    Thread.Sleep(0x3e8);
                }
                catch (ThreadInterruptedException)
                {
                }
                this.rootHandle.ManagerFactory.ConnectManager.ReleaseDisconnectToken(disconnectToken);
                this.ready();
            }
            else
            {
                this.ready();
            }
        }

        public virtual void ready()
        {
            this.Status = gettableMutex;
            this.AcquireMutext();
            this.Status = ungettableMutex;
        }

        public virtual void releaseMutext()
        {
            lock (this)
            {
                Monitor.Pulse(this);
            }
        }

        public abstract BinaryReader DataInputStream { set; }

        public abstract BinaryWriter DataOutputStream { set; }

        public abstract string Name { get; }

        public virtual bool Run
        {
            get
            {
                return this.bRun;
            }
            set
            {
                this.bRun = value;
            }
        }

        public abstract int Status { get; set; }
    }
}
