using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using System.Reflection;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class Queue<T>
    {
        protected List<T> list;
        protected ILog logger;
        protected bool running;
        protected readonly object SyncRoot;

        public Queue()
        {
            this.logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            this.SyncRoot = new object();
            this.list = new List<T>();
            this.running = true;
        }

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this.list.Clear();
            }
        }

        public T Dequeue()
        {
            lock (this.SyncRoot)
            {
                while (this.Empty && this.running)
                {
                    try
                    {
                        if (!Monitor.Wait(this.SyncRoot, 0x7530))
                        {
                            break;
                        }
                    }
                    catch (ThreadInterruptedException exception)
                    {
                        this.logger.Warn("", exception);
                    }
                }
                if (this.Empty)
                {
                    return default(T);
                }
                T local = this.list[0];
                this.list.RemoveAt(0);
                return local;
            }
        }

        public virtual void Enqueue(T item)
        {
            lock (this.SyncRoot)
            {
                if (item != null)
                {
                    this.list.Add(item);
                    Monitor.PulseAll(this.SyncRoot);
                }
            }
        }

        public virtual void EnqueueFirst(T item)
        {
            lock (this.SyncRoot)
            {
                if (item != null)
                {
                    this.list.Insert(0, item);
                    Monitor.PulseAll(this.SyncRoot);
                }
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
    }
}
