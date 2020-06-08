using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MPC
{
   public class Class1
    {
       public void test()
       {
           var c = LogManager.GetLogger(typeof(Class1));
           c.Debug("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
       }
    }
}
