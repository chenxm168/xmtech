using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
  public  class MessageEncoderFactory
    {

      public static IMessageEncoder getEncoder(string name)
      {
          name = name.ToUpper().Trim();

          switch (name)
          {
              case "FIXEDBUFFERASCII":


                  return  new FixedBufAsciiEncoder();
             

              case "FIXEDBUFFERBINARY":

                  return new FixedBufBinaryEndcoder();

              default:

                  return new FixedBufAsciiEncoder();


                  

          }



      }




    }
}
