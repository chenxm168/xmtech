
namespace EQPIO.Common
{
    using log4net;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class ILicenseCheck
    {
        private ILog logger = LogManager.GetLogger(typeof(ILicenseCheck));
        private bool m_bLicenseCheckFlag = false;
        private bool m_bStartCheckComplete = false;
        private readonly DateTime m_hDueDate = new DateTime(0x7e1, 8, 0x1a, 0, 0, 0);
        private DateTime m_hGracePeriod = new DateTime();
        private readonly int m_iCheckCycle = 10;
        private readonly int m_iGracePeriodSetHour = 0x18;
        private int m_iModuleID = 0;
        private static ILicenseCheck m_instance;
        private readonly int m_iRetryCount = 3;
        private const string m_strErrorMessage = "License Check {0}\nTerms of USE : EQPIO Expire Date {1}.";
        private Thread m_tLicenseCheckThread = null;

        public event ExpireDate OnExpireDate;

        public event OPCallGracePeriod OnGracePeriod;

        public event OPCallMessage OnOPCall;

        protected ILicenseCheck()
        {
        }

        public abstract bool CheckNetworkLicense();
        private bool CheckTimeLimit()
        {
            try
            {
                int num = (this.DueDate > DateTime.Now) ? 0 : -1;
                return (num == 0);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return false;
            }
        }

        public abstract bool CheckUSBKeyLock();
        public void LicenseCheckSet()
        {
            m_instance = this;
            this.m_hGracePeriod = this.m_hGracePeriod.AddHours((double)this.GracePeriodSetHour);
            this.LicenseCheckFlag = true;
            this.m_tLicenseCheckThread = new Thread(new ThreadStart(this.LicenseCheckThreadProc));
            this.m_tLicenseCheckThread.Name = "LicenseCheckThread";
            this.m_tLicenseCheckThread.IsBackground = true;
            this.m_tLicenseCheckThread.Start();
            this.logger.Info(string.Format("Local License Check Started. (Cycle : {0} Minutes) (DueDate :{1}) (GracePeriod : {2} Hour)\n", this.CheckCycle, this.DueDate.ToString("yyyy.MM.dd"), this.GracePeriodSetHour));
        }

        public void LicenseCheckStop()
        {
            this.m_bLicenseCheckFlag = false;
            try
            {
                if (this.m_tLicenseCheckThread != null)
                {
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void LicenseCheckThreadProc()
        {
            while (this.LicenseCheckFlag)
            {
                try
                {
                    bool flag = this.CheckNetworkLicense();
                    bool flag2 = this.CheckUSBKeyLock();
                    if (!(flag | flag2))
                    {
                        if (!this.CheckTimeLimit())
                        {
                            if (this.m_hGracePeriod.Ticks > 0L)
                            {
                                this.LicenseCheckStatus = true;
                                this.m_hGracePeriod = this.m_hGracePeriod.AddMinutes((double)-this.m_iCheckCycle);
                                this.OnExpireDate(string.Format("EQPIO Expire Date :{0}", this.DueDate.ToString("yyyy.MM.dd")));
                                this.OnGracePeriod(this.m_hGracePeriod);
                                goto Label_01FE;
                            }
                            this.LicenseCheckStatus = false;
                            this.logger.Info(string.Format(this.ErrorMessage, "Fail", this.DueDate.ToString("yyyy.MM.dd")));
                            this.OnExpireDate(string.Format("EQPIO Expire Date :{0}", this.DueDate.ToString("yyyy.MM.dd")));
                            this.OnOPCall(string.Format(this.ErrorMessage, "Fail", this.DueDate.ToString("yyyy.MM.dd")));
                            break;
                        }
                        this.LicenseCheckStatus = true;
                        this.logger.Info(string.Format(this.ErrorMessage, "Success", this.DueDate.ToString("yyyy.MM.dd")));
                        this.OnExpireDate(string.Format("EQPIO Expire Date :{0}", this.DueDate.ToString("yyyy.MM.dd")));
                    }
                    else
                    {
                        this.LicenseCheckStatus = true;
                        this.logger.Info(string.Format(this.ErrorMessage, "Success", this.DueDate.ToString("yyyy.MM.dd")));
                        this.OnExpireDate("EQPIO");
                    }
                Label_01FE:
                    if (!this.m_bStartCheckComplete)
                    {
                        this.m_bStartCheckComplete = true;
                    }
                    Thread.Sleep((int)(0xea60 * this.CheckCycle));
                }
                catch (ThreadAbortException)
                {
                    this.logger.Error("License Check Thread Aborted");
                }
                catch (Exception exception)
                {
                    this.logger.Error(exception);
                }
            }
        }

        public abstract void Start();
        public abstract void Stop();

        public int CheckCycle
        {
            get
            {
                return this.m_iCheckCycle;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return this.m_hDueDate;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return "License Check {0}\nTerms of USE : EQPIO Expire Date {1}.";
            }
        }

        public int GracePeriodSetHour
        {
            get
            {
                return this.m_iGracePeriodSetHour;
            }
        }

        public static ILicenseCheck Instance
        {
            get
            {
                return m_instance;
            }
        }

        public bool LicenseCheckFlag
        {
            get
            {
                return this.m_bLicenseCheckFlag;
            }
            set
            {
                this.m_bLicenseCheckFlag = value;
            }
        }

        public bool LicenseCheckStatus { get; set; }

        public int ModuleID
        {
            get
            {
                return this.m_iModuleID;
            }
            set
            {
                this.m_iModuleID = value;
            }
        }

        public int RetryCount
        {
            get
            {
                return this.m_iRetryCount;
            }
        }

        public delegate void ExpireDate(string message);

        public delegate void OPCallGracePeriod(DateTime time);

        public delegate void OPCallMessage(string message);
    }
}
