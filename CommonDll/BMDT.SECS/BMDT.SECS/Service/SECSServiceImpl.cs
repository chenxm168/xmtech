using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.SECS.Message;
using System.Collections;
using log4net;


namespace BMDT.SECS.Service
{
  public  class SECSServiceImpl:IDisposable,ISECSService
    {
      ILog logger = LogManager.GetLogger(typeof(SECSServiceImpl));


      

     public  SECSWrapper Secs
      {
          get;
          set;
      }
      const string CONTROL_REMOTE = "1";
      const string CONTROL_LOCAL = "2";
      const string CONTROL_OFFLINE = "0";
      const string LOCAL_CEID = "112";
      const string OFFLINE_CEID = "111";
      const string EQP_STATE_CEID = "114";

      public bool Send_S5F1_AlarmReport(String alst, String alcd, String alid, String altx, String unitid)
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          if(alid.Length>ConstDef.ALID_LEN)
          {
              alid = alid.Substring(alid.Length - ConstDef.ALID_LEN);
          }else
          {
              if(alid.Length!=ConstDef.ALID_LEN)
              {
                  alid = alid.PadLeft(5, '0');
              }
          }
          if(unitid.Length>ConstDef.UNNIT_LEN)
          {
              unitid.Substring(0, ConstDef.UNNIT_LEN);
          }else
          {
              unitid = unitid.PadRight(ConstDef.UNNIT_LEN, ' ');
          }

          if(altx.Length>ConstDef.ALTX_LEN)
          {
              altx = altx.Substring(0, ConstDef.ALTX_LEN);
          }else
          {
              altx = altx.PadRight(ConstDef.ALTX_LEN, ' ');
          }
          
          var alarm = S5F1_AlarmReport.makeTransaction(false, alst, alcd, alid, altx, unitid);
         return Secs.SendRequest(alarm).isSuccess();
      }

      public bool Send_S2F17_DateTimeRequest()
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S2F17_DateTimeRequest.makeTransaction(false);
         var rtn =  Secs.SendRequest(trx);
          return rtn.isSuccess();
      }

      //protected bool Send_S6F11_StateChangeReport(String ceid, String rptid, String crst, String eqst, String unitid)
      //{
      //    var trx = S6F11_ControlStateChangeReport.makeTransaction(false, ceid, rptid, crst, eqst, unitid);
          
      //    return Secs.SendRequest(trx).isSuccess();
      //}
      public bool Send_S6F11_ControlStateOfflineChangeReport( String eqst, String unitid)
      {
         // var trx = S6F11_ControlOfflineChangeReport.makeTransaction(false, OFFLINE_CEID, "100", CONTROL_OFFLINE, eqst, unitid);
        //  return Secs.SendRequest(trx).isSuccess();
          if(!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S6F11_ControlOfflineChangeReport.makeTransaction(false, OFFLINE_CEID, "100", CONTROL_OFFLINE, eqst, unitid);
          return Secs.SendRequest(trx).isSuccess();
          //return Send_S6F11_StateChangeReport(OFFLINE_CEID, "100", CONTROL_OFFLINE, eqst, unitid);
      }

      public bool Send_S6F11_ControlStateLocalChangeReport( String eqst, String unitid)
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S6F11_ControlLocalChangeReport.makeTransaction(false, LOCAL_CEID, "100", CONTROL_LOCAL, eqst, unitid);
          return Secs.SendRequest(trx, "S6F12_ControlLocalChangeReportAck").isSuccess();
          
      }
      

      public bool Send_S6F11_EQPStateChangeReport(String crst, String eqst, String unitid)
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S6F11_EQPStateChangeReport.makeTransaction(false, "114", "100", crst, eqst, unitid);

          return Secs.SendRequest(trx).isSuccess();
          //return Send_S6F11_StateChangeReport( EQP_STATE_CEID, "100", crst, eqst, unitid);
         
      }

      public bool Send_S6F113_CurrentEQPDataReport(string unitid, Dictionary<string, string> items)
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          List<S6F113_CurrentEQPDataReport_> dvs = new List<S6F113_CurrentEQPDataReport_>();
          foreach(KeyValuePair<string,string> item in items)
          {
              dvs.Add(new S6F113_CurrentEQPDataReport_(item.Key, item.Value));
          }

          var trx = S6F113_CurrentEQPDataReport.makeTransaction(false, unitid, dvs);
          return Secs.SendRequest(trx).isSuccess();
      }

      public bool Send_S1F1_AreYouThere()
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S1F1_AreYouThere.makeTransaction(false);
          return Secs.SendRequest(trx).isSuccess();
      }

      public bool Send_S1F0(long systembyte)
      {
          if (!Secs.IsConnected)
          {
              logger.Error("Secs Not Connected!");
              return false;
          }
          var trx = S1F0.makeTransaction(false,systembyte);
          return Secs.SendRequest(trx).isSuccess();
      }



      public void Dispose()
      {
          
      }


      public bool Reply(WinSECS.structure.SECSTransaction trx)
      {
          return Secs.SendRequest(trx).isSuccess();
      }


      public bool Reply_RequestOnLineAck_NoGood(long systembyte)
      {
          var trx = S1F18_RequestOnLineAck.makeTransaction(false, ConstDef.LOCAL_CONDITION_NG.ToString());
          trx.Systembyte = systembyte;
          return Secs.SendRequest(trx).isSuccess();
      }


      public bool Reply_RequestOnLineAck_AlreadyLocal(long systembyte)
      {
          var trx = S1F18_RequestOnLineAck.makeTransaction(false,ConstDef.ALREADY_LOCAL.ToString());
          trx.Systembyte = systembyte;
          return Secs.SendRequest(trx).isSuccess();
      }


      public bool Reply_RequestOnLineAck_Ok(long systembyte)
      {
          var trx = S1F18_RequestOnLineAck.makeTransaction(false, ConstDef.LOCAL_OK.ToString());
          trx.Systembyte = systembyte;
          return Secs.SendRequest(trx).isSuccess();
      }


      public bool Send_S9F7()
      {
         // var trx = IllegalData.makeTransaction(false,"")
          return true;
      }

      
    }
}
