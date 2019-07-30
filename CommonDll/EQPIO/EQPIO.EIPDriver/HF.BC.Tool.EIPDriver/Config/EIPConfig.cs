
namespace HF.BC.Tool.EIPDriver.Config
{
    using HF.BC.Tool.EIPDriver.Utils;
    using System;
    using System.Reflection;
    using System.Xml;

    public class EIPConfig : ICloneable
    {
        private int analysisCount = 5;
        private string driverName = string.Empty;
        private string eipMapFile = string.Empty;
        private string log4NetPath = string.Empty;
        private string logRootDir = string.Empty;
        private string timeOutCheckInfoPath = string.Empty;

        public object Clone()
        {
            return (base.MemberwiseClone() as EIPConfig);
        }

        public void Load(string filename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                PropertyInfo property = base.GetType().GetProperty(node.Name);
                if (property != null)
                {
                    XmlUtils.SetProperty(property, this, node.InnerText);
                }
            }
        }

        public int AnalysisCount
        {
            get
            {
                return this.analysisCount;
            }
            set
            {
                this.analysisCount = value;
            }
        }

        public string DriverName
        {
            get
            {
                return this.driverName;
            }
            set
            {
                this.driverName = value;
            }
        }

        public string EIPMapFile
        {
            get
            {
                return this.eipMapFile;
            }
            set
            {
                this.eipMapFile = value;
            }
        }

        public string Log4NetPath
        {
            get
            {
                return this.log4NetPath;
            }
            set
            {
                this.log4NetPath = value;
            }
        }

        public string LogRootDir
        {
            get
            {
                return this.logRootDir;
            }
            set
            {
                this.logRootDir = value;
            }
        }

        public string TimeOutCheckInfoPath
        {
            get
            {
                return this.timeOutCheckInfoPath;
            }
            set
            {
                this.timeOutCheckInfoPath = value;
            }
        }
    }
}
