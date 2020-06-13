using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
   public  class PLCListenerFactory
    {

       public static PLCListener getPlcTCPListener(string ip, int port)
       {
           return new PLCTcpListener(ip, port);
       }


    }
}
