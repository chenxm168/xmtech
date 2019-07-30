using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Scheduling.Quartz;
using Common.Logging;

namespace MPC.Schedule
{
    public class SchedulJob:QuartzJobObject
    {
        private string jobName;

        public string JobName
        {
            get { return jobName; }
            set { jobName = value; }
        }
        /*
        protected override void ExecuteInternal(Quartz.IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger("EcsLog");
            log.DebugFormat("Start SchedulJog: {0} \n", new Object[] { jobName });
            System.Console.WriteLine("-----------------------------------------------------------");
            log.DebugFormat("End SchedulJob: {0} \n", new Object[] { jobName });
        }*/



        protected override void ExecuteInternal(Quartz.IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger("EcsLog");
            log.DebugFormat("Start SchedulJog: {0} \n", new Object[] { jobName });
            log.Info("-----------------------------------------------------------\n");
            log.DebugFormat("End SchedulJob: {0} \n", new Object[] { jobName });
        }
    }
}
