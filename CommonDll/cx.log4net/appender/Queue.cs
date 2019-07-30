using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using log4net;

namespace cx.log4net.appender
{
    public class Queue
    {
        protected ArrayList list = new ArrayList();
        protected ILog logger = LogManager.GetLogger(typeof(Queue));
        protected bool running = true;
        protected readonly object SyncRoot = new object();

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this.list.Clear();
            }
        }

        public object Dequeue()
        {
            lock (this.SyncRoot)
            {
                while (this.Empty && this.running)
                {
                    try
                    {
                        Monitor.Wait(this.SyncRoot);
                        continue;
                    }
                    catch (ThreadInterruptedException exception)
                    {
                        Console.WriteLine(exception);
                        continue;
                    }
                }
                if (this.Empty)
                {
                    return null;
                }
                object obj2 = this.list[0];
                this.list.RemoveAt(0);
                return obj2;
            }
        }

        public virtual void Enqueue(object o)
        {
            lock (this.SyncRoot)
            {
                if (o != null)
                {
                    this.list.Add(o);
                    Monitor.PulseAll(this.SyncRoot);
                }
            }
        }

        public virtual void EnqueueFirst(object o)
        {
            lock (this.SyncRoot)
            {
                if (o != null)
                {
                    this.list.Insert(0, o);
                    Monitor.PulseAll(this.SyncRoot);
                }
            }
        }

        public static void Sleep(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch
            {
            }
        }

        public void Terminate()
        {
            lock (this.SyncRoot)
            {
                this.running = false;
                Monitor.PulseAll(this.SyncRoot);
                this.Clear();
            }
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public bool Empty
        {
            get
            {
                return (this.list.Count == 0);
            }
        }

        public string LoggerName
        {
            get
            {
                return this.logger.Logger.Name;
            }
            set
            {
                this.logger = LogManager.GetLogger(value);
            }
        }

        public bool Running
        {
            get
            {
                return this.running;
            }
        }
    }
}
