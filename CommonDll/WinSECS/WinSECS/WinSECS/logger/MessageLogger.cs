using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using log4net;
using WinSECS.global;
using WinSECS.structure;
using WinSECS.Utility;

namespace WinSECS.logger
{
    [ComVisible(false)]
    public class MessageLogger
    {
        private SECSConfig config;
        private const string IgnoreNoLoggig = "IGNORENOLOGGING";
        private ILog secs1Logger;
        private ILog secs2Logger;
        private ILog unknownLogger;

        public MessageLogger(SECSConfig config, ILog secs1Logger, ILog secs2Logger, ILog unknownLogger)
        {
            this.config = config;
            this.secs1Logger = secs1Logger;
            this.secs2Logger = secs2Logger;
            this.unknownLogger = unknownLogger;
        }

        private string makeLogFormatSECS1Body(SECSTransaction trx, int length)
        {
            if ((trx.Body == null) || (trx.Body.Length <= 0))
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("    ");
            int num = 1;
            for (int i = 0; i < length; i++)
            {
                byte num3 = trx.Body[i];
                builder.Append(StringUtils.toHex2String(num3));
                if (num++ >= 20)
                {
                    builder.Append(ConstUtils.NEWLINE);
                    builder.Append("    ");
                    num = 1;
                }
            }
            return builder.ToString();
        }

        public virtual void WriteInvalidLog(SECSTransaction trx, string invalidReason)
        {
            switch (this.config.SecsLogMode)
            {
                case 0:
                    this.writeInvalidSECS1(trx, invalidReason);
                    break;

                case 1:
                    this.writeInvalidSECS1(trx, invalidReason);
                    this.writeInvalidSECS2(trx, invalidReason);
                    break;

                case 2:
                    this.writeInvalidSECS1(trx, invalidReason);
                    break;

                case 3:
                    this.writeInvalidSECS2(trx, invalidReason);
                    break;
            }
        }

        public virtual void writeInvalidSECS1(SECSTransaction trx, string InvalidReason)
        {
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, trx.SECS1HeaderLoggingString + " INVALID_MESSAGE:" + InvalidReason + ConstUtils.NEWLINE + trx.SECS1BodyString, null);
        }

        public virtual void writeInvalidSECS2(SECSTransaction trx, string InvalidReason)
        {
            if (!trx.ControlMessage)
            {
                this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, trx.SECS2HeaderLoggingString + " INVALID_MESSAGE:" + InvalidReason + ConstUtils.NEWLINE + trx.SECS2BodyString, null);
            }
        }

        public virtual void WriteLog(SECSTransaction trx, bool isModelingData)
        {
            if (!isModelingData)
            {
                switch (this.config.SecsLogMode)
                {
                    case 0:
                        this.writeSECS1Header(trx);
                        break;

                    case 1:
                        this.writeSECS1(trx);
                        this.writeSECS2(trx);
                        break;

                    case 2:
                        this.writeSECS1(trx);
                        break;

                    case 3:
                        this.writeSECS2(trx);
                        break;
                }
            }
            else
            {
                if ((!trx.IsLogging && (trx.MessageData != null)) && (trx.MessageData.ToUpper().IndexOf("IGNORENOLOGGING") >= 0))
                {
                    trx.IsLogging = true;
                }
                switch (this.config.SecsLogMode)
                {
                    case 0:
                        this.writeSECS1Header(trx);
                        break;

                    case 1:
                        if (!trx.IsLogging)
                        {
                            this.writeSECS1Header(trx);
                            this.writeSECS2Header(trx);
                            break;
                        }
                        this.writeSECS1(trx);
                        this.writeSECS2(trx);
                        break;

                    case 2:
                        if (!trx.IsLogging)
                        {
                            this.writeSECS1Header(trx);
                            break;
                        }
                        this.writeSECS1(trx);
                        break;

                    case 3:
                        if (!trx.IsLogging)
                        {
                            this.writeSECS2Header(trx);
                            break;
                        }
                        this.writeSECS2(trx);
                        break;
                }
            }
        }

        public virtual void WriteLog(string log, bool isReceived)
        {
            if (isReceived)
            {
                log = "RECV " + log;
            }
            else
            {
                log = "SEND " + log;
            }
            switch (this.config.SecsLogMode)
            {
                case 1:
                    if (isReceived)
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, log, null);
                        this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, log, null);
                    }
                    else
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_S, log, null);
                        this.secs2Logger.Logger.Log(null, MyLevel.SECS2_S, log, null);
                    }
                    break;

                case 2:
                    if (!isReceived)
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_S, log, null);
                        break;
                    }
                    this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, log, null);
                    break;

                case 3:
                    if (!isReceived)
                    {
                        this.secs2Logger.Logger.Log(null, MyLevel.SECS2_S, log, null);
                        break;
                    }
                    this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, log, null);
                    break;
            }
        }

        public virtual void WriteLogSECS1Only(string log, bool isReceived)
        {
            if (isReceived)
            {
                log = "RECV " + log;
            }
            else
            {
                log = "SEND " + log;
            }
            switch (this.config.SecsLogMode)
            {
                case 1:
                    if (isReceived)
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, log, null);
                    }
                    else
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_S, log, null);
                    }
                    break;

                case 2:
                    if (!isReceived)
                    {
                        this.secs1Logger.Logger.Log(null, MyLevel.SECS1_S, log, null);
                        break;
                    }
                    this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, log, null);
                    break;

                case 3:
                    if (!isReceived)
                    {
                        this.secs2Logger.Logger.Log(null, MyLevel.SECS2_S, log, null);
                        break;
                    }
                    this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, log, null);
                    break;
            }
        }

        private void writeSECS1(SECSTransaction trx)
        {
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, trx.SECS1HeaderLoggingString + ConstUtils.NEWLINE + trx.SECS1BodyString, null);
        }

        private void writeSECS1Header(SECSTransaction trx)
        {
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, trx.SECS1HeaderLoggingString, null);
        }

        private void writeSECS2(SECSTransaction trx)
        {
            if (!trx.ControlMessage)
            {
                this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, trx.SECS2HeaderLoggingString + ConstUtils.NEWLINE + trx.SECS2BodyString, null);
            }
        }

        private void writeSECS2Header(SECSTransaction trx)
        {
            this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, trx.SECS2HeaderLoggingString, null);
        }

        public virtual void WriteUnknownLog(SECSTransaction trx)
        {
            switch (this.config.SecsLogMode)
            {
                case 0:
                    this.writeUnknownSECS1(trx, true);
                    break;

                case 1:
                    this.writeUnknownSECS1(trx, false);
                    this.writeUnknownSECS2(trx);
                    break;

                case 2:
                    this.writeUnknownSECS1(trx, false);
                    break;

                case 3:
                    this.writeUnknownSECS2(trx);
                    break;
            }
        }

        public virtual void writeUnknownSECS1(SECSTransaction trx, bool isOnlyHeader)
        {
            if (this.config.SeparateUnknownFolder)
            {
                this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, trx.SECS1HeaderLoggingString + " UNKNOWN_MESSAGE", null);
                if (!isOnlyHeader)
                {
                    this.unknownLogger.Logger.Log(null, MyLevel.UNKNOWN, trx.SECS1HeaderLoggingString + " UNKNOWN_MESSAGE" + ConstUtils.NEWLINE + trx.SECS1BodyString, null);
                }
            }
            else
            {
                this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, trx.SECS1HeaderLoggingString + " UNKNOWN_MESSAGE" + ConstUtils.NEWLINE + trx.SECS1BodyString, null);
            }
        }

        public virtual void writeUnknownSECS1ByLengthFilter(SECSTransaction trx, string unknownReason)
        {
            string message = trx.SECS1HeaderLoggingString + " " + unknownReason + ConstUtils.NEWLINE + this.makeLogFormatSECS1Body(trx, 500);
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, message, null);
        }

        public virtual void writeUnknownSECS2(SECSTransaction trx)
        {
            if (!trx.ControlMessage)
            {
                if (this.config.SeparateUnknownFolder)
                {
                    this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, trx.SECS2HeaderLoggingString + " UNKNOWN_MESSAGE", null);
                    this.unknownLogger.Logger.Log(null, MyLevel.UNKNOWN, trx.SECS2HeaderLoggingString + " UNKNOWN_MESSAGE" + ConstUtils.NEWLINE + trx.SECS2BodyString, null);
                }
                else
                {
                    this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, trx.SECS2HeaderLoggingString + " UNKNOWN_MESSAGE" + ConstUtils.NEWLINE + trx.SECS2BodyString, null);
                }
            }
        }
    }
}
