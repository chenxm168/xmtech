using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{

    [XmlRoot(ElementName = "Message")]
   public class ProductInfoMessage:AbstractMessage
    {
        [XmlElement]
        public GlassInfo Body;


        private static ProductInfoMessage getInstance(string messageName)
        {
            ProductInfoMessage msg = new ProductInfoMessage();
            msg.Body = new GlassInfo();
            msg.Init();

            msg.Header.MESSAGENAME=messageName;
            msg.Body.MACHINENAME = StaticVarible.MachineID;
            return msg;
        }

        public static ProductInfoMessage getProductInMessage()
        {
            return getInstance("ProductIn");
        }

        public static ProductInfoMessage getProductOutMessage()
        {
            return getInstance("ProductOut");
        }

        public static ProductInfoMessage getProductScrapMessage()
        {
            return getInstance("ProductScrap");
        }

        public static ProductInfoMessage getProductScrapCancelMessage()
        {
            return getInstance("ProductScrapCancel");
        }
        

    }
}
