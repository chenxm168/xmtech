using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinSECS.global;
using WinSECS.Utility;
using System.Reflection;
using System.Xml;

namespace WinSECS.structure
{
    public class ListFormat : Format
    {
        private FormatCollection children = null;
        private bool isUncoutableLength = false;
        private int length = -1;
        private int maxPossibleByteLength = -1;
        private bool rawLength = false;
        private const long serialVersionUID = 1L;
        public static readonly byte TYPE = 0;

        public ListFormat()
        {
            this.children = new FormatCollection(this);
        }

        public override IFormat add(IFormat format)
        {
            IFormat format2;
            if (this.RawLength)
            {
                if (this.Length < this.Children.Count)
                {
                    return this.Previous.add(format);
                }
                if (0 == this.Children.Count)
                {
                    format.Previous = this;
                    this.Next = format;
                }
                else
                {
                    format2 = this.Children[this.Children.Count - 1];
                    format2.Next = format;
                    format.Previous = format2;
                }
                this.Children.Add(format);
                format.Parent = this;
                return format;
            }
            if (!this.Variable)
            {
                if (0 == this.Length)
                {
                    format.Previous = this;
                    this.Next = format;
                }
                else
                {
                    format2 = this.Children[this.Children.Count - 1];
                    format2.Next = format;
                    format.Previous = format2;
                }
            }
            this.Children.Add(format);
            format.Parent = this;
            return format;
        }

        protected internal override IFormat attach(IFormat format)
        {
            format.Parent = this.Parent;
            format.Previous = this.Previous;
            return format;
        }

        public override object Clone()
        {
            ListFormat parent = (ListFormat)base.MemberwiseClone();
            parent.Children = new FormatCollection(parent);
            foreach (IFormat format2 in this.Children)
            {
                parent.add((IFormat)format2.Clone());
            }
            return parent;
        }

        public ReturnObject compareRegularPattern(ListFormat modelingFormat, ReturnObject returnObject)
        {
            string regexPattern = modelingFormat.GetRegexPattern();
            string regexInput = this.GetRegexInput();
            if (regexInput.Length > 50)
            {
                if (modelingFormat.Variable)
                {
                    if (modelingFormat.Length < this.Length)
                    {
                        returnObject.setError(string.Concat(new object[] { "List Data Too long! ModelingList=", modelingFormat.Length, " ReceivedList=", this.Length }));
                        return returnObject;
                    }
                }
                else
                {
                    if (modelingFormat.Length > this.Length)
                    {
                        returnObject.setError(string.Concat(new object[] { "List Data Too Short! ModelingList=", modelingFormat.Length, " ReceivedList=", this.Length }));
                        return returnObject;
                    }
                    if (modelingFormat.Length < this.Length)
                    {
                        returnObject.setError(string.Concat(new object[] { "List Data Too long! ModelingList=", modelingFormat.Length, " ReceivedList=", this.Length }));
                        return returnObject;
                    }
                }
                int num = 0;
                if (modelingFormat.Variable)
                {
                    if ((modelingFormat.children.Count >= 1) || (this.children.Count >= 1))
                    {
                        if ((modelingFormat.children.Count < 1) && (this.children.Count > 0))
                        {
                            returnObject.setError("Modeled variable list don't have child data!");
                            return returnObject;
                        }
                        IFormat format = modelingFormat.children[0];
                        while ((num < 5) && (this.children.Count > num))
                        {
                            if ((format.Type == TYPE) && (this.children[num].Type == TYPE))
                            {
                                returnObject = ((ListFormat)this.children[num]).compareRegularPattern((ListFormat)format, returnObject);
                                if (!returnObject.isSuccess())
                                {
                                    return returnObject;
                                }
                            }
                            else if (!Regex.Match(this[num].GetRegexInput(), format.GetRegexPattern()).Success)
                            {
                                returnObject.setError("Modeled Format=" + format.GetRegexPattern() + " Received Format=" + this.children[num].GetRegexInput());
                                return returnObject;
                            }
                            num++;
                        }
                    }
                    return returnObject;
                }
                foreach (IFormat format in modelingFormat.children)
                {
                    if ((format.Type == TYPE) && (this.Children[num].Type == TYPE))
                    {
                        returnObject = ((ListFormat)this[num]).compareRegularPattern(format as ListFormat, returnObject);
                        if (!returnObject.isSuccess())
                        {
                            return returnObject;
                        }
                    }
                    else if (!Regex.Match(this[num].GetRegexInput(), format.GetRegexPattern()).Success)
                    {
                        returnObject.setError("Modeled Format=" + format.GetRegexPattern() + " Received Format=" + this[num].GetRegexInput());
                        return returnObject;
                    }
                    num++;
                }
                return returnObject;
            }
            if (!Regex.Match(regexInput, regexPattern).Success)
            {
                returnObject.setError("Modeled Format=" + regexPattern + " Received Format=" + regexInput);
                return returnObject;
            }
            return returnObject;
        }

        internal override int decoding(byte[] bs, int pos)
        {
            this.RawLength = true;
            pos = base.decoding(bs, pos);
            for (int i = 0; i < this.Length; i++)
            {
                int type = bs[pos];
                IFormat format = FormatFactory.newInstance(type);
                pos = ((Format)format).decoding(bs, pos);
                this.add(format);
            }
            return pos;
        }

        public override int encoding(int startPos, byte[] bs)
        {
            bs[startPos] = this.EncodingFormatByte;
            startPos++;
            byte[] sourceArray = this.getEncodingLengthBytes(1);
            Array.Copy(sourceArray, 0, bs, startPos, sourceArray.Length);
            startPos += this.getEncodingLengthForLengthByte();
            foreach (IFormat format in this.Children)
            {
                startPos = ((Format)format).encoding(startPos, bs);
            }
            return startPos;
        }

        public override bool equal(IFormat comparatee)
        {
            foreach (IFormat format in this.Children)
            {
                if (!this.equal(format))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual IFormat fromElement(XmlElement element)
        {
            base.fromElement(element);
            foreach (XmlElement element2 in element.ChildNodes)
            {
                Format format = FormatFactory.newInstance(element2.Name) as Format;
                format.fromElement(element2);
                this.add(format);
            }
            return this;
        }

        public override int getMaxPossibleByteLength()
        {
            if (this.maxPossibleByteLength == -1)
            {
                this.maxPossibleByteLength = 0;
                this.maxPossibleByteLength += 4;
                foreach (Format format in this.children)
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

        public override string GetRegexInput()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.getformatTypeRegex());
            builder.Append("<");
            foreach (IFormat format in this.children)
            {
                builder.Append(format.GetRegexInput());
            }
            builder.Append(">");
            return builder.ToString();
        }

        public override string GetRegexPattern()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.LogType);
            builder.Append("<");
            if (this.Variable)
            {
                if ((this.children != null) && (this.children.Count > 0))
                {
                    builder.Append("(");
                    builder.Append(this.children[0].GetRegexPattern());
                    builder.Append("){0,");
                    builder.Append(this.Length);
                    builder.Append("}");
                }
            }
            else
            {
                foreach (IFormat format2 in this.children)
                {
                    builder.Append(format2.GetRegexPattern());
                }
            }
            builder.Append(">");
            return builder.ToString();
        }

        public override XmlElement toElement()
        {
            XmlElement element = base.toElement();
            foreach (IFormat format in this.Children)
            {
                element.AppendChild(format.toElement());
            }
            return element;
        }

        public override XmlElement toElement(XmlDocument doc)
        {
            XmlElement element = base.toElement(doc);
            foreach (IFormat format in this.Children)
            {
                element.AppendChild(format.toElement(doc));
            }
            return element;
        }

        public override string treeString()
        {
            string str = ((this.Name == null) || this.Name.Equals("")) ? "" : ("[" + this.Name + "]");
            if (this.Variable)
            {
                return string.Concat(new object[] { this.LogType, ",", this.Length, "V ", str });
            }
            return string.Concat(new object[] { this.LogType, ",", this.Length, " ", str });
        }

        public override int valueCopy(byte[] bs, int pos)
        {
            return pos;
        }

        public override int ByteLength
        {
            get
            {
                int num = this.getEncodingLengthForLengthByte(this.Length) + 1;
                foreach (IFormat format in this.Children)
                {
                    Format format2 = (Format)format;
                    num += format2.ByteLength;
                }
                return num;
            }
        }

        public override IFormatCollection Children
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

        public override IFormat this[int index]
        {
            get
            {
                if (index > (this.children.Count - 1))
                {
                    return null;
                }
                return this.children[index];
            }
        }

        public override IFormat this[string itemName]
        {
            get
            {
                if (this.Name.ToUpper() == itemName.ToUpper())
                {
                    return this;
                }
                foreach (IFormat format in this.children)
                {
                    if (format[itemName] != null)
                    {
                        return format[itemName];
                    }
                }
                return null;
            }
        }

        public override int Length
        {
            get
            {
                if (this.RawLength)
                {
                    return this.length;
                }
                if (this.Variable)
                {
                    return this.length;
                }
                return this.Children.Count;
            }
            set
            {
                if (value >= 0)
                {
                    this.length = value;
                    this.RawLength = true;
                }
                this.Variable = false;
            }
        }

        public override int Level
        {
            set
            {
                base.Level = value;
                foreach (IFormat format in this.Children)
                {
                    format.Level = value + 1;
                }
            }
        }

        public override string LogFormat
        {
            get
            {
                StringBuilder builder = new StringBuilder(this.Children.Count * 100);
                builder.Append(this.tab(this.Level) + "<" + this.treeString() + ConstUtils.NEWLINE);
                foreach (IFormat format in this.Children)
                {
                    builder.Append(format.LogFormat);
                }
                if (this.Level == 0)
                {
                    builder.Append(this.tab(this.Level) + ">.");
                }
                else
                {
                    builder.Append(this.tab(this.Level) + ">" + ConstUtils.NEWLINE);
                }
                return builder.ToString();
            }
        }

        public override string LogType
        {
            get
            {
                return "L";
            }
        }

        protected internal virtual bool RawLength
        {
            get
            {
                return this.rawLength;
            }
            set
            {
                this.rawLength = value;
            }
        }

        public override byte Type
        {
            get
            {
                return TYPE;
            }
        }

        public override string Value
        {
            get
            {
                return string.Format("{0}", this.Length);
            }
            set
            {
            }
        }
    }
}
