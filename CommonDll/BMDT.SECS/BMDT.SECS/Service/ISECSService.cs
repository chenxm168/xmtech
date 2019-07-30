using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSECS.structure;

namespace BMDT.SECS.Service
{
   public interface ISECSService
    {
       bool Send_S5F1_AlarmReport(String alst, String alcd, String alid, String altx, String unitid);
       bool Send_S2F17_DateTimeRequest();
       bool Send_S6F11_ControlStateOfflineChangeReport(String eqst, String unitid);
       bool Send_S6F11_ControlStateLocalChangeReport( String eqst, String unitid);
       bool Send_S6F11_EQPStateChangeReport(String crst, String eqst, String unitid);
       bool Send_S6F113_CurrentEQPDataReport(string unitid, Dictionary<string, string> items);

       bool Send_S1F1_AreYouThere();

       bool Send_S1F0(long systembyte);

       bool Reply(SECSTransaction trx);

       bool Reply_RequestOnLineAck_NoGood(long systembyte);

       bool Reply_RequestOnLineAck_AlreadyLocal(long systembyte);

       bool Reply_RequestOnLineAck_Ok(long systembyte);

       bool Send_S9F7();
       
    }
}
