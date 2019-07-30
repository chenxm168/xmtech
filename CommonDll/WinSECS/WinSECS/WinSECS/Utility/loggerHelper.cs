using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class loggerHelper
    {

        public static string getExceptionString(Exception e)
        {
            return (" (" + e.ToString() + ") : " + e.Message);
        }

 


    }
}
