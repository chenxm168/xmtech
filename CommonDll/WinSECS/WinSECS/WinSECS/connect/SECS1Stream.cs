using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using log4net.Core;
using WinSECS.logger;
using WinSECS.structure;
using WinSECS.Utility;
using WinSECS.driver;
using WinSECS.global;
using WinSECS.timeout;


namespace WinSECS.connect
{
    [ComVisible(false)]
    public class SECS1Stream : IThreadRunnable
    {
        private bool allowSendRequest = true;
        private WinSECS.Utility.Queue<SECSTransaction> beingSentReplyQueue;
        private WinSECS.Utility.Queue<SECSTransaction> beingSentRequestQueue;
        private SECS1BlockDispatcher blockDispatcher = null;
        internal bool bRun = true;
        private int deviceId = 0;
        private ILog logger;
        private WinSECS.logger.LoggerManager loggerMgr;
        
        private SerialPort port;
        private readerUtility readerLib;
        private Reserved reserved = Reserved.RECV;
        private SinglePlugIn rootHandle;
        private WriterUtility writerUtil = null;

        public SECS1Stream(SinglePlugIn rootHandle, SerialPort port)
        {
            this.rootHandle = rootHandle;
            this.deviceId = rootHandle.Config.DeviceId;
            this.beingSentReplyQueue = rootHandle.ManagerFactory.ConnectManager.BeingSentReplyQueue;
            this.beingSentRequestQueue = rootHandle.ManagerFactory.ConnectManager.BeingSentRequestQueue;
            this.blockDispatcher = new SECS1BlockDispatcher(this.T4ReadTimeout);
            this.blockDispatcher.OnT4Detected += new T4ElapsedEventHandler(this.OnT4Detected);
            this.readerLib = new readerUtility(rootHandle);
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
            this.port = port;
            this.writerUtil = new WriterUtility(rootHandle);
        }

        private void ClearReceiveBuffer()
        {
            this.port.ReadTimeout = this.T1ReadTimeout;
            StringBuilder builder = new StringBuilder();
            builder.Append("CLEAR BUFFER\r\n");
            try
            {
                builder.AppendFormat("{0} ", this.port.ReadByte().ToString("X2"));
            }
            catch
            {
            }
            this.loggerMgr.WriteLogSECS1Only(builder.ToString(), true);
        }

        private void EncodeMessage(SECSTransaction msg)
        {
            msg.encoding();
            if (msg.ByteLength < 10)
            {
                string log = string.Format("ENCODING ERROR. S{0}F{1}({2}) : INVALIDE PACKET LENGTH {3}", new object[] { msg.Stream, msg.Function, msg.MessageName, msg.ByteLength });
                this.loggerMgr.WriteLogSECS1Only(log, false);
            }
            else
            {
                msg.Receive = false;
            }
        }

        public bool isRun()
        {
            return this.bRun;
        }

        private void OnT4Detected(SECS1BlockCollection collection)
        {
            SECSTimeout timeout = new SECSTimeout(-4);
            SECSTransaction transaction = new SECSTransaction
            {
                Header = collection.MakeHSMSHeader()
            };
            timeout.Message = transaction;
            this.rootHandle.ManagerFactory.CallbackManager.onTimeout("", timeout);
            if (!(!this.NotSupportMessageInterleaving || this.AllowSendRequest))
            {
                this.AllowSendRequest = true;
                this.loggerMgr.WriteLogSECS1Only("ALLOW=TRUE,  BECAUSE FAIL RECEIVE MESSAGE", true);
            }
        }

        private byte[] ReadCheckSum()
        {
            return new byte[] { ((byte)this.port.ReadByte()), ((byte)this.port.ReadByte()) };
        }

        private void ReadComplete(SECS1Block msg)
        {
            if (msg.IsValidCheckSum())
            {
                this.WriteHandshake(WinSECS.connect.Handshake.ACK);
                if (this.NotSupportMessageInterleaving && msg.IsWait)
                {
                    this.AllowSendRequest = false;
                    this.loggerMgr.WriteLogSECS1Only("ALLOW=FALSE, BECAUSE RECEIVE WAIT MESSAGE", false);
                }
                SECS1BlockCollection collection = this.blockDispatcher.AddMessage(msg);
                if ((collection != null) && collection.IsCompleted)
                {
                    try
                    {
                        this.readerLib.doDispatching(collection.ToSECSMessage(), true);
                    }
                    catch (Exception exception)
                    {
                        this.logger.Error("[SECS1Stream][doDispatching] -" + loggerHelper.getExceptionString(exception));
                        this.loggerMgr.WriteConnnectionLog(Level.Error, "SECS1 READER ERROR(Exception):" + exception.Message, false);
                    }
                    this.blockDispatcher.RemoveCollection(collection);
                    if (this.NotSupportMessageInterleaving && ((msg.Function % 2) == 0))
                    {
                        this.AllowSendRequest = true;
                        this.loggerMgr.WriteLogSECS1Only("ALLOW=TRUE,  BECAUSE RECEIVE REPLY MESSAGE", true);
                    }
                }
            }
            else
            {
                this.ClearReceiveBuffer();
                this.WriteHandshake(WinSECS.connect.Handshake.NAK);
            }
        }

        private WinSECS.connect.Handshake ReadHandshake()
        {
            try
            {
                this.port.ReadTimeout = this.T2ReadTimeout;
                WinSECS.connect.Handshake handshake = (WinSECS.connect.Handshake)this.port.ReadByte();
                if ((((handshake == WinSECS.connect.Handshake.ACK) || (handshake == WinSECS.connect.Handshake.NAK)) || (handshake == WinSECS.connect.Handshake.ENQ)) || (handshake == WinSECS.connect.Handshake.EOT))
                {
                    this.loggerMgr.WriteLogSECS1Only(handshake.ToString(), true);
                }
                else
                {
                    this.loggerMgr.WriteLogSECS1Only("HANDSHAKE : 0x" + ((int)handshake).ToString("X2"), true);
                }
                return handshake;
            }
            catch (TimeoutException)
            {
                this.loggerMgr.WriteTimeoutLog(EnumSet.TIMEOUT.T2, "T2 TIMEOUT (READ HANDSHAKE)");
                return WinSECS.connect.Handshake.TIMEOUT;
            }
        }

        private WinSECS.connect.Handshake ReadHandshakeForSendProcedure()
        {
            WinSECS.connect.Handshake tIMEOUT = WinSECS.connect.Handshake.TIMEOUT;
            if ((this.reserved == Reserved.RECV) && this.NeedReceive)
            {
                tIMEOUT = this.ReadHandshake();
                if (tIMEOUT == WinSECS.connect.Handshake.ENQ)
                {
                    return tIMEOUT;
                }
            }
            for (int i = 0; i <= this.rootHandle.Config.RetryLimit; i++)
            {
                this.WriteHandshake(WinSECS.connect.Handshake.ENQ);
                while ((tIMEOUT = this.ReadHandshake()) != WinSECS.connect.Handshake.TIMEOUT)
                {
                    switch (tIMEOUT)
                    {
                        case WinSECS.connect.Handshake.EOT:
                            return tIMEOUT;

                        case WinSECS.connect.Handshake.ENQ:
                            if (!this.rootHandle.Config.IsMaster)
                            {
                                this.loggerMgr.WriteLogSECS1Only("IGNORED SENT ENQ, BECAUSE THIS IS SLAVE.", false);
                                return tIMEOUT;
                            }
                            this.loggerMgr.WriteLogSECS1Only("IGNORED RECEIVED ENQ, BECAUSE THIS IS MASTER.", true);
                            this.reserved = Reserved.RECV;
                            break;
                    }
                }
            }
            return WinSECS.connect.Handshake.TIMEOUT;
        }

        private byte[] ReadPacket(int packetLength)
        {
            int offset = 0;
            int num2 = 0;
            byte[] buffer = new byte[packetLength];
            while (offset < packetLength)
            {
                num2 = this.port.Read(buffer, offset, packetLength - offset);
                if (num2 == 0)
                {
                    Thread.Sleep(500);
                }
                if (num2 < 0)
                {
                    throw new Exception(num2.ToString());
                }
                offset += num2;
            }
            return buffer;
        }

        private int ReadPacketLength()
        {
            this.port.ReadTimeout = this.T2ReadTimeout;
            int num = -1;
            try
            {
                num = this.port.ReadByte();
            }
            catch (TimeoutException)
            {
                this.loggerMgr.WriteTimeoutLog(EnumSet.TIMEOUT.T2, "T2 TIMEOUT (READ LENGTH)");
                this.WriteHandshake(WinSECS.connect.Handshake.NAK);
                return -1;
            }
            if ((num < 10) || (num > 0xfe))
            {
                this.loggerMgr.WriteLogSECS1Only("INVALID LENGTH : " + num, true);
                this.ClearReceiveBuffer();
                this.WriteHandshake(WinSECS.connect.Handshake.NAK);
                return -1;
            }
            return num;
        }

        private void ReadProcedure()
        {
            int packetLength = -1;
            byte[] packet = null;
            byte[] checksum = null;
            packetLength = this.ReadPacketLength();
            if (packetLength != -1)
            {
                try
                {
                    this.port.ReadTimeout = this.T1ReadTimeout;
                    packet = this.ReadPacket(packetLength);
                    checksum = this.ReadCheckSum();
                }
                catch (TimeoutException)
                {
                    this.loggerMgr.WriteTimeoutLog(EnumSet.TIMEOUT.T1, "T1 TIMEOUT");
                    this.WriteHandshake(WinSECS.connect.Handshake.NAK);
                }
                SECS1Block msg = new SECS1Block(packet, checksum);
                this.loggerMgr.WriteLogSECS1Only(msg.ToSECS1LogString(), true);
                this.ReadComplete(msg);
            }
        }

        public virtual void Run()
        {
            while (this.bRun)
            {
                try
                {
                    if (this.reserved == Reserved.RECV)
                    {
                        if (this.NeedReceive)
                        {
                            this.RunReceive();
                        }
                        else if (this.NeedSend)
                        {
                            this.RunSend();
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    else if (this.NeedSend)
                    {
                        this.RunSend();
                    }
                    else if (this.NeedReceive)
                    {
                        this.RunReceive();
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("[SECS1Stream][Run] -" + loggerHelper.getExceptionString(exception));
                    this.loggerMgr.WriteConnnectionLog(Level.Error, "UNEXPECTED EXCEPTION DETECT" + loggerHelper.getExceptionString(exception), false);
                }
            }
        }

        private void RunReceive()
        {
            if (this.ReadHandshake() == WinSECS.connect.Handshake.ENQ)
            {
                this.WriteHandshake(WinSECS.connect.Handshake.EOT);
                this.ReadProcedure();
                this.reserved = Reserved.SEND;
            }
        }

        private void RunSend()
        {
            bool flag = true;
            SECSTransaction trx = null;
            if (this.beingSentReplyQueue.Count > 0)
            {
                trx = this.beingSentReplyQueue.Dequeue();
                flag = true;
            }
            else if ((this.beingSentRequestQueue.Count > 0) && (this.SupportMessageInterleaving || this.AllowSendRequest))
            {
                trx = this.beingSentRequestQueue.Dequeue();
                flag = false;
            }
            if (trx == null)
            {
                this.reserved = Reserved.RECV;
                return;
            }
            bool flag2 = false;
            int num = 0;
            List<SECS1Block> list = SECSTransactionUtilcs.ToSECS1BlockList(trx, this.rootHandle.Config.Host);
            for (int i = 0; i < list.Count; i++)
            {
                switch (this.ReadHandshakeForSendProcedure())
                {
                    case WinSECS.connect.Handshake.ENQ:
                        this.WriteHandshake(WinSECS.connect.Handshake.EOT);
                        this.ReadProcedure();
                        i--;
                        if (!this.NotSupportMessageInterleaving)
                        {
                            break;
                        }
                        if (flag)
                        {
                            this.beingSentReplyQueue.EnqueueFirst(trx);
                        }
                        else
                        {
                            this.beingSentRequestQueue.EnqueueFirst(trx);
                        }
                        return;

                    case WinSECS.connect.Handshake.EOT:
                        this.writerUtil.checkDoTimerMessage(trx);
                        if (!this.WriteProcedure(list[i]))
                        {
                            i--;
                            num++;
                            if (num > this.rootHandle.Config.RetryLimit)
                            {
                                flag2 = true;
                                goto Label_019F;
                            }
                        }
                        break;

                    default:
                        flag2 = true;
                        goto Label_019F;
                }
            }
        Label_019F:
            if (!flag2)
            {
                this.loggerMgr.WriteLog(trx, true);
                if (this.NotSupportMessageInterleaving)
                {
                    if (trx.Wbit)
                    {
                        this.AllowSendRequest = false;
                        this.loggerMgr.WriteLogSECS1Only("ALLOW=FALSE, BECAUSE SENT WAIT MESSAGE", false);
                    }
                    else
                    {
                        this.AllowSendRequest = true;
                        this.loggerMgr.WriteLogSECS1Only("ALLOW=TRUE,  BECAUSE SENT REPLY MESSAGE OR NO WAIT MESSAGE", false);
                    }
                }
            }
            else
            {
                this.writerUtil.stopTimerForSendingFailMessage(trx);
                this.rootHandle.ManagerFactory.CallbackManager.onSendFailed("", trx);
            }
            this.reserved = Reserved.RECV;
        }

        public void setRun(bool run)
        {
            this.bRun = run;
        }

        public void Terminate()
        {
            this.beingSentReplyQueue.Clear();
            this.beingSentRequestQueue.Clear();
        }

        private void WriteHandshake(WinSECS.connect.Handshake shake)
        {
            this.port.Write(new byte[] { (byte)shake }, 0, 1);
            this.loggerMgr.WriteLogSECS1Only(shake.ToString(), false);
        }

        private bool WriteProcedure(SECS1Block message)
        {
            byte[] buffer = new byte[] { (byte)(message.Header.Length + message.Text.Length) };
            this.port.Write(buffer, 0, 1);
            this.port.Write(message.Header, 0, message.Header.Length);
            this.port.Write(message.Text, 0, message.Text.Length);
            this.port.Write(message.CheckSum, 0, message.CheckSum.Length);
            if (this.rootHandle.Config.BlockLogging)
            {
                this.loggerMgr.WriteLogSECS1Only(message.ToSECS1LogString(), false);
            }
            return (this.ReadHandshake() == WinSECS.connect.Handshake.ACK);
        }

        internal bool AllowSendRequest
        {
            get
            {
                return this.allowSendRequest;
            }
            set
            {
                this.allowSendRequest = value;
            }
        }

        public string Name
        {
            get
            {
                return "SECS1STREAM";
            }
        }

        public bool NeedReceive
        {
            get
            {
                return (this.port.BytesToRead > 0);
            }
        }

        public bool NeedSend
        {
            get
            {
                return ((this.beingSentReplyQueue.Count > 0) || (this.beingSentRequestQueue.Count > 0));
            }
        }

        internal bool NotSupportMessageInterleaving
        {
            get
            {
                return !this.rootHandle.Config.AllowInterleaving;
            }
        }

        public SerialPort Port
        {
            set
            {
                this.port = value;
            }
        }

        internal bool SupportMessageInterleaving
        {
            get
            {
                return this.rootHandle.Config.AllowInterleaving;
            }
        }

        private int T1ReadTimeout
        {
            get
            {
                return (int)(this.rootHandle.Config.Timeout1 * 1000f);
            }
        }

        private int T2ReadTimeout
        {
            get
            {
                return (int)(this.rootHandle.Config.Timeout2 * 1000f);
            }
        }

        private int T4ReadTimeout
        {
            get
            {
                return (this.rootHandle.Config.Timeout4 * 0x3e8);
            }
        }
    }
}
