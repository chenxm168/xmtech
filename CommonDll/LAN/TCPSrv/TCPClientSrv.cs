using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using log4net;
using System.IO;
using System.Threading;



namespace TCPSrv
{
    public class TCPClientSrv:IDisposable
    {

        protected ILog logger = LogManager.GetLogger(typeof(TCPClientSrv));
        protected TcpClient tcp;
        protected Stream stream;

        public bool IsConnected;
       IConnectStateCallback callback;
       object WRLocker = new object();

        
       public void StartConnect(IConnectStateCallback callback, string ip, int port)
       {
           if(tcp==null)
           {
               lock(WRLocker)
               {
                   if(tcp==null)
                   {
                       tcp = new TcpClient();
                   }
               }
               
           }
           //TcpClient client = new TcpClient();

           try
           {
               tcp.Connect(ip, port);
              // tcp = client;
               IsConnected = true;
               callback.ConnectSuccess(tcp, "connecet success");
               

               //test
               //byte[] data = new byte[1024];

               ////var dataSpan = new Span<byte>(data);
               //byte[] rbs = new byte[] { 103, 13 };
               //NetworkStream sm = tcp.GetStream();
               //// sm.Write(rbs, 0, rbs.Length);
               //// sm.Flush();
               //// byte[] data = new byte[1024];
               //Thread.Sleep(300);
               //if (sm.CanRead)
               //{
               //    //do
               //    //{
               //    //    sm.Read(data, 0, data.Length);
               //    //}
               //    //while (sm.DataAvailable);

               //    for (; ; )
               //    {
               //        sm.Write(rbs, 0, rbs.Length);
               //        sm.Flush();
               //        int d = sm.ReadByte();
               //        if (d == -1)
               //        {
               //            break;
               //        //}
               //        if(sm.DataAvailable)
               //        {
               //            logger.Debug("Network can read");
               //        }
               //        int datalenght=sm.Read(data, 0, data.Length);
               //        bool ddf = sm.DataAvailable;
               //        logger.Debug(datalenght.ToString());
               //    }

               //}
               //}
               //end test


           
           }
           catch(Exception e)
           {
               callback.ConnectFail(tcp, e.Message);
           }

       }


        public void StartConnectAsyn(IConnectStateCallback callback, string ip, int port)
       {
           this.callback = callback;
            if(tcp==null)
            {
                lock(WRLocker)
                {
                  if(tcp==null)
                  {
                      tcp = new TcpClient();
                  }
                }
                
            }
            try
            {
                tcp.BeginConnect(ip, port, new AsyncCallback(ConnectCallBack), tcp);
            }catch(Exception e)
            {
                logger.ErrorFormat("TCP Conect Error[{0}]", e.Message);
            }
            

       }

       public void Disconnet(IConnectStateCallback callback)
        {
            if (tcp != null)
            {
                lock (WRLocker)
                {
                    if (tcp != null)
                    {
                        try
                        {
                            tcp.Close();
                            if(callback!=null)
                            {
                                callback.DisconnectSuccess();
                            }
                        }
                        catch (Exception e)
                        {
                            logger.ErrorFormat("TCP Disconnect Error[{0}]", e.Message);
                        }
                    }
                }

            }

        }

        protected virtual void ConnectCallBack(IAsyncResult ar)
        {
            TcpClient t = (TcpClient)ar.AsyncState;
            try
            {
                if(t.Connected)
                {
                    t.EndConnect(ar);
                    IsConnected = true;
                    callback.ConnectSuccess(t, "Connect Success!");

                    //test
                    //byte[] rbs = new byte[] { 103, 13 };
                    //NetworkStream sm = t.GetStream();
                    //sm.Write(rbs, 0, rbs.Length);
                    //sm.Flush();
                    //byte[] data = new byte[1024];
                    //Thread.Sleep(300);
                    //if(sm.CanRead)
                    //{
                    //    //do
                    //    //{
                    //    //    sm.Read(data, 0, data.Length);
                    //    //}
                    //    //while (sm.DataAvailable);

                    //    for (; ; )
                    //    {
                    //        int d = sm.ReadByte();
                    //        if (d == -1)
                    //        {
                    //            break;
                    //        }
                    //    }

                    //}//end test



                }
                else
                {
                    callback.ConnectFail(t, "Connect Fail!");
                }
            }
            catch(Exception e)
            {
                callback.ConnectFail(t, e.Message);
            }


        }


       public void Dispose()
       {
           if(stream!=null)
           {
               stream.Close();
           }
           if(tcp!=null)
           {
               tcp.Close();
           }
           
       }


        public bool SendBytes(byte[] bytes,Stream stream)
       {
            lock(WRLocker)
            {
               // NetworkStream ns = tcp.GetStream();
                try
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    return true;
                    
                }catch(Exception e)
                {
                    logger.Error("Write Bytes Error!");
                    return false;
                }
            }
           return true;
       }

        public bool SendBytes(byte[] bytes)
        {
            lock (WRLocker)
            {
                try
                {
                if(stream==null)
                {
                    stream = tcp.GetStream();
                }
                // NetworkStream ns = tcp.GetStream();
                
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();


                    //test
                    /*
                    if (stream != null)
                    {
                        while (true)
                        {
                            NetworkStream ns = (NetworkStream)stream;
                            if (ns.DataAvailable)
                            {
                                byte[] bs = new byte[1024];
                                ns.Read(bs, 0, bs.Length);
                                // bytes = bs;
                                //return true;
                                break;
                            }
                        }
                    } */ //end test



                    return true;

                }
                catch (Exception e)
                {
                    logger.Error("Write Bytes Error!");
                    return false;
                }
            }
            return true;
        }

        public bool ReadBytes(out byte[] bytes)
        {
            Byte[] bs = new  Byte[1024];
            lock(WRLocker)
            {

           
            try
            {
                if(stream!=null)
                {
                    NetworkStream ns = tcp.GetStream();
                   /*
                    * while (true)
                    {
                        
                       if( ns.DataAvailable)
                       {
                           ns.Read(bs, 0, bs.Length);
                          // bytes = bs;
                           //return true;
                           break;
                       }
                    }*/
                    if(ns.CanRead)
                    {
                        do
                        {
                            ns.Read(bs, 0, bs.Length);
                            //ns.Read(bs, 0, bs.Length);
                            bytes = bs;
                            //return true;
                        }
                        while (ns.DataAvailable);
                    }
              
                }
              
            }catch(Exception e)
            {
                logger.ErrorFormat("Read Bytes Error[{0}]! ",e.Message);
                bytes = bs;
                return false;
            }
            }
            bytes = bs;
            return true;
        }
    }
}
