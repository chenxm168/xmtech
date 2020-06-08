using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml;
using System.IO;

namespace MPC.Server.TIB
{
   public class TIBMessageHandler
    {

        ILog logger = LogManager.GetLogger(typeof(TIBMessageHandler));
        public void Tib_OnMessageReceived(string suject, object listen, string message)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(message));
            try
            {
                XmlNode xmlNode = doc.SelectSingleNode("Message/Header/MESSAGENAME");
                string messageName = xmlNode.InnerText.Trim();
                //string handlerName = messageName.ToLower() + "handler";
                string handlerName = messageName + "Proc";
                var handler = ObjectManager.getObject(handlerName) as IMessageHandler;
                if (handler != null)
                {
                    handler.doWork(message);
                }
                else
                {
                    logger.ErrorFormat("Get Process handler Error! Handler Name:{0}", handlerName);
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Get Process handler Error! Error Message:{0}", e.Message);
            }
        }

    }
}
