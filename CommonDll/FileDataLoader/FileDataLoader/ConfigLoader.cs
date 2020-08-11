using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileDataLoader
{
   public class ConfigLoader
    {
       private static  ConfigLoader instance;
       Dictionary<string, string> cfg = new Dictionary<string, string>();
       static object  locker = new object();

       private ConfigLoader()
       {
           Init("Config.ini");
       }

       private ConfigLoader(string file)
       {
           Init(file);
       }

       private void Init(string file)
       {

           FileStream fs = null;
           StreamReader rs = null;
           if(!File.Exists(file))
           {
               return;
           }

           try
           {


               string[] pms = File.ReadAllLines(file);

               foreach(string str in pms)
               {

                   if (str.Trim().Length > 2 && str[0] != '#' && str.Contains("="))
                   {
                       string s1 = str.Substring(0, str.IndexOf('='));
                       string s2 = str.Substring(str.IndexOf('=') + 1);
                       cfg.Add(s1.Trim().ToUpper(), s2.Trim());
                   }

               }


           }catch(Exception e)
           {
               
               var Clog = LogFactory.getLogger(getParam("Logger"));
               Clog.error(e.Message);
           }


       }


       public string getParam(string name)
       {
           string sRtn = "";
           cfg.TryGetValue(name.ToUpper(), out sRtn);

            return sRtn;

       }

       public string getAsciiParam(string name)
       {
           string sRtn = "";
           string s1 = "";
          if( cfg.TryGetValue(name.ToUpper(), out s1))
          {
              string[] sAry =new string[ s1.Length / 2];
              byte[] bAry= new byte[s1.Length/2];

              for (int i = 0; i<bAry.Length;i++ )
              {
                  bAry[i] = Convert.ToByte(s1.Substring(i * 2, 2),16);
              }

              string s2 = System.Text.Encoding.ASCII.GetString(bAry);
              sRtn = s2;
          }

           return sRtn;
       }

       public static ConfigLoader getConfigInstance()
       {
           if (instance == null)
           {
               instance = new ConfigLoader();
           }
           return instance;
       }

       public static ConfigLoader getConfigInstance(string file)
       {
           if (instance == null)
           {
               lock(locker)
               {
                   if(instance==null)
                   {
                       instance = new ConfigLoader(file);
                   }
               }
               
           }
           return instance;
       }


    }

   


}
