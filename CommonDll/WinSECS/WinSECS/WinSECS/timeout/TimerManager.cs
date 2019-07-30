using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using log4net.Core;
using WinSECS.manager;
using WinSECS.driver;
using WinSECS.structure;
using WinSECS.global;
using WinSECS.Utility;

namespace WinSECS.timeout
{
    internal class TimerManager : abstractManager
    {
        private object mutex = new object();
        private Dictionary<string, SECSTimeout> timeoutlist;
        private TimerChecker timerchecker;

        public virtual bool ClearTimerList()
        {
            if ((this.timeoutlist != null) && (this.timeoutlist != null))
            {
                lock (this.timeoutlist)
                {
                    if (this.timeoutlist.Count > 0)
                    {
                        IEnumerator enumerator = this.timeoutlist.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            SECSTimeout current = enumerator.Current as SECSTimeout;
                            current = null;
                        }
                    }
                }
            }
            return true;
        }

        public override void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            base.Initialize(rootHandle, config, returnObject);
            this.timeoutlist = new Dictionary<string, SECSTimeout>();
            this.timerchecker = new TimerChecker(this, this.timeoutlist);
            this.timerchecker.IsBackground = true;
            this.timerchecker.Start();
        }

        public virtual void OnTimeOut(SECSTimeout timeout)
        {
            if (timeout != null)
            {
                long disconnectToken;
                if (timeout.Id == -6)
                {
                    base.rootHandle.ManagerFactory.LoggerManager.WriteTimeoutLog(timeout.Message as SECSTransaction);
                    disconnectToken = base.rootHandle.ManagerFactory.ConnectManager.GetDisconnectToken();
                    if (disconnectToken != 0L)
                    {
                        base.rootHandle.ManagerFactory.ConnectManager.releaseMutext();
                        base.rootHandle.ManagerFactory.LoggerManager.Logger.Info("T6 Timeout Release Mutex for Disconnection " + timeout.Message.SECS2HeaderLoggingString);
                        base.rootHandle.ManagerFactory.ConnectManager.ReleaseDisconnectToken(disconnectToken);
                    }
                    else
                    {
                        base.rootHandle.ManagerFactory.LoggerManager.Logger.Info("T6 Timeout Release Mutex for Disconnection, but No Token " + timeout.Message.SECS2HeaderLoggingString);
                    }
                }
                else if (timeout.Id == -3)
                {
                    base.rootHandle.ManagerFactory.LoggerManager.WriteTimeoutLog(timeout.Message as SECSTransaction);
                    base.rootHandle.ManagerFactory.ConnectManager.sendS9Fx(9, timeout.Message as SECSTransaction);
                    base.rootHandle.ManagerFactory.CallbackManager.onTimeout("", timeout);
                }
                else if (timeout.Id == -7)
                {
                    base.rootHandle.ManagerFactory.LoggerManager.WriteTimeoutLog(EnumSet.TIMEOUT.T7, "Not recievd valid Select.req");
                    base.rootHandle.ManagerFactory.CallbackManager.onTimeout("", timeout);
                    disconnectToken = base.rootHandle.ManagerFactory.ConnectManager.GetDisconnectToken();
                    if (disconnectToken != 0L)
                    {
                        base.rootHandle.ManagerFactory.ConnectManager.releaseMutext();
                        base.rootHandle.ManagerFactory.LoggerManager.Logger.Info("T7 Timeout Release Mutex for Disconnection ");
                        base.rootHandle.ManagerFactory.ConnectManager.ReleaseDisconnectToken(disconnectToken);
                    }
                }
            }
        }

        public virtual void OnTimeOutClear(SECSTimeout timeout)
        {
        }

        public virtual SECSTransaction ReleaseTimeOut(long key)
        {
            lock (this)
            {
                Dictionary<string, SECSTimeout> dictionary;
                SECSTransaction transaction = null;
                string str = key.ToString();
                SECSTimeout timeout = null;
                Monitor.Enter(dictionary = this.timeoutlist);
                try
                {
                    timeout = this.timeoutlist[str];
                    if (timeout != null)
                    {
                        transaction = (SECSTransaction)timeout.Message.Clone();
                        this.timeoutlist.Remove(str);
                        timeout = null;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    Monitor.Exit(dictionary);
                }
                this.timerchecker.releaseMutext();
                return transaction;
            }
        }

        public virtual bool ReleaseTimeOut(string key)
        {
            bool flag = false;
            lock (this)
            {
                lock (this.timeoutlist)
                {
                    SECSTimeout timeout = this.timeoutlist[key];
                    if (timeout != null)
                    {
                        this.timeoutlist.Remove(key);
                    }
                    else
                    {
                        flag = false;
                    }
                    this.timerchecker.releaseMutext();
                }
                return flag;
            }
        }

        public virtual bool SetTimeOut(SECSTimeout timeout)
        {
            lock (this)
            {
                Dictionary<string, SECSTimeout> dictionary;
                long num = 0L;
                if (timeout.Id == -3)
                {
                    num = base.config.Timeout3 * 0x3e8;
                }
                else if (timeout.Id == -6)
                {
                    num = base.config.Timeout6 * 0x3e8;
                }
                else if (timeout.Id == -7)
                {
                    num = base.config.Timeout7 * 0x3e8;
                }
                timeout.TimeoutTime = num + CSharpUtil.currentTimeMillis();
                Monitor.Enter(dictionary = this.timeoutlist);
                try
                {
                    if ((timeout.Id == -3) || (timeout.Id == -6))
                    {
                        this.timeoutlist.Add(timeout.Message.Systembyte.ToString(), timeout);
                    }
                    else
                    {
                        this.timeoutlist.Add(timeout.Type, timeout);
                    }
                }
                catch (ArgumentException exception)
                {
                    base.rootHandle.ManagerFactory.LoggerManager.Logger.Error("[TimerManager][SetTimeout]TimeoutDictionary Error. " + loggerHelper.getExceptionString(exception));
                    base.rootHandle.ManagerFactory.LoggerManager.WriteConnnectionLog(Level.Error, "WARN SETTIMEOUT DICTIONARY ERROR(ArgumentException):" + exception.Message, false);
                    this.timerchecker.releaseMutext();
                    return false;
                }
                finally
                {
                    Monitor.Exit(dictionary);
                }
                this.timerchecker.releaseMutext();
                return true;
            }
        }

        public override void Terminate(ReturnObject returnObject)
        {
            if (this.timeoutlist != null)
            {
                if (this.timeoutlist.Count > 0)
                {
                    IEnumerator enumerator = this.timeoutlist.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        SECSTimeout current = enumerator.Current as SECSTimeout;
                        current = null;
                    }
                    this.timeoutlist.Clear();
                }
                this.timeoutlist = null;
            }
        }
    }
}
