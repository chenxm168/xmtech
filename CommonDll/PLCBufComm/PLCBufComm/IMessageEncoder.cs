using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
   public  interface IMessageEncoder
    {

       int  getMessageString(byte[] bytes,out string message);

       int getMessageLen(byte[] bytes);

       byte[] getRtnBytes(byte[] bytes);

      // byte[] getBytesCode(string message);

       int getSendResponseCode(byte[] bytes);
       byte[] getSendBytes(string message);

       int getValueStringHL(byte[] bytes,out string values);

       int getValueStringLH(byte[] bytes ,out string values);

    }
}
