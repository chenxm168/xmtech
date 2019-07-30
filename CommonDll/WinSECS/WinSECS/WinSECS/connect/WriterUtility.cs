using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSECS.timeout;
using WinSECS.structure;
using WinSECS.global;
using WinSECS.driver;
using WinSECS.logger;
using log4net;


namespace WinSECS.connect
{
    internal class WriterUtility
    {
        private ILog logger;
        private LoggerManager loggerMgr;
        private SinglePlugIn rootHandle;

        public WriterUtility(SinglePlugIn rootHandle)
        {
            this.rootHandle = rootHandle;
            this.loggerMgr = rootHandle.ManagerFactory.LoggerManager;
            this.logger = this.loggerMgr.Logger;
        }

        public void checkDoTimerMessage(SECSTransaction message)
        {
            SECSTimeout timeout;
            if (message.ControlMessage)
            {
                switch (message.Stype)
                {
                    case 1:
                    case 5:
                        timeout = new SECSTimeout(-6)
                        {
                            Message = message
                        };
                        this.rootHandle.ManagerFactory.TimerManager.SetTimeOut(timeout);
                        break;
                }
            }
            else if (!message.Secondary && message.Wbit)
            {
                timeout = new SECSTimeout(-3)
                {
                    Message = message
                };
                this.rootHandle.ManagerFactory.TimerManager.SetTimeOut(timeout);
            }
        }

        public void stopTimerForSendingFailMessage(SECSTransaction message)
        {
            if (message.ControlMessage)
            {
                switch (message.Stype)
                {
                    case 1:
                    case 5:
                        if (this.rootHandle.ManagerFactory.TimerManager.ReleaseTimeOut(message.Systembyte) == null)
                        {
                            this.logger.Warn("[WriterUtility][stopTimerForSendingFailMessage] (There is No valid Stop Timer, for Message " + message.HeaderString);
                        }
                        break;
                }
            }
            else if ((!message.Secondary && message.Wbit) && (this.rootHandle.ManagerFactory.TimerManager.ReleaseTimeOut(message.Systembyte) == null))
            {
                this.logger.Warn("[WriterUtility][stopTimerForSendingFailMessage] (There is No valid Stop Timer, for Message " + message.HeaderString);
            }
        }
    }
}
