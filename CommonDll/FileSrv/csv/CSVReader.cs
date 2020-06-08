using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using log4net;

namespace FileSrv.csv
{
   public  class CSVReader:IDisposable
    {
       public CSVConfig config
       {
           get;
           set;
       }


       protected FileStream fs;
       protected StreamReader sr;
       protected ILog logger = LogManager.GetLogger(typeof(CSVReader));
       object readlock = new object();

       public CSVReader()
       {
           if (!Init("CSVConfig.xml"))
           {
               logger.ErrorFormat("Init CVSConfig fail");
           }
       }

       public bool ReadCSV(string file, out string[] items,out string header)
       {
           lock(readlock)
           {

          
           try{

           
           if(!File.Exists(file))
           {
               items = null;
               header = null;
               return false;
           }

           fs = File.Open(file, FileMode.Open);
          // sr = new StreamReader(fs ,Encoding.GetEncoding("Unicode"));
           sr = new StreamReader(fs, Encoding.Default);
          // sr = new StreamReader(fs);
           List<string> sList = new List<string>();
           while (sr.Peek() >= 0)
           {
               sList.Add(sr.ReadLine());
           }

           header = sList[0];
           sList.RemoveAt(0);
           items = sList.ToArray<string>();

           Dispose();
           return true;
           }catch(Exception e)
           {
               logger.ErrorFormat("Read CSV File fail[{0}]", e.Message);
               Dispose();
               header = null;
               items = null;
               return false;

               }
           }
       }


       protected bool Init(string file)
       {
           try
           {



               if (!File.Exists(file))
               {
                   logger.ErrorFormat("INIT Error,File is not exist[{0}]", file);
                   return false;
               }

               XmlSerializer xs = new XmlSerializer(typeof(CSVConfig));
               using (Stream reader = new FileStream(file, FileMode.Open))
               {
                   config = (CSVConfig)xs.Deserialize(reader);


               }//end using

           }
           catch (Exception e)
           {
               logger.ErrorFormat("INIT Error[{0}]", e.Message);
               return false;

           }


           return true;
       }




       public void Dispose()
       {
           


               if (sr != null)
               {
                   try
                   {
                       sr.Close();
                   }catch(Exception e)
                   {
                       logger.ErrorFormat("StreamReader Close Error[{0}]", e.Message);
                   }
                   
               }
               if (fs != null)
               {
                   try
                   {
                       fs.Close();
                   }catch (Exception e)
                   {
                       logger.ErrorFormat("FileStream Close Error[{0}]", e.Message);
                   }
                   
               }

           
          
    
       }
    }
}
