using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
   public class FormatUtility
    {
       public static byte[] swapDouble(byte[] origin)
       {
           if (origin.Length == 8)
           {
               return new byte[] { origin[7], origin[6], origin[5], origin[4], origin[3], origin[2], origin[1], origin[0] };
           }
           return new byte[8];
       }

       public static byte[] swapFloat(byte[] origin)
       {
           if (origin.Length == 4)
           {
               return new byte[] { origin[3], origin[2], origin[1], origin[0] };
           }
           return new byte[4];
       }

 

 

    }
}
