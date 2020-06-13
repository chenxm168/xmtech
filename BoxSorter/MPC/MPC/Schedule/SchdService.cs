using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace MPC.Schedule
{
   public class SchdService
    {
        private string schdName;

        public string SchdName
        {
            get { return schdName; }
            set { schdName = value; }
        }
        public void Excute()
        {
            ILog log = LogManager.GetLogger("EcsLog");
            log.DebugFormat("Start Schedul: {0} \n",new Object[]{ schdName});
            log.Info("-----------------------------------------------------------");
            log.DebugFormat("End Schedul: {0} \n", new Object[] { schdName });

        }

 
    }
}
