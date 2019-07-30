using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
   public class LotProcessInfo:AbstractMessage
    {
        [XmlElement(ElementName = "Body")]
        public LotProcessInfoBody Body;




       private static  LotProcessInfo getLotInfoMessage(string msgName)
        {
            LotProcessInfo downRQS = new LotProcessInfo();
            downRQS.Init();
            downRQS.Body = new LotProcessInfoBody();
            downRQS.Header.MESSAGENAME = msgName;
            return downRQS;
        }
        public static LotProcessInfo getLotInfoDownLoadRequestMessage(string port,string carrier,string carrierType,string slotMap)
        {
            LotProcessInfo downRQS = getLotInfoMessage("LotInfoDownloadRequest");
          

            downRQS.Body.MACHINENAME = StaticVarible.MachineID;
            downRQS.Body.PORTNAME = port;
            downRQS.Body.CARRIERNAME = carrier;
            downRQS.Body.CARRIERTYPE = carrierType;
            downRQS.Body.SLOTMAP = slotMap;            
            return downRQS;

        }

        public static LotProcessInfo getLotProcessStartedMessage(string port, string carrier, string lot, string recipe)
        {
            LotProcessInfo downRQS = getLotInfoMessage("LotProcessStarted");


            downRQS.Body.MACHINENAME = StaticVarible.MachineID;
            downRQS.Body.PORTNAME = port;
            downRQS.Body.CARRIERNAME = carrier;
            downRQS.Body.LOTNAME = lot;
            downRQS.Body.MACHINERECIPENAME = recipe;
            return downRQS;

        }

        public static LotProcessInfo getLotProcessAbortedMessage(string port, string carrier, string lot, string recipe,string reasonCode, string reasonText)
        {
            LotProcessInfo downRQS = getLotInfoMessage("LotProcessAborted");


            downRQS.Body.MACHINENAME = StaticVarible.MachineID;
            downRQS.Body.PORTNAME = port;
            downRQS.Body.CARRIERNAME = carrier;
            downRQS.Body.LOTNAME = lot;
            downRQS.Body.MACHINERECIPENAME = recipe;
            downRQS.Body.REASONCODE = reasonCode;
            downRQS.Body.REASONTEXT = reasonText;

            return downRQS;

        }

        public static LotProcessInfo getLotProcessCancelledMessage(string port, string carrier, string lot, string recipe, string reasonCode, string reasonText)
        {
            LotProcessInfo downRQS = getLotInfoMessage("LotProcessCancelled");


            downRQS.Body.MACHINENAME = StaticVarible.MachineID;
            downRQS.Body.PORTNAME = port;
            downRQS.Body.CARRIERNAME = carrier;
            downRQS.Body.LOTNAME = lot;
            downRQS.Body.MACHINERECIPENAME = recipe;
            downRQS.Body.REASONCODE = reasonCode;
            downRQS.Body.REASONTEXT = reasonText;

            return downRQS;

        }

        public static LotProcessInfo getLotProcessEndedMessage(LotProcessInfoBody lotinfo)
        {
            LotProcessInfo downRQS = getLotInfoMessage("LotProcessEnded");


            downRQS.Body = lotinfo;


            return downRQS;

        }
    }


    
}
