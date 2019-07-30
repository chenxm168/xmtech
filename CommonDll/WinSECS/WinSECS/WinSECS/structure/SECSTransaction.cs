using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using WinSECS.global;
using WinSECS.Utility;

namespace WinSECS.structure
{
    public class SECSTransaction : Format, ISECSTransaction
    {
        private bool autoreply;
        private static string AUTOREPLY = "autoreply";
        private byte[] body;
        private FormatCollection children;
        private object correlation;
        private string description;
        private int deviceId;
        private string direction;
        private static string DIRECTION = "direction";
        private static string FUNCTION = "function";
        private bool hasItemKey;
        private byte[] header;
        private string headerString;
        private string id;
        private bool islogging;
        private static string LOGGING = "logging";
        private int maxPossibleByteLength;
        private string messageData;
        private string messageName;
        private string pairName;
        private bool receive;
        private string receivedTime;
        private const string RECV = "RECV";
        private string secs1BodyString;
        private string secs1HeaderString;
        private string secs2BodyString;
        private string secs2HeaderString;
        private const string SEND = "SEND";
        private const long serialVersionUID = 1L;
        private static string STREAM = "stream";
        private string streamfunctionString;
        public static readonly byte TYPE = 0x3f;
        private static string WBIT = "wbit";

        public SECSTransaction()
        {
            this.secs1HeaderString = null;
            this.secs2HeaderString = null;
            this.secs1BodyString = null;
            this.secs2BodyString = null;
            this.receive = false;
            this.correlation = null;
            this.messageData = "";
            this.headerString = "";
            this.direction = "H<->E";
            this.autoreply = false;
            this.islogging = true;
            this.deviceId = -1;
            this.pairName = "";
            this.description = "";
            this.children = new FormatCollection(null);
            this.receivedTime = "";
            this.streamfunctionString = "";
            this.hasItemKey = false;
            this.maxPossibleByteLength = -1;
            this.InitBlock();
        }

        public SECSTransaction(int stream, int function, bool waitbit)
        {
            this.secs1HeaderString = null;
            this.secs2HeaderString = null;
            this.secs1BodyString = null;
            this.secs2BodyString = null;
            this.receive = false;
            this.correlation = null;
            this.messageData = "";
            this.headerString = "";
            this.direction = "H<->E";
            this.autoreply = false;
            this.islogging = true;
            this.deviceId = -1;
            this.pairName = "";
            this.description = "";
            this.children = new FormatCollection(null);
            this.receivedTime = "";
            this.streamfunctionString = "";
            this.hasItemKey = false;
            this.maxPossibleByteLength = -1;
            this.InitBlock();
            this.setStreamNWbit(stream, waitbit);
            this.Function = function;
        }

        public override IFormat add(IFormat format)
        {
            this.Children.Add(format);
            format.Previous = this;
            format.Parent = this;
            return format;
        }

        public override IFormat add(byte type, int length, string name, string value_Renamed)
        {
            IFormat format = FormatFactory.newInstance(type, length);
            format.Length = length;
            format.Name = name;
            format.Value = value_Renamed;
            return this.add(format);
        }

        public IFormat addFormat(IFormat format)
        {
            return this.add(format);
        }

        public object Clone()
        {
            SECSTransaction transaction = (SECSTransaction)base.MemberwiseClone();
            transaction.Header = this.copyHeader();
            transaction.Children = new FormatCollection(null);
            foreach (IFormat format in this.Children)
            {
                Format format2 = (Format)format;
                transaction.add((IFormat)format2.Clone());
            }
            return transaction;
        }

        public virtual byte[] copyHeader()
        {
            byte[] destinationArray = new byte[10];
            Array.Copy(this.header, 0, destinationArray, 0, 10);
            return destinationArray;
        }

        public virtual SECSException decoding()
        {
            return Visitor.decoding(this.Body, this);
        }

        public virtual SECSException decoding(int overRawLimit)
        {
            return Visitor.decoding(this.Body, this, overRawLimit);
        }

        public virtual void encoding()
        {
            this.Body = Visitor.encoding(this.Children);
        }

        public override int encoding(int startPos, byte[] bs)
        {
            return 0;
        }

        public bool equals(SECSTransaction comparatee)
        {
            if (this.Children.Count != comparatee.Children.Count)
            {
                return false;
            }
            IEnumerator<IFormat> enumerator = this.Children.GetEnumerator();
            IEnumerator<IFormat> enumerator2 = this.Children.GetEnumerator();
            while (enumerator.MoveNext() && enumerator2.MoveNext())
            {
                IFormat current = enumerator.Current;
                IFormat format2 = enumerator2.Current;
                if (!current.Equals(format2))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual IFormat fromElement(XmlElement element)
        {
            this.MessageName = element.Name;
            this.Function = int.Parse(element.GetAttribute(FUNCTION));
            this.setStreamNWbit(int.Parse(element.GetAttribute(STREAM)), bool.Parse(element.GetAttribute(WBIT)));
            this.Direction = element.GetAttribute(DIRECTION);
            this.Autoreply = bool.Parse(element.GetAttribute(AUTOREPLY));
            this.IsLogging = bool.Parse(element.GetAttribute(LOGGING));
            IEnumerator enumerator = element.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XmlElement current = (XmlElement)enumerator.Current;
                IFormat format = FormatFactory.newInstance(current.Name);
                format.fromElement(current);
                this.add(format);
            }
            return this;
        }

        public virtual IFormat getByIndex(string indexString, string delimeter)
        {
            try
            {
                char[] separator = delimeter.ToCharArray();
                string[] strArray = indexString.Split(separator);
                IFormat format = null;
                bool flag = true;
                foreach (string str in strArray)
                {
                    int num = int.Parse(str);
                    if (flag)
                    {
                        format = this.children[num];
                        flag = false;
                    }
                    else
                    {
                        format = format[num];
                    }
                    if (format == null)
                    {
                        return null;
                    }
                }
                return format;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual IFormat getByName(string itemName)
        {
            try
            {
                bool flag = false;
                while (!flag)
                {
                    foreach (IFormat format in this.children)
                    {
                        if (format.Name.ToUpper() == itemName.ToUpper())
                        {
                            return format;
                        }
                        if ((format.Type == ListFormat.TYPE) && (format[itemName] != null))
                        {
                            return format[itemName];
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int getByteLength()
        {
            return Visitor.getByteLength(this.Children);
        }

        public int getLength()
        {
            return (this.header.Length + this.body.Length);
        }

        public string getLogType()
        {
            return this.StreamFunctionString;
        }

        public int getMaxPossibleByteLength()
        {
            if (this.maxPossibleByteLength == -1)
            {
                this.maxPossibleByteLength = 0;
                foreach (IFormat format in this.children)
                {
                    int num = format.getMaxPossibleByteLength();
                    if (num == 0x7fffffff)
                    {
                        this.maxPossibleByteLength = 0x7fffffff;
                        break;
                    }
                    this.maxPossibleByteLength += num;
                }
            }
            return this.maxPossibleByteLength;
        }

        public string GetRegexInput()
        {
            StringBuilder builder = new StringBuilder();
            foreach (IFormat format in this.children)
            {
                builder.Append(format.GetRegexInput());
            }
            return builder.ToString();
        }

        public string GetRegexPattern()
        {
            StringBuilder builder = new StringBuilder();
            foreach (IFormat format in this.children)
            {
                builder.Append(format.GetRegexPattern());
            }
            return builder.ToString();
        }

        public byte[] getRowSystembyte()
        {
            return new byte[] { this.header[6], this.header[7], this.header[8], this.header[9] };
        }

        private void InitBlock()
        {
            this.body = new byte[0];
            this.header = new byte[10];
            //this.children = this.children;
        }

        public string logFormat()
        {
            return "";
        }

        private void makeLogFormatSECS1Body()
        {
            if ((this.body == null) || (this.body.Length <= 0))
            {
                this.secs1BodyString = "";
            }
            if (this.secs1BodyString == null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.tab(0));
                int num = 1;
                for (int i = 0; i < this.body.Length; i++)
                {
                    byte num3 = this.body[i];
                    builder.Append(StringUtils.toHex2String(num3));
                    if (num++ >= 20)
                    {
                        builder.Append(ConstUtils.NEWLINE);
                        builder.Append(this.tab(0));
                        num = 1;
                    }
                }
                this.secs1BodyString = builder.ToString();
            }
        }

        private void makeLogFormatSECS1Header()
        {
            if (this.ControlMessage)
            {
                this.SECS1HeaderLoggingString = string.Format("{0} {1} [len={2}] [SB={3}] ({4})", new object[] { this.Receive ? "RECV" : "SEND", this.HeaderString, this.getLength(), this.Systembyte, this.ControlMessageType });
            }
            else
            {
                this.SECS1HeaderLoggingString = string.Format("{0} {1} {2} {3} [len={4}] [SB={5}]", new object[] { this.Receive ? "RECV" : "SEND", this.HeaderString, string.Format("S{0}F{1}", this.Stream, this.Function), this.WbitString, this.getLength(), this.Systembyte });
            }
        }

        private void makeLogFormatSECS2Body()
        {
            if (this.Receive && ((this.body == null) || (this.body.Length <= 0)))
            {
                this.secs2BodyString = "";
            }
            else
            {
                Visitor.setLevel(this.Children);
                this.secs2BodyString = Visitor.getBodyLogTree(this.getByteLength(), this.Children);
            }
        }

        private void makeLogFormatSECS2BodyForConvert()
        {
            Visitor.setLevel(this.Children);
            this.secs2BodyString = Visitor.getBodyLogTree(this.getByteLength(), this.Children);
        }

        private void makeLogFormatSECS2Header()
        {
            if (this.ControlMessage)
            {
                this.SECS2HeaderLoggingString = string.Format("{0} {1} [len={2}] [SB={3}] ({4})", new object[] { this.Receive ? "RECV" : "SEND", this.HeaderString, this.getLength(), this.Systembyte, this.ControlMessageType });
            }
            else
            {
                this.SECS2HeaderLoggingString = string.Format("{0} {1}:{2} {3} [D={4}] [len={5}] [SB={6}]", new object[] { this.Receive ? "RECV" : "SEND", string.Format("S{0}F{1}", this.Stream, this.Function), this.MessageName, this.WbitString, this.DeviceId, this.getLength(), this.Systembyte });
            }
        }

        public virtual void setStreamNWbit(int stream, bool wbit)
        {
            this.header[2] = wbit ? ((byte)(stream + 0x80)) : ((byte)stream);
        }

        public void setSystemByte(byte[] systembyte)
        {
            this.header[6] = systembyte[0];
            this.header[7] = systembyte[1];
            this.header[8] = systembyte[2];
            this.header[9] = systembyte[3];
        }

        public XmlElement toElement()
        {
            XmlDocument doc = new XmlDocument();
            if ((this.MessageName == null) || (this.MessageName == ""))
            {
                this.MessageName = this.streamfunctionString;
            }
            XmlElement element = doc.CreateElement(this.messageName);
            foreach (IFormat format in this.Children)
            {
                Format format2 = (Format)format;
                element.AppendChild(format2.toElement(doc));
            }
            return element;
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            return 0;
        }

        public virtual bool Autoreply
        {
            get
            {
                return this.autoreply;
            }
            set
            {
                this.autoreply = value;
            }
        }

        public virtual byte[] Body
        {
            get
            {
                return this.body;
            }
            set
            {
                this.body = value;
            }
        }

        public IFormatCollection Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value as FormatCollection;
            }
        }

        public virtual bool ControlMessage
        {
            get
            {
                return (this.header[5] != 0);
            }
        }

        private string ControlMessageType
        {
            get
            {
                switch (this.header[5])
                {
                    case 1:
                        return "(Select.req)";

                    case 2:
                        return "(Select.rsp)";

                    case 5:
                        return "(Linktest.req)";

                    case 6:
                        return "(Linktest.rsp)";

                    case 7:
                        return "(Reject.req)";

                    case 9:
                        return "(Separate.req)";
                }
                return "(Unknown control message)";
            }
        }

        public virtual object Correlation
        {
            get
            {
                return this.correlation;
            }
            set
            {
                this.correlation = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int DeviceId
        {
            get
            {
                if (this.deviceId == -1)
                {
                    byte[] destinationArray = new byte[2];
                    Array.Copy(this.header, 0, destinationArray, 0, 2);
                    destinationArray[0] = (byte)(destinationArray[0] & 0x7f);
                    this.deviceId = ByteToObject.byte2Short(destinationArray);
                }
                return this.deviceId;
            }
            set
            {
                Array.Copy(ObjectToByte.short2Byte((short)value), 0, this.header, 0, 2);
            }
        }

        public virtual string Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }

        public virtual int Function
        {
            get
            {
                return (this.header[3] & 0xff);
            }
            set
            {
                this.header[3] = (byte)value;
            }
        }

        public bool HasItemKey
        {
            get
            {
                return this.hasItemKey;
            }
            set
            {
                this.hasItemKey = value;
            }
        }

        public virtual byte[] Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
            }
        }

        public string HeaderString
        {
            get
            {
                if ((this.headerString == null) || (this.headerString.Length <= 0))
                {
                    StringBuilder builder = new StringBuilder(10);
                    foreach (byte num in this.header)
                    {
                        builder.Append(StringUtils.toHex2String(num));
                    }
                    this.HeaderString = builder.ToString();
                }
                return this.headerString;
            }
            set
            {
                this.headerString = value;
            }
        }

        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public virtual bool IsLogging
        {
            get
            {
                return this.islogging;
            }
            set
            {
                this.islogging = value;
            }
        }

        public byte[] LengthBytes
        {
            get
            {
                return ObjectToByte.uint4ToByte(this.getLength().ToString());
            }
        }

        private string LogBytes
        {
            get
            {
                StringBuilder builder = new StringBuilder(this.getLength() * 3);
                builder.Append(this.Receive ? "RECV" : "SEND");
                foreach (byte num in this.header)
                {
                    builder.Append(StringUtils.toHex2String(num));
                }
                builder.Append(this.ControlMessageType);
                return builder.ToString();
            }
        }

        public override string LogType
        {
            get
            {
                return "";
            }
        }

        public string MessageData
        {
            get
            {
                return this.messageData;
            }
            set
            {
                this.messageData = value;
            }
        }

        public string MessageName
        {
            get
            {
                if ((this.messageName == null) || this.messageName.Equals(""))
                {
                    this.messageName = string.Concat(new object[] { "S", this.Stream, "F", this.Function });
                }
                return this.messageName;
            }
            set
            {
                this.messageName = value;
            }
        }

        public string PairName
        {
            get
            {
                return this.pairName;
            }
            set
            {
                this.pairName = value;
            }
        }

        public virtual int Ptype
        {
            get
            {
                return (this.header[6] & 0xff);
            }
        }

        public virtual int ReasonCode
        {
            get
            {
                return (this.header[3] & 0xff);
            }
            set
            {
                this.header[3] = (byte)value;
            }
        }

        public virtual bool Receive
        {
            get
            {
                return this.receive;
            }
            set
            {
                this.receive = value;
            }
        }

        public string ReceivedTime
        {
            get
            {
                return this.receivedTime;
            }
            set
            {
                this.receivedTime = value;
            }
        }

        public virtual bool Secondary
        {
            get
            {
                return ((this.Function % 2) == 0);
            }
        }

        public string SECS1BodyString
        {
            get
            {
                if (this.secs1BodyString == null)
                {
                    this.makeLogFormatSECS1Body();
                }
                if (this.secs1BodyString == "")
                {
                    return this.secs1BodyString;
                }
                return (this.secs1BodyString + ConstUtils.NEWLINE);
            }
            set
            {
                this.secs1BodyString = value;
            }
        }

        public string SECS1HeaderLoggingString
        {
            get
            {
                if (this.secs1HeaderString == null)
                {
                    this.makeLogFormatSECS1Header();
                }
                return this.secs1HeaderString;
            }
            set
            {
                this.secs1HeaderString = value;
            }
        }

        public string SECS2BodyString
        {
            get
            {
                if (this.secs2BodyString == null)
                {
                    this.makeLogFormatSECS2Body();
                }
                if (this.secs2BodyString == "")
                {
                    return this.secs2BodyString;
                }
                return (this.secs2BodyString + ConstUtils.NEWLINE);
            }
            set
            {
                this.secs2BodyString = value;
            }
        }

        public string SECS2BodyStringForLogConvert
        {
            get
            {
                if (this.secs2BodyString == null)
                {
                    this.makeLogFormatSECS2BodyForConvert();
                }
                if (this.secs2BodyString == "")
                {
                    return this.secs2BodyString;
                }
                return (this.secs2BodyString + ConstUtils.NEWLINE);
            }
            set
            {
                this.secs2BodyString = value;
            }
        }

        public string SECS2HeaderLoggingString
        {
            get
            {
                if (this.secs2HeaderString == null)
                {
                    this.makeLogFormatSECS2Header();
                }
                return this.secs2HeaderString;
            }
            set
            {
                this.secs2HeaderString = value;
            }
        }

        public virtual int Stream
        {
            get
            {
                int num = this.header[2] & 0xff;
                return (this.Wbit ? (num - 0x80) : num);
            }
        }

        public string StreamFunctionString
        {
            get
            {
                if (this.streamfunctionString.Equals(""))
                {
                    this.streamfunctionString = string.Format("S{0}F{1}", this.Stream, this.Function);
                }
                return this.streamfunctionString;
            }
        }

        public virtual int Stype
        {
            get
            {
                return (this.header[5] & 0xff);
            }
            set
            {
                this.header[0] = 0xff;
                this.header[1] = 0xff;
                this.header[5] = (byte)value;
            }
        }

        public virtual long Systembyte
        {
            get
            {
                byte[] destinationArray = new byte[8];
                Array.Copy(this.header, 6, destinationArray, 4, 4);
                return ByteToObject.byte2Long(destinationArray);
            }
            set
            {
                Array.Copy(ObjectToByte.uint4ToByte(value.ToString()), 0, this.header, 6, 4);
            }
        }

        public override byte Type
        {
            get
            {
                return 0;
            }
        }

        public virtual bool Wbit
        {
            get
            {
                return ((this.header[2] & 0xff) > 0x80);
            }
        }

        public virtual string WbitString
        {
            get
            {
                return (this.Wbit ? "W" : " ");
            }
        }
    }
}
