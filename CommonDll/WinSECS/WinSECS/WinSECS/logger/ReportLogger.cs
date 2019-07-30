using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using log4net;
using WinSECS.global;
using WinSECS.structure;

namespace WinSECS.logger
{
    [ComVisible(false)]
    public class ReportLogger
    {
        private SECSConfig config;
        private bool isConnection = false;
        private bool isTimeout = false;
        private bool isTransaction = false;
        private ILog reportLogger;

        public ReportLogger(SECSConfig config, ILog rptLogger)
        {
            this.config = config;
            this.reportLogger = rptLogger;
            this.SetupConfig();
        }

        public void SetupConfig()
        {
            int analyzerOption = this.config.AnalyzerOption;
            if ((analyzerOption > 0) || (analyzerOption < 0x80))
            {
                if ((analyzerOption & 1) == 1)
                {
                    this.isTransaction = true;
                }
                if ((analyzerOption & 2) == 2)
                {
                    this.isConnection = true;
                }
                if ((analyzerOption & 4) == 4)
                {
                    this.isTimeout = true;
                }
                if ((analyzerOption & 8) == 8)
                {
                }
                if ((analyzerOption & 0x10) == 0x10)
                {
                }
                if ((analyzerOption & 0x20) == 0x20)
                {
                }
                if ((analyzerOption & 0x40) == 0x40)
                {
                }
            }
        }

        protected internal virtual void WriteConnectionLog(string info)
        {
            if (this.isConnection)
            {
                this.reportLogger.Logger.Log(null, MyLevel.REPORT, string.Format("CONN {0}", info), null);
            }
        }

        protected internal virtual void WriteRequestedLog(SECSTransaction trx)
        {
            if (this.isTransaction && !trx.ControlMessage)
            {
                string message = string.Format("REQT S{0}F{1} {2} Systebyte={3}", new object[] { trx.Stream, trx.Function, trx.MessageName, trx.Systembyte });
                this.reportLogger.Logger.Log(null, MyLevel.REPORT, message, null);
            }
        }

        protected internal virtual void WriteTimeoutLog(string log)
        {
            if (this.isTimeout)
            {
                this.reportLogger.Logger.Log(null, MyLevel.REPORT, "TOUT " + log, null);
            }
        }

        protected internal virtual void WriteTransactionLog(SECSTransaction trx)
        {
            if (this.isTransaction && !trx.ControlMessage)
            {
                string message = string.Format("{0} S{1}F{2} {3} Systebyte={4}", new object[] { trx.Receive ? "RECV" : "SEND", trx.Stream, trx.Function, trx.MessageName, trx.Systembyte });
                this.reportLogger.Logger.Log(null, MyLevel.REPORT, message, null);
            }
        }

        protected internal virtual void WriteTransactionLog(SECSTransaction trx, string Error)
        {
            if (this.isTransaction && !trx.ControlMessage)
            {
                string message = string.Format("{0} S{1}F{2} {3} Systebyte={4} {5}", new object[] { trx.Receive ? "RECV" : "SEND", trx.Stream, trx.Function, trx.MessageName, trx.Systembyte, Error });
                this.reportLogger.Logger.Log(null, MyLevel.REPORT, message, null);
            }
        }
    }
}
