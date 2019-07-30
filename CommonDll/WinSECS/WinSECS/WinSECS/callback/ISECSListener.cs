using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.structure;
using WinSECS.timeout;

namespace WinSECS.callback
{
   public interface ISECSListener
    {
        void onConnected(string driverID);
        void onDisconnected(string driverID);
        void onIllegal(string driverID, SECSTransaction transaction, string illegalMessage);
        void onLog(string driverID, string log);
        void onReceived(string driverID, SECSTransaction transaction);
        void onSendComplete(string driverID, SECSTransaction transaction);
        void onSendFailed(string driverID, SECSTransaction transaction);
        void onTimeout(string driverID, SECSTimeout timeout);
        void onUnknownReceived(string driverID, SECSTransaction transaction);

    }
}
