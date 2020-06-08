using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using log4net;
using System.Xml.Serialization;

namespace FileSrv.csv
{
   public  class CSVWriter
    {

       public EventHandler WriteSuccessEvent
       {
           get;
           set;
       }

       public EventHandler<ErrorEventArgs> WriteFailEvent
       {
           get;
           set;
       }

       protected CSVConfig config;


       ILog logger = LogManager.GetLogger(typeof(CSVWriter));
       public string CsvHeader
       {
           get;
           set;
       }

       public string FilePaht
       {
           get;
           set;
       }

       object writelocker = new object();

       public CSVWriter()
       {
           Init("CsvConfig.xml");
       }

       public CSVWriter(string CfgFile)
       {
           Init(CfgFile);
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



       protected virtual void WriteStringToFlie(string file,string head,string items)
       {
           FileStream fs =null;
           StreamWriter sw =null;

           lock(writelocker)
           {

               try
               {

                   if (!File.Exists(file))
                   {
                       string pathname = Path.GetDirectoryName(file);
                       if (!Directory.Exists(pathname))
                       {

                           Directory.CreateDirectory(pathname);
                       }

                       

                   }
                   fs = File.Open(file, FileMode.Create);

                    sw= new StreamWriter(fs, Encoding.Default);

                    string s = head + "\n" + items;
                    sw.WriteLine(s);
                    sw.Flush();
                   

               }
               catch(Exception e)
               {
                   logger.ErrorFormat("Write file Error[{0}]", e.Message);
               }

               finally
               {
                   if(sw!=null)
                   {
                       sw.Close();
                   }
                   if(fs!=null)
                   {
                       fs.Close();
                   }
               }


           }//end lock

       }

       protected virtual void AppendToFile(string file,string sItems)
       {
           lock(writelocker)
           {
               FileStream fs = null;
               StreamWriter sw = null;
               try
               {
                   fs = File.Open(file, FileMode.Append);
                   sw = new StreamWriter(fs, Encoding.Default);
                   sw.WriteLine(sItems);
                   sw.Flush();

               }catch(Exception e)
               {
                   logger.ErrorFormat("Write file Error[{0}]", e.Message);
               }

               finally
               {
                   if (sw != null)
                   {
                       sw.Close();
                   }
                   if (fs != null)
                   {
                       fs.Close();
                   }
               }


           }//end lock
       }


       public void WriteNewFileAsyn(string file,string head,string sItems)
       {
           string[] args = new string[] { file, head, sItems };
           Thread t = new Thread(WriteNewFileStart);
           t.Name = "WriteNewFile";
           t.Start(args);
             
       }

       public void WriteNewFileAsyn(string sItems)
       {
           WriteNewFileAsyn(config.FilePath, config.ItemHead, sItems);
       }


       protected void WriteNewFileStart(object ars)
       {
           var args = (string[])ars;
           string file = args[0];
           string head = args[1];
           string sItem = args[2];
           WriteStringToFlie(file, head, sItem);
       }

       protected void AppendFileStart(object ars)
       {
           var args = (string[])ars;
           string file = args[0];
           string sItem = args[1];
           AppendToFile(file,  sItem);
       }

       public void AppendFileAsyn(string file,string sItems)
       {
           if(File.Exists(file))
           {
               string[] args = new string[] { file, sItems };
               Thread t = new Thread(AppendFileStart);
               t.Start(args);
           }else
           {
               WriteNewFileAsyn(file, config.ItemHead, sItems);
           }

       }

       public void AppendFileAsyn( string sItems)
       {
           AppendFileAsyn(config.FilePath, sItems);
       }

    }


    
}
