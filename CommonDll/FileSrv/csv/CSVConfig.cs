using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileSrv.csv
{
   public class CSVConfig
    {

       [XmlAttribute]
       public string FilePath
       {
           get;
           set;
       }

       [XmlAttribute]
       public string ItemHead
       {
           get;
           set;
       }

    }
}
