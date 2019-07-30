using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Common;

namespace EQPIO.Controller.Proxy
{
   public interface IMapInfo
    {
       Transaction getTrx(string local);
       BlockMap getBlockMap(string local);
    }
}
