using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BMDT.SECS
{
    public  abstract class AbsSecsHandler :ISecsHandler
    {
        protected ILog logger = LogManager.GetLogger(typeof(AbsSecsHandler));
        protected SECSWrapper secs;
        protected object args;
        private ICallback callback;

        protected ICallback Callback
        {
            get { return callback; }
            set { callback = value; }
        }

        

        public SECSWrapper Secs
        {
            get { return secs; }
            set { secs = value; }
        }

        



        public virtual void doWork(string driverId, object message)
        {
            proc(driverId, message);

            if(Callback!=null)
            {
                Callback.Callback(args);
            }
        }

       protected  abstract void proc(string driverId, object message);
    }
}
