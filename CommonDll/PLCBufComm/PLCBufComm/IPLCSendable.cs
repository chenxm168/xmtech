using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
   public interface IPLCSendable:IDisposable
    {
       int SendMessage(string message);

       bool Open();

       bool IsOpen();

       int SendByte(byte[] bytes);



    }
}
