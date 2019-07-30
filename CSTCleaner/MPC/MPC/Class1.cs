using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EQPIO.Controller;


namespace MPC
{
   public class Class1
    {
       public static ControlManager cm = null;
       public void test()
       {
           var c = LogManager.GetLogger(typeof(Class1));
           c.Debug("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
       }

       public static void EQPIOTest()
       {
           ControlManagerFactory factory = new ControlManagerFactory();
           cm = factory.getControlManager();
          // cm.Init();
       }

       public static void CloseEQPIO()
       {
           if(cm!=null)
           {
               cm.Dispose();
           }
       }

    }
}
