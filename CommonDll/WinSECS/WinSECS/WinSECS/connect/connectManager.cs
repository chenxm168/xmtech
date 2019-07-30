using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Reflection;
using System.IO.Ports;
using log4net;
using log4net.Core;
using WinSECS.manager;
using WinSECS.Utility;
using WinSECS.structure;
using WinSECS.global;
using WinSECS.driver;
using WinSECS.timeout;


namespace WinSECS.connect
{
    internal class connectManager : abstractManager, IThreadRunnable
    {
        private bool bConnectable = false;
        private WinSECS.Utility.Queue<SECSTransaction> beingSentQueue;
        private WinSECS.Utility.Queue<SECSTransaction> beingSentReplyQueue;
        private WinSECS.Utility.Queue<SECSTransaction> beingSentRequestQueue;
        private bool bRun = true;
        private static long connectToken;
        private int CurrentStatus;
        private BinaryReader dis;
        private static long disconnectToken;
        private BinaryWriter dos;
        private HSMSReader hsmsReader;
        private HSMSWriter hsmsWriter;
        private bool isJustAgoDisconnted = false;
        private long lastActionTime = 0L;
        private ILog logger;
        private WinSECS.logger.LoggerManager loggerMgr;
        private TcpClient mainClientSocket;
        private TcpListener mainServerSocket;
        private int nConnect = 0;
        private int nDisConnect = 0;
        private SerialPort port;
        private SECS1Stream secs1stream;
        private int statusConnected = 2;
        private int statusDisconnected = 0;
        private int statusSelected = 4;
        public const int statusSerailOpend = 12;
        public const int statusSerialClosed = 10;
        protected readonly object SyncConnection = new object();
        internal long systemByte = 0L;
        private IPAddress thisAddress;

        public connectManager(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            this.InitBlock();
            base.Initialize(rootHandle, config, returnObject);
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
            if (config.Hsmsmode)
            {
                this.beingSentQueue = new WinSECS.Utility.Queue<SECSTransaction>();
            }
            else
            {
                this.beingSentReplyQueue = new WinSECS.Utility.Queue<SECSTransaction>();
                this.beingSentRequestQueue = new WinSECS.Utility.Queue<SECSTransaction>();
            }
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = rootHandle.ManagerFactory.LoggerManager.Logger;
        }

        public virtual void AcquireMutext()
        {
            connectManager manager;
            Monitor.Enter(manager = this);
            try
            {
                Monitor.Wait(this);
            }
            catch (ThreadInterruptedException)
            {
                this.logger.Info("Wait Until calling Disconnection");
            }
            finally
            {
                Monitor.Exit(manager);
            }
        }

        public void awakeHsmsThread()
        {
            this.hsmsReader.DataInputStream = this.dis;
            this.hsmsReader.releaseMutext();
            this.hsmsWriter.DataOutputStream = this.dos;
            this.hsmsWriter.releaseMutext();
        }

        public virtual void closeDataStream()
        {
            try
            {
                if (this.dis != null)
                {
                    this.dis.Close();
                }
                if (this.dos != null)
                {
                    this.dos.Close();
                }
            }
            catch (IOException)
            {
            }
            finally
            {
                this.dis = null;
                this.dos = null;
            }
        }

        public virtual void connect()
        {
            IOException exception3;
            ThreadInterruptedException exception4;
            bool flag;
            string info = string.Format("INITIALIZE ({0}) (PROCID:{1}) (VER:{2})", base.config.DriverId, Process.GetCurrentProcess().Id, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            this.loggerMgr.WriteConnnectionLog(Level.Info, info, false);
            base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, info);
            this.logger.Info("[connectManager][connect] - Start");
            if (!base.config.Active)
            {
                this.serverReady();
                this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS LISTENNING PASSIVE:" + base.config.Port, false);
                base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, "HSMS LISTENNING PASSIVE:" + base.config.Port);
                while (true)
                {
                    flag = true;
                    try
                    {
                        if ((this.mainClientSocket != null) && this.mainClientSocket.Connected)
                        {
                            this.logger.Info("[connectManager][connect] - mainClientSocket not null or not closed");
                            this.mainClientSocket.GetStream().Close();
                            this.mainClientSocket.Close();
                            this.mainClientSocket = null;
                        }
                        this.logger.Info("[connectManager][connect] - Accept Ready");
                        this.mainClientSocket = this.mainServerSocket.AcceptTcpClient();
                        this.mainServerSocket.Stop();
                        this.logger.Info("[connectManager][connect] - Accepted & closed");
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS ACCEPTED :" + this.mainClientSocket.Client.RemoteEndPoint.ToString(), false);
                        base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, "HSMS ACCEPTED :" + base.config.Port);
                        SECSTimeout timeout = new SECSTimeout(-7);
                        base.rootHandle.ManagerFactory.TimerManager.SetTimeOut(timeout);
                        goto Label_058E;
                    }
                    catch (SocketException exception)
                    {
                        this.logger.Error("[connectManager][connect] - Create Server Socket Error " + loggerHelper.getExceptionString(exception));
                    }
                    catch (InvalidOperationException exception2)
                    {
                        this.logger.Error("[connectManager][connect] - Create Server Socket Error " + loggerHelper.getExceptionString(exception2));
                        if (!this.bRun)
                        {
                            goto Label_058E;
                        }
                    }
                    catch (IOException exception7)
                    {
                        exception3 = exception7;
                        this.logger.Error("[connectManager][connect] - Create Server Socket Error " + loggerHelper.getExceptionString(exception3));
                        if (!this.bRun)
                        {
                            goto Label_058E;
                        }
                    }
                }
            }
            while (true)
            {
                flag = true;
                try
                {
                    if (this.isJustAgoDisconnted)
                    {
                        this.logger.Info("[connectManager][connect] - sleep T5");
                        Thread.Sleep((int)(base.config.Timeout5 * 0x3e8));
                        this.isJustAgoDisconnted = false;
                    }
                    this.loggerMgr.WriteConnnectionLog(Level.Info, string.Concat(new object[] { "HSMS CONNECTING...(ACTIVE ", base.config.IpAddress, "/", base.config.Port, ")" }), false);
                    base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, string.Concat(new object[] { "HSMS CONNECTING...(ACTIVE ", base.config.IpAddress, "/", base.config.Port, ")" }));
                    if (!((this.mainClientSocket == null) || this.mainClientSocket.Connected))
                    {
                        this.logger.Info("[connectManager][connect] - mainClientSocket not null or not closed");
                        this.mainClientSocket.GetStream().Close();
                        this.mainClientSocket.Close();
                        this.mainClientSocket = null;
                    }
                    this.mainClientSocket = new TcpClient(this.thisAddress.ToString(), base.config.Port);
                    this.logger.Info("[connectManager][connect] - create & connect client socket");
                    break;
                }
                catch (IOException exception8)
                {
                    exception3 = exception8;
                    try
                    {
                        this.logger.Error("[connectManager][connect] -" + loggerHelper.getExceptionString(exception3));
                        this.loggerMgr.WriteTimeoutLog(EnumSet.TIMEOUT.T5, "IOException " + exception3.Message);
                        if (!this.bRun)
                        {
                            break;
                        }
                        Thread.Sleep((int)(base.config.Timeout5 * 0x3e8));
                    }
                    catch (ThreadInterruptedException exception9)
                    {
                        exception4 = exception9;
                        this.logger.Error("[connectManager][connect] - Interrupted During Wait T5 " + loggerHelper.getExceptionString(exception3));
                    }
                }
                catch (Exception exception5)
                {
                    try
                    {
                        this.logger.Error("[connectManager][connect] - " + loggerHelper.getExceptionString(exception5));
                        this.loggerMgr.WriteTimeoutLog(EnumSet.TIMEOUT.T5, "Exception " + exception5.Message);
                        if (!this.bRun)
                        {
                            break;
                        }
                        Thread.Sleep((int)(base.config.Timeout5 * 0x3e8));
                    }
                    catch (ThreadInterruptedException exception11)
                    {
                        exception4 = exception11;
                        this.logger.Info("[connectManager][connect] - Interrupted During Wait T5 " + loggerHelper.getExceptionString(exception5));
                    }
                }
            }
        Label_058E:
            if (this.bRun && this.CreateDataStream())
            {
                int num = 0;
                while (this.hsmsReader.Status != HSMSInterface.gettableMutex)
                {
                    this.logger.Info("[connectManager][connect] - Waiting HSMSReader ready before Creationg DataStream ");
                    num++;
                    try
                    {
                        Thread.Sleep(0x3e8);
                    }
                    catch (ThreadInterruptedException exception12)
                    {
                        exception4 = exception12;
                        this.logger.Info(exception4.StackTrace);
                    }
                    if (num >= 10)
                    {
                        break;
                    }
                }
                this.logger.Info("[connectManager][connect] - HSMS CONNECTED");
                this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS CONNECTED " + this.mainClientSocket.Client.RemoteEndPoint.ToString(), true);
                base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, "HSMS CONNECTED " + this.mainClientSocket.Client.RemoteEndPoint.ToString());
            }
            this.logger.Info("[connectManager][connect] - End");
        }

        public virtual bool CreateDataStream()
        {
            this.logger.Info("[connectManager][CreateDataStream] - Start");
            try
            {
                while ((this.dis == null) || (this.dos == null))
                {
                    try
                    {
                        if (this.dis == null)
                        {
                            this.dis = new BinaryReader(this.mainClientSocket.GetStream());
                        }
                        if (this.dos == null)
                        {
                            this.dos = new BinaryWriter(this.mainClientSocket.GetStream());
                        }
                        if ((this.dis == null) || (this.dos == null))
                        {
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (SocketException exception2)
            {
                this.logger.Info("[connectManager][CreateDataStream] - SocketException", exception2);
                return false;
            }
            catch (ThreadInterruptedException exception3)
            {
                this.logger.Info("[connectManager][CreateDataStream] - InterruptedException", exception3);
                return false;
            }
            catch (IOException exception4)
            {
                this.logger.Info("[connectManager][CreateDataStream] - IOException", exception4);
                return false;
            }
            this.logger.Info("[connectManager][CreateDataStream] - End");
            return true;
        }

        public virtual void disconnect()
        {
            this.logger.Info("[connectManager][disconnect] - Start");
            try
            {
                if (this.dis != null)
                {
                    this.dis.Close();
                }
                if (this.dos != null)
                {
                    this.dos.Close();
                }
                if ((this.mainClientSocket != null) && this.mainClientSocket.Connected)
                {
                    this.mainClientSocket.GetStream().Close();
                    this.mainClientSocket.Close();
                }
                this.mainClientSocket = null;
                try
                {
                    Thread.Sleep(1);
                }
                catch (ThreadInterruptedException exception)
                {
                    this.logger.Info(exception.StackTrace);
                }
            }
            catch (IOException exception2)
            {
                this.logger.Error("[connectManager][disconnect] - During Closing DataStream(IOException", exception2);
                this.loggerMgr.WriteConnnectionLog(Level.Error, "During Closing DataStream " + loggerHelper.getExceptionString(exception2), false);
            }
            this.dis = null;
            this.dos = null;
            if (this.beingSentQueue != null)
            {
                this.beingSentQueue.Clear();
            }
            base.rootHandle.ManagerFactory.CallbackManager.getCallbackQueue().Clear();
            base.rootHandle.ManagerFactory.TimerManager.ClearTimerList();
            this.setLastActionTime();
            this.logger.Info("[connectManager][disconnect] - HSMS DISCONNECTED");
            if (this.CurrentStatus == this.statusSelected)
            {
                base.rootHandle.ManagerFactory.CallbackManager.onDisconnected(base.rootHandle.Config.Id);
            }
            this.CurrentStatus = this.statusDisconnected;
            this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS DISCONNECTED... " + base.config.Id, true);
            this.isJustAgoDisconnted = true;
        }

        private string eqSideS9FxSheadOrMhead(int function)
        {
            return ((function == 9) ? "SHEAD" : "MHEAD");
        }

        public virtual long GetDisconnectToken()
        {
            lock (this)
            {
                if (this.nDisConnect == 0)
                {
                    disconnectToken = DateTime.Now.Ticks;
                    this.nDisConnect = 1;
                    return disconnectToken;
                }
                if ((DateTime.Now.Ticks - disconnectToken) > 0x7d0L)
                {
                    disconnectToken = DateTime.Now.Ticks;
                    return disconnectToken;
                }
                return 0L;
            }
        }

        public virtual long getLastActionTime()
        {
            if (this.Sendable)
            {
                return this.lastActionTime;
            }
            return 0L;
        }

        private void InitBlock()
        {
        }

        public void InitializeLastActionTime()
        {
            this.lastActionTime = 0L;
        }

        public virtual SECSException Ready()
        {
            try
            {
                this.thisAddress = Dns.GetHostByName(base.config.IpAddress).AddressList[0];
            }
            catch (Exception exception)
            {
                return new SECSException(exception.ToString());
            }
            if (this.hsmsReader == null)
            {
                this.hsmsReader = new HSMSReader(base.rootHandle);
            }
            new SupportClass.ThreadClass(new ThreadStart(this.hsmsReader.Run)) { Name = this.hsmsReader.Name, IsBackground = true }.Start();
            if (this.hsmsWriter == null)
            {
                this.hsmsWriter = new HSMSWriter(base.rootHandle);
            }
            new SupportClass.ThreadClass(new ThreadStart(this.hsmsWriter.Run)) { Name = this.hsmsWriter.Name, IsBackground = true }.Start();
            return null;
        }

        public virtual void ReleaseDisconnectToken(long tokenValue)
        {
            lock (this)
            {
                if (tokenValue == disconnectToken)
                {
                    this.nDisConnect = 0;
                }
            }
        }

        public virtual void releaseMutext()
        {
            lock (this)
            {
                Monitor.Pulse(this);
            }
        }

        public void ReloadConfig(SECSConfig NewConfig, bool forceReconnect)
        {
            this.loggerMgr = base.rootHandle.ManagerFactory.LoggerManager;
            this.logger = base.rootHandle.ManagerFactory.LoggerManager.Logger;
            base.config = NewConfig;
            if (forceReconnect)
            {
                this.releaseMutext();
            }
        }

        public virtual ReturnObject reply(SECSTransaction message)
        {
            if (!this.Sendable)
            {
                return new ReturnObject(SEComError.SEComConnection.ERR_NOT_SENDABLE_STATUS);
            }
            if (message == null)
            {
                return new ReturnObject(0x6f);
            }
            message.DeviceId = base.rootHandle.Config.DeviceId;
            if (!message.ControlMessage)
            {
                try
                {
                    message.encoding();
                }
                catch (IOException)
                {
                    return new ReturnObject(SEComError.SEComPlugIn.ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE_UnsupportedEncodingException);
                }
                catch (Exception)
                {
                    return new ReturnObject(SEComError.SEComMessageHanlder.FAIL_DURING_MESSAGE_ENCODING);
                }
            }
            if (base.config.Hsmsmode)
            {
                this.beingSentQueue.Enqueue(message);
            }
            else
            {
                this.beingSentReplyQueue.Enqueue(message);
            }
            this.loggerMgr.WriteRequestedLog(message);
            return new ReturnObject(0);
        }

        public virtual SECSException replyBeforeSelected(SECSTransaction message)
        {
            message.DeviceId = base.rootHandle.Config.DeviceId;
            if (base.config.Hsmsmode)
            {
                this.beingSentQueue.Enqueue(message);
            }
            else
            {
                this.BeingSentReplyQueue.Enqueue(message);
            }
            this.loggerMgr.WriteRequestedLog(message);
            return new SECSException();
        }

        public virtual ReturnObject request(SECSTransaction message)
        {
            if (message == null)
            {
                return new ReturnObject(0x6f);
            }
            if (!this.Sendable)
            {
                return new ReturnObject(SEComError.SEComConnection.ERR_NOT_SENDABLE_STATUS);
            }
            if (message.Systembyte < 1L)
            {
                message.Systembyte = this.systemByte += 1L;
            }
            if (!message.ControlMessage)
            {
                message.DeviceId = base.rootHandle.Config.DeviceId;
                try
                {
                    message.encoding();
                }
                catch (IOException)
                {
                    return new ReturnObject(SEComError.SEComPlugIn.ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE_UnsupportedEncodingException);
                }
                catch (Exception)
                {
                    return new ReturnObject(SEComError.SEComMessageHanlder.FAIL_DURING_MESSAGE_ENCODING);
                }
            }
            if (base.config.Hsmsmode)
            {
                this.beingSentQueue.Enqueue(message);
            }
            else
            {
                this.beingSentRequestQueue.Enqueue(message);
            }
            this.loggerMgr.WriteRequestedLog(message);
            return new ReturnObject();
        }

        private void requestBeforeSelected(SECSTransaction message)
        {
            if (message.Systembyte < 1L)
            {
                message.Systembyte = this.systemByte += 1L;
            }
            if (base.config.Hsmsmode)
            {
                this.beingSentQueue.Enqueue(message);
            }
            else
            {
                this.BeingSentRequestQueue.Enqueue(message);
            }
            this.loggerMgr.WriteRequestedLog(message);
        }

        public virtual void Run()
        {
            if (this.Ready() == null)
            {
                if (base.config.Hsmsmode)
                {
                    while (this.bRun)
                    {
                        this.connect();
                        this.CurrentStatus = this.statusConnected;
                        this.awakeHsmsThread();
                        if (!this.bRun)
                        {
                            break;
                        }
                        if (base.config.Active)
                        {
                            this.sendSelectRequest();
                        }
                        this.AcquireMutext();
                        this.disconnect();
                    }
                }
                else
                {
                    this.serialReady();
                    this.SerialConnect();
                }
            }
        }

        public virtual void sendLinkTestRequest()
        {
            SECSTransaction message = new SECSTransaction();
            message.Header[0] = 0xff;
            message.Header[1] = 0xff;
            message.Stype = 5;
            this.request(message);
        }

        public virtual void sendLinkTestResponse(byte[] header)
        {
            SECSTransaction message = new SECSTransaction
            {
                Header = header,
                Stype = 6
            };
            this.request(message);
        }

        public virtual void sendRejectRequest()
        {
            SECSTransaction message = new SECSTransaction();
            message.Header[0] = 0xff;
            message.Header[1] = 0xff;
            message.Stype = 7;
            this.request(message);
        }

        public virtual void sendRejectRequest(int nReasonCode)
        {
            SECSTransaction message = new SECSTransaction();
            message.Header[0] = 0xff;
            message.Header[1] = 0xff;
            message.Stype = 7;
            message.ReasonCode = nReasonCode;
            this.request(message);
        }

        public virtual void sendS9Fx(int errorCode, SECSTransaction original)
        {
            if (!base.config.Host)
            {
                SECSTransaction message = new SECSTransaction();
                message.setStreamNWbit(9, false);
                message.Function = errorCode;
                if (original != null)
                {
                    message.add(BinaryFormat.TYPE, 10, (errorCode == 9) ? "SHEAD" : "MHEAD", ByteToObject.byte2Binary(original.Header));
                }
                else
                {
                    message.add(BinaryFormat.TYPE, 10, (errorCode == 9) ? "SHEAD" : "MHEAD", "0 0 0 0 0 0 0 0 0 0");
                }
                this.request(message);
            }
        }

        public virtual void sendSelectRequest()
        {
            SECSTransaction message = new SECSTransaction();
            message.Header[0] = 0xff;
            message.Header[1] = 0xff;
            message.Stype = 1;
            this.requestBeforeSelected(message);
        }

        public virtual void sendSelectResponse(byte[] header)
        {
            SECSTransaction message = new SECSTransaction
            {
                Header = header,
                Stype = 2
            };
            this.reply(message);
        }

        public virtual void sendSeperateMessage()
        {
            SECSTransaction message = new SECSTransaction();
            message.Header[0] = 0xff;
            message.Header[1] = 0xff;
            message.Stype = 9;
            this.request(message);
        }

        public virtual void SerialConnect()
        {
            lock (this.SyncConnection)
            {
                if (this.CurrentStatus == 12)
                {
                    base.rootHandle.ManagerFactory.ConnectManager.SelectedStatus = true;
                    base.rootHandle.ManagerFactory.CallbackManager.onConnected(base.rootHandle.Config.DriverId);
                    this.loggerMgr.WriteConnnectionLog(Level.Info, "SERIAL Already OPEN", false);
                    return;
                }
            }
            bool flag = false;
            while (!flag)
            {
                try
                {
                    this.port = new SerialPort(base.config.PortName, base.config.BaudRate, Parity.None, 8, StopBits.One);
                    this.port.Open();
                    if (!this.isRun)
                    {
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "SECS-I OPEN SCHEDULE CANCELED", false);
                        this.port.Close();
                    }
                    flag = true;
                }
                catch (Exception exception)
                {
                    if (this.isRun)
                    {
                        this.logger.Info("[connectManager][serialConnect] - SECS-I OPEN ERROR : " + loggerHelper.getExceptionString(exception));
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "SECS-I OPEN ERROR", false);
                        Thread.Sleep(0x1388);
                    }
                    else
                    {
                        this.logger.Info("[connectManager][serialConnect] - SECS-I OPEN CANCELED : " + loggerHelper.getExceptionString(exception));
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "SECS-I OPEN SCHEDULE CANCELED", false);
                        break;
                    }
                }
            }
            base.rootHandle.ManagerFactory.ConnectManager.SelectedStatus = true;
            base.rootHandle.ManagerFactory.CallbackManager.onConnected(base.rootHandle.Config.DriverId);
            this.loggerMgr.WriteConnnectionLog(Level.Info, "SERIAL OPEN", false);
            if (this.secs1stream == null)
            {
                this.secs1stream = new SECS1Stream(base.rootHandle, this.port);
            }
            new SupportClass.ThreadClass(new ThreadStart(this.secs1stream.Run)) { Name = this.secs1stream.Name, IsBackground = true }.Start();
        }

        public virtual void serialDisconnect()
        {
            this.isRun = false;
            try
            {
                Thread.Sleep(0x3e8);
                this.port.Close();
            }
            finally
            {
                this.loggerMgr.WriteConnnectionLog(Level.Info, "SECS-I CLOSE ", false);
                base.rootHandle.ManagerFactory.CallbackManager.onDisconnected(base.rootHandle.Config.DriverId);
            }
        }

        public virtual void serialReady()
        {
            string info = string.Format("INITIALIZE ({0}) (PROCID:{1}) (VER:{2})", base.config.DriverId, Process.GetCurrentProcess().Id, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            this.loggerMgr.WriteConnnectionLog(Level.Info, info, false);
            base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, info);
        }

        public virtual void serverReady()
        {
            this.logger.Info("[connectManager][serverReady] - Start");
            while (true)
            {
                try
                {
                    this.logger.Info("[connectManager][serverReady] - HSMS BINDING");
                    string info = "HSMS BINDING... " + base.config.Port;
                    this.loggerMgr.WriteConnnectionLog(Level.Info, info, false);
                    if ((this.mainServerSocket == null) || ((this.mainServerSocket != null) && !this.mainServerSocket.Server.Connected))
                    {
                        TcpListener listener = new TcpListener(base.config.Port);
                        listener.Start();
                        this.mainServerSocket = listener;
                    }
                    this.logger.Info("[connectManager][serverReady] - HSMS BINDED");
                    this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS BINDED " + base.config.Port, false);
                    return;
                }
                catch (SocketException exception)
                {
                    this.logger.Error(string.Concat(new object[] { "[connectManager][serverReady] - HSMS BINDING ", base.config.Port, " (", exception.ToString(), ") : ", exception.Message }));
                    this.loggerMgr.WriteConnnectionLog(Level.Error, string.Concat(new object[] { "HSMS BINDING ", base.config.Port, " (", exception.ToString(), ") : ", exception.Message }), false);
                    base.rootHandle.ManagerFactory.CallbackManager.onLog(base.config.DriverId, string.Concat(new object[] { "HSMS BINDING ERROR", base.config.Port, " :", exception.Message }));
                    Thread.Sleep((int)(base.config.Timeout5 * 0x3e8));
                }
                catch (IOException exception2)
                {
                    try
                    {
                        this.logger.Error(string.Concat(new object[] { "[connectManager][serverReady] - HSMS BINDING ", base.config.Port, " (", exception2.ToString(), ") : ", exception2.Message }));
                        this.loggerMgr.WriteConnnectionLog(Level.Error, string.Concat(new object[] { "HSMS BINDING ", base.config.Port, " (", exception2.ToString(), ") : ", exception2.Message }), false);
                        Thread.Sleep((int)(base.config.Timeout7 * 0x3e8));
                    }
                    catch (ThreadInterruptedException exception3)
                    {
                        this.logger.Error("[connectManager][serverReady] - InterruptedException", exception3);
                    }
                }
            }
        }

        public virtual void setLastActionTime()
        {
            lock (this)
            {
                if (this.Sendable)
                {
                    this.lastActionTime = CSharpUtil.currentTimeMillis();
                }
                else
                {
                    this.lastActionTime = 0L;
                }
            }
        }

        public override void Terminate(ReturnObject returnObject)
        {
            this.logger.Info("[connectManager][Terminate] - Start");
            this.sendSeperateMessage();
            Thread.Sleep(20);
            this.bRun = false;
            try
            {
                if (base.config.Hsmsmode)
                {
                    this.beingSentQueue.Clear();
                    this.beingSentQueue = null;
                    if (this.mainServerSocket != null)
                    {
                        this.mainServerSocket.Stop();
                    }
                    if (this.mainClientSocket != null)
                    {
                        this.mainClientSocket.Close();
                    }
                }
                else
                {
                    this.beingSentReplyQueue.Clear();
                    this.beingSentRequestQueue.Clear();
                    this.beingSentReplyQueue = null;
                    this.beingSentRequestQueue = null;
                }
                if (this.port != null)
                {
                    this.secs1stream.setRun(false);
                    if (this.port.IsOpen)
                    {
                        this.port.Close();
                        this.logger.Info("[connectManager][Terminate] - SERIAL PORT CLOSED");
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "SERIAL DISCONNECTED... " + base.config.Id, true);
                        base.rootHandle.ManagerFactory.CallbackManager.onDisconnected("");
                    }
                    this.port = null;
                }
            }
            catch (IOException exception)
            {
                this.logger.Info("[connectManager][Terminate] - During close socket " + loggerHelper.getExceptionString(exception));
                returnObject.setError(exception.StackTrace);
                return;
            }
            this.hsmsReader.bRun = false;
            this.hsmsWriter.bRun = false;
            this.hsmsReader.releaseMutext();
            this.hsmsWriter.releaseMutext();
            this.logger.Info("[connectManager][Terminate] - [END]");
        }

        public virtual WinSECS.Utility.Queue<SECSTransaction> BeingSentQueue
        {
            get
            {
                return this.beingSentQueue;
            }
            set
            {
                this.beingSentQueue = value;
            }
        }

        public virtual WinSECS.Utility.Queue<SECSTransaction> BeingSentReplyQueue
        {
            get
            {
                return this.beingSentReplyQueue;
            }
            set
            {
                this.beingSentReplyQueue = value;
            }
        }

        public virtual WinSECS.Utility.Queue<SECSTransaction> BeingSentRequestQueue
        {
            get
            {
                return this.beingSentRequestQueue;
            }
            set
            {
                this.beingSentRequestQueue = value;
            }
        }

        public virtual BinaryReader DataInputStream
        {
            get
            {
                return this.dis;
            }
        }

        public virtual BinaryWriter DataOutputStream
        {
            get
            {
                return this.dos;
            }
        }

        public virtual bool isRun
        {
            get
            {
                return this.bRun;
            }
            set
            {
                this.bRun = value;
            }
        }

        public virtual bool SelectedStatus
        {
            set
            {
                if (value)
                {
                    this.CurrentStatus = this.statusSelected;
                    if (!base.config.Active)
                    {
                        base.rootHandle.ManagerFactory.TimerManager.ReleaseTimeOut(new SECSTimeout(-7).Type);
                    }
                    if (base.config.Hsmsmode)
                    {
                        this.loggerMgr.WriteConnnectionLog(Level.Info, "HSMS SELECTED " + base.config.Id, true);
                    }
                }
            }
        }

        public virtual bool Sendable
        {
            get
            {
                return (this.CurrentStatus == this.statusSelected);
            }
        }
    }
}
