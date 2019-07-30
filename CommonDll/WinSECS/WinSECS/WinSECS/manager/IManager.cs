using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.global;
using WinSECS.driver;

namespace WinSECS.manager
{
    internal interface IManager
    {
        // Methods
        void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnobject);
        void ReloadConfig(SECSConfig newConfig, bool enforceReconnect, bool reloadSMD, ReturnObject returnobject);
        void Terminate(ReturnObject returnObject);
    }

 

}
