using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPSrv
{
   public interface IlinstenerCallBack
    {
       void CallBack(object sender, byte[] message);
    }
}
