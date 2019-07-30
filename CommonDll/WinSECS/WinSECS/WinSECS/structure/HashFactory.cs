using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinSECS.structure
{
    [ComVisible(false)]
    public class HashFactory
    {
        private Dictionary<string, Dictionary<string, object>> doubleHash = new Dictionary<string, Dictionary<string, object>>();
        internal object lock_Renamed;

        public HashFactory()
        {
            this.InitBlock();
        }

        public virtual bool Add(string bigCategory, string SmallCategory, object obj)
        {
            Dictionary<string, object> dictionary = this.getBigCategory(bigCategory) as Dictionary<string, object>;
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                this.doubleHash.Add(bigCategory, dictionary);
            }
            try
            {
                dictionary.Add(SmallCategory, obj);
            }
            catch (ArgumentException)
            {
                SmallCategory = SmallCategory + "_" + dictionary.Count;
                dictionary.Add(SmallCategory, obj);
            }
            return true;
        }

        public virtual bool clear()
        {
            lock (this.doubleHash)
            {
                foreach (string str in this.doubleHash.Keys)
                {
                    IDictionary<string, object> dictionary = this.doubleHash[str];
                    if (dictionary != null)
                    {
                        foreach (string str2 in dictionary.Keys)
                        {
                            object obj2 = dictionary[str2];
                            if (obj2 != null)
                            {
                                obj2 = null;
                            }
                        }
                        dictionary.Clear();
                    }
                }
                this.doubleHash.Clear();
            }
            return true;
        }

        public IDictionary<string, object> getBigCategory(string bigCategory)
        {
            try
            {
                if (this.doubleHash.ContainsKey(bigCategory))
                {
                    return this.doubleHash[bigCategory];
                }
                return null;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public virtual object getSamllCategory(string bigCategory, string smallCategory)
        {
            IDictionary<string, object> dictionary = this.getBigCategory(bigCategory);
            if (dictionary != null)
            {
                if (dictionary.ContainsKey(smallCategory))
                {
                    return dictionary[smallCategory];
                }
                return null;
            }
            return null;
        }

        private void InitBlock()
        {
            this.lock_Renamed = new object();
        }

        public virtual bool Remove(string bigCategory)
        {
            if (this.doubleHash.ContainsKey(bigCategory))
            {
                IDictionary<string, object> dictionary = this.doubleHash[bigCategory];
                lock (dictionary)
                {
                    if (dictionary != null)
                    {
                        foreach (string str in dictionary.Keys)
                        {
                            object obj2 = dictionary[str];
                            dictionary.Remove(str);
                            if (obj2 != null)
                            {
                                obj2 = null;
                            }
                        }
                    }
                }
                dictionary.Clear();
                this.doubleHash.Remove(bigCategory);
                return true;
            }
            return false;
        }

        public virtual bool Remove(string bigCategory, string SmallCategory)
        {
            if (this.doubleHash.ContainsKey(bigCategory) && this.doubleHash[bigCategory].ContainsKey(SmallCategory))
            {
                object obj2 = this.doubleHash[bigCategory][SmallCategory];
                if (obj2 != null)
                {
                    obj2 = null;
                }
                this.doubleHash[bigCategory].Remove(SmallCategory);
            }
            return true;
        }

        public virtual int size()
        {
            int num = 0;
            foreach (string str in this.doubleHash.Keys)
            {
                IDictionary<string, object> dictionary = this.doubleHash[str];
                num += dictionary.Count;
            }
            return num;
        }
    }
}
