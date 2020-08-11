using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileDataLoader
{
   public  class CustomizedSimpleLog : ICustomizedLog
    {
        public void debug(string log)
        {
            addLog(log);
        }

        public void info(string log)
        {
            addLog(log);
        }

        public void error(string log)
        {
            addLog(log);
        }

       protected  void addLog(string log)
       {
              /*if(!  File.Exists(Application.StartupPath+"//"+DateTime.Now.ToString("yyyyMMHH")+".log")){
                  File.Create(Application.StartupPath+"//"+DateTime.Now.ToString("yyyyMMHH")+".log");

                }*/
                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory( "logs");
                }
                File.AppendAllText("logs//loader" + DateTime.Now.ToString("yyyyMMdd") + ".log", "[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "]" + log+"\n", Encoding.Default);
            }
    }
}
