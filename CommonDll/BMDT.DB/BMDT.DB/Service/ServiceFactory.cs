using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDT.DB.Service
{
   public  class ServiceFactory
    {

        public static IAlarmService GetAlarmService()
        {
            AlarmServiceImpl impl = new AlarmServiceImpl();
            impl.SqlUtil = new BMDTSqlUtil();
            AlarmServiceImplProxy proxy = new AlarmServiceImplProxy(impl);
            
            return proxy;
        }

       public static IEquipmentService GetEquipmentService()
        {
            EquipmentServiceImpl impl = new EquipmentServiceImpl();
           impl.SqlUtil =  new BMDTSqlUtil();
           EquipmentServiceImplProxy proxy = new EquipmentServiceImplProxy(impl);
           return proxy;
        }

       public static  IGlassService GetGlassService()
       {
           GlassServiceImpl impl = new GlassServiceImpl();
           impl.SqlUtil = new BMDTSqlUtil();
           GlassServiceImplProxy proxy = new GlassServiceImplProxy(impl);
           return proxy;
       }
    }
}
