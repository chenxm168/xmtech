using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.global
{
    public interface ISECSConfig
    {
        // Methods
        object Clone();
        int getTimeOut(int nTimeOutType);

        // Properties
        bool Active { get; set; }
        bool AllowInterleaving { get; set; }
        int AnalyzerOption { get; set; }
        int BaseMessageFilteringSize { get; set; }
        int BaudRate { get; set; }
        bool BlockLogging { get; set; }
        int DeviceId { get; set; }
        bool DispatchOn { get; set; }
        string DriverId { get; set; }
        int DriverLogLevel { get; set; }
        bool Host { get; set; }
        bool Hsmsmode { get; set; }
        string Id { get; set; }
        string IpAddress { get; set; }
        bool IsMaster { get; set; }
        int LinktestDuration { get; set; }
        bool LogModeDaily { get; set; }
        int LogModeDeleteDuration { get; set; }
        string LogRootPath { get; set; }
        long MaxLength { get; set; }
        string ModelingInfoFromFile { get; set; }
        string ModelingInfoFromXMLString { get; set; }
        int OverRawBinaryLength { get; set; }
        int Port { get; set; }
        string PortName { get; set; }
        int RetryLimit { get; set; }
        int SecsLogMode { get; set; }
        bool SeparateUnknownFolder { get; set; }
        float Timeout1 { get; set; }
        float Timeout2 { get; set; }
        int Timeout3 { get; set; }
        int Timeout4 { get; set; }
        int Timeout5 { get; set; }
        int Timeout6 { get; set; }
        int Timeout7 { get; set; }
        float Timeout8 { get; set; }
        bool UseRawBinary { get; set; }

    }
}
