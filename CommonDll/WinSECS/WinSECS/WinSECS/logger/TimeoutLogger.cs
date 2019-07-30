using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using log4net;
using WinSECS.global;

namespace WinSECS.logger
{
    [ComVisible(false)]
    public class TimeoutLogger
    {
        private SECSConfig config;
        private ILog secs1Logger;
        private ILog secs2Logger;

        public TimeoutLogger(SECSConfig config, ILog secs1Logger, ILog secs2Logger)
        {
            this.config = config;
            this.secs1Logger = secs1Logger;
            this.secs2Logger = secs2Logger;
        }

        public void WriteLog(string info)
        {
            switch (this.config.SecsLogMode)
            {
                case 0:
                    this.writeSECS1File(info);
                    break;

                case 1:
                    this.writeSECS1File(info);
                    this.writeSECS2File(info);
                    break;

                case 2:
                    this.writeSECS1File(info);
                    break;

                case 3:
                    this.writeSECS2File(info);
                    break;
            }
        }

        private void writeSECS1File(string log)
        {
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, string.Format("WARN {0}", log), null);
        }

        private void writeSECS2File(string log)
        {
            this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, string.Format("WARN {0}", log), null);
        }
    }
}
