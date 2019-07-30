using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net.Core;
using log4net.Util;
using log4net.Appender;


namespace Log4netCustomizedAppender
{
    public class LogEventAppender : AppenderSkeleton
    {
        private Queue queue = new Queue();

        public static event AppendEventHandler OnAppended;

        public LogEventAppender()
        {
            ThreadStart start = new ThreadStart(this.EventFireCallBack);
            new Thread(start) { IsBackground = true }.Start();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            this.queue.Enqueue(loggingEvent);
        }

        private void EventFireCallBack()
        {
            while (this.queue.Running)
            {
                try
                {
                    object obj2 = this.queue.Dequeue();
                    if ((obj2 != null) && (obj2 is LoggingEvent))
                    {
                        LoggingEvent loggingEvent = obj2 as LoggingEvent;
                        if (OnAppended != null)
                        {
                            OnAppended(loggingEvent, base.RenderLoggingEvent(loggingEvent));
                        }
                    }
                    continue;
                }
                catch (Exception exception)
                {
                    LogLog.Error(typeof(LogEventAppender), exception.Message);
                    continue;
                }
            }
        }

        protected override void OnClose()
        {
            this.queue.Terminate();
            base.OnClose();
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public delegate void AppendEventHandler(LoggingEvent loggingEvent, string message);
    }
}
