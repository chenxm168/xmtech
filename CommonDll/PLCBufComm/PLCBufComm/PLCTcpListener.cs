using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Net;

namespace PLCBufComm
{
   public  class PLCTcpListener:PLCListener
    {

      // private string ecType = "FIXEDBUFFERASCII";
       private string ecType = "FIXEDBUFFERBINARY";
       private bool disposeRequest = false;

       

        public string EcType
        {
            get { return ecType; }
            set { ecType = value; }
        }
        private IMessageEncoder ec;

        public IMessageEncoder Ec
        {
            get { return ec; }
            set { ec = value; }
        }

        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private int tcpPort;

        public int TcpPort
        {
            get { return tcpPort; }
            set { tcpPort = value; }
        }



        private TcpListener listener;
        ManualResetEvent tcpClientConnected =   new ManualResetEvent(false);

       public PLCTcpListener(string ip,int port)
        {
           if(ip==null||ip.Trim().Length<1)
           {
               this.ip = "127.0.0.1";
           }else
           {
               this.Ip = ip;
           }
            
            this.TcpPort = port;
            IPAddress localip;

           try
           {
                localip = IPAddress.Parse(this.Ip);
           }catch(Exception e)
           {
               localip = IPAddress.Parse("127.0.0.1");
               logger.ErrorFormat("Parse Ipaddress Error! [Error:{0}]", e.Message);
               
           }

           try
           {
               listener = new TcpListener(localip, this.TcpPort);
           }catch(Exception e2)
           {
               logger.ErrorFormat("Construct Tcplisten Error! [Error:{0}]", e2.Message);
           }
           


            
          
        }


       private void startAnsy()
       {
           try
           {

          
           
           if(this.listener!=null)
           {
               listener.Start();
               logger.Info("tcplisten start!");
               isStart = true;
           }

               while(true)
               {
                   tcpClientConnected.Reset();
                   listener.BeginAcceptTcpClient(new AsyncCallback(doAcceptTcpClientCallback), listener);
                   tcpClientConnected.WaitOne();
                   if(disposeRequest)
                   {
                       listener.Stop();
                       logger.Info("tcplisten stop!");
                       break;
                   }

               }


          }catch (Exception e)
           {
               logger.ErrorFormat("TcpListner Start Error![Error:{0}]", e.Message);

           }


       }


       private void doAcceptTcpClientCallback(IAsyncResult ar)
       {
           try
           {
               TcpListener ln = (TcpListener)ar.AsyncState;
               TcpClient client = ln.EndAcceptTcpClient(ar);
               tcpClientConnected.Set();
               string remoteip = client.Client.RemoteEndPoint.ToString();
               onConnectedEvent(client, remoteip);
               while(true)
               {
                  if((client.Available>0)||(client.Client.Available>0))
                  {
                      NetworkStream ns = client.GetStream();
                      List<byte> listbyte = new List<byte>();
                      while(ns.DataAvailable)
                      {
                          listbyte.Add((byte)ns.ReadByte());
                      }

                      byte[] bytes = listbyte.ToArray<byte>();
                   

                    IMessageEncoder encoder = MessageEncoderFactory.getEncoder(ecType);
                    byte[] bRtn = encoder.getRtnBytes(bytes);
                    ns.Write(bRtn, 0, bRtn.Length);
                    ns.Flush();
                    logger.InfoFormat("Response Plc![Ascii:{0}]", ASCIIEncoding.UTF8.GetString(bRtn));

                    string message="";
                      int iRt = encoder.getMessageString(bytes,out message);
                      if(iRt==0)
                      {
                          onMessageEvent(client, message);

                      }


                  }else
                  {
                      if (((client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected))
                      {
                          onDisconnectedEvent(client, remoteip);
                          logger.InfoFormat("Client Disconnected![client:{0}]", remoteip);
                          break;
                      }

                      Thread.Sleep(100);
                      if(disposeRequest)
                      {
                          client.Close();
                          onDisconnectedEvent(client, remoteip);
                          break;
                      }
                  }
                  
               }


           }catch(Exception e)
           {
               onErrorEvent(this, e.Message);
               logger.ErrorFormat(e.Message);
           }

           finally
           {
               
           }
       }



       public override void Start()
       {
           Thread t = new Thread(startAnsy);
           t.Name = "PLCTcpListener";
           t.Priority = ThreadPriority.Normal;
           t.Start();
       }

       public override void Stop()
       {
           disposeRequest = true;
           tcpClientConnected.Set();
           isStart = false;
       }

       public override void Dispose()
       {
           Stop();
       }
    }
}
