using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TIBMessageIo;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
  public  class AreYouThereRequest: AbstractMessage
    {
      //[XmlElement]
      //public MessageHead Head;

      [XmlElement(ElementName = "Body")]
      public AreYouThereRequestBody Body;

      //[XmlElement]
      //public MessageReturn Return;



      public AreYouThereRequest():base()
      {

          Body = new AreYouThereRequestBody();
      }

     public AreYouThereRequest(MessageInfo msgInfo, string machine):base(msgInfo)
      {
          //Head = new MessageHead();
          //Return = new MessageReturn();
          //Head.MESSAGENAME = "AreYouThereRequest";

          Header.MESSAGENAME = "AreYouThereRequest";

          Body = new AreYouThereRequestBody();

          //string[] str = msgInfo.SourceSubject.Split('.');
          //Head.EVENTUSER = str[str.Length - 1];
          //Head.MESSAGENAME = "AREYOUTHEREREQUEST";
          //Head.ORIGINALSOURCESUBJECTNAME = msgInfo.SourceSubject;
          //Head.ORIGINALTRANSACTIONID = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
          //Return.RETURNCODE = "0";
          Body.MACHINENAME = machine;
          //Body.SUBJECTNAME = msgInfo.SourceSubject;

      }

        public static  AreYouThereRequest getInstance()
     {
         AreYouThereRequest msg = new AreYouThereRequest();
         msg.Body = new AreYouThereRequestBody();
         msg.Init();
         msg.Header.MESSAGENAME = "AreYouThereRequest";
         msg.Body.MACHINENAME = StaticVarible.MachineID;
         return msg;
     }


     public AreYouThereRequest GetInstance(string xmlString)
     {
         return Util.XmlToObj<AreYouThereRequest>(xmlString);
     }



    }
}
