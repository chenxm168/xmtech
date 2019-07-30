using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.global
{
   public  interface IReturnObject
    {
        string getErrorDescription();
        object getReturnData();
        bool isSuccess();

    }
}
