using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Service;
using System.Reflection;
using HF.DB;

namespace HF.DB.ObjectService
{
    public class ServiceManager
    {
        public static IEquipmentService GetEquipmentService()
        {
            EquipmentServiceImpl impl = new EquipmentServiceImpl();
            EquipmentServiceImplProxy proxy = new EquipmentServiceImplProxy(impl);
            return proxy;
        }

        public static IMachineService GetMachineService()
        {
            var impl = new MachineServiceImpl();
            var proxy = new MachineServiceImplProxy(impl);
            return proxy;
        }

        public static IPortService GetPortService()
        {
            var impl = new PortServiceImpl();
            var proxy = new PortServiceImplProxy(impl);
            return proxy;
        }

        //public  static object GetService(string name)
        //{
             
        //}

    }
}
