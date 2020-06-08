using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSrv.csv
{
   public interface ICSVWriter
    {

       void WriteCSV(string[] args,FileMode filemode);
       void WriteCSVAsyn(string[] args,FileMode filemode);

    }
}
