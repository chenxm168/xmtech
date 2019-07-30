using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDT.SECS
{
  public  interface ISecsHandler
    {
      void doWork(string driverId, object message);

    }
}
