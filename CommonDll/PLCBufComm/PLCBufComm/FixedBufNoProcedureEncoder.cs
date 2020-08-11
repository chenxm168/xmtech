using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PLCBufComm
{
   public class FixedBufNoProcedureEncoder:IMessageEncoder
    {
       public const string NAME = "FIXEDBUFFERNOPROCEDURE";
       private ILog logger = LogManager.GetLogger(typeof(FixedBufNoProcedureEncoder));
        public int getMessageString(byte[] bytes, out string message)
        {
            string s = "";
            int iRt = -1;
            try
            {
                foreach(byte b in bytes)
                {
                    s += Convert.ToString(b, 16);
                }
                iRt = 0;

                message = s;

            }catch(Exception e)
            {
                message = "";

                logger.Error(e.Message);
            }
            return iRt;
        }

        public int getMessageLen(byte[] bytes)
        {
            return bytes.Length;
        }

        public byte[] getRtnBytes(byte[] bytes)
        {
            return null;
        }

        public int getSendResponseCode(byte[] bytes)
        {
            return 0;
        }

        public byte[] getSendBytes(string message)
        {
            try
            {


            }catch (Exception e)
            {

            }
            return null;
        }

        public int getValueStringHL(byte[] bytes, out string values)
        {
            string s = "";
            int iRt = -1;
            try
            {
                for (int i = 0; i < bytes.Length;)
                {
                    if (i != 0)
                    {

                        s += "-";

                    }

                        s += Convert.ToString(bytes[i+1], 16);
                        s += "-";
                        s += Convert.ToString(bytes[i], 16);
                    
                    i += 2;
                }
                values = s;

            }
            catch (Exception e)
            {
                values = "";
                logger.Error(e.Message);
            }
            return iRt;
        }

        public int getValueStringLH(byte[] bytes, out string values)
        {
            string s = "";
            int iRt=-1;
            try{
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (i == 0)
                    {
                        s += "-";
                    }
                        s += Convert.ToString(bytes[i], 16);
                    
                }
                values = s;

            }catch(Exception e)
            {
                values = "";
                logger.Error(e.Message);
            }
            return iRt;
            
        }


        public string getName()
        {
            return NAME;
        }
    }
}
