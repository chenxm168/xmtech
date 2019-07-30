using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using WinSECS.Utility;
using WinSECS.driver;

namespace WinSECS.connect
{
    [ComVisible(false)]
    public class LinkTestActor : IThreadRunnable
    {
        private bool bPause = true;
        private bool isIntendedStop = false;
        private int linkTestDuation = 0;
        private int mutexCount = 0;
        private SinglePlugIn rootHandle;

        public LinkTestActor(SinglePlugIn rootHandle, int linktestduration)
        {
            this.rootHandle = rootHandle;
            this.linkTestDuation = linktestduration * 0x3e8;
        }

        public void acquireMutex()
        {
            lock (this)
            {
                if (this.mutexCount == 0)
                {
                    try
                    {
                        this.mutexCount = 1;
                        Monitor.Wait(this);
                    }
                    catch (ThreadInterruptedException)
                    {
                    }
                }
            }
        }

        public void releaseMutex()
        {
            lock (this)
            {
                if (this.mutexCount == 1)
                {
                    Monitor.Pulse(this);
                    this.mutexCount = 0;
                }
            }
        }

        public virtual void Run()
        {
            long num = 0L;
            int millisecondsTimeout = 0;
            long num4 = 0L;
            while (true)
            {
                ThreadInterruptedException exception;
                if (this.bPause)
                {
                    this.acquireMutex();
                    if (this.isIntendedStop)
                    {
                        return;
                    }
                }
                try
                {
                    num = this.rootHandle.ManagerFactory.ConnectManager.getLastActionTime();
                }
                catch
                {
                }
                if (num > 0L)
                {
                    num4 = CSharpUtil.currentTimeMillis() - num;
                    if (num4 >= this.linkTestDuation)
                    {
                        this.rootHandle.ManagerFactory.ConnectManager.sendLinkTestRequest();
                        try
                        {
                            Thread.Sleep(this.linkTestDuation);
                        }
                        catch (ThreadInterruptedException exception1)
                        {
                            exception = exception1;
                            if (this.isIntendedStop)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        millisecondsTimeout = this.linkTestDuation - ((int)num4);
                        if (millisecondsTimeout > 0)
                        {
                            try
                            {
                                Thread.Sleep(millisecondsTimeout);
                            }
                            catch (ThreadInterruptedException exception2)
                            {
                                exception = exception2;
                                if (this.isIntendedStop)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        Thread.Sleep(this.linkTestDuation);
                    }
                    catch (ThreadInterruptedException exception3)
                    {
                        exception = exception3;
                        if (this.isIntendedStop)
                        {
                            return;
                        }
                    }
                }
                if (this.isIntendedStop)
                {
                    return;
                }
            }
        }

        public bool IsIntendedStop
        {
            set
            {
                this.isIntendedStop = value;
            }
        }

        public int LinkTestDuation
        {
            get
            {
                return (this.linkTestDuation / 0x3e8);
            }
            set
            {
                this.linkTestDuation = value * 0x3e8;
            }
        }

        public bool Pause
        {
            get
            {
                return this.bPause;
            }
            set
            {
                this.bPause = value;
            }
        }
    }
}
