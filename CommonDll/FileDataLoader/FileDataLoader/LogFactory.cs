using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataLoader
{
   public  class LogFactory
    {
       public static object getLog4netLogger(string logname)
       {
           //todo
           return null;
       }

       
       public static ICustomizedLog getSimpleLogger()
       {
           return new CustomizedSimpleLog();
       }

       public static ICustomizedLog getLogger(string name)
       {
           
           if(name==null)
           {
               return new CustomizedSimpleLog();
           }
           string n = name.ToLower().Trim();
           switch (n)
           {
              

               case "log4net":
                   return null;
               default:
                   return new CustomizedSimpleLog();
           }

       }

    }
}
