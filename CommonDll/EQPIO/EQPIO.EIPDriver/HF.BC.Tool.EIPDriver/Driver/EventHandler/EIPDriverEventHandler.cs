
namespace HF.BC.Tool.EIPDriver.Driver.EventHandler
{
    using System;
    using System.Runtime.CompilerServices;

    internal class EIPDriverEventHandler
    {
        private EIPDriverEventHandler()
        {
        }

        public delegate void ReadCompleteEventHandler(byte[] readbytes, long sendTime, long recvTime);

        public delegate void ReadErrorEventHandler(string errmsg);

        public delegate void WriteCompleteEventHandler(object sendObj, byte[] sendbytes);

        public delegate void WriteErrorEventHandler(string errmsg);
    }
}
