using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPSrv.Reader
{
   public  interface IVCReader
    {
        void Connect();
        void ConnectAsyn();

        void StartRead();

        void StopRead();



    }
}
