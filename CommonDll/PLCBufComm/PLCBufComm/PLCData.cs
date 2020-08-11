using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
   public class PLCData
    {
       public string Name
       {
           get;
           set;
       }

       public string Value
       {
           get;
           set;
       }

       public int Len
       {
           get;
           set;
       }

       public Representation ValueRepresentation
       {
           get;
           set;
       }
       //public override string toString()
       //{

       //    string sRt = "";
       //    switch(ValueRepresentation)
       //    {

       //        case Representation.ASCII:

       //            break;

       //        case Representation.INTEGER:
       //            break;


       //        default:
                  
       //            break;
       //    }


       //    return sRt;

       //}
     
    }



    public enum Representation
    {
        ASCII,INTEGER,FLOAT
    }
}
