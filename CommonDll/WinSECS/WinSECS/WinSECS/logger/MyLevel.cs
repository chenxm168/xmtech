using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using log4net.Core;

namespace WinSECS.logger
{
    [ComVisible(false)]
    public class MyLevel
    {
        public static readonly Level REPORT = new Level(REPORT_INT, "REPT", "REPT");
        public static readonly int REPORT_INT = (Level.Info.Value + 0x15);
        public static readonly Level SECS1_R = new Level(SECSI_RECV_INT, "RECV", "RECV");
        public static readonly Level SECS1_S = new Level(SECSI_SEND_INT, "SEND", "SEND");
        public static readonly Level SECS2_R = new Level(SECSII_RECV_INT, "RECV", "RECV");
        public static readonly Level SECS2_S = new Level(SECSII_SEND_INT, "SEND", "SEND");
        public static readonly int SECSI_RECV_INT = (Level.Info.Value + 1);
        public static readonly int SECSI_SEND_INT = (Level.Info.Value + 2);
        public static readonly int SECSII_RECV_INT = (Level.Info.Value + 11);
        public static readonly int SECSII_SEND_INT = (Level.Info.Value + 12);
        public static readonly Level UNKNOWN = new Level(UNKNOWN_INT, "UNKN", "UNKN");
        public static readonly int UNKNOWN_INT = (Level.Info.Value + 0x1f);
    }
}
