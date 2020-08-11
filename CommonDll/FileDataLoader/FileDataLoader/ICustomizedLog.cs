using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataLoader
{
   public  interface ICustomizedLog
    {
       void debug(string log);
       void info(string log);

       void error(string log);



    }
}
