
namespace HF.BC.Tool.EIPDriver.Driver.EventHandler
{
    using HF.BC.Tool.EIPDriver.Data;
    using System;
    using System.Runtime.CompilerServices;

    public class EIPEventHandler
    {
        private EIPEventHandler()
        {
        }

        public delegate void ConnectEventHandler();

        public delegate void DisconnectEventHandler(object sender, string errmsg);

        public delegate void ReceiveCompleteEventHandler(object sender, Trx trx);

        public delegate void SVDataEventHandler(object sender, Trx trx);
    }
}
