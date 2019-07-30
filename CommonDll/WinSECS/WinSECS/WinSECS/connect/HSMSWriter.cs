using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Sockets;
using System.Net;
using WinSECS.Utility;
using WinSECS.structure;
using WinSECS.driver;
using WinSECS.global;
using log4net;
using log4net.Core;

namespace WinSECS.connect
{
    [ComVisible(false)]
    internal class HSMSWriter : HSMSInterface, IThreadRunnable
    {
        private WinSECS.Utility.Queue<SECSTransaction> beingSentQueue;
        internal bool bRun = true;
        private BinaryWriter dos;
        private ILog logger;
        private WinSECS.logger.LoggerManager loggerMgr;
        private WriterUtility writerUtil = null;
        private TcpClient writeSocket;

        public HSMSWriter(SinglePlugIn rootHandle)
        {
            this.InitBlock();
            base.rootHandle = rootHandle;
            this.beingSentQueue = rootHandle.ManagerFactory.ConnectManager.BeingSentQueue;
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
            this.writerUtil = new WriterUtility(rootHandle);
        }

        public override void doRun()
        {
            try
            {
                while (this.bRun)
                {
                    this.logger.Debug("[QUEUE_CHECK]Before requesting Deque, Sending queue's size is " + this.beingSentQueue.Count);
                    SECSTransaction message = this.beingSentQueue.Dequeue();
                    this.logger.Debug("[QUEUE_CHECK]After Dequeueing, Sending queue's size is " + this.beingSentQueue.Count);
                    if (message == null)
                    {
                        this.logger.Debug("[WRITER_CHECK]return SECSTransaction is null");
                    }
                    else
                    {
                        if (message.ControlMessage && (message.Stype == 1))
                        {
                            Thread.Sleep(10);
                        }
                        this.write(message);
                        if (base.rootHandle.Config.LinktestDuration != 0)
                        {
                            base.rootHandle.ManagerFactory.ConnectManager.setLastActionTime();
                        }
                    }
                }
                this.logger.Debug("[WRITER_CHECK]reach while's End in doRun() and bRun Value is " + this.bRun);
            }
            catch (SocketException exception)
            {
                this.logger.Error("[HSMSWriter][doRun] -" + loggerHelper.getExceptionString(exception));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "HSMSWriter SocketException" + SEComError.WinSockErrorCode.getErrDescription(exception.ErrorCode), false);
                this.preready();
            }
            catch (IOException exception2)
            {
                this.logger.Error("[HSMSWriter][doRun] -" + loggerHelper.getExceptionString(exception2));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "WRITER WARN SOCKET WRITE ERROR " + exception2.Message, false);
                this.preready();
            }
            catch (Exception exception3)
            {
                this.logger.Error("[HSMSWriter][doRun] -" + loggerHelper.getExceptionString(exception3));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "WRITER WARN SOCKET WRITE ERROR(Exception):" + exception3.Message, false);
                this.preready();
            }
            this.logger.Debug("[WRITER_CHECK]reach doRun's End and bRun Value is " + this.bRun);
        }

        private void InitBlock()
        {
        }

        public bool isRun()
        {
            return this.bRun;
        }

        public virtual void Run()
        {
            this.ready();
            while (this.bRun)
            {
                this.doRun();
            }
        }

        public void setRun(bool run)
        {
            this.bRun = run;
        }

        private void write(SECSTransaction message)
        {
            this.writerUtil.checkDoTimerMessage(message);
            byte[] destinationArray = new byte[14];
            Array.Copy(message.LengthBytes, 0, destinationArray, 0, 4);
            Array.Copy(message.Header, 0, destinationArray, 4, 10);
            this.dos.Write(destinationArray);
            this.dos.Flush();
            if (message.Body.Length > 0)
            {
                this.dos.Write(message.Body);
                this.dos.Flush();
            }
            base.rootHandle.ManagerFactory.CallbackManager.onSendComplete("", message);
            this.loggerMgr.WriteLog(message, true);
        }

        public override BinaryReader DataInputStream
        {
            set
            {
            }
        }

        public override BinaryWriter DataOutputStream
        {
            set
            {
                this.dos = value;
            }
        }

        public override string Name
        {
            get
            {
                return "HSMSWRITER";
            }
        }

        public override int Status
        {
            get
            {
                return base.status;
            }
            set
            {
                base.status = value;
            }
        }
    }
}
