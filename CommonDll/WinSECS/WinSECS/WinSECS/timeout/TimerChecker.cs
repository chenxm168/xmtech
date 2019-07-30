using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using WinSECS.Utility;
using WinSECS.global;

namespace WinSECS.timeout
{
    internal class TimerChecker : SupportClass.ThreadClass
    {
        internal TimerManager parentHandle;
        private IDictionary<string, SECSTimeout> timeoutlist;

        public TimerChecker(TimerManager parentHandle, IDictionary<string, SECSTimeout> timeoutlist)
        {
            this.timeoutlist = timeoutlist;
            this.parentHandle = parentHandle;
            base.Name = "TimeOutChecker";
        }

        public virtual void AcquireMutext()
        {
            TimerChecker checker;
            Monitor.Enter(checker = this);
            try
            {
                Monitor.Wait(this);
            }
            catch (ThreadInterruptedException)
            {
            }
            finally
            {
                Monitor.Exit(checker);
            }
        }

        public virtual void AcquireMutext(long waitTime)
        {
            TimerChecker checker;
            Monitor.Enter(checker = this);
            try
            {
                Monitor.Wait(this, TimeSpan.FromMilliseconds((double)waitTime));
            }
            catch (ThreadInterruptedException)
            {
            }
            finally
            {
                Monitor.Exit(checker);
            }
        }

        public virtual void releaseMutext()
        {
            lock (this)
            {
                Monitor.Pulse(this);
            }
        }

        public override void Run()
        {
            bool flag;
        Label_0182:
            flag = true;
            if (this.timeoutlist.Count == 0)
            {
                this.AcquireMutext();
            }
            else
            {
                SECSTimeout timeout = null;
                string key = "";
                long waitTime = 0L;
                lock (this.timeoutlist)
                {
                    foreach (string str2 in this.timeoutlist.Keys)
                    {
                        if (timeout == null)
                        {
                            timeout = this.timeoutlist[str2];
                            key = str2;
                        }
                        else
                        {
                            SECSTimeout timeout2 = this.timeoutlist[str2];
                            if (timeout2.TimeoutTime < timeout.TimeoutTime)
                            {
                                timeout = timeout2;
                                key = str2;
                            }
                        }
                    }
                    if ((this.timeoutlist.Count == 0) && (timeout == null))
                    {
                        goto Label_0182;
                    }
                    waitTime = timeout.TimeoutTime - CSharpUtil.currentTimeMillis();
                    if (waitTime <= 0L)
                    {
                        SECSTimeout timeout3 = (SECSTimeout)timeout.Clone();
                        this.parentHandle.OnTimeOut(timeout3);
                        SECSTimeout timeout4 = this.timeoutlist[key];
                        if (timeout4 != null)
                        {
                            timeout4 = null;
                        }
                        this.timeoutlist.Remove(key);
                    }
                }
                if (waitTime > 0L)
                {
                    this.AcquireMutext(waitTime);
                }
            }
            goto Label_0182;
        }
    }
}
