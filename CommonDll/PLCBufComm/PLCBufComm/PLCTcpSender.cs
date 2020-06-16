using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;

namespace PLCBufComm
{
   public class PLCTcpSender:IPLCSendable
    {

       public event EventHandler<object> ResponseTimeout;
       private ILog logger = LogManager.GetLogger(typeof(PLCTcpSender));
       private TcpClient client;

       public bool KeepAlive
       { get; set; }

       public string EcType
       { get; set; }

       public int Timeout
       { get; set; }

       private string remoteip;
       private int remoteport;

      // private Queue<object> waitSendQueue = new Queue<object>();

       public PLCTcpSender(string remoteip, int remoteport)
       {
           this.remoteip= remoteip;
           this.remoteport =remoteport;
           this.KeepAlive = false;
           this.EcType = "FIXEDBUFFERASCII";
           this.Timeout = 50000;
         
       }
        public int SendMessage(string message)
        {
            var ec = MessageEncoderFactory.getEncoder(this.EcType);
            var bytes = ec.getSendBytes(message);
            return SendByte(bytes);
        }

        public bool Open()
        {
            try
            {
                if (client == null)
                {
                    client = new TcpClient(this.remoteip, this.remoteport);
                    return true;
                }else
                {
                    if (((client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected))
                    {
                        client = new TcpClient(this.remoteip, this.remoteport);
                        return true;
                    }else
                    {
                        return true;
                    }
                }

           }catch(Exception e)
            {
                logger.Error(e.Message);
                return false;
            }
        }

        public bool IsOpen()
        {
            if(client==null||((client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected))
            {
                return false;
            }else
            {
                return true;
            }
        }

        public int SendByte(byte[] bytes)
        {

               if(!Open())
               {
                   return -1;
               }
      

            try
            {
                NetworkStream ns = client.GetStream();
                ns.Write(bytes, 0, bytes.Length);
                ns.Flush();
                var ec = MessageEncoderFactory.getEncoder(EcType);
                if(ec.getName()=="FIXEDBUFFERNOPROCEDURE")
                {
                    ns.Close();
                    client.Close();
                }

                int count = 1;
                while(true)
                {
                    if(client.Available>0)
                    {
                        
                        List<byte> listbyte = new List<byte>();
                        while(ns.DataAvailable)
                        {

                            listbyte.Add((byte)ns.ReadByte());
                        }

                        byte[] bRtn = listbyte.ToArray<byte>();

                        
                        int iRt= ec.getSendResponseCode(bRtn);
                        if(!KeepAlive)
                        {
                            ns.Close();
                            client.Close();
                        }
                        return iRt;


                    }else
                    {
                        if(client==null||((client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected))
                        {
                            return -1;
                        }
                      
                        if(count*10>this.Timeout)
                        {
                            if(ResponseTimeout!=null)
                            {
                                ResponseTimeout(client, "Plc Response Timeout");
                                return -1;
                            }else
                            {
                                count++;
                                Thread.Sleep(10); 
                            }
                        }


                    }


                }

                
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Send bytes Error![error:{0}]", e.Message);
                return -1;
            }
        }

        public void Dispose()
        {
            if(client!=null)
            {
                try
                {
                    client.Close();
                    logger.InfoFormat("Tcp Client[{0}] Close!", client.Client.RemoteEndPoint.ToString());
                }catch(Exception e)
                {

                }
            }
        }
    }
}
