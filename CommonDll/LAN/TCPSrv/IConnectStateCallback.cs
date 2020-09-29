using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPSrv
{
  public  interface IConnectStateCallback
    {
      void ConnectSuccess(object sender,string message);
      void ConnectFail(object sender, string messsage);
      void DisconnectSuccess();

    }
}
