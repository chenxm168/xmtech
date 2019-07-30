using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HF.DB
{
   public  class HFDBParameter
    {
       public string KeyName
       {
           get;
           set;
       }

       public object Value
       {
           get;
           set;
       }

       public string CondictionChar
       {
           get;
           set;
       }


       

       
    }

    public enum SqlCondictionType
    {
        EQUAL,LIKE
    }
}
