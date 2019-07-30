using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQPIO.Controller;
using EQPIO.Controller.Proxy;
using EQPIO.Common;
using EQPIO.MessageData;
using HF.BC.JSON;

namespace BMDTEQP.Service
{
    public  class CommandServiceImpl:AbsCommandService,  ICommandService
    {
        long messageId;

        public void SendCIMMessageSetCommand(string local, string text)
        {
            string messageid = getMessageID().ToString();
            MessageData<PLCMessageBody> msg = new MessageData<PLCMessageBody>();
            PLCMessageBody body = new PLCMessageBody();
            msg.MessageBody = body;

            msg.MessageName = "WriteRequest";
            msg.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            msg.MessageBody.EventName = local.ToUpper() + "_CIMMessageSetCommand";
            msg.ReturnMessage = "0";
            msg.ReturnMessage = string.Empty;
            msg.MachineName = local;
            msg.MessageType = PLCManager.UseBoard ? "MNET" : "ETHERNET";
            
            JSONConverter<Block> JC = new JSONConverter<Block>();
            var blockMap = getBlockMap(local);
            // messageid = local.ToUpper()+"__W_CIMMessageSetCommandBlock";
            Block bk = null;
            Dictionary<string, string> dValue = new Dictionary<string, string>();
            /*
            foreach(Block bk1 in blockMap.Block)
            {
                if(bk1.Name.Trim().ToUpper().Equals(messageid))
                {
                    bk =  JC.StringToObject(JC.ObjectToString(bk1)) ;
                    break;
                }
            }

            if(bk!=null)
            {
                foreach(Item item  in bk.Item)
                {
                    if(item.Name=="CIMMessage")
                    {
                        int ln = Convert.ToInt16(item.Points);
                        item.Value = text;
                        continue;
                        dValue.Add("CIMMessage", text);
                    }
                    if (item.Name == "CIMMessageID")
                    {
                        dValue.Add("CIMMessageID", messageid);
                    }
                }
            } */
            if(text.Length>40)
            {
                text = text.Substring(0, 40);
            }
            else
            {
                text = text.PadRight(40, ' ');
            }
            dValue.Add("CIMMessage", text);
            dValue.Add("CIMMessageID", messageid);
            Dictionary<string, Dictionary<string, string>> WriteList = new Dictionary<string, Dictionary<string, string>>();

            WriteList.Add(local.ToUpper() + "_W_CIMMessageSetCommandBlock", dValue);
            Dictionary<string,string> d = new Dictionary<string,string>();
            d.Add("CIMMessageSetCommand","1");
            WriteList.Add(local.ToUpper() + "_B_CIMMessageSetCommand", d);

            msg.MessageBody.WriteDataList = WriteList;
            PLCManager.RequestForServer(msg);

        }


        public void SendControlStateChangeCommand(string local, int state)
        {
            MessageData<PLCMessageBody> msg = new MessageData<PLCMessageBody>();
            PLCMessageBody body = new PLCMessageBody();
            msg.MessageBody = body;

            msg.MessageName = "WriteRequest";
            msg.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            msg.MessageBody.EventName = local.ToUpper() + "_ControlStateChangeCommand";
            msg.ReturnMessage = string.Empty;
            msg.ReturnMessage = string.Empty;
            msg.MachineName = local;
            msg.MessageType = PLCManager.UseBoard ? "MNET" : "ETHERNET";
            Dictionary<string, string> dv2 = new Dictionary<string, string>();

            dv2.Add("ControlState", state.ToString());

            Dictionary<string, string> dv = new Dictionary<string, string>();
            dv.Add("ControlStateChangeCommand", "1");

            msg.MessageBody.WriteDataList.Add(local.ToUpper() + "_W_ControlStateChangeCommandBlock", dv2);
            msg.MessageBody.WriteDataList.Add(local.ToUpper() + "_B_ControlStateChangeCommand", dv);
            PLCManager.RequestForServer(msg);
        }

        public void SendDateTimeSetCommand(string local, string time)
        {
            MessageData<PLCMessageBody> msg = new MessageData<PLCMessageBody>();
            PLCMessageBody body = new PLCMessageBody();
            msg.MessageBody = body;

            msg.MessageName = "WriteRequest";
            msg.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            msg.MessageBody.EventName = local.ToUpper() + "_DateTimeSetCommand";
            msg.ReturnMessage = string.Empty;
            msg.ReturnMessage = string.Empty;
            msg.MachineName = local;
            msg.MessageType = PLCManager.UseBoard ? "MNET" : "ETHERNET";
            Dictionary<string, string> dv2 = new Dictionary<string, string>();

            dv2.Add("DateTime", time);

            Dictionary<string, string> dv = new Dictionary<string, string>();
            dv.Add("DateTimeSetCommand", "1");

            msg.MessageBody.WriteDataList.Add(local.ToUpper() + "_W_DateTimeSetCommandBlock", dv2);
            msg.MessageBody.WriteDataList.Add(local.ToUpper() + "_B_EquipmentDownCommand", dv);
            PLCManager.RequestForServer(msg);
        }

        public void SendEquipmentDownCommand(string local)
        {
            MessageData<PLCMessageBody> msg = new MessageData<PLCMessageBody>();
            PLCMessageBody body = new PLCMessageBody();
            msg.MessageBody = body;

            msg.MessageName = "WriteRequest";
            msg.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            msg.MessageBody.EventName = local.ToUpper() + "_EquipmentDownCommand";
            msg.ReturnMessage = string.Empty;
            msg.ReturnMessage = string.Empty;
            msg.MachineName = local;
            msg.MessageType = PLCManager.UseBoard ? "MNET" : "ETHERNET";
            Dictionary<string, string> dv = new Dictionary<string, string>();
            dv.Add("CIMMessageSetCommand", "1");
            msg.MessageBody.WriteDataList.Add(local.ToUpper() + "_B_EquipmentDownCommand", dv);
            PLCManager.RequestForServer(msg);

        }

        long getMessageID()
        {
            if(messageId>=999999)
            {
                messageId = 0;
            }
            return messageId;
        }

    }
}
