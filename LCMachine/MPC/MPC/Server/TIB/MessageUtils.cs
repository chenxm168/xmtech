using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPC.Server.TIB
{
    public  class MessageUtils
    {
        public static T Convert<T>(object message)
        {
            object ob = new object();
            string msg = message as string;
            T t = default(T);
            try
            {
                lock(ob)
                {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(new StringReader(msg));
                }
            }
            catch (System.InvalidOperationException e)
            {
                log4net.LogManager.GetLogger(typeof(MessageUtils)).Error(e.InnerException.Message);

            }

            return t;
        }
       }
    }

