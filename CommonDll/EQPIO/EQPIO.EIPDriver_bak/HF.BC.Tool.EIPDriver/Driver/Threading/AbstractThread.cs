
namespace HF.BC.Tool.EIPDriver.Driver.Threading
{
    using log4net;
    using System;
    using System.Threading;

    public abstract class AbstractThread
    {
        protected ILog logger = LogManager.GetLogger(typeof(AbstractThread));
        protected bool running = false;
        protected Thread thread;

        public AbstractThread()
        {
            this.thread = new Thread(new ThreadStart(this.RunThread));
            this.thread.IsBackground = true;
        }

        protected abstract void Run();
        protected void RunThread()
        {
            this.logger.Info(string.Format("{0} Thread Status = {1}", this.Name, this.running));
            this.Run();
            this.logger.Info(string.Format("{0} Thread Status = {1}", this.Name, this.running));
        }

        public virtual void Start()
        {
            this.logger.Info(string.Format("Start {0} Thread.", this.Name));
            this.thread.Name = this.Name;
            this.running = true;
            this.thread.Start();
        }

        public virtual void Terminate()
        {
            this.logger.Info(string.Format("Terminate {0} Thread.", this.Name));
            this.running = false;
        }

        public string LoggerName
        {
            set
            {
                this.logger = LogManager.GetLogger(value);
            }
        }

        public abstract string Name { get; }

        public System.Threading.ThreadState ThreadState
        {
            get
            {
                return this.thread.ThreadState;
            }
        }
    }
}
