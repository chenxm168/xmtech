using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using WinSECS.Utility;
using WinSECS.global;
using WinSECS.manager;
using WinSECS.driver;
using WinSECS.structure;
//using Log4netCustomizedAppender;
using cx.log4net;
using log4net.Config;
using log4net.Core;
using log4net;
using cx.log4net.appender;

namespace WinSECS.logger
{
    internal class LoggerManager : abstractManager
    {
        private ConnectionLogger conLoggerMgr;
        private const string driverAppender = "SEComDriver.DRIVER";
        private ILog driverLogger;
        public static readonly string key_deleteDatePattern = "deleteDatePattern";
        public static readonly string key_deletePeriod = "deletePeriod";
        public static readonly string key_file = "file";
        public static readonly string key_rootpath = "rootDir";
        private static readonly string logger_property_file = "log4net.xml";
        private MessageLogger msgLoggerMgr;
        internal int nDeleteDuration = 0;
        internal int nLogMode = 0;
        private const string reportAppender = "SEComDriver.REPORT";
        private ILog reportingLogger;
        private ReportLogger rptLoggerMgr;
        private const string secs1Appender = "SEComDriver.SECS-I";
        private ILog secs1Logger;
        private const string secs2Appender = "SEComDriver.SECS-II";
        private ILog secs2Logger;
        internal string sRootPath = ".";
        private TimeoutLogger toutLoggerMgr;
        private const string unknownAppender = "SEComDriver.UNKNOWN";
        private ILog unknownLogger;

        public void Configure(string fileName, string driver, string rootPath, string deleteduration, bool isDailyLog, int analyzerOption, int secsMode, bool isSeparateUnknownFolder, ReturnObject returnObject)
        {
            string str2;
            string embeddedResource = GetResource.GetEmbeddedResource("log4net.xml") as string;
            //string embeddedResource = GetResource.GetEmbeddedResource("log4net") as string;
            XmlDocument document = new XmlDocument();
            document.LoadXml(embeddedResource);
            XmlElement documentElement = document.DocumentElement;
            XmlElement element = document.CreateElement("log4net");
            foreach (XmlNode node in documentElement.SelectNodes("logger"))
            {
                if (node.Attributes["name"].Value == "SEComDriver.UNKNOWN")
                {
                    if (!isSeparateUnknownFolder)
                    {
                        continue;
                    }
                }
                else if (node.Attributes["name"].Value == "SEComDriver.REPORT")
                {
                    if (analyzerOption == 0)
                    {
                        continue;
                    }
                }
                else if (node.Attributes["name"].Value == "SEComDriver.SECS-I")
                {
                    if (((secsMode != 0) && (secsMode != 1)) && (secsMode != 2))
                    {
                        continue;
                    }
                }
                else if ((node.Attributes["name"].Value == "SEComDriver.SECS-II") && ((secsMode != 1) && (secsMode != 3)))
                {
                    continue;
                }
                node.Attributes["name"].Value = node.Attributes["name"].Value + "." + driver;
                foreach (XmlNode node2 in node.SelectNodes("appender-ref"))
                {
                    node2.Attributes["ref"].Value = node2.Attributes["ref"].Value + "." + driver;
                }
                if ((base.config.DriverLogLevel > 0) && node.Attributes["name"].Value.Contains("SEComDriver.DRIVER"))
                {
                    if (base.config.DriverLogLevel == 1)
                    {
                        str2 = "INFO";
                    }
                    else
                    {
                        str2 = "WARN";
                    }
                    foreach (XmlNode node3 in node.SelectNodes("level"))
                    {
                        node3.Attributes["value"].Value = str2;
                    }
                }
                element.AppendChild(node);
            }
            foreach (XmlNode node4 in documentElement.SelectNodes("appender"))
            {
                if (node4.Attributes["name"].Value.ToUpper() == "UNKNOWN")
                {
                    if (!isSeparateUnknownFolder)
                    {
                        continue;
                    }
                }
                else if (node4.Attributes["name"].Value.ToUpper() == "REPORT")
                {
                    if (analyzerOption == 0)
                    {
                        continue;
                    }
                }
                else if (node4.Attributes["name"].Value.ToUpper() == "SECS-I")
                {
                    if (((secsMode != 0) && (secsMode != 1)) && (secsMode != 2))
                    {
                        continue;
                    }
                }
                else if ((node4.Attributes["name"].Value.ToUpper() == "SECS-II") && ((secsMode != 1) && (secsMode != 3)))
                {
                    continue;
                }
                node4.Attributes["name"].Value = node4.Attributes["name"].Value + "." + driver;
                XmlNode node5 = node4.SelectSingleNode(key_rootpath);
                if (node5 != null)
                {
                    node5.Attributes["value"].Value = rootPath + Path.DirectorySeparatorChar.ToString() + driver;
                    node5 = null;
                }
                node5 = node4.SelectSingleNode(key_deletePeriod);
                if (node5 != null)
                {
                    node5.Attributes["value"].Value = deleteduration;
                    node5 = null;
                }
                node5 = node4.SelectSingleNode(key_file);
                if (node5 != null)
                {
                    string[] strArray = node5.Attributes["value"].Value.Split(new char[] { '/' });
                    if (strArray.Length == 4)
                    {
                        if (isDailyLog)
                        {
                            str2 = strArray[0] + Path.DirectorySeparatorChar.ToString() + strArray[2];
                        }
                        else
                        {
                            str2 = strArray[0] + Path.DirectorySeparatorChar.ToString() + strArray[1] + Path.DirectorySeparatorChar.ToString() + strArray[3];
                        }
                        node5.Attributes["value"].Value = str2;
                    }
                    node5 = null;
                }
                element.AppendChild(node4);
            }
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetAssembly(typeof(DynamicFileAppender))), element);
        }

        public override void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            base.Initialize(rootHandle, config, returnObject);
            this.Configure(logger_property_file, config.DriverId, config.LogRootPath, string.Format("{0}", config.LogModeDeleteDuration), config.LogModeDaily, config.AnalyzerOption, config.SecsLogMode, config.SeparateUnknownFolder, returnObject);
            if (returnObject.isSuccess())
            {
                this.setAssignLogger();
            }
        }

        public void ReloadConfig(SECSConfig NewConfig, ReturnObject returnObject)
        {
            bool flag = false;
            if (base.config.DriverId != NewConfig.DriverId)
            {
                flag = true;
            }
            if (!(flag || (base.config.LogModeDaily == NewConfig.LogModeDaily)))
            {
                flag = true;
            }
            if (!(flag || (base.config.LogModeDeleteDuration == NewConfig.LogModeDeleteDuration)))
            {
                flag = true;
            }
            if (!(flag || !(base.config.LogRootPath != NewConfig.LogRootPath)))
            {
                flag = true;
            }
            if (flag)
            {
                this.Configure(logger_property_file, NewConfig.DriverId, NewConfig.LogRootPath, string.Format("{0}", NewConfig.LogModeDeleteDuration), NewConfig.LogModeDaily, NewConfig.AnalyzerOption, NewConfig.SecsLogMode, base.config.SeparateUnknownFolder, returnObject);
                if (returnObject.isSuccess())
                {
                    base.config = NewConfig;
                    this.setAssignLogger();
                }
            }
        }

        private void setAssignLogger()
        {
            this.driverLogger = LogManager.GetLogger("SEComDriver.DRIVER." + base.config.DriverId);
            this.secs1Logger = LogManager.GetLogger("SEComDriver.SECS-I." + base.config.DriverId);
            this.secs2Logger = LogManager.GetLogger("SEComDriver.SECS-II." + base.config.DriverId);
            this.reportingLogger = LogManager.GetLogger("SEComDriver.REPORT." + base.config.DriverId);
            this.unknownLogger = LogManager.GetLogger("SEComDriver.UNKNOWN." + base.config.DriverId);
            this.msgLoggerMgr = new MessageLogger(base.config, this.secs1Logger, this.secs2Logger, this.unknownLogger);
            this.conLoggerMgr = new ConnectionLogger(base.config, this.secs1Logger, this.secs2Logger);
            this.rptLoggerMgr = new ReportLogger(base.config, this.reportingLogger);
            this.toutLoggerMgr = new TimeoutLogger(base.config, this.secs1Logger, this.secs2Logger);
        }

        public override void Terminate(ReturnObject returnobject)
        {
        }

        public void WriteConnnectionLog(Level level, string info, bool reportData)
        {
            this.conLoggerMgr.WriteLog(level, info, reportData);
            if (reportData)
            {
                this.rptLoggerMgr.WriteConnectionLog(info);
            }
        }

        public void WriteInvalidLog(SECSTransaction trx, string invalidReason)
        {
            this.msgLoggerMgr.WriteInvalidLog(trx, invalidReason);
            this.rptLoggerMgr.WriteTransactionLog(trx, invalidReason);
        }

        public void writeInvalidSECS1(SECSTransaction trx, string InvalidReason)
        {
            this.msgLoggerMgr.writeInvalidSECS1(trx, InvalidReason);
        }

        public void WriteLog(SECSTransaction trx, bool isModelingData)
        {
            this.msgLoggerMgr.WriteLog(trx, isModelingData);
            this.rptLoggerMgr.WriteTransactionLog(trx);
        }

        public void WriteLog(string log, bool isReceived)
        {
            this.msgLoggerMgr.WriteLog(log, isReceived);
        }

        public void WriteLogSECS1Only(string log, bool isReceived)
        {
            this.msgLoggerMgr.WriteLogSECS1Only(log, isReceived);
        }

        public void WriteReportConnectionLog(string info)
        {
            this.rptLoggerMgr.WriteConnectionLog(info);
        }

        public void WriteReportRequestedLog(SECSTransaction trx)
        {
            this.rptLoggerMgr.WriteRequestedLog(trx);
        }

        public void WriteReportTimeoutLog(string log)
        {
            this.rptLoggerMgr.WriteTimeoutLog(log);
        }

        public void WriteReportTransactionLog(SECSTransaction trx)
        {
            this.rptLoggerMgr.WriteTransactionLog(trx);
        }

        public void WriteReportTransactionLog(SECSTransaction trx, string Error)
        {
            this.rptLoggerMgr.WriteTransactionLog(trx, Error);
        }

        public void WriteRequestedLog(SECSTransaction trx)
        {
            this.rptLoggerMgr.WriteRequestedLog(trx);
        }

        public void WriteTimeoutLog(SECSTransaction trx)
        {
            string str;
            if (trx.ControlMessage)
            {
                str = string.Format("Timeout_T6 {0}", headerInformation.getControlMessageType(trx.Header));
            }
            else
            {
                str = string.Format("Timeout_T3 S{0}F{1} {2} Systebyte={3}", new object[] { trx.Stream, trx.Function, trx.MessageName, trx.Systembyte });
            }
            this.toutLoggerMgr.WriteLog(str);
            this.rptLoggerMgr.WriteTimeoutLog(str);
        }

        public void WriteTimeoutLog(EnumSet.TIMEOUT category, string info)
        {
            string str = string.Format("Timeout_{0} {1}", category.ToString(), info);
            this.toutLoggerMgr.WriteLog(str);
            this.rptLoggerMgr.WriteTimeoutLog(str);
        }

        public void WriteUnknownLog(SECSTransaction trx)
        {
            this.msgLoggerMgr.WriteUnknownLog(trx);
            this.rptLoggerMgr.WriteTransactionLog(trx, "UNKNOWN_Message");
        }

        public void writeUnknownSECS1ByLengthFilter(SECSTransaction trx, string unKnownReason)
        {
            this.msgLoggerMgr.writeUnknownSECS1ByLengthFilter(trx, unKnownReason);
        }

        public ILog Logger
        {
            get
            {
                return this.driverLogger;
            }
        }
    }
}
