using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIBMessageIo
{
   public  interface ISendable:IDisposable
    {
        void Send(object msg);
        object SendRequest(Object msg);
    }
}
