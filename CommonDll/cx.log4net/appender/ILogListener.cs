using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;

namespace cx.log4net.appender
{
    public interface ILogListener
    {
        // Methods
        void OnAppend(LoggingEvent evt, string log);
    }


}
