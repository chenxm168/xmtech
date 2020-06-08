using Spring.Scheduling.Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace MPC.Schedule
{
    /*
     * 继承QuartzJobObject的任务方法
     * 
     */
    public class ExtendQuartzSchedul:QuartzJobObject
    {
        private static Common.Logging.ILog log = LogManager.GetLogger(typeof(ExtendQuartzSchedul));
        private string schedulName = "";

        public string SchedulName
        {
            get { return schedulName; }
            set { schedulName = value; }
        }
        protected override void ExecuteInternal(Quartz.IJobExecutionContext context)
        {
            log.Info("Start ExtendQuartSchedul:" + SchedulName);
            log.Info("Do something work");
            System.Threading.Thread.Sleep(1000);
            log.Info("End ExtendQuartSchedul:" + SchedulName);
        }
    }
}
