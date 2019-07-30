
namespace HF.BC.Tool.EIPDriver.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class DictionaryList<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dictionary;
        private List<TKey> list;
        private readonly object syncRoot;

        public DictionaryList()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.list = new List<TKey>();
            this.syncRoot = new object();
        }

        internal void Add(TKey key, TValue value)
        {
            lock (this.syncRoot)
            {
                if (this.dictionary.ContainsKey(key))
                {
                    throw new ArgumentException("Already Exist " + key.ToString());
                }
                this.list.Add(key);
                this.dictionary.Add(key, value);
            }
        }

        internal void Clear()
        {
            lock (this.syncRoot)
            {
                this.list.Clear();
                this.dictionary.Clear();
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (this.syncRoot)
            {
                return this.dictionary.ContainsKey(key);
            }
        }

        internal bool Remove(TKey key)
        {
            lock (this.syncRoot)
            {
                this.list.Remove(key);
                return this.dictionary.Remove(key);
            }
        }

        public int Count
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.list.Count;
                }
            }
        }

        public TValue this[int pos]
        {
            get
            {
                TValue local;
                lock (this.syncRoot)
                {
                    if ((pos <= -1) || (pos >= this.list.Count))
                    {
                        throw new IndexOutOfRangeException("Out of range!");
                    }
                    return this[this.list[pos]];
                }
                return local;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue local;
                lock (this.syncRoot)
                {
                    if (!this.dictionary.ContainsKey(key))
                    {
                        throw new KeyNotFoundException("Could not found " + key);
                    }
                    return this.dictionary[key];
                }
                return local;
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.dictionary.Keys;
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.dictionary.Values;
                }
            }
        }
    }
}
