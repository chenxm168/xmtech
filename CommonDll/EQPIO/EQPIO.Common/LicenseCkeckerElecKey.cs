
namespace EQPIO.Common
{
    //using Ekc;
    using log4net;
    using System;
    using System.Threading;
    //using System.Windows.Forms;

    public class LicenseCkeckerElecKey// : ILicenseCheck
    {
        //private ILog logger = LogManager.GetLogger(typeof(LicenseCkeckerElecKey));
        //private static readonly uint m_iKeyID = 0x71c02e27;
        //private static uint m_iPermissionModule = 4;
        //private static KeyCheck m_keyCheck;

        //public LicenseCkeckerElecKey(string moduleID)
        //{
        //    int num = Convert.ToInt32(moduleID);
        //    if (num > 0)
        //    {
        //        m_iPermissionModule = (uint)num;
        //    }
        //}

        //public override bool CheckNetworkLicense()
        //{
        //    bool flag = false;
        //    try
        //    {
        //        int retryCount = 0;
        //        while (retryCount < base.RetryCount)
        //        {
        //            if (m_keyCheck == null)
        //            {
        //                retryCount = base.RetryCount;
        //            }
        //            else
        //            {
        //                m_keyCheck.SrvIdx = 1;
        //                if (m_keyCheck.OpenKeySrv(1))
        //                {
        //                    if (!m_keyCheck.NetKeyConnect("EQPIO.INI"))
        //                    {
        //                        this.logger.Error(string.Format("[Network License] Fail : Server Not found - {0}", m_keyCheck.ErrStr));
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                    }
        //                    else if (!m_keyCheck.UpdateLimitKey())
        //                    {
        //                        this.logger.Error(string.Format("[Network License] Fail : {0}", m_keyCheck.ErrStr));
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                    }
        //                    else if (!m_keyCheck.NetKeyLogin("EQPIO", "EQPIO", 1))
        //                    {
        //                        this.logger.Error(string.Format("[Network License] Fail : NetKey Login Fail - {0}", m_keyCheck.ErrStr));
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                    }
        //                    else
        //                    {
        //                        int num2 = (int)Math.Pow(2.0, (double)(m_iPermissionModule - 1));
        //                        if ((num2 & m_keyCheck.Modules) == num2)
        //                        {
        //                            this.logger.Error("[Network License] Success");
        //                            flag = true;
        //                            m_keyCheck.SetClientBackgroundCheck(20, 0, new KeyCheck.evResultProc(this.ClientResultProc));
        //                            m_keyCheck.SetServerBackgroundCheck(10, 0);
        //                            retryCount = base.RetryCount;
        //                        }
        //                        else
        //                        {
        //                            this.logger.Error(string.Format("[Network License] Fail : PermissionModule : {0}, KeyCheck.Modules : {1}", num2, m_keyCheck.Modules));
        //                            flag = false;
        //                            retryCount++;
        //                            Thread.Sleep((int)(100 * retryCount));
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    flag = false;
        //                    retryCount++;
        //                    Thread.Sleep((int)(100 * retryCount));
        //                    this.logger.Error("[Network License] Fail : NKag20.exe not found");
        //                }
        //                if (!flag)
        //                {
        //                    m_keyCheck.NetKeyLogout();
        //                    m_keyCheck.NetKeyDisconnect();
        //                    m_keyCheck.CloseKeySrv(1);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.Error(exception);
        //    }
        //    return flag;
        //}

        //public override bool CheckUSBKeyLock()
        //{
        //    bool flag = false;
        //    try
        //    {
        //        int retryCount = 0;
        //        while (retryCount < base.RetryCount)
        //        {
        //            if (m_keyCheck == null)
        //            {
        //                retryCount = base.RetryCount;
        //            }
        //            else
        //            {
        //                m_keyCheck.SrvIdx = 0;
        //                if (m_keyCheck.OpenKeySrv(0))
        //                {
        //                    m_keyCheck.InitLicKey();
        //                    if (m_keyCheck.CheckKey())
        //                    {
        //                        if (m_iPermissionModule == 0)
        //                        {
        //                            flag = true;
        //                            retryCount = base.RetryCount;
        //                        }
        //                        else
        //                        {
        //                            m_keyCheck.GetKeyProperties();
        //                            int num2 = (int)Math.Pow(2.0, (double)(m_iPermissionModule - 1));
        //                            if ((num2 & m_keyCheck.Modules) == num2)
        //                            {
        //                                this.logger.Error("[USB License] Success");
        //                                flag = true;
        //                                retryCount = base.RetryCount;
        //                            }
        //                            else
        //                            {
        //                                this.logger.Error(string.Format("[USB License] Fail : PermissionModule[{0}, KeyCheck.Module[{1}]]", num2, m_keyCheck.Modules));
        //                                flag = false;
        //                                retryCount++;
        //                                Thread.Sleep((int)(100 * retryCount));
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.logger.Error(string.Format("[USB License] Fail : TYPE [{0}], CODE [{1}], ERROR[{2}], KeyLocation[{3}]", new object[] { m_keyCheck.ErrType, m_keyCheck.ErrCode, m_keyCheck.ErrStr, m_keyCheck.KeyLocation }));
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                    }
        //                }
        //                else
        //                {
        //                    this.logger.Error("[USB License] Fail : OpenKeyServer(Local) Error");
        //                    flag = false;
        //                    retryCount++;
        //                    Thread.Sleep((int)(100 * retryCount));
        //                }
        //                m_keyCheck.CloseKeySrv(0);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.Error(exception);
        //        if (m_keyCheck != null)
        //        {
        //            m_keyCheck.CloseKeySrv(0);
        //        }
        //        return false;
        //    }
        //    return flag;
        //}

        //public void ClientResultProc(int nType, int nErrCode, string strErrStr)
        //{
        //    this.logger.Error(strErrStr + "\r\nDo you want to exit?");
        //}

        //private bool NetworkLicenseCheck(bool failover)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        bool flag2 = false;
        //        int retryCount = 0;
        //        while (retryCount < base.RetryCount)
        //        {
        //            if (m_keyCheck == null)
        //            {
        //                retryCount = base.RetryCount;
        //            }
        //            else
        //            {
        //                m_keyCheck.SrvIdx = 1;
        //                if (m_keyCheck.OpenKeySrv(1))
        //                {
        //                    if (!failover)
        //                    {
        //                        flag2 = m_keyCheck.NetKeyConnect("EQPIO.INI");
        //                    }
        //                    if (!flag2)
        //                    {
        //                        this.logger.Error(m_keyCheck.ErrStr);
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                        this.logger.Error("[Network License] Server Not found");
        //                    }
        //                    else if (!m_keyCheck.UpdateLimitKey())
        //                    {
        //                        this.logger.Error(m_keyCheck.ErrStr);
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                    }
        //                    else if (!m_keyCheck.NetKeyLogin("EQPIO", "EQPIO", 1))
        //                    {
        //                        this.logger.Error(m_keyCheck.ErrStr);
        //                        flag = false;
        //                        retryCount++;
        //                        Thread.Sleep((int)(100 * retryCount));
        //                        this.logger.Error("[Network License] NetKey Login Fail");
        //                    }
        //                    else
        //                    {
        //                        int num2 = (int)Math.Pow(2.0, (double)(m_iPermissionModule - 1));
        //                        if ((num2 & m_keyCheck.Modules) == num2)
        //                        {
        //                            this.logger.Error("[Network License] Success");
        //                            flag = true;
        //                            m_keyCheck.SetClientBackgroundCheck(20, 0, new KeyCheck.evResultProc(this.ClientResultProc));
        //                            m_keyCheck.SetServerBackgroundCheck(10, 0);
        //                            retryCount = base.RetryCount;
        //                        }
        //                        else
        //                        {
        //                            this.logger.Error(string.Format("[Network License] m_iPermissionModuleBIT : {0}, m_keyCheck.Modules : {1}", num2, m_keyCheck.Modules));
        //                            flag = false;
        //                            retryCount++;
        //                            Thread.Sleep((int)(100 * retryCount));
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    flag = false;
        //                    retryCount++;
        //                    Thread.Sleep((int)(100 * retryCount));
        //                    this.logger.Error("[Network License] NKag20.exe not found");
        //                }
        //                if (!flag)
        //                {
        //                    m_keyCheck.NetKeyLogout();
        //                    m_keyCheck.NetKeyDisconnect();
        //                    m_keyCheck.CloseKeySrv(1);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.Error(exception);
        //    }
        //    return flag;
        //}

        //public override void Start()
        //{
        //    m_keyCheck = new KeyCheck(m_iKeyID, 0, (int)m_iPermissionModule, 30, "", Application.ExecutablePath);
        //    base.LicenseCheckSet();
        //}

        //public override void Stop()
        //{
        //    if (m_keyCheck != null)
        //    {
        //        m_keyCheck.CloseKeySrv(0);
        //    }
        //    base.LicenseCheckFlag = false;
        //    base.LicenseCheckStop();
        //}
    }
}
