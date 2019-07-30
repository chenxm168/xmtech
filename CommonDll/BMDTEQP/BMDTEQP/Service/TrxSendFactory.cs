using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using EQPIO.Controller.Proxy;
using EQPIO.MessageData;
using EQPIO.Common;
using log4net;
using HF.BC.JSON;

namespace BMDTEQP.Service
{
   public class TrxSendFactory
    {
       protected ILog logger = LogManager.GetLogger(typeof(TrxSendFactory));
       public ControlManager PLCManager
       {
           get;
           set;
       }

       public Trx getTrxSend(string name)
       {
        return   getTrxSend("L2", name);


       }


       public Trx getTrxSend(string local, string name )
      {
          Send send = null;

          if (PLCManager.UseEthernet)
          {
              PLCMap map;
              if(PLCManager.getMProtocolProxy().MapList.TryGetValue(local,out map))
              {
                  send = map.transaction.Send;

              }else
              {
                  return null;
              }
              
          }
          if (PLCManager.UseBoard)
          {
              send = PLCManager.getMNetProxy().EQPPlcMap.transaction.Send;
          }

          Trx trx = null;
          foreach (Trx trx1 in send.Trx)
          {
              if (trx1.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()) || trx1.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
              {
                  trx = trx1;
                  break;
              }
          }

          if (trx == null)
          {
              return null;
          }

          JSONConverter<Trx> JC = new JSONConverter<Trx>();

          string s = JC.ObjectToString(trx);

          return JC.StringToObject(s);
       }


       public MessageData<PLCMessageBody> MakeCIMMessageSet (string local,string trxname,string evName,string messageid,string text)
       {
           var trx = getTrxSend(trxname);

           MessageData<PLCMessageBody> msg = new MessageData<PLCMessageBody>();
           msg.MessageName = evName;
           msg.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
           msg.ReturnMessage = "0";
           msg.ReturnMessage = string.Empty;
           msg.MachineName = local;
    

               return null;
       }
    }
}
