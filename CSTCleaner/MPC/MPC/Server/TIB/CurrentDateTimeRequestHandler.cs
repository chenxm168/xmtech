using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using TIBMessageIo.MessageSet;

namespace MPC.Server.TIB
{
   public class CurrentDateTimeRequestHandler:IMessageHandler
    {
       public event MessageEventHandler OnCurrentDataTimeRequest;
        public void doWork(object ob)
        {
            var msg = MessageUtils.Convert<CurrentDateTimeRequest>((string)ob);
            if(msg!=null)
            {
                Reply(msg);
            }
            if(OnCurrentDataTimeRequest!=null)
            {
                OnCurrentDataTimeRequest(this, new object[] { msg });
            }
        }

       private void Reply(object msg)
        {
            var ob = msg as CurrentDateTimeRequest;
            ob.Header.MESSAGENAME = "CurrentDateTimeResponse";
            ob.Body.ACKNOWLEDGE = "Y";
            var s = ObjectManager.getObject("TibSender") as TIBMessageIo.ISendable;
            s.Send(ob);
        }
    }
}
