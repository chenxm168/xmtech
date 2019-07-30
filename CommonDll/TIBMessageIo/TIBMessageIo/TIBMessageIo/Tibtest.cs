using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;
using log4net;

namespace TIBMessageIo
{
   public class Tibtest
    {
       private Transport transport;
       private ILog logger = LogManager.GetLogger(typeof(Tibtest));
       public Tibtest()
       {

           init();
       }

       public void init()
       {
           try
           {
               TIBCO.Rendezvous.Environment.Open();
               transport = new NetTransport("8200", ";225.11.11.2", "tcp:10.116.111.201:7500");
           }
           catch (Exception e)
           {

               logger.Error(e.StackTrace);
           }
       }

       public void send(string msg)
       {
           Message message = new Message();
           message.SendSubject = "TRULY.G5A.MES.TST.FAB.PEMsvr";
           message.AddField("xmlData", msg, 0);
           transport.Send(message);

       }




    }
}
