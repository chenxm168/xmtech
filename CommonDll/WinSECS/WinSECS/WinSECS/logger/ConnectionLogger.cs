using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using log4net;
using log4net.Core;
using WinSECS.global;

namespace WinSECS.logger
{
    [ComVisible(false)]
    public class ConnectionLogger
    {
        private SECSConfig config;
        private ILog secs1Logger;
        private ILog secs2Logger;

        public ConnectionLogger(SECSConfig config, ILog secs1Logger, ILog secs2Logger)
        {
            this.config = config;
            this.secs1Logger = secs1Logger;
            this.secs2Logger = secs2Logger;
        }

        public virtual void WriteLog(Level level, string info, bool reportData)
        {
            switch (this.config.SecsLogMode)
            {
                case 0:
                    this.writeSECS1File(level, info);
                    break;

                case 1:
                    this.writeSECS1File(level, info);
                    this.writeSECS2File(level, info);
                    break;

                case 2:
                    this.writeSECS1File(level, info);
                    break;

                case 3:
                    this.writeSECS2File(level, info);
                    break;
            }
        }

        private void writeSECS1File(Level level, string log)
        {
            this.secs1Logger.Logger.Log(null, MyLevel.SECS1_R, string.Format("{0} {1}", level.ToString(), log), null);
        }

        private void writeSECS2File(Level level, string log)
        {
            this.secs2Logger.Logger.Log(null, MyLevel.SECS2_R, string.Format("{0} {1}", level.ToString(), log), null);
        }
    }
}
