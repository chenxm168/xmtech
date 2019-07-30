using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBMessageIo.MessageSet;

namespace MPC.Server.TIB
{
   public class AreYouThereResponseHandler:IMessageHandler
    {
       public event MessageEventHandler OnAreYouThereResponse;
        public void doWork(object ob)
        {
           var msg= MessageUtils.Convert<AreYouThereRequest>((string)ob);
            if(OnAreYouThereResponse!=null)
            {
                OnAreYouThereResponse(this,new  object[]{msg});
            }
        }
    }
}
