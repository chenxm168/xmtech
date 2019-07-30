using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WinSECS.structure
{
    public class FormatCollection : IFormatCollection, IEnumerable<IFormat>, IEnumerable
    {
        private List<IFormat> formatList = new List<IFormat>();
        private IFormat parentFormat = null;

        public FormatCollection(IFormat parent)
        {
            this.parentFormat = parent;
        }

        public void Add(IFormat format)
        {
            if (this.Count > 0)
            {
                IFormat format2 = this[this.Count - 1];
                format.Previous = format2;
                format2.Next = format;
            }
            format.Parent = this.parentFormat;
            format.Owner = this;
            this.formatList.Add(format);
        }

        public void Clear()
        {
            foreach (IFormat format in this)
            {
                format.Previous = null;
                format.Next = null;
                format.Parent = null;
            }
            this.formatList.Clear();
        }

        public bool Contains(IFormat format)
        {
            return this.formatList.Contains(format);
        }

        public IEnumerator<IFormat> GetEnumerator()
        {
            return this.formatList.GetEnumerator();
        }

        public int IndexOf(IFormat format)
        {
            return this.formatList.IndexOf(format);
        }

        public void Insert(int index, IFormat format)
        {
            if (((index - 1) > 0) && ((index - 1) < this.Count))
            {
                IFormat format2 = this[index - 1];
                format.Previous = format2;
                format2.Next = format;
            }
            if (((index + 1) > 0) && ((index + 1) < this.Count))
            {
                IFormat format3 = this[index + 1];
                format.Next = format3;
                format3.Previous = format;
            }
            format.Parent = this.parentFormat;
            format.Owner = this;
            this.formatList.Insert(index, format);
        }

        public void InsertAfter(IFormat newChild, IFormat refChild)
        {
            int index = this.IndexOf(refChild);
            this.Insert(index + 1, newChild);
        }

        public void InsertBefore(IFormat newChild, IFormat refChild)
        {
            int index = this.IndexOf(refChild);
            this.Insert(index, newChild);
        }

        public bool Remove(IFormat format)
        {
            if (format.Previous != null)
            {
                format.Previous.Next = format.Next;
            }
            if (format.Next != null)
            {
                format.Next.Previous = format.Previous;
            }
            format.Previous = null;
            format.Next = null;
            format.Parent = null;
            format.Owner = null;
            return this.formatList.Remove(format);
        }

        public void Replace(IFormat oldFormat, IFormat newFormat)
        {
            int index = this.formatList.IndexOf(oldFormat);
            if (index >= 0)
            {
                this.Remove(oldFormat);
                this.Insert(index, newFormat);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.formatList.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.formatList.Count;
            }
        }

        public IFormat this[int index]
        {
            get
            {
                return this.formatList[index];
            }
        }

        public IFormat this[string name]
        {
            get
            {
                foreach (IFormat format in this.formatList)
                {
                    if (format.Name == name)
                    {
                        return format;
                    }
                }
                return null;
            }
        }
    }
}
