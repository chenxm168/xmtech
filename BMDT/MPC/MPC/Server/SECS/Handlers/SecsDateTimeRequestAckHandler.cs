using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDTEQP.Service;
using BMDT.SECS.Service;
using BMDT.DB.Service;
using BMDT.SECS;
using WinSECS.structure;
using BMDT.SECS.Message;
using System.Globalization;
using BMDTEQP.Service;

namespace MPC.Server.SECS.Handlers
{
    class SecsDateTimeRequestAckHandler : AbsSecsHandler
    {
        protected override void proc(string driverId, object message)
        {
            var msg = message as SECSTransaction;
            S2F18_DateTimeDataRequestAck rMsg = new S2F18_DateTimeDataRequestAck(msg);
            try
            {
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.LongDatePattern = "yyyyMMddHHmmss";
                DateTime dt = DateTime.ParseExact(rMsg.TIME, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                if(dt!=null)
                {
                    string sDate = dt.ToString("yyMMddHHmmss");
                    var cmdSrv = ObjectManager.getObject("commandService") as ICommandService;

                    cmdSrv.SendDateTimeSetCommand("L2", sDate);

                }

            }catch(Exception e)
            {
                logger.ErrorFormat("Date Format Error:[{0}]",e.Message);
            }


        }
    }
}
