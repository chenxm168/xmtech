using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSrv;
using System.Xml.Serialization;
using System.IO;
using log4net;

namespace FileSrv.csv
{
   public class CSVWriter2:FileWriter,ICSVWriter
    {
       public CSVConfig config;

       protected string header;
       public CSVWriter2():base()
       {
          Init("CSVConfig.xml");

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
                 config =(CSVConfig) xs.Deserialize(reader);


                      }//end using

               }catch(Exception e)
           {
               logger.ErrorFormat("INIT Error[{0}]", e.Message);
               return false;

           }


           return true;
       }




       public void WriteCSV(string[] items,FileMode filemode)
       {
           //string filepath ;
           //string header="";
           //if(config==null)
           //{
           //    filepath = "CsvFile.csv";
           //    for(int i=0;i<items.Length;i++)
           //    {
           //        if(i<=items.Length-2)
           //        {
           //            header+="Item" + i + ",";
           //        }else
           //        {
           //            header += "Item" + i;
           //        }
           //    }
               
           //}else
           //{
           //    filepath = config.FilePath;
           //    header = config.ItemHead;
           //}
           //header += "\n";

           //string sItems = "";
           //for (int j = 0; j > items.Length;j++ )
           //{
           //    if (j <= items.Length - 2)
           //    {
           //        sItems += items[j] + j + ",";
           //    }
           //    else
           //    {
           //        sItems += items[j] + j;
           //    }
           //}
           //sItems += "\n";


           //    lock (writelock)
           //    {

           //        if(File.Exists(filepath))
           //        {
           //            sItems = header + sItems;
           //        }

           //    }//end lock

           string sItem = getCsvString(items);
           if(config==null)
           {
               Write(sItem, "CsvFile.csv", filemode);
           }else
           {
               Write(sItem, config.FilePath,filemode);
           }

             
       }

       public void WriteCSVAsyn(string[] items,FileMode filemode)
       {
           string sItem = getCsvString(items);
           if (config == null)
           {
               WriteAsyn(sItem, "CsvFile.csv", filemode);
           }
           else
           {
               WriteAsyn(sItem, config.FilePath,filemode);
           }
       }


       private string getCsvString(string[] items)
       {
           string filepath;
           string header = "";
           if (config == null)
           {
               filepath = "CsvFile.csv";
               for (int i = 0; i < items.Length; i++)
               {
                   if (i <= items.Length - 2)
                   {
                       header += "Item" + i + ",";
                   }
                   else
                   {
                       header += "Item" + i;
                   }
               }
               
           }
           else
           {
               filepath = config.FilePath;
               header = config.ItemHead;
           }
           header += "\n";
           string sItems = "";
           for (int j = 0; j < items.Length; j++)
           {
               if (j <= items.Length - 2)
               {
                   sItems += items[j]  + ",";
               }
               else
               {
                   sItems += items[j] ;
               }
           }
           sItems += "\n";


           lock (writelock)
           {

               if (File.Exists(filepath))
               {
                   sItems = header + sItems;
               }

               return sItems;

           }//end lock

           
       }



      //protected  override void  WriteCallBack(bool success,object message)
      // {
      //     if(success)
      //     {

      //     }
      // }



    }
}
