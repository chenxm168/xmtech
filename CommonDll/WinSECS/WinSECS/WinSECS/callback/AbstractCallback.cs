using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.callback
{
    public abstract class AbstractCallback : Callback
    {
        protected internal object object_Renamed;

        public AbstractCallback(object object_Renamed)
        {
            this.object_Renamed = object_Renamed;
        }

        public virtual object getObject()
        {
            return this.object_Renamed;
        }

        public abstract int getType();
    }
}
