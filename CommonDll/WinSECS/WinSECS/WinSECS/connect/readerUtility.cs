using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using WinSECS.logger;
using WinSECS.global;
using WinSECS.driver;
using WinSECS.structure;
using WinSECS.Utility;

namespace WinSECS.connect
{
    internal class readerUtility
    {
        private SECSConfig config = null;
        private int deviceId = 0;
        private bool isUseRawByte = false;
        private ILog logger;
        private LoggerManager loggerMgr;
        private int overRawByteLimit = -1;
        private SinglePlugIn rootHandle;

        public readerUtility(SinglePlugIn rootHandle)
        {
            this.rootHandle = rootHandle;
            this.deviceId = rootHandle.Config.DeviceId;
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
            this.config = rootHandle.Config as SECSConfig;
            if (rootHandle.Config.UseRawBinary)
            {
                this.isUseRawByte = true;
                this.overRawByteLimit = rootHandle.Config.OverRawBinaryLength;
            }
        }

        private SECSTransaction checkAutoReply(SECSTransaction receivedMsg)
        {
            SECSTransaction transaction = null;
            ReturnObject definedMessageFirstItem =new ReturnObject();
            string pairName = receivedMsg.PairName;
            if (!pairName.Equals(""))
            {
                definedMessageFirstItem = this.rootHandle.GetDefinedMessage(receivedMsg.Stream, receivedMsg.Function, pairName) as ReturnObject;
                if (definedMessageFirstItem.isSuccess())
                {
                    return (SECSTransaction)definedMessageFirstItem.getReturnData();
                }
                this.logger.Warn("Fail To find reply message DESC=" + definedMessageFirstItem.getErrorObject().getErrorDiscription());
                return transaction;
            }
            definedMessageFirstItem = this.rootHandle.ManagerFactory.MessageFactory.GetDefinedMessageFirstItem(receivedMsg.Stream, receivedMsg.Function + 1);
            if (definedMessageFirstItem.isSuccess())
            {
                return (SECSTransaction)definedMessageFirstItem.getReturnData();
            }
            this.logger.Warn("Fail To find reply message DESC=" + definedMessageFirstItem.getErrorObject().getErrorDiscription());
            return transaction;
        }

        private bool checkDecoding(SECSTransaction message)
        {
            if (!message.ControlMessage)
            {
                try
                {
                    SECSException exception;
                    if (this.isUseRawByte)
                    {
                        exception = message.decoding(this.overRawByteLimit);
                    }
                    else
                    {
                        exception = message.decoding();
                    }
                    if (exception != null)
                    {
                        this.logger.Error(exception.Message + " " + message.getLogType());
                        return false;
                    }
                }
                catch (Exception exception2)
                {
                    this.logger.Error("[HSMSReader][checkDecoding] (" + exception2.ToString() + ") DURING MESSAGE DECODING Exception", exception2);
                    return false;
                }
            }
            return true;
        }

        public void doDispatching(SECSTransaction message, bool isSECS1Mode)
        {
            message.ReceivedTime = CSharpUtil.CurrentTime();
            if (!isSECS1Mode && !(message.ControlMessage || this.rootHandle.ManagerFactory.ConnectManager.Sendable))
            {
                this.rootHandle.ManagerFactory.ConnectManager.sendRejectRequest(4);
            }
            if (!(message.ControlMessage || (message.DeviceId == this.deviceId)))
            {
                this.rootHandle.ManagerFactory.ConnectManager.sendS9Fx(1, message);
                this.logger.Warn("Invalid Message: Illegal deviceID message Header = " + ByteToObject.byte2Binary(message.Header));
                this.loggerMgr.WriteInvalidLog(message, "Illegal_DeviceID");
                this.rootHandle.ManagerFactory.CallbackManager.onIllegal(this.config.DriverId, message, "Illegal_DeviceID");
            }
            else if (((this.rootHandle.Config.BaseMessageFilteringSize > 0x3e8) && ((message.Body != null) && (message.Body.Length >= this.rootHandle.Config.BaseMessageFilteringSize))) && !this.rootHandle.ManagerFactory.MessageFactory.isExistLengthFilter(message.StreamFunctionString, message.Body.Length))
            {
                this.loggerMgr.writeUnknownSECS1ByLengthFilter(message, "UNKNOWN_MESSAGE_BY_LENGTH_FILTER");
            }
            else if (!this.checkDecoding(message))
            {
                this.rootHandle.ManagerFactory.ConnectManager.sendS9Fx(7, message);
                this.loggerMgr.writeInvalidSECS1(message, "Illegal Format");
                this.rootHandle.ManagerFactory.CallbackManager.onIllegal(this.config.DriverId, message, "Illegal Format");
            }
            else if (!this.stopTimeoutAndInputCorrelationObject(message))
            {
                this.loggerMgr.WriteInvalidLog(message, "Illegal Secondary Message");
                this.rootHandle.ManagerFactory.CallbackManager.onIllegal(this.config.DriverId, message, "Illegal Secondary Message");
            }
            else
            {
                if (this.rootHandle.Config.DispatchOn && !message.ControlMessage)
                {
                    ReturnObject returnObject = new ReturnObject();
                    this.rootHandle.ManagerFactory.MessageFactory.CheckDefinedMessage(message, returnObject);
                    if (!returnObject.isSuccess())
                    {
                        this.loggerMgr.WriteUnknownLog(message);
                        this.logger.Warn(returnObject.getErrorObject().getErrorDiscription());
                        this.rootHandle.ManagerFactory.CallbackManager.onUnknownReceived("", message);
                        this.rootHandle.ManagerFactory.ConnectManager.sendS9Fx(3, message);
                        return;
                    }
                    SECSTransaction trx = (SECSTransaction)returnObject.getReturnData();
                    this.loggerMgr.WriteLog(trx, true);
                    this.rootHandle.ManagerFactory.CallbackManager.onReceived("", (SECSTransaction)returnObject.getReturnData());
                    SECSTransaction transaction2 = null;
                    if (!trx.Secondary && trx.Autoreply)
                    {
                        transaction2 = this.checkAutoReply(trx);
                        if (transaction2 != null)
                        {
                            transaction2.setSystemByte(trx.getRowSystembyte());
                            this.rootHandle.ManagerFactory.ConnectManager.reply(transaction2);
                        }
                    }
                }
                else
                {
                    this.loggerMgr.WriteLog(message, false);
                    this.rootHandle.ManagerFactory.CallbackManager.onReceived("", message);
                }
                if (!isSECS1Mode)
                {
                    this.processCtrlMessage(message);
                    if (this.rootHandle.Config.LinktestDuration != 0)
                    {
                        this.rootHandle.ManagerFactory.ConnectManager.setLastActionTime();
                    }
                }
            }
        }

        private void processCtrlMessage(SECSTransaction message)
        {
            if (message.ControlMessage)
            {
                int stype = message.Stype;
                if (stype == 9)
                {
                    throw new IOException("Receive Separate Message");
                }
                if (!this.rootHandle.ManagerFactory.ConnectManager.Sendable)
                {
                    switch (stype)
                    {
                        case 1:
                        case 2:
                            this.receivedSelectTransaction(message, stype);
                            return;
                    }
                    this.rootHandle.ManagerFactory.ConnectManager.sendRejectRequest(4);
                }
                else
                {
                    switch (stype)
                    {
                        case 1:
                        case 2:
                            this.logger.Warn("unExpected select.xxx");
                            return;

                        case 3:
                        case 4:
                            this.logger.Warn("Not support Generic Session");
                            return;

                        case 5:
                            this.rootHandle.ManagerFactory.ConnectManager.sendLinkTestResponse(message.copyHeader());
                            return;

                        case 6:
                            return;

                        case 7:
                            this.receivedRejectRequest(message.ReasonCode);
                            return;
                    }
                    this.rootHandle.ManagerFactory.ConnectManager.sendRejectRequest(1);
                }
            }
        }

        private void receivedRejectRequest(int nReasonCode)
        {
            string str;
            if (nReasonCode == 1)
            {
                str = "SType not supported";
            }
            else if (nReasonCode == 2)
            {
                str = "pType not supported";
            }
            else if (nReasonCode == 3)
            {
                str = "Transaction not open";
            }
            else if (nReasonCode == 4)
            {
                str = "entity not selected";
            }
            else
            {
                str = "UnKnown";
            }
            this.logger.Warn("received Reject.req Reason : " + str);
        }

        private void receivedSelectTransaction(SECSTransaction message, int sTypeVal)
        {
            if ((sTypeVal == 2) && (message.ReasonCode != 0))
            {
                this.logger.Warn("received Select.rsp " + this.selectFailCode(message.ReasonCode));
            }
            else
            {
                this.rootHandle.ManagerFactory.ConnectManager.SelectedStatus = true;
                this.rootHandle.ManagerFactory.CallbackManager.onConnected(this.rootHandle.Config.DriverId);
                if (sTypeVal == 1)
                {
                    this.rootHandle.ManagerFactory.ConnectManager.sendSelectResponse(message.copyHeader());
                }
            }
        }

        private string selectFailCode(int nReason)
        {
            switch (nReason)
            {
                case 1:
                    return ": Communication Already Active";

                case 2:
                    return ": Connection Not Ready";

                case 3:
                    return ": Connect Exhaust";
            }
            return ": Unknown";
        }

        private bool stopTimeoutAndInputCorrelationObject(SECSTransaction message)
        {
            if (message.ControlMessage || message.Secondary)
            {
                if (message.ControlMessage)
                {
                    if (((2 == message.Stype) || (6 == message.Stype)) && (this.rootHandle.ManagerFactory.TimerManager.ReleaseTimeOut(message.Systembyte) == null))
                    {
                        return false;
                    }
                }
                else
                {
                    SECSTransaction transaction2 = this.rootHandle.ManagerFactory.TimerManager.ReleaseTimeOut(message.Systembyte);
                    if (transaction2 != null)
                    {
                        message.Correlation = transaction2.Correlation;
                        message.MessageData = transaction2.MessageData;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
