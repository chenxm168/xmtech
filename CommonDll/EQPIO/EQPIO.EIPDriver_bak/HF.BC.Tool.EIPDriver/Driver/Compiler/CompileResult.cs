
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using EQPIO.EIPDriver.Compiler.Resources;
    using System;
    using System.Text;
    using System.Xml;

    public class CompileResult
    {
        private ErrorCode errorCode;
        private XmlNode resource;
        private XmlNode root;

        public static string GetDescription(ErrorCode errorCode)
        {
            string str = EQPIO.EIPDriver.Compiler.Resources.Description.ResourceManager.GetString(errorCode.ToString());
            if ((str != null) && (str != ""))
            {
                return str;
            }
            return errorCode.ToString();
        }

        public string ToLogString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.resource.Name);
            if ((this.resource.Attributes.Count > 0) && (this.resource.Attributes[EIPConst.ATTRIBUTE_NAME] != null))
            {
                builder.Append(" ");
                builder.Append(this.resource.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
            }
            builder.Append(" : ");
            builder.Append(this.Description);
            builder.Append(" at ");
            builder.Append(this.root.Name);
            if ((this.resource.Attributes.Count > 0) && (this.resource.Attributes[EIPConst.ATTRIBUTE_NAME] != null))
            {
                builder.Append(" ");
                builder.Append(this.resource.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
            }
            return builder.ToString();
        }

        public string Description
        {
            get
            {
                string str = EQPIO.EIPDriver.Compiler.Resources.Description.ResourceManager.GetString(this.errorCode.ToString());
                if ((str != null) && (str != ""))
                {
                    return str;
                }
                return this.errorCode.ToString();
            }
        }

        public ErrorCode Error
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }

        public XmlNode Resource
        {
            get
            {
                return this.resource;
            }
            set
            {
                this.resource = value;
            }
        }

        public XmlNode Root
        {
            get
            {
                return this.root;
            }
            set
            {
                this.root = value;
            }
        }

        public HF.BC.Tool.EIPDriver.Driver.Compiler.Severity Severity
        {
            get
            {
                if (this.errorCode < ErrorCode.ErrorBegin)
                {
                    return HF.BC.Tool.EIPDriver.Driver.Compiler.Severity.Warn;
                }
                return HF.BC.Tool.EIPDriver.Driver.Compiler.Severity.Error;
            }
        }
    }
}
