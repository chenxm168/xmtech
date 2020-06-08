using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSrv
{
   public interface IFileWriter:IDisposable
    {
       void Write(string message, string file,FileMode filemode);
       void WriteAsyn(string message, string file, FileMode filemode);
    }
}
