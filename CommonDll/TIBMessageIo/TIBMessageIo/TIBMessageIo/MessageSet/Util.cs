using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    public class Util
    {
        public static T XmlToObj<T>(string xmlString)
        {
            T t = default(T);
            try
            {
                //string[] rl = typeof(T).ToString().Split('.');
                //将TRX中的transation置换成相对应的TRX名称。
                //xmlString = xmlString.Replace("transaction", rl[rl.Length - 1]);
              //cxm 20190404  Byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(xmlString);
                //Byte[] bytes = Encoding.GetEncoding(1208).GetBytes(xmlString);
                //cxm 20190404  MemoryStream ms = new MemoryStream(bytes);
                //cxm 20190404  StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("UTF-8")); //
                //StreamReader sr = new StreamReader(ms, Encoding.GetEncoding(1208));

                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(new StringReader(xmlString));
            }
            catch (System.InvalidOperationException e)
            {
                log4net.LogManager.GetLogger(typeof(Util)).Error(e.InnerException.Message);

            }

            return t;

        }
    }
}
