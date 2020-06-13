using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Server
{
  public  interface IMessageHandler
    {
      void doWork(object ob);
    }

  public delegate void  MessageEventHandler(object sender, object[] args);
}
