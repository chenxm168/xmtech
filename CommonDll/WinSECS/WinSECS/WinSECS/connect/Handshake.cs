using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.connect
{
    internal enum Handshake
    {
        ACK = 6,
        ENQ = 5,
        EOT = 4,
        NAK = 0x15,
        TIMEOUT = -1
    }

 

}
