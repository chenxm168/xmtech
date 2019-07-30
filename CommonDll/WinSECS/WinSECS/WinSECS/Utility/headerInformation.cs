using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.Utility
{
   public class headerInformation
    {

       public static string getControlMessageType(byte[] header)
       {
           switch (header[5])
           {
               case 1:
                   return "Select.req";

               case 2:
                   return "Select.rsp";

               case 5:
                   return "Linktest.req";

               case 6:
                   return "Linktest.rsp";

               case 7:
                   return "Reject.req";

               case 9:
                   return "Separate.req";
           }
           return "Unknown control message";
       }

 

    }
}
