using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF.DB.ObjectService.Type1.Pojo
{
   public class AlarmSpec
    {
       [ColummAttribute(PrimaryKey = true)]
       public string UnitId
       {
           get;
           set;
       }

       [ColummAttribute(PrimaryKey = true)]
        public string Alid
        {
            get;
            set;
        }

        public string Altx
        {
            get;
            set;
        }
 

       public int Alcd
       {
           get;
           set;
       }
    }
}
