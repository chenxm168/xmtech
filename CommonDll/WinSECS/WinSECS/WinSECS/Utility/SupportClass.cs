using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class SupportClass
    {
        public static double Identity(double literal)
        {
            return literal;
        }

        public static long Identity(long literal)
        {
            return literal;
        }

        public static float Identity(float literal)
        {
            return literal;
        }

        public static ulong Identity(ulong literal)
        {
            return literal;
        }

        [ComVisible(false)]
        public class ThreadClass : IThreadRunnable
        {
            private Thread threadField;

            public ThreadClass()
            {
                this.threadField = new Thread(new ThreadStart(this.Run));
            }

            public ThreadClass(string Name)
            {
                this.threadField = new Thread(new ThreadStart(this.Run));
                this.Name = Name;
            }

            public ThreadClass(ThreadStart Start)
            {
                this.threadField = new Thread(Start);
            }

            public ThreadClass(ThreadStart Start, string Name)
            {
                this.threadField = new Thread(Start);
                this.Name = Name;
            }

            public void Abort()
            {
                this.threadField.Abort();
            }

            public void Abort(object stateInfo)
            {
                lock (this)
                {
                    this.threadField.Abort(stateInfo);
                }
            }

            public static SupportClass.ThreadClass Current()
            {
                return new SupportClass.ThreadClass { Instance = Thread.CurrentThread };
            }

            public virtual void Interrupt()
            {
                this.threadField.Interrupt();
            }

            public void Join()
            {
                this.threadField.Join();
            }

            public void Join(long MiliSeconds)
            {
                lock (this)
                {
                    this.threadField.Join(new TimeSpan(MiliSeconds * 0x2710L));
                }
            }

            public void Join(long MiliSeconds, int NanoSeconds)
            {
                lock (this)
                {
                    this.threadField.Join(new TimeSpan((MiliSeconds * 0x2710L) + (NanoSeconds * 100)));
                }
            }

            public void Resume()
            {
                this.threadField.Resume();
            }

            public virtual void Run()
            {
            }

            public virtual void Start()
            {
                this.threadField.Start();
            }

            public void Suspend()
            {
                this.threadField.Suspend();
            }

            public override string ToString()
            {
                return ("Thread[" + this.Name + "," + this.Priority.ToString() + ",]");
            }

            public Thread Instance
            {
                get
                {
                    return this.threadField;
                }
                set
                {
                    this.threadField = value;
                }
            }

            public bool IsAlive
            {
                get
                {
                    return this.threadField.IsAlive;
                }
            }

            public bool IsBackground
            {
                get
                {
                    return this.threadField.IsBackground;
                }
                set
                {
                    this.threadField.IsBackground = value;
                }
            }

            public string Name
            {
                get
                {
                    return this.threadField.Name;
                }
                set
                {
                    if (this.threadField.Name == null)
                    {
                        this.threadField.Name = value;
                    }
                }
            }

            public ThreadPriority Priority
            {
                get
                {
                    return this.threadField.Priority;
                }
                set
                {
                    this.threadField.Priority = value;
                }
            }
        }
    }
}
