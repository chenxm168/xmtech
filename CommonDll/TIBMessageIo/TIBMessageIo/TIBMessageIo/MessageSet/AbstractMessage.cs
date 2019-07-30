using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using TIBMessageIo;

namespace TIBMessageIo.MessageSet
{
   public  class AbstractMessage
    {

       [XmlElement(ElementName="Header")]
       public MessageHead Header;


       //public AbstractMessageBody ;

       [XmlElement]
       public MessageReturn Return;

       public AbstractMessage()
       {
           Init();
       }

       public void Init()
       {
           Header = new MessageHead();
           Return = new MessageReturn();
           Header.ORIGINALTRANSACTIONID = "";
           Header.TRANSACTIONID = DateTime.Now.ToString("yyyyMMddHHmmssffffff");// +DateTime.Now.Millisecond.ToString();
           Header.EVENTCOMMENT = "";
           Header.EVENTUSER = StaticVarible.MachineID;
           Return.RETURNCODE = "0";
           Return.RETURNMESSAGE = "";
           Header.ORIGINALSOURCESUBJECTNAME = StaticVarible.DefauleSourceSubject;
       }

       public void Init(string sourceSuject)
       {
           Header = new MessageHead();
           Return = new MessageReturn();
           Header.ORIGINALTRANSACTIONID = "";
           Header.TRANSACTIONID = DateTime.Now.ToString("yyyyMMddHHmmssffffff");// +DateTime.Now.Millisecond.ToString();
           Header.EVENTUSER = StaticVarible.MachineID;
           Header.EVENTCOMMENT = "";
           Return.RETURNCODE = "0";
           Return.RETURNMESSAGE = "";
           Header.ORIGINALSOURCESUBJECTNAME = sourceSuject;
       }

       protected void Init(MessageInfo msgInfo)
       {
           Header = new MessageHead();
           Return = new MessageReturn();

           //string[] str = msgInfo.SourceSubject.Split('.');
           Header.EVENTUSER = StaticVarible.MachineID;
           Header.ORIGINALSOURCESUBJECTNAME = StaticVarible.DefauleSourceSubject;
           Header.ORIGINALTRANSACTIONID = "";
           Header.TRANSACTIONID = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
           Header.EVENTCOMMENT = "";
           Header.EVENTUSER = StaticVarible.MachineID;
           Return.RETURNCODE = "0";
           Return.RETURNMESSAGE = "";
       }

       public AbstractMessage(MessageInfo msgInfo)
       {

           Init(msgInfo);
       }

       public AbstractMessage(string  messageName)
       {

           Init(messageName);
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
