using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using WinSECS.global;
using WinSECS.structure;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class SyncActivator
    {
        internal static ReturnObject returnObject = new ReturnObject();
        internal static SyncActivator singleInstance;
        private static Dictionary<long, object> syncMap = new Dictionary<long, object>();

        public virtual void getSyncActor(long Systembyte, SECSTransaction receivedTrx)
        {
            object obj2 = syncMap[Systembyte];
            syncMap.Remove(Systembyte);
            if (obj2 != null)
            {
                returnObject.setReturnData(receivedTrx);
                Monitor.Pulse(obj2);
            }
        }

        public virtual ReturnObject setSysncActor(long Systembyte)
        {
            object obj2;
            obj2 = obj2 = syncMap[Systembyte];
            if (obj2 == null)
            {
                obj2 = new object();
                syncMap.Add(Systembyte, obj2);
            }
            try
            {
                Monitor.Wait(obj2, TimeSpan.FromMilliseconds(5000.0));
                returnObject.setError(SEComError.SEComETC.ERR_FAIL_TO_GET_SYNC_REPLY);
            }
            catch (ThreadInterruptedException)
            {
            }
            return returnObject;
        }

        public static SyncActivator This()
        {
            if (singleInstance == null)
            {
                singleInstance = new SyncActivator();
            }
            return singleInstance;
        }
    }
}
