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
    internal class HSMSReader : HSMSInterface, IThreadRunnable
    {
        private int deviceId = 0;
        private BinaryReader dis;
        private ILog logger;
        private WinSECS.logger.LoggerManager loggerMgr;
        private long maxlength = 0L;
        private readerUtility readerLib;
        private TcpClient readerSocket;

        public HSMSReader(SinglePlugIn rootHandle)
        {
            base.rootHandle = rootHandle;
            this.maxlength = rootHandle.Config.MaxLength;
            this.deviceId = rootHandle.Config.DeviceId;
            this.readerLib = new readerUtility(rootHandle);
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
        }

        private SECSTransaction allocMessage(byte[] total)
        {
            int length = total.Length;
            SECSTransaction transaction = (SECSTransaction)FormatFactory.newInstance(SECSTransaction.TYPE);
            transaction.Receive = true;
            byte[] destinationArray = new byte[10];
            byte[] buffer2 = new byte[length - 10];
            Array.Copy(total, 0, destinationArray, 0, 10);
            Array.Copy(total, 10, buffer2, 0, buffer2.Length);
            transaction.Header = destinationArray;
            transaction.Body = buffer2;
            return transaction;
        }

        public override void doRun()
        {
            int length = 0;
            try
            {
                while (((length = IPAddress.NetworkToHostOrder(this.dis.ReadInt32())) >= 0) && base.bRun)
                {
                    if ((length < 10) || (length > this.maxlength))
                    {
                        throw new IOException("WARN INVALID SECS LENGTH ERROR:" + length);
                    }
                    SECSTransaction message = new SECSTransaction();
                    message = this.readStream(length);
                    this.readerLib.doDispatching(message, false);
                }
            }
            catch (SocketException exception)
            {
                this.logger.Error("[HSMSReader][doRun] -" + loggerHelper.getExceptionString(exception));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "HSMSWriter SocketException" + SEComError.WinSockErrorCode.getErrDescription(exception.ErrorCode), false);
                this.preready();
            }
            catch (IOException exception2)
            {
                this.logger.Error("[HSMSReader][doRun] -" + loggerHelper.getExceptionString(exception2));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "READER WARN SOCKET READ ERROR " + loggerHelper.getExceptionString(exception2), false);
                this.preready();
            }
            catch (Exception exception3)
            {
                this.logger.Error("[HSMSReader][doRun] -" + loggerHelper.getExceptionString(exception3));
                this.loggerMgr.WriteConnnectionLog(Level.Error, "READER WARN SOCKET READ ERROR(Exception):" + exception3.Message, false);
                this.preready();
            }
            this.logger.Info("[HSMSReader][doRun] - End");
        }

        private SECSTransaction readStream(int length)
        {
            int index = 0;
            int num2 = 0;
            byte[] buffer = new byte[length];
            while (index < length)
            {
                num2 = this.dis.Read(buffer, index, length - index);
                if (num2 == 0)
                {
                    Thread.Sleep(100);
                }
                if (num2 < 0)
                {
                    throw new IOException("socket stream was closed/read negative value");
                }
                index += num2;
            }
            return this.allocMessage(buffer);
        }

        public virtual void Run()
        {
            this.ready();
            while (base.bRun)
            {
                this.doRun();
            }
        }

        public override BinaryReader DataInputStream
        {
            set
            {
                this.dis = value;
            }
        }

        public override BinaryWriter DataOutputStream
        {
            set
            {
            }
        }

        public override string Name
        {
            get
            {
                return "HSMSREADER";
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
