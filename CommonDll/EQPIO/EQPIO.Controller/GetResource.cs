using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EQPIO.Controller
{
    [ComVisible(false)]
  public   class GetResource
    {

        public static object GetEmbeddedResource(string resName)
        {
            string[] manifestResourceNames = Assembly.GetManifestResourceNames();
            resName = resName.Replace("/", ".").Replace(@"\", ".");
            if (resName.IndexOf(ClassType.Namespace) != 0)
            {
                // resName = ClassType.Namespace + "." + resName;
                resName = "EQPIO.Controller.Resources." + resName;
            }
            Stream manifestResourceStream = Assembly.GetManifestResourceStream(resName);
            if (manifestResourceStream != null)
            {
                switch (resName.Substring(resName.LastIndexOf(".") + 1).ToLower())
                {
                    case "csproj":
                    case "vbproj":
                    case "user":
                    case "resources":
                    case "cs":
                    case "txt":
                    case "htm":
                    case "html":
                    case "aim":
                    case "xml":
                    case "vb":
                        {
                            StreamReader reader = new StreamReader(manifestResourceStream);
                            return reader.ReadToEnd();
                        }
                    case "config":
                        {
                            StreamReader reader2 = new StreamReader(manifestResourceStream);
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(reader2.ReadToEnd());
                            return document;
                        }
                    //case "ico":
                    //    return new Icon(manifestResourceStream);




                    //case "bmp":
                    //case "gif":
                    //case "jpg":
                    //case "jpeg":
                    //case "exif":
                    //case "wmf":
                    //case "emf":
                    //case "png":
                    //case "tif":
                    //case "tiff":
                    //    return new Bitmap(manifestResourceStream);
                }
                return manifestResourceStream;
            }
            return null;
        }

        public static System.Reflection.Assembly Assembly
        {
            get
            {
                return ClassType.Assembly;
            }
        }

        public static Type ClassType
        {
            get
            {
                return typeof(GetResource);
            }
        }




    }
}
