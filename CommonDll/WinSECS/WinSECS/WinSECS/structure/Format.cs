using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WinSECS.Utility;

namespace WinSECS.structure
{
    public abstract class Format : IFormat, ICloneable
    {
        private bool bItemKey = false;
        private int byteLength = -1;
        private byte encodingFormatByte = 0x11;
        private string error;
        protected static string FIXED = "fixed";
        protected int length = -1;
        protected static string LENGTH = "length";
        private int level;
        protected string logformat = "";
        private IFormatCollection m_owner;
        private IFormat m_parent;
        private IFormat m_previous;
        private string name = "";
        protected static string NAME = "name";
        private IFormat next_Renamed_Field;
        protected const string SPACE19 = "                   ";
        private string value_Renamed = "";
        private bool variable = false;

        protected Format()
        {
        }

        public virtual IFormat add(IFormat format)
        {
            format.Previous = this;
            this.Next = format;
            this.Previous.add(format);
            return format;
        }

        public virtual IFormat add(byte type, int length, string name, string value_Renamed)
        {
            IFormat format = FormatFactory.newInstance(type, length);
            format.Length = length;
            format.Name = name;
            format.Value = (value_Renamed != null) ? value_Renamed : "";
            return this.add(format);
        }

        public IFormat addFormat(IFormat format)
        {
            return this.add(format);
        }

        protected internal virtual IFormat attach(IFormat format)
        {
            format.Parent = this.Parent;
            format.Previous = this;
            format.Next = this.Next;
            this.Next = format;
            return format;
        }

        public virtual object Clone()
        {
            IFormat format = (IFormat)base.MemberwiseClone();
            return this.detach(format);
        }

        internal virtual int decoding(byte[] bs, int pos)
        {
            int num = bs[pos] & 0xff;
            int length = num & 3;
            pos++;
            this.Length = this.getLength4Type(length, bs, pos) / this.DefaultByteLength;
            pos += length;
            try
            {
                pos = this.valueCopy(bs, pos);
            }
            catch (Exception)
            {
                pos = this.valueCopy(bs, pos);
            }
            return pos;
        }

        protected internal virtual IFormat detach(IFormat format)
        {
            format.Parent = null;
            format.Previous = null;
            format.Next = null;
            return format;
        }

        public abstract int encoding(int startPos, byte[] bs);
        protected virtual int encodingHeader(int startPos, byte[] bs)
        {
            bs[startPos] = this.EncodingFormatByte;
            startPos++;
            byte[] sourceArray = this.getEncodingLengthBytes();
            Array.Copy(sourceArray, 0, bs, startPos, sourceArray.Length);
            return (startPos += this.getEncodingLengthForLengthByte());
        }

        public virtual bool equal(IFormat comparatee)
        {
            return ((this.Value == comparatee.Value) && (this.Type == comparatee.Type));
        }

        public virtual IFormat fromElement(XmlElement element)
        {
            try
            {
                this.Name = element.GetAttribute(NAME);
                this.Value = (element.Value == null) ? "" : element.Value.Trim();
                this.Length = int.Parse(element.GetAttribute(LENGTH));
                this.Variable = !bool.Parse(element.GetAttribute(FIXED));
                return this;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual byte[] getEncodingLengthBytes()
        {
            byte[] sourceArray = ObjectToByte.int2Byte(this.Length * this.DefaultByteLength);
            byte[] destinationArray = new byte[this.getEncodingLengthForLengthByte()];
            Array.Copy(sourceArray, sourceArray.Length - destinationArray.Length, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

        protected virtual byte[] getEncodingLengthBytes(int multiple)
        {
            byte[] sourceArray = ObjectToByte.int2Byte(this.Length * multiple);
            byte[] destinationArray = new byte[this.getEncodingLengthForLengthByte()];
            Array.Copy(sourceArray, sourceArray.Length - destinationArray.Length, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

        protected virtual int getEncodingLengthForLengthByte()
        {
            if (this.Length <= 0xff)
            {
                return 1;
            }
            if (this.Length <= 0xfe01)
            {
                return 2;
            }
            return 3;
        }

        public virtual int getEncodingLengthForLengthByte(int length)
        {
            if (length <= 0xff)
            {
                return 1;
            }
            if (length <= 0xfe01)
            {
                return 2;
            }
            return 3;
        }

        public virtual string getformatTypeRegex()
        {
            return this.LogType;
        }

        protected internal virtual int getLength4Type(int length, byte[] bs, int pos)
        {
            byte[] destinationArray = new byte[4];
            switch (length)
            {
                case 0:
                    return 0;

                case 1:
                    return ByteToObject.byte2UnsignedByte(bs[pos]);

                case 2:
                    Array.Copy(bs, pos, destinationArray, 2, 2);
                    return (int)ByteToObject.byte2UnsignedInt(destinationArray);

                case 3:
                    Array.Copy(bs, pos, destinationArray, 1, 3);
                    return (int)ByteToObject.byte2UnsignedInt(destinationArray);
            }
            return 0;
        }

        protected internal virtual int getLowerLoopCountBetweenLengthAndSplits(string[] splits)
        {
            if ((splits.Length == 1) && splits[0].Equals(""))
            {
                return 0;
            }
            return ((this.Length > splits.Length) ? splits.Length : this.Length);
        }

        public virtual int getMaxPossibleByteLength()
        {
            return (4 + (this.DefaultByteLength * this.Length));
        }

        public virtual int getPossibleByteLength()
        {
            return 0;
        }

        public virtual string GetRegexInput()
        {
            return string.Format("{0}<{1}>", this.getformatTypeRegex(), this.length);
        }

        public virtual string GetRegexPattern()
        {
            if (this.Variable)
            {
                return string.Format(@"{0}<\d*>", this.getformatTypeRegex());
            }
            return string.Format("{0}<{1}>", this.getformatTypeRegex(), this.length);
        }

        public virtual bool hasNext()
        {
            return (null != this.Next);
        }

        public virtual void remove()
        {
            this.Previous.Next = this.Next;
            if (this.hasNext())
            {
                this.Next.Previous = this.Previous;
            }
        }

        protected internal virtual string tab(int level)
        {
            level += 5;
            string str = "";
            for (int i = 0; i < level; i++)
            {
                str = str + "    ";
            }
            return str;
        }

        public virtual XmlElement toElement()
        {
            XmlDocument document = new XmlDocument();
            XmlElement newChild = document.CreateElement(((this.Name == null) || this.Name.Equals("")) ? this.LogType : this.Name);
            newChild.SetAttribute(LENGTH, this.Length.ToString());
            newChild.SetAttribute(NAME, this.name);
            //newChild.SetAttribute(FIXED, !this.Variable.ToString());
            newChild.SetAttribute(FIXED, (!this.Variable).ToString());
            if (this.LogType != "L")
            {
                newChild.InnerText = this.Value;
            }
            document.AppendChild(newChild);
            return newChild;
        }

        public virtual XmlElement toElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(this.LogType);
            element.SetAttribute(LENGTH, this.Length.ToString());
            element.SetAttribute(NAME, this.name);
            //element.SetAttribute(FIXED, !this.Variable.ToString());
            element.SetAttribute(FIXED, (!this.Variable).ToString());
            if (this.LogType != "L")
            {
                element.InnerText = this.Value;
            }
            return element;
        }

        public virtual string treeString()
        {
            string str = ((this.Name == null) || this.Name.Equals("")) ? "" : ("[" + this.Name + "]");
            string str2 = (this.Value == null) ? "" : this.Value;
            StringBuilder builder = new StringBuilder();
            if (this.Variable)
            {
                return builder.Append(this.LogType).Append(",").Append(this.Length + "V ").Append(str).Append(" '").Append(str2).Append("'").ToString();
            }
            return builder.Append(this.LogType).Append(",").Append(this.Length + " ").Append(str).Append(" '").Append(str2).Append("'").ToString();
        }

        public abstract int valueCopy(byte[] bs, int pos);

        public virtual int ByteLength
        {
            get
            {
                if (this.byteLength == -1)
                {
                    this.byteLength = (this.getEncodingLengthForLengthByte(this.Length * this.DefaultByteLength) + (this.Length * this.DefaultByteLength)) + 1;
                }
                return this.byteLength;
            }
        }

        public virtual IFormatCollection Children
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual int DefaultByteLength
        {
            get
            {
                return 1;
            }
        }

        protected virtual byte EncodingFormatByte
        {
            get
            {
                if (this.encodingFormatByte == 0x11)
                {
                    this.encodingFormatByte = (byte)((this.Type << 2) + this.getEncodingLengthForLengthByte());
                }
                return this.encodingFormatByte;
            }
        }

        public virtual IFormat this[int index]
        {
            get
            {
                return null;
            }
        }

        public virtual IFormat this[string itemName]
        {
            get
            {
                if (this.Name.ToUpper() == itemName.ToUpper())
                {
                    return this;
                }
                return null;
            }
        }

        public virtual bool ItemKey
        {
            get
            {
                return this.bItemKey;
            }
            set
            {
                this.bItemKey = value;
            }
        }

        public virtual int Length
        {
            get
            {
                if (this.length < 0)
                {
                    string[] strArray = this.Value.Split(new char[] { ' ' });
                    this.length = strArray.Length;
                }
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        public virtual int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public virtual string LogFormat
        {
            get
            {
                if (this.logformat.Equals(""))
                {
                    if (this.Level == 0)
                    {
                        this.logformat = this.tab(this.Level) + "<" + this.treeString() + ">.";
                    }
                    else
                    {
                        this.logformat = this.tab(this.Level) + "<" + this.treeString() + ">" + ConstUtils.NEWLINE;
                    }
                }
                return this.logformat;
            }
        }

        public abstract string LogType { get; }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public virtual IFormat Next
        {
            get
            {
                return this.next_Renamed_Field;
            }
            set
            {
                this.next_Renamed_Field = value;
            }
        }

        public virtual IFormatCollection Owner
        {
            get
            {
                return this.m_owner;
            }
            set
            {
                this.m_owner = value;
            }
        }

        public virtual IFormat Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                this.m_parent = value;
            }
        }

        public virtual IFormat Previous
        {
            get
            {
                return this.m_previous;
            }
            set
            {
                this.m_previous = value;
            }
        }

        public abstract byte Type { get; }

        public virtual string Value
        {
            get
            {
                return this.value_Renamed;
            }
            set
            {
                this.value_Renamed = (value != null) ? value : "";
            }
        }

        public virtual bool Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = value;
            }
        }
    }
}
