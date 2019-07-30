using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.callback
{
   public interface Callback
    {
        object getObject();
        int getType();

    }
}
