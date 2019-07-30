using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIBMessageIo
{

    public class TIBAdapter : ISendable, IDisposable
    {
        //private TIBMessageService tib;

        public TIBMessageService TIB
        {
            get;
            set;
        }

        public TIBAdapter(TIBMessageService tibco)
        {
            TIB = tibco;
        }
        public TIBAdapter()
        {

        }

        public void Send(object msg)
        {
            var m = msg as TIBMessageIo.MessageSet.AbstractMessage;

            TIB.Send(m.ToXmlString());
        }

        public object SendRequest(object msg)
        {
            string m = msg as string;

            return TIB.SendRequest(m);
        }

        public void Dispose()
        {
            TIB.Dispose();
        }
    }
}
