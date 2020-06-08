using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
namespace MPC.Schedule
{
    /*
     * 定制化的计划任务例子
     * 
     */
    public class CustomizeSchedul
    {
        private static Common.Logging.ILog log = LogManager.GetLogger(typeof(CustomizeSchedul));
        private string schedulName ="";

        public string SchedulName
        {
          get { return schedulName; }
          set { schedulName = value; }
         }
        public void Excute()
        {
            log.Info("Start CustomizeSchedul:" + schedulName);
            log.Info("Do something work");
            System.Threading.Thread.Sleep(1000);
            log.Info("End CustomizeSchedul:" + schedulName);
        }
    }
}
