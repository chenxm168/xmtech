using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDTEQP.Service
{
    public  interface ICommandService
    {

        void SendCIMMessageSetCommand(string local, string text);
        void SendControlStateChangeCommand(string local, int state);
        void SendDateTimeSetCommand(string local, string time);

        void SendEquipmentDownCommand(string local);

    }
}
