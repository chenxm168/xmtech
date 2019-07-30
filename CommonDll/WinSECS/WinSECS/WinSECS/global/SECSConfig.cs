using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinSECS.global
{
    public class SECSConfig : ISECSConfig, ICloneable
    {
        private bool active = true;
        private bool allowInterleaving = true;
        private int analyzerOption = 7;
        private int baseMessageFilteringSize = 0;
        private int baudRate = 0x2580;
        private int deviceId;
        private bool dispatchOn = true;
        private string driverId;
        private int driverLogLevel = 1;
        private string eqpType = "";
        private bool host = true;
        private bool hsmsmode = true;
        private string id = "WinSECS1.0";
        private string ipAddress = "127.0.0.1";
        private bool isBlockLogging = false;
        private bool isMaster;
        private int linktestDuration = 120;
        private bool logModeDaily = true;
        private int logModeDeleteDuration = 3;
        private string logRootPath = ".";
        private long maxLength = 0x9c4000L;
        private string modelingInfoFromFile = "";
        private string modelingInfoFromXMLString = "";
        private int overRawBinaryLength = 0x3e8;
        private int port = 0x1388;
        private string portName;
        private int retryLimit = 3;
        private int secsLogMode = 1;
        private bool separateUnknownFolder = false;
        private float t1 = 0.5f;
        private float t2 = 1f;
        private float t3 = 45f;
        private float t4 = 0.5f;
        private float timeout1 = 0.5f;
        private float timeout2 = 1f;
        private int timeout3 = 0x2d;
        private int timeout4 = 0x2d;
        private int timeout5 = 10;
        private int timeout6 = 10;
        private int timeout7 = 10;
        private float timeout8 = 0.5f;
        private bool useRawBinary = false;

        public object Clone()
        {
            return (SECSConfig)base.MemberwiseClone();
        }

        public static SECSConfig getSECSConfigFromXML(XmlNode driverInfo)
        {
            //return new SECSConfig
            //{
            //    DriverId = driverInfo.Name,
            //    DeviceId = int.Parse(driverInfo.SelectSingleNode("DRVINFO/DEVICEID").InnerText),
            //    Host = bool.Parse(driverInfo.SelectSingleNode("DRVINFO/HOST").InnerText),
            //    Hsmsmode = bool.Parse(driverInfo.SelectSingleNode("DRVINFO/HSMSMODE").InnerText),
            //    //ModelingInfoFromFile = driverInfo.SelectSingleNode("DRVINFO/SMDFILEPATH").InnerText.Trim(),  //cxm add
            //    Active = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/HSMS/ACTIVE").InnerText),
            //    IpAddress = driverInfo.SelectSingleNode("SECSMODE/HSMS/IP").InnerText,
            //    Port = int.Parse(driverInfo.SelectSingleNode("SECSMODE/HSMS/PORT").InnerText),
            //    IsMaster = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/MASTER").InnerText),
            //    AllowInterleaving = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/INTERLEAVE").InnerText),
            //    PortName = driverInfo.SelectSingleNode("SECSMODE/SECS1/COMPORT").InnerText,
            //    BaudRate = int.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/BAUDRATE").InnerText),
            //    RetryLimit = int.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/RETRYCOUNT").InnerText),
            //    isBlockLogging = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/BLOCKLOGGING").InnerText),
            //    LogRootPath = driverInfo.SelectSingleNode("LOGINFO/ROOTDIR").InnerText,
            //    SecsLogMode = int.Parse(driverInfo.SelectSingleNode("LOGINFO/LOGMODE").InnerText),
            //    SeparateUnknownFolder = bool.Parse(driverInfo.SelectSingleNode("LOGINFO/SEPARATEUNKNOWN").InnerText),
            //    DriverLogLevel = int.Parse(driverInfo.SelectSingleNode("LOGINFO/LOGLEVEL").InnerText),
            //    LogModeDeleteDuration = int.Parse(driverInfo.SelectSingleNode("LOGINFO/DURATION").InnerText),
            //    LogModeDaily = bool.Parse(driverInfo.SelectSingleNode("LOGINFO/TIMEBASE").InnerText),         //
            //    DispatchOn = bool.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/DISPATCH").InnerText),   //
            //    MaxLength = long.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/SOCKETMAXLENGTH").InnerText),
            //    AnalyzerOption = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/REPORTLOG").InnerText),
            //    UseRawBinary = bool.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/USERAWBINARY").InnerText),
            //    OverRawBinaryLength = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/RAWBINARYLENGTH").InnerText),
            //    baseMessageFilteringSize = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/BASEMESSAGEFILTERINGSIZE").InnerText),
            //    Timeout1 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T1").InnerText),
            //    Timeout2 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T2").InnerText),
            //    Timeout3 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T3").InnerText),
            //    Timeout4 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T4").InnerText),
            //    Timeout5 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T5").InnerText),
            //    Timeout6 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T6").InnerText),
            //    Timeout7 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T7").InnerText),
            //    Timeout8 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T8").InnerText),
            //    LinktestDuration = int.Parse(driverInfo.SelectSingleNode("EXT/LINKTEST").InnerText),
            //    eqpType = driverInfo.SelectSingleNode("EQPTYPE").InnerText
            //};

            SECSConfig cfg = new SECSConfig();

            try {
                cfg.DriverId = driverInfo.Name;
                cfg.DeviceId = int.Parse(driverInfo.SelectSingleNode("DRVINFO/DEVICEID").InnerText);
                cfg.Host = bool.Parse(driverInfo.SelectSingleNode("DRVINFO/HOST").InnerText);
                cfg.Hsmsmode = bool.Parse(driverInfo.SelectSingleNode("DRVINFO/HSMSMODE").InnerText);
                //ModelingInfoFromFile = driverInfo.SelectSingleNode("DRVINFO/SMDFILEPATH").InnerText.Trim(),  //cxm add
                
                cfg.Active = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/HSMS/ACTIVE").InnerText);
                cfg.IpAddress = driverInfo.SelectSingleNode("SECSMODE/HSMS/IP").InnerText;
                cfg.Port = int.Parse(driverInfo.SelectSingleNode("SECSMODE/HSMS/PORT").InnerText);
                if (!cfg.Hsmsmode)
                { 
                cfg.IsMaster = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/MASTER").InnerText);
                cfg.AllowInterleaving = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/INTERLEAVE").InnerText);
                cfg.PortName = driverInfo.SelectSingleNode("SECSMODE/SECS1/COMPORT").InnerText;
                cfg.BaudRate = int.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/BAUDRATE").InnerText);
                cfg.RetryLimit = int.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/RETRYCOUNT").InnerText);
                cfg.isBlockLogging = bool.Parse(driverInfo.SelectSingleNode("SECSMODE/SECS1/BLOCKLOGGING").InnerText);
                }
                cfg.LogRootPath = driverInfo.SelectSingleNode("LOGINFO/ROOTDIR").InnerText;
                cfg.SecsLogMode = int.Parse(driverInfo.SelectSingleNode("LOGINFO/LOGMODE").InnerText);
                cfg.SeparateUnknownFolder = bool.Parse(driverInfo.SelectSingleNode("LOGINFO/SEPARATEUNKNOWN").InnerText);
                cfg.DriverLogLevel = int.Parse(driverInfo.SelectSingleNode("LOGINFO/LOGLEVEL").InnerText);
                cfg.LogModeDeleteDuration = int.Parse(driverInfo.SelectSingleNode("LOGINFO/DURATION").InnerText);
                cfg.LogModeDaily = bool.Parse(driverInfo.SelectSingleNode("LOGINFO/TIMEBASE").InnerText);         //
                cfg.DispatchOn = bool.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/DISPATCH").InnerText);   //
                cfg.MaxLength = long.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/SOCKETMAXLENGTH").InnerText);
                cfg.AnalyzerOption = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/REPORTLOG").InnerText);
                cfg.UseRawBinary = bool.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/USERAWBINARY").InnerText);
                cfg.OverRawBinaryLength = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/RAWBINARYLENGTH").InnerText);
                cfg.baseMessageFilteringSize = int.Parse(driverInfo.SelectSingleNode("SPECIFICOPTION/BASEMESSAGEFILTERINGSIZE").InnerText);
                cfg.Timeout1 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T1").InnerText);
                cfg.Timeout2 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T2").InnerText);
                cfg.Timeout3 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T3").InnerText);
                cfg.Timeout4 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T4").InnerText);
                cfg.Timeout5 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T5").InnerText);
                cfg.Timeout6 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T6").InnerText);
                cfg.Timeout7 = int.Parse(driverInfo.SelectSingleNode("TIMEOUT/T7").InnerText);
                cfg.Timeout8 = float.Parse(driverInfo.SelectSingleNode("TIMEOUT/T8").InnerText);
                cfg.LinktestDuration = int.Parse(driverInfo.SelectSingleNode("EXT/LINKTEST").InnerText);
                cfg.eqpType = driverInfo.SelectSingleNode("EQPTYPE").InnerText;
                 }
            catch (Exception e)
            {
                log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SECSConfig));
                logger.Error(e.Message);
            }

     
                return cfg;

            
        }

        public virtual int getTimeOut(int nTimeOutType)
        {
            switch (nTimeOutType)
            {
                case -8:
                    return (int)this.Timeout8;

                case -7:
                    return this.Timeout7;

                case -6:
                    return this.Timeout6;

                case -5:
                    return this.Timeout5;

                case -3:
                    return this.Timeout3;

                case -100:
                    return this.LinktestDuration;
            }
            return 0;
        }

        public virtual bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
            }
        }

        public bool AllowInterleaving
        {
            get
            {
                return this.allowInterleaving;
            }
            set
            {
                this.allowInterleaving = value;
            }
        }

        public virtual int AnalyzerOption
        {
            get
            {
                return this.analyzerOption;
            }
            set
            {
                this.analyzerOption = value;
            }
        }

        public virtual int BaseMessageFilteringSize
        {
            get
            {
                return this.baseMessageFilteringSize;
            }
            set
            {
                this.baseMessageFilteringSize = value;
            }
        }

        public int BaudRate
        {
            get
            {
                return this.baudRate;
            }
            set
            {
                this.baudRate = value;
            }
        }

        public bool BlockLogging
        {
            get
            {
                return this.isBlockLogging;
            }
            set
            {
                this.isBlockLogging = value;
            }
        }

        public virtual int DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                this.deviceId = value;
            }
        }

        public virtual bool DispatchOn
        {
            get
            {
                return this.dispatchOn;
            }
            set
            {
                this.dispatchOn = value;
            }
        }

        public virtual string DriverId
        {
            get
            {
                return this.driverId;
            }
            set
            {
                this.driverId = value;
            }
        }

        public virtual int DriverLogLevel
        {
            get
            {
                return this.driverLogLevel;
            }
            set
            {
                this.driverLogLevel = value;
            }
        }

        public virtual string EqpType
        {
            get
            {
                return this.eqpType;
            }
            set
            {
                this.eqpType = value;
            }
        }

        public virtual bool Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        public bool Hsmsmode
        {
            get
            {
                return this.hsmsmode;
            }
            set
            {
                this.hsmsmode = value;
            }
        }

        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public virtual string IpAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
            }
        }

        public bool IsMaster
        {
            get
            {
                return this.isMaster;
            }
            set
            {
                this.isMaster = value;
            }
        }

        public virtual int LinktestDuration
        {
            get
            {
                return this.linktestDuration;
            }
            set
            {
                this.linktestDuration = value;
            }
        }

        public virtual bool LogModeDaily
        {
            get
            {
                return this.logModeDaily;
            }
            set
            {
                this.logModeDaily = value;
            }
        }

        public virtual int LogModeDeleteDuration
        {
            get
            {
                return this.logModeDeleteDuration;
            }
            set
            {
                this.logModeDeleteDuration = value;
            }
        }

        public virtual string LogRootPath
        {
            get
            {
                return this.logRootPath;
            }
            set
            {
                this.logRootPath = value;
            }
        }

        public virtual long MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                this.maxLength = value;
            }
        }

        public virtual string ModelingInfoFromFile
        {
            get
            {
                return this.modelingInfoFromFile;
            }
            set
            {
                this.modelingInfoFromFile = value;
            }
        }

        public virtual string ModelingInfoFromXMLString
        {
            get
            {
                return this.modelingInfoFromXMLString;
            }
            set
            {
                this.modelingInfoFromXMLString = value;
            }
        }

        public virtual int OverRawBinaryLength
        {
            get
            {
                return this.overRawBinaryLength;
            }
            set
            {
                this.overRawBinaryLength = value;
            }
        }

        public virtual int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }

        public string PortName
        {
            get
            {
                return this.portName;
            }
            set
            {
                this.portName = value;
            }
        }

        public int RetryLimit
        {
            get
            {
                return this.retryLimit;
            }
            set
            {
                this.retryLimit = value;
            }
        }

        public virtual int SecsLogMode
        {
            get
            {
                return this.secsLogMode;
            }
            set
            {
                this.secsLogMode = value;
            }
        }

        public bool SeparateUnknownFolder
        {
            get
            {
                return this.separateUnknownFolder;
            }
            set
            {
                this.separateUnknownFolder = value;
            }
        }

        public virtual float Timeout1
        {
            get
            {
                return this.timeout1;
            }
            set
            {
                this.timeout1 = value;
            }
        }

        public virtual float Timeout2
        {
            get
            {
                return this.timeout2;
            }
            set
            {
                this.timeout2 = value;
            }
        }

        public virtual int Timeout3
        {
            get
            {
                return this.timeout3;
            }
            set
            {
                this.timeout3 = value;
            }
        }

        public virtual int Timeout4
        {
            get
            {
                return this.timeout4;
            }
            set
            {
                this.timeout4 = value;
            }
        }

        public virtual int Timeout5
        {
            get
            {
                return this.timeout5;
            }
            set
            {
                this.timeout5 = value;
            }
        }

        public virtual int Timeout6
        {
            get
            {
                return this.timeout6;
            }
            set
            {
                this.timeout6 = value;
            }
        }

        public virtual int Timeout7
        {
            get
            {
                return this.timeout7;
            }
            set
            {
                this.timeout7 = value;
            }
        }

        public virtual float Timeout8
        {
            get
            {
                return this.timeout8;
            }
            set
            {
                this.timeout8 = value;
            }
        }

        public virtual bool UseRawBinary
        {
            get
            {
                return this.useRawBinary;
            }
            set
            {
                this.useRawBinary = value;
            }
        }
    }
}
