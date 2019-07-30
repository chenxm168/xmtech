using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.global
{
    [ComVisible(false)]
    internal class EnumSet
    {
        internal enum CONNECTION
        {
            Connected,
            Disconnected,
            Selected
        }

        internal enum REPORTTYPE
        {
            TimeOut,
            Connection,
            Transaction
        }

        internal enum TIMEOUT
        {
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8
        }

        internal enum TRANSACTION
        {
            SEND,
            RECV,
            REQT
        }
    }
}
