using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPSrv;
using System.Net.Sockets;
using log4net;
using System.Threading;

namespace MPC.LAN
{
   public class ReaderTCP :IConnectStateCallback, IReader,IDisposable
    {
       public TCPClientSrv tcp;

       public string RemoteIp
       {
           get;
           set;
       }
       public int Port
       {
           get;
           set;
       }

       public byte Terminator
       { 
           get;
           set; 
       }

       public int ReveiveTimeout
       {
           get;
           set;
       }

       

       ILog logger = LogManager.GetLogger(typeof(ReaderTCP));
       Object locker = new object();
        public void ConnectSuccess(object sender, string message)
        {
            logger.Info("Reader TCP Connect Successs!");


            //throw new NotImplementedException();
        }

        public void ConnectFail(object sender, string messsage)
        {
            logger.ErrorFormat("Reader TCP Connect Fail:[{0}]", messsage);
           // throw new NotImplementedException();
        }

        public void Connect(string ip,int port)
        {
            if(tcp!=null)
            {
                logger.Debug("Reader Start Connect");
               // tcp.StartConnect(this, ip, port);
            }else
            {
                tcp = new TCPClientSrv();
               // tcp.StartConnect(this, ip, port);

            }
            tcp.StartConnect(this, ip, port);
        }

       public void Connect()
        {
            Connect(RemoteIp, Port);
        }


        public ReaderTCP()
        {
             tcp = new TCPClientSrv();
        }


        public void ConnectAsyn(string ip, int port)
        {
            if (tcp != null)
            {
                logger.Debug("Reader Start Connect");
                // tcp.StartConnect(this, ip, port);
            }
            else
            {
                tcp = new TCPClientSrv();
                // tcp.StartConnect(this, ip, port);

            }
            tcp.StartConnectAsyn(this, ip, port);
        }

        public void Dispose()
        {
            if(tcp!=null)
            {
                tcp.Dispose();
            }
        }


        public void Disconnection()
        {
            Dispose();
        }

       public bool SendBytes(Byte[] bytes)
        {
           
           //if(tcp.IsConnected)
           //{

           //}
            return true;
        }

       public bool ReadIDOnice(out byte[] rtnbytes)
       {
           byte[] rbs = new byte[] { 103, 13 };
           if(tcp.SendBytes(rbs))
           {
              // byte[] rbs;
               int count = 0;
               Thread.Sleep(100);
             while( true)
             {

                 if( tcp.ReadBytes(out rbs))
                 {
                     byte[] rbs2 = rbs;
                     rtnbytes = rbs2;
                     return true;
                 }else
                 {
                     if(count>100)
                     {
                         rtnbytes = null;
                         return false;
                     }else
                     {
                         count++;
                         Thread.Sleep(50);
                     }
                 }
                
             }
                
           }else
           {
               rtnbytes = null;
               return false;
           }
       }


       public bool ReadIDCycleUntilSuccess(out byte[] bytes,ReaderCallback callback)
       {
           byte[] bs;
           byte[] rbs = new byte[]{103,13};

           tcp.SendBytes(rbs);
           bytes = null;
           return true;
       }
    }
}
