using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCBufComm
{
   public class FixedBufBinaryEndcoder:IMessageEncoder
    {
       public const string NAME = "FIXEDBUFFERBINARY";
        public int getMessageString(byte[] bytes, out string message)
        {
            int len =getMessageLen(bytes);

            string mes1 = bytes2Str(bytes);
            mes1 = mes1.Substring(4, mes1.Length - 4);
            message = mes1;
            if((bytes.Length-4)!=len*2)
            {
                return -1;
            }else
            {
                return 0;
            }
        }

        public int getMessageLen(byte[] bytes)
        {
            

            byte[] bLen = new byte[2];

            List<byte> listLen = new List<byte>();
            Array.Copy(bytes,2,bLen,0,2);
            listLen.AddRange(bLen);
            listLen.Reverse();
            bLen = listLen.ToArray<byte>();

           string sLen = bytes2Str(bLen);
           return Convert.ToInt16(sLen, 16);
        }

        public byte[] getRtnBytes(byte[] bytes)
        {
            byte[] bRtn = new byte[2];
            bRtn[0] = 0xE0;
            bRtn[1] = 0;
            return bRtn;
        }

        public int getSendResponseCode(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] getSendBytes(string message)
        {
            throw new NotImplementedException();
        }

       public string bytes2Str(byte[] bytes)
        {
            string s = "";
           foreach(byte b in bytes)
           {
               s += Convert.ToString(b, 16);
           }
           return s;
        }


       public int getValueStringHL(byte[] bytes, out string values)
       {
           int len = getMessageLen(bytes);
           values = "";
           if ((bytes.Length - 4) != len * 2)
           {
               return -1;
           }
           else
           {
               string s = "";
               for(int i=4;i<len*2;)
               {
                   if(i!=4)
                   {
                       s += "-";
                   }
                   s += Convert.ToString(bytes[i + 1], 16);
                   s+="-";
                   s += Convert.ToString(bytes[i ], 16);

                   i = i + 2;
               }

               return 0;
           }
       }

       public int getValueStringLH(byte[] bytes, out string values)
       {
           int len = getMessageLen(bytes);
           values = "";
           if ((bytes.Length - 4) != len * 2)
           {
               return -1;
           }
           else
           {
               string s = "";
               for (int i = 4; i < len*2; )
               {
                   if (i != 4)
                   {
                       s += "-";
                   }
                   s += Convert.ToString(bytes[i ], 16);
                   s += "-";
                   s += Convert.ToString(bytes[i + 1], 16);

                   i = i + 2;
               }

               return 0;
           }
       }


       public string getName()
       {
           return NAME;
       }
    }
}
