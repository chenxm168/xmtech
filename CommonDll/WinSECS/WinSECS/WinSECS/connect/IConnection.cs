using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WinSECS.connect
{
    internal interface IConnection
    {
        // Methods
        TcpClient Connect();
    }


}
