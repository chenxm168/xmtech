using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.global;
using WinSECS.structure;
using WinSECS.timeout;

namespace WinSECS.driver
{

        public interface ISingleplugIn
        {
            // Methods
            IReturnObject GetDefinedMessage(int Stream, int Function, string MessageName);
            IReturnObject Initialize();
            IReturnObject Initialize(ISECSConfig config);
            IReturnObject Initialize(string driverId);
            IReturnObject Initialize(string driverId, string SEComINIXMLFIlePath);
            void reconnect();
            IReturnObject ReloadConfiguration(ISECSConfig newConfig, bool enforceReconnect, bool reloadSMD);
            IReturnObject ReloadSMD(ISECSConfig newConfig);
            IReturnObject reply(ISECSTransaction message);
            IReturnObject request(ISECSTransaction message);
            IReturnObject Terminate();
        }

        public delegate void driverIDDelegate(string driverID);

        public delegate void logDelegate(string driverID, string log);

        public delegate void timeoutDelegate(string driverID, SECSTimeout timeout);

        public delegate void transactionDelegate(string driverID, SECSTransaction trx);





 

    
}
