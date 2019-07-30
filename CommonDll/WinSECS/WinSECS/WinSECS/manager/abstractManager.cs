using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.global;
using WinSECS.driver;

namespace WinSECS.manager
{
    internal abstract class abstractManager : IManager
    {
        protected internal SECSConfig config;
        protected internal SinglePlugIn rootHandle;

        protected abstractManager()
        {
        }

        public virtual void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            this.rootHandle = rootHandle;
            this.config = config;
            returnObject.setError(0);
        }

        public virtual void ReloadConfig(SECSConfig newConfig, bool enforceReconnect, bool reloadSMD, ReturnObject returnobject)
        {
            this.config = newConfig;
        }

        public virtual void Terminate(ReturnObject returnObject)
        {
        }
    }
}
