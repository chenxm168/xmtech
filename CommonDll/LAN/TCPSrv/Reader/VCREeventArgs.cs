using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPSrv.Reader
{
   public class VCREeventArgs:EventArgs
    {
       public string Message
       {
           get;
           set;
       }

       public Object[] Args
       {
           get;
           set;
       }

       public VCREeventArgs(string message)
       {
           this.Message = message;
       }

       public VCREeventArgs(object[] args)
       {
           this.Args = args;
       }

       public VCREeventArgs(string message,object[] args)
       {
           this.Args = args;
           this.Message = message;
       } 
    }
}
