
namespace HF.BC.Tool.EIPDriver.Driver.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class BlockingQueue<T>
    {
        private Queue<T> queue;
        private Semaphore semaphore;

        public BlockingQueue()
        {
            this.queue = new Queue<T>();
            this.semaphore = new Semaphore(0, 0x7fffffff);
        }

        public void Clear()
        {
            lock (((ICollection)this.queue).SyncRoot)
            {
                this.queue.Clear();
            }
            this.semaphore.Release();
        }

        public T Dequeue()
        {
            this.semaphore.WaitOne();
            lock (((ICollection)this.queue).SyncRoot)
            {
                if (this.queue.Count > 0)
                {
                    return this.queue.Dequeue();
                }
                return default(T);
            }
        }

        public void Enqueue(T item)
        {
            lock (((ICollection)this.queue).SyncRoot)
            {
                this.queue.Enqueue(item);
            }
            this.semaphore.Release();
        }

        public int Count
        {
            get
            {
                lock (((ICollection)this.queue).SyncRoot)
                {
                    return this.queue.Count;
                }
            }
        }
    }
}
