using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSECS.structure;

namespace WinSECS.connect
{
    internal class SECS1BlockDispatcher
    {
        private Dictionary<string, SECS1BlockCollection> dispatcher = new Dictionary<string, SECS1BlockCollection>();
        private int t4;

        public event T4ElapsedEventHandler OnT4Detected;

        internal SECS1BlockDispatcher(int t4)
        {
            this.t4 = t4;
        }

        internal SECS1BlockCollection AddMessage(SECS1Block msg)
        {
            lock (this.dispatcher)
            {
                string key = string.Format("S{0}F{1}-{2}", msg.Stream, msg.Function, msg.SystemByte);
                SECS1BlockCollection blocks = null;
                if (this.dispatcher.ContainsKey(key))
                {
                    blocks = this.dispatcher[key];
                    blocks.AddMessage(msg);
                }
                else
                {
                    blocks = new SECS1BlockCollection(key, this.t4);
                    blocks.OnT4Elapsed += new T4ElapsedEventHandler(this.OnT4Elapsed);
                    this.dispatcher.Add(key, blocks);
                    blocks.AddMessage(msg);
                }
                return blocks;
            }
        }

        private void OnT4Elapsed(SECS1BlockCollection collection)
        {
            lock (this.dispatcher)
            {
                if (!collection.IsCompleted)
                {
                    this.RemoveCollection(collection);
                    if (this.OnT4Detected != null)
                    {
                        this.OnT4Detected(collection);
                    }
                }
            }
        }

        internal void RemoveCollection(SECS1BlockCollection collection)
        {
            lock (this.dispatcher)
            {
                if (this.dispatcher.ContainsKey(collection.Key))
                {
                    this.dispatcher.Remove(collection.Key);
                }
            }
        }
    }
}
