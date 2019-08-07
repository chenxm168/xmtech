using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using EQPIO.MessageData;
using EQPIO.Common;
using EQPIO.Controller.Proxy;

namespace MPC.Server.EQP
{
   public class PLCRequest
    {
       public ControlManager Plc
       { get; set; }

       public void SendRequest(string trx,string action)
       {

           switch(action)
           {

               case "R":
                   {
                       PLCReadRequest(trx);
                       break;
                   }

               case "W":
                   {
                       break;
                   }

           }


       }//end function


       private void PLCReadRequest(string trx)
       {
           if (Plc.UseEthernet)
           {

               PLCMap pMap;

               Trx sTrx = null;

               if (Plc.getMProtocolProxy().MapList.TryGetValue("L2", out pMap))
               {
                   var mTrx = pMap.transaction.Send;

                   foreach (Trx t in mTrx.Trx)
                   {
                       if (t.Name.ToUpper().Equals(trx.ToUpper()))
                       {
                           sTrx = t;
                           break;
                       }
                   }

                   if (sTrx != null)
                   {

                       MessageData<PLCMessageBody> message = new MessageData<PLCMessageBody>();

                       message.MessageName = "ReadRequest";
                       message.Transaction = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_Server";
                       message.MessageType = "ETHERNET";
                       message.MachineName = "L2";
                       PLCMessageBody body = new PLCMessageBody();
                       body.EventName = trx;

                       Dictionary<string, Dictionary<string, string>> rList = new Dictionary<string, Dictionary<string, string>>();
                       foreach (MultiBlock mb in sTrx.MultiBlock)
                       {
                           if (mb.Action == "R")
                           {
                               foreach (Block b in mb.Block)
                               {
                                   Dictionary<string, string> iList = new Dictionary<string, string>();

                                   foreach(var b2 in pMap.blockMap.Block)
                                   {
                                       if(b2.Name.ToUpper().Equals(b.Name.ToUpper()))
                                       {
                                           foreach(var item in b2.Item)
                                           {
                                               iList.Add(item.Name,"");
                                           }
                                       }
                                   }
                                  
                                   rList.Add(b.Name, iList);
                               }
                           }
                       }
                       message.MessageBody = body;
                       message.MessageBody.ReadDataList = rList;
                       Plc.RequestForServer(message);


                   }

               }
           }
       }//end function
    }
}
