using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public interface IFormatCollection : IEnumerable<IFormat>, IEnumerable
    {
        // Methods

        void Add(IFormat format);
        void Clear();
        bool Contains(IFormat format);
        int IndexOf(IFormat format);
        void Insert(int index, IFormat format);
        void InsertAfter(IFormat newChild, IFormat refChild);
        void InsertBefore(IFormat newChild, IFormat refChild);
        bool Remove(IFormat format);
        void Replace(IFormat oldFormat, IFormat newFormat);

        // Properties
        int Count { get; }
        IFormat this[int index] { get; }
        IFormat this[string name] { get; }

    }
}
