using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PLCBufComm
{
    public   class FixedBufAsciiEncoder:IMessageEncoder
    {
        ILog logger = LogManager.GetLogger(typeof(FixedBufAsciiEncoder));
        public const string NAME = "FIXEDBUFFERASCII";



        public FixedBufAsciiEncoder()
        {

        }

        public int getMessageString(byte[] bytes, out string message)
        {
            int iRt = 0;
            string s = ASCIIEncoding.UTF8.GetString(bytes);

            int len = Convert.ToInt16(s.Substring(4, 4));

            if((s.Length-8)!=len*4)
            {
                iRt = -1;
                message = "Message Length Error";
                return iRt;
            }
            string msg1 = s.Substring(8);

            string[] msgworldlist = new string[len];
            for (int i = 0; i < len;i++ )
            {
                msgworldlist[i] = msg1.Substring(i * 4, 4);

            }

            string[] msgbytelist = new string[len * 2];
            for (int i = 0; i < msgworldlist.Length;i++ )
            {
                string ls = msgworldlist[i].Substring(2, 2);
                string hs = msgworldlist[i].Substring(0, 2);

              byte lb=  Convert.ToByte(ls.Substring(0, 1), 16);
              byte hb = Convert.ToByte(hs.Substring(1, 1), 16);





              msgbytelist[i * 2] = ls;
              msgbytelist[i * 2 + 1] = hs;

            }

            string mes3 = "";
            for (int i = 0; i < msgbytelist.Length;i++ )
            {
                mes3 += msgbytelist[i];
            }

            message = mes3;
               // message = s.Substring(4, s.Length - 4);

            return iRt;
        }


        public int getMessageLen(byte[] bytes)
        {
            try
            {
                string s = ASCIIEncoding.UTF8.GetString(bytes);

                return  Convert.ToInt16(s.Substring(4, 4));
            }
            catch(Exception e)
            {
                return -1;
            }
            
        }


        public byte[] getRtnBytes(byte[] bytes)
        {
            byte[] sendbytes = new byte[4];
            sendbytes[0] = 0x45;
            sendbytes[1] = 0x30;
            sendbytes[2] = 0x30;
            sendbytes[3] = 0x30;

            return sendbytes;
        }

        private  string getString(byte[] bytes)
        {
            try
            {
                return ASCIIEncoding.UTF8.GetString(bytes);


            }catch(Exception e)
            {
                return null;
            }
        }


        private bool varidateLen(byte[] bytes)
        {
            return varidateLen(ASCIIEncoding.UTF8.GetString(bytes));
        }

        private bool varidateLen(string s)
        {
            try
            {
                int len = Convert.ToInt16(s.Substring(4, 4));
                if ((s.Length - 8) != len * 4)
                {
                    logger.ErrorFormat("Message Length Unmatch! [Length:{0}]",len );
                    return false;
                }else
                {
                    return true;
                }

            }

            catch(Exception e)
            {
                logger.ErrorFormat("Message Length Error! [{0}]", e.Message);
                return false;
            }


        }



        public int getSendResponseCode(byte[] bytes)
        {
            int iRt = -99;
            string s = ASCIIEncoding.UTF8.GetString(bytes);
            if(s.Length!=4)
            {
                return -11;
            }
            string s1 = s.Substring(2,2);

            try
            {
                iRt = Convert.ToInt16(s1, 16);
            }
             
            catch(Exception e)
            {
                logger.ErrorFormat("Resopose Code is Illegal[{0}]", s1);
            }

            return iRt;
           
        }

        public byte[] getSendBytes(string message)
        {
            int len = 0;
            if(message.Length%4==0)
            {
                len = message.Length / 4;
            }else
            {
                if(message.Length/4<1)
                {
                    message.PadLeft(4, '0');
                    len = 1;
                }else
                {
                    len = message.Length / 4;
                    string mes1 = message.Substring(0, len * 4);
                    string mes2 = message.Substring(len * 4);
                    mes2 = mes2.PadLeft(4, '0');
                    message = mes1 + mes2;
                    len++;
                }
            }

           string sLen = Convert.ToString(len, 16);
           sLen = sLen.PadLeft(4, '0');
           message = "6000" + sLen + message;
            return System.Text.Encoding.ASCII.GetBytes(message);


        }


        public int getValueStringHL(byte[] bytes, out string values)
        {
            
            try
            {
                string s = ASCIIEncoding.UTF8.GetString(bytes);

                int len = Convert.ToInt16(s.Substring(4, 4));
                if ((s.Length - 8) != len * 4)
                {
                    values = "";
                    return -1;
                }
                else
                {
                    values = "";
                    for (int i = 8; i < len * 4; )
                    {
                        string h = s.Substring(i, 2);
                        string l = s.Substring(i + 2, 2);
                       if(i!=8)
                       {
                           values += "-";
                       }
                       values = values + h+"-" + l;
                       i += 4;
                    }
                    return 0;
                }

            }catch (Exception e)
            {
                logger.ErrorFormat(e.Message);
                values = "";
                return -1;
            }

        }

        public int getValueStringLH(byte[] bytes, out string values)
        {
            try
            {
                string s = ASCIIEncoding.UTF8.GetString(bytes);

                int len = Convert.ToInt16(s.Substring(4, 4));
                if ((s.Length - 8) != len * 4)
                {
                    values = "";
                    return -1;
                }
                else
                {
                    values = "";
                    for (int i = 8; i < len * 4; )
                    {
                        string h = s.Substring(i, 2);
                        string l = s.Substring(i + 2, 2);
                        if (i != 8)
                        {
                            values += "-";
                        }
                        values = values + l +"-"+ h;
                        i += 4;
                    }
                    return 0;
                }

            }
            catch (Exception e)
            {
                logger.ErrorFormat(e.Message);
                values = "";
                return -1;
            }
        }


        public string getName()
        {
            return NAME;
        }
    }
}
