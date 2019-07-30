using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using log4net.Appender;
using log4net.Core;
using log4net.Util;


namespace Log4netCustomizedAppender
{
    public class DynamicFileAppender : FileAppender
    {
        protected string deleteDatePattern;
        protected string deleteExtension;
        protected string deletePath;
        protected int deletePeriod;
        protected RollPoint deleteRollPoint;
        protected string filePattern;
        private ILogListener listener;
        private DateTime nextCheckDate = DateTime.Now.AddSeconds(-1.0);
        protected RollPoint rollPoint = RollPoint.InvalidRollPoint;
        protected string rootDir;
        private static readonly DateTime s_date1970 = new DateTime(0x7b2, 1, 1);

        public override void ActivateOptions()
        {
            if (this.File != null)
            {
                this.filePattern = this.File;
                Regex regex = new Regex("[%][d][{][^%]+[}]");
                foreach (Match match in regex.Matches(this.filePattern))
                {
                    string datePattern = match.Value.Substring(3, match.Value.Length - 4);
                    RollPoint point = this.ComputeCheckPeriod(datePattern);
                    if ((this.rollPoint == RollPoint.InvalidRollPoint) || (this.rollPoint > point))
                    {
                        this.rollPoint = point;
                    }
                }
                if (this.rollPoint == RollPoint.InvalidRollPoint)
                {
                    throw new ArgumentException("Invalid RollPoint, unable to parse [" + this.filePattern + "]");
                }
            }
            else
            {
                LogLog.Error(typeof(DynamicFileAppender), "Either File or DatePattern options are not set for appender [" + base.Name + "].");

            }
            this.ChangeFileName();
            base.ActivateOptions();
        }

        private void AdjustFileBeforeAppend()
        {
            DateTime now = DateTime.Now;
            if (now >= this.nextCheckDate)
            {
                this.ChangeFileName();
                this.nextCheckDate = this.NextCheckDate(now, this.rollPoint);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.FilterEvent(loggingEvent))
            {
                this.AdjustFileBeforeAppend();
                base.Append(loggingEvent);
                if (this.listener != null)
                {
                    this.listener.OnAppend(loggingEvent, base.RenderLoggingEvent(loggingEvent));
                }
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            List<LoggingEvent> list = new List<LoggingEvent>();
            foreach (LoggingEvent event2 in loggingEvents)
            {
                if (this.FilterEvent(event2))
                {
                    list.Add(event2);
                }
            }
            if (list.Count != 0)
            {
                this.AdjustFileBeforeAppend();
                base.Append(list.ToArray());
            }
        }

        private void ChangeFileName()
        {
            this.File = this.rootDir + "/" + this.CreateNewFilePath(this.filePattern);
            base.ActivateOptions();
            this.DeleteOldFiles();
        }

        private RollPoint ComputeCheckPeriod(string datePattern)
        {
            string str = s_date1970.ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
            for (int i = 0; i <= 5; i++)
            {
                string str2 = this.NextCheckDate(s_date1970, (RollPoint)i).ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
                LogLog.Debug(typeof(DynamicFileAppender), string.Concat(new object[] { "RollingFileAppender: Type = [", i, "], r0 = [", str, "], r1 = [", str2, "]" }));
                if (((str != null) && (str2 != null)) && !str.Equals(str2))
                {
                    return (RollPoint)i;
                }
            }
            return RollPoint.InvalidRollPoint;
        }

        private string CreateNewFilePath(string filePattern)
        {
            DateTime now = DateTime.Now;
            Regex regex = new Regex("[%][d][{][^%]+[}]");
            string str = "";
            int startIndex = 0;
            foreach (Match match in regex.Matches(filePattern))
            {
                str = str + filePattern.Substring(startIndex, match.Index - startIndex);
                str = str + now.ToString(match.Value.Substring(3, match.Value.Length - 4));
                startIndex = match.Index + match.Length;
            }
            if (startIndex < filePattern.Length)
            {
                str = str + filePattern.Substring(startIndex);
            }
            return str;
        }

        private void DeleteFiles(DirectoryInfo dInfo, long lastModified)
        {
            foreach (FileInfo info in dInfo.GetFiles(this.deleteExtension))
            {
                if (info.LastWriteTime.Ticks < lastModified)
                {
                    try
                    {
                        LogLog.Debug(typeof(DynamicFileAppender), string.Concat(new object[] { "Delete ", info.Name, " ", info.LastWriteTime }));
                        info.Delete();
                    }
                    catch (Exception exception)
                    {
                        LogLog.Error(typeof(DynamicFileAppender), "Delete File", exception);
                    }
                }
            }
            foreach (DirectoryInfo info2 in dInfo.GetDirectories())
            {
                this.DeleteFiles(info2, lastModified);
                if (info2.GetFileSystemInfos().Length == 0)
                {
                    try
                    {
                        LogLog.Debug(typeof(DynamicFileAppender), "Delete " + info2.Name);
                        info2.Delete();
                    }
                    catch (Exception exception2)
                    {
                        LogLog.Error(typeof(DynamicFileAppender), "Delete Directory", exception2);
                    }
                }
            }
        }

        private void DeleteOldFiles()
        {
            string path = FileAppender.ConvertToFullPath(this.rootDir + "/" + this.deletePath);
            if (((this.deleteRollPoint != RollPoint.InvalidRollPoint) && (this.deletePeriod > 0)) && (path != ""))
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                if (dInfo.Exists)
                {
                    long lastModified = 0L;
                    if (this.deleteRollPoint == RollPoint.TopOfMinute)
                    {
                        lastModified = DateTime.Now.AddMinutes((double)-this.deletePeriod).Ticks;
                    }
                    else if (this.deleteRollPoint == RollPoint.TopOfHour)
                    {
                        lastModified = DateTime.Now.AddHours((double)-this.deletePeriod).Ticks;
                    }
                    else if (this.deleteRollPoint == RollPoint.TopOfDay)
                    {
                        lastModified = DateTime.Now.AddDays((double)-this.deletePeriod).Ticks;
                    }
                    else if (this.deleteRollPoint == RollPoint.TopOfWeek)
                    {
                        lastModified = DateTime.Now.AddDays((double)-(this.deletePeriod * 7)).Ticks;
                    }
                    else if (this.deleteRollPoint == RollPoint.TopOfMonth)
                    {
                        lastModified = DateTime.Now.AddMonths(-this.deletePeriod).Ticks;
                    }
                    else
                    {
                        return;
                    }
                    this.DeleteFiles(dInfo, lastModified);
                }
            }
        }

        protected DateTime NextCheckDate(DateTime currentDateTime, RollPoint rollPoint)
        {
            DateTime time = currentDateTime;
            switch (rollPoint)
            {
                case RollPoint.TopOfMinute:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    return time.AddSeconds((double)-time.Second).AddMinutes(1.0);

                case RollPoint.TopOfHour:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    time = time.AddSeconds((double)-time.Second);
                    return time.AddMinutes((double)-time.Minute).AddHours(1.0);

                case RollPoint.HalfDay:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    time = time.AddSeconds((double)-time.Second);
                    time = time.AddMinutes((double)-time.Minute);
                    if (time.Hour >= 12)
                    {
                        return time.AddHours((double)-time.Hour).AddDays(1.0);
                    }
                    return time.AddHours((double)(12 - time.Hour));

                case RollPoint.TopOfDay:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    time = time.AddSeconds((double)-time.Second);
                    time = time.AddMinutes((double)-time.Minute);
                    return time.AddHours((double)-time.Hour).AddDays(1.0);

                case RollPoint.TopOfWeek:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    time = time.AddSeconds((double)-time.Second);
                    time = time.AddMinutes((double)-time.Minute);
                    time = time.AddHours((double)-time.Hour);
                    return time.AddDays((double)(7 - time.DayOfWeek));

                case RollPoint.TopOfMonth:
                    time = time.AddMilliseconds((double)-time.Millisecond);
                    time = time.AddSeconds((double)-time.Second);
                    time = time.AddMinutes((double)-time.Minute);
                    time = time.AddHours((double)-time.Hour);
                    return time.AddDays((double)(1 - time.Day)).AddMonths(1);
            }
            return time;
        }

        public string DeleteDatePattern
        {
            get
            {
                return this.deleteDatePattern;
            }
            set
            {
                this.deleteDatePattern = value;
                this.deleteRollPoint = this.ComputeCheckPeriod(this.deleteDatePattern);
            }
        }

        public string DeleteExtension
        {
            get
            {
                return this.deleteExtension;
            }
            set
            {
                this.deleteExtension = value;
            }
        }

        public string DeletePath
        {
            get
            {
                return this.deletePath;
            }
            set
            {
                this.deletePath = value;
            }
        }

        public int DeletePeriod
        {
            get
            {
                return this.deletePeriod;
            }
            set
            {
                this.deletePeriod = value;
            }
        }

        public ILogListener LogListener
        {
            get
            {
                return this.listener;
            }
            set
            {
                this.listener = value;
            }
        }

        public string RootDir
        {
            get
            {
                return this.rootDir;
            }
            set
            {
                this.rootDir = value;
            }
        }

        protected enum RollPoint
        {
            HalfDay = 2,
            InvalidRollPoint = -1,
            TopOfDay = 3,
            TopOfHour = 1,
            TopOfMinute = 0,
            TopOfMonth = 5,
            TopOfWeek = 4
        }
    }
}
