using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.Utility
{
    internal class CSharpUtil
    {
        public static string CurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.SSS");
        }

        public static long currentTimeMillis()
        {
            return ((DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L);
        }
    }
}
