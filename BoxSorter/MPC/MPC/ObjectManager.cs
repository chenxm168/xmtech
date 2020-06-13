using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Spring.Context.Support;
namespace MPC
{
    public class ObjectManager
    {
        static ILog log = LogManager.GetLogger(typeof(ObjectManager));
        static XmlApplicationContext ctx = new XmlApplicationContext("spring-objects.xml");
        //static object locker = new object();

        public static Object getObject(string name)
        {
            Object obj = null;
            try
            {


                    if (ctx == null)
                    {
                        ctx = new XmlApplicationContext("spring-objects.xml");
                    }

                obj = ctx.GetObject(name);

            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return obj;
        }

        public static T getObject<T>(string name)
        {
            T obj = default(T);
            try
            {
                if (ctx == null)
                {
                    ctx = new XmlApplicationContext("spring-objects.xml");
                }
                obj = ctx.GetObject<T>(name);

            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return obj;
        }

        public static XmlApplicationContext getXmlApplicationContext()
        {

            try
            {
                if (ctx == null)
                {
                    ctx = new XmlApplicationContext("spring-objects.xml");
                }


            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return ctx;
        }
    }
}
