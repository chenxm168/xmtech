
namespace EQPIO.Common
{
    using log4net;
    using System;
    using System.Threading;
    //using WibuKeyClassLibrary;

    public class LicenseCkeckerWibuKey //: ILicenseCheck
    {
        //private readonly ILog logger = LogManager.GetLogger(typeof(LicenseCkeckerWibuKey));
        //private static WibuKeyClass m_lockClass;

        //public override bool CheckNetworkLicense()
        //{
        //    try
        //    {
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.Error(exception);
        //    }
        //    return false;
        //}

        //public override bool CheckUSBKeyLock()
        //{
        //    bool flag = false;
        //    try
        //    {
        //        int retryCount = 0;
        //        while (retryCount < base.RetryCount)
        //        {
        //            if (m_lockClass == null)
        //            {
        //                retryCount = base.RetryCount;
        //            }
        //            else if (m_lockClass.CkeckLicense() == 0)
        //            {
        //                flag = true;
        //                retryCount = base.RetryCount;
        //            }
        //            else
        //            {
        //                flag = false;
        //                retryCount++;
        //                Thread.Sleep((int)(100 * retryCount));
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.Error(exception);
        //        return false;
        //    }
        //    return flag;
        //}

        //public override void Start()
        //{
        //    m_lockClass = new WibuKeyClass();
        //    base.LicenseCheckSet();
        //}

        //public override void Stop()
        //{
        //    base.LicenseCheckFlag = false;
        //    base.LicenseCheckStop();
        //}
    }
}
