using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TIBMessageIo;
using System.IO;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
  public  class AreYouThereRequest2
    {
      //[XmlElement]
      //public MessageHead Head;


        [XmlElement]
        public MessageHead Head;


        [XmlElement(ElementName = "Body")]
        public AreYouThereRequestBody Body;

        [XmlElement]
        public MessageReturn Return;

     

      //[XmlElement]
      //public MessageReturn Return;



      public AreYouThereRequest2()
      {

          Body = new AreYouThereRequestBody();
      }

      public AreYouThereRequest2(MessageInfo msgInfo, string machine)
      {
          //Head = new MessageHead();
          //Return = new MessageReturn();
          //Head.MESSAGENAME = "AreYouThereRequest";


          Head = new MessageHead();
          Return = new MessageReturn();

          string[] str = msgInfo.SourceSubject.Split('.');
          Head.EVENTUSER = str[str.Length - 1];
          Head.ORIGINALSOURCESUBJECTNAME = msgInfo.SourceSubject;
          Head.ORIGINALTRANSACTIONID = "";
          Head.TRANSACTIONID = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
          Head.EVENTCOMMENT = "";
          Return.RETURNCODE = "0";
          Return.RETURNMESSAGE = "";


          Head.MESSAGENAME = "AreYouThereRequest";

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


     public AreYouThereRequest GetInstance(string xmlString)
     {
         return Util.XmlToObj<AreYouThereRequest>(xmlString);
     }

     public string ToXmlString()
     {
         MemoryStream stream = new MemoryStream();
         StreamWriter sw = new StreamWriter(stream);
         Type type = this.GetType();


         XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
         xsn.Add("", "");
         XmlSerializer xs = new XmlSerializer(type);
         xs.Serialize(sw, this, xsn);

         StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
         stream.Position = 0;
         string s = sr.ReadToEnd();
         return s;
     }



    }
}
