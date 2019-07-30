using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQPIO.Controller
{
   public  interface IEPQEventHandler
    {
      void  EQPEventProcess(object message);

    }
}
