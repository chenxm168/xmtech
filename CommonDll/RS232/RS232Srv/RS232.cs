using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using log4net;
using System.Threading;

namespace RS232Srv
{
   public class RS232:IDisposable
    {
       private SerialPort rs232;
       private ILog logger = LogManager.GetLogger(typeof(RS232));
       Object openLocker = new object();
       private int timeout=10;

       object sendLocker = new object();
       object recieveLocker = new object();

       public RS232()
       {
           rs232 = new SerialPort();
           rs232.BaudRate = 9600;
           
           rs232.DataBits=8;
           rs232.StopBits=StopBits.One;
           rs232.Parity= Parity.None;
           rs232.PortName="COM1";
           
       }

       public RS232(RS232Config rsp)
       {
           rs232 = new SerialPort();
           rs232.BaudRate = rsp.BaudRate;
           rs232.DataBits = rsp.DataBits;
           timeout = rsp.RsRecvTimeout;

           switch (rsp.RsStopBits)
           {
               case "1":
                   rs232.StopBits = StopBits.One;
                   break;
               case "1.5":
                   rs232.StopBits = StopBits.OnePointFive;
                   break;
               case "2":
                   rs232.StopBits = StopBits.Two;
                   break;
               default:
                   rs232.StopBits = StopBits.None;
                   break;

           }


           switch (rsp.RsParity.ToUpper())
           {
               case "ODD":
                   rs232.Parity = Parity.Odd;
                   break;
               case "EVEN":
                   rs232.Parity = Parity.Even;
                   break;
               default:
                   rs232.Parity = Parity.None;
                   break;
           }

           //sp.StopBits = rsp.RsStopBits;
           //sp.Parity = rsp.RsParity;
           rs232.PortName = rsp.PortName;


       }

       ~RS232()
       {
           Dispose();
       }


       public bool Open()
       {
           lock(openLocker)
           {
               try
               {
                   if (!rs232.IsOpen)
                   {
                       rs232.Open();
                       
                   }
               }catch(Exception e)
               {
                   logger.ErrorFormat("RS232 Open Error[{0}]", e.Message);
                   return false;
               }

               return true;
               
           }
       }


       public bool SendBytes(byte[] data)
       {

           lock(sendLocker)
           {
              try
              {

              
               if(!rs232.IsOpen)
               {
                   if(!Open())
                   {
                       return false;
                   }

                   //rs232.DiscardInBuffer();
                  
                   
               }
               rs232.DiscardOutBuffer();
               rs232.Write(data, 0, data.Length);
               
                }catch(Exception e)
              {
                  return false;
                  logger.ErrorFormat("Send Bytes Error[{0}]", e.Message);
              }

           }

           return true;
       }


       public bool ReceiveBytes(out byte[] data)
       {

           return ReceiveBytes(out data, this.timeout);
       }

       public bool ReceiveBytes(out byte[] data,int timeout)
       {

           lock(recieveLocker)
           {

           
           List<byte> lbs = new List<byte>();
           if (!rs232.IsOpen)
           {
               data = null;
               return false;
           }
           try
           {


               for (int i = 0; ; i++)
               {


                   int count = rs232.BytesToRead;
                   if (count >= 0)
                   {
                       byte[] bytes = new byte[count];
                       rs232.Read(bytes, 0, count);
                       lbs.AddRange(bytes);
                   }
                   if (i * 10 >= timeout)
                   {
                       break;
                   }else
                   {
                       Thread.Sleep(10);
                   }

               }//end for
           }
           catch (Exception e)
           {
               logger.ErrorFormat("Receive Bytes Error[{0]}");
               data = null;
               return false;
           }
           if (lbs.Count > 0)
           {
               data = lbs.ToArray<byte>();
               return true;

           }
           else
           {
               data = null;
               return false;
           }
           }//end lock

       }

       public bool ReceiveBytes(out byte[] data,byte terminator,int timeout)
       {
           lock (recieveLocker)
           {


               List<byte> lbs = new List<byte>();
               if (!rs232.IsOpen)
               {
                   data = null;
                   return false;
               }
               try
               {
                   for (int i = 0; i < timeout / 10; i++)
                   {
                       for (int j = 0; j < rs232.BytesToRead; j++)
                       {
                           byte bData = (byte)rs232.ReadByte();
                           lbs.Add(bData);
                           if (bData == terminator)
                           {
                               data = lbs.ToArray<byte>();
                               return true;
                           }

                       }

                       Thread.Sleep(10);

                   }

                   //for (int i = 0; ; i++)
                   //{


                   //    int count = rs232.BytesToRead;
                   //    if (count >= 0)
                   //    {
                   //             rs232.ReadByte()
                   //    }
                   //    if (i * 10 >= timeout)
                   //    {
                   //        break;
                   //    }

                   //}//end for
               }
               catch (Exception e)
               {
                   logger.ErrorFormat("Receive Bytes Error[{0]}");
                   data = null;
                   return false;
               }
               logger.ErrorFormat("Receive Bytes Timeout[{0}ms]", timeout);
               data = null;
               return false;
           }//end lock
       }


       public bool ReceiveBytes(out byte[] data,byte terminator)
       {
           return ReceiveBytes(out data, terminator, this.timeout);
       }

       public void Close()
       {
           try
           {
               rs232.Close();
           }catch(Exception e)
           {
               logger.ErrorFormat("RS232 Close Error[{0}]", e.Message);
           }
       }


       public void Dispose()
       {
           Close();
       }
    }
}
