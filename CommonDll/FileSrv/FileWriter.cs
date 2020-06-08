using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using log4net;
namespace FileSrv 
{
    public delegate void WriteFileAsynHandler(object args);
    public delegate void WriteResultHandler(object sender,object args);
   public  class FileWriter:IFileWriter
    {
       public event WriteResultHandler WriteSuccess;
       public event WriteResultHandler WriteFail;

       protected object writelock = new object();
       protected ILog logger = LogManager.GetLogger(typeof(FileWriter));
       protected FileStream fs;
       protected StreamWriter sw;

       public virtual  void Write(string message,string file,FileMode filemode)
       {
          // FileStream fs;
           try
           {
               lock (writelock)
               {
               if (!File.Exists(file))
               {
                   string pathname = Path.GetDirectoryName(file);
                   if (!Directory.Exists(pathname))
                   {

                       Directory.CreateDirectory(pathname);
                   }

                   fs = File.Open(file, filemode);
                 
               }else
               {
                   fs = File.Open(file, filemode);
               }

               StreamWriter sw = new StreamWriter(fs,Encoding.Default);
               
                   sw.WriteLine(message);
                   sw.Flush();
                   Dispose();
                   WriteCallBack(true, null);

               
               }// end lock
           }catch (Exception e)
           {
               //try
               //{
               //   if(fs!=null)
               //   {
               //       fs.Close();
               //       logger.ErrorFormat("Write File Fail[{0}]", e.Message);
               //       WriteCallBack(false, e.Message);
               //   }
                   
               //}catch(Exception e2)
               //{
               //    logger.ErrorFormat("Write File Fail[{0}]", e2.Message);
               //    WriteCallBack(false, e.Message);
               //}
               logger.ErrorFormat("Write File Fail[{0}]", e.Message);
               Dispose();
                    WriteCallBack(false, e.Message);

           }


       }



       private void StartWrite(object args)
       {
           object[] ars = (object[])args;
           string ms =(string) ars[0];
           string fs = (string) ars[1];
           FileMode filemode = (FileMode) ars[2];
           Write(ms, fs,filemode);

       }






       public void WriteAsyn(string message, string file,FileMode filemode)
       {
           object[] args = new object[] { message, file };
          // WriteFileAsynHandler st =new WriteFileAsynHandler( this.StartWrite);

           Thread t = new Thread(StartWrite);
           t.Priority = ThreadPriority.BelowNormal;
           t.Start(args);
       }


       protected virtual void WriteCallBack(bool success, object message)
       {
           if(success)
           {
               if (WriteSuccess != null)
               {
                   WriteSuccess(this, "Write Success");
               }
           }else
           {
               if (WriteFail != null)
               {
                   WriteFail(this, (string)message);
               }
           }

       }

       public void Dispose()
       {
           
               if (sw != null)
               {
                   try
                   {
                       sw.Close();
                   }
                   catch(Exception e)
                   {

                   }
                  
               }
               if (fs != null)
               {
                   try
                   {
                       fs.Close();
                   }
                   catch(Exception e)
                   {

                   }
                   
               }

       }
    }
}
