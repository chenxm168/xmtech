using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Net.Sockets;

namespace TCPSrv.Reader
{

    public delegate void ConnectedHandler(object sender,object args);
    public delegate void ReadCodeHandler(object sender,object[] args);


   public  class VCReaderTCP:TCPClientSrv,IConnectStateCallback,IVCReader
    {
       public event EventHandler ConnectedSuccessEvent;
       public event EventHandler<VCREeventArgs> ConnectedFailEvent;

       public event EventHandler<VCREeventArgs> ReadSuccess;
       public event EventHandler<VCREeventArgs> ReadFail;

       public event EventHandler ReadStartEvent;
       public event EventHandler ReadStopEvent;
       private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

       object stateLock = new object();
       object readLock = new object();

       private ReadState readstate;

       public new byte[] ReadBytes
       {
           get;
           set;
       }
      public  ReadState VCReadState
       {
           get
           {
               lock(stateLock)
               {
                   return readstate;
               }
           }
           set
           {
               lock (stateLock)
               {
                    readstate=value;
               }
           }
       }
       public string RemoteIp
       {
           get;
           set;
       }

       public int TcpPort
       {
           get;
           set;
       }
        object ConnectedLock = new object();
        //public bool IsConnected
        //{
        //    get
        //   {
        //       lock(ConnectedLock)
        //       {
        //           return IsConnected;
        //       }
        //   }
        //    set
        //    {
        //        lock (ConnectedLock)
        //        {
        //            IsConnected = value;
        //        }
        //    }
        //}



       public VCReaderTCP():base()
       {
           IsConnected = false;
           Init("VCRConfigTCP.xml");
           //Init();
       }

       public VCReaderTCP(string file):base()
       {
           IsConnected = false;
           Init(file);
       }

       public VCRConfigTCP Config
       {
           get;
           set;
       }



       public void ConnectSuccess(object sender, string message)
       {
           IsConnected = true;
           if(ConnectedSuccessEvent!=null)
           {
               ConnectedSuccessEvent(this, null);
           }
           logger.Debug("VCReader TCP Connected!");
       }

       public void ConnectFail(object sender, string messsage)
       {
           IsConnected = false;
           if(ConnectedFailEvent!=null)
           {
               ConnectedFailEvent(this, new VCREeventArgs(messsage));
           }
           logger.ErrorFormat("VCReader TCP Connected fail[{0}]",messsage);
       }

       public void Connect()
       {
           if(Init("VCRConfigTCP.xml"))
           {
               StartConnect(this, RemoteIp, TcpPort);
           }
           
       }

       public void ConnectAsyn()
       {
           if (Init("VCRConfigTCP.xml"))
           {
               StartConnectAsyn(this, RemoteIp, TcpPort);
           }
           
       }


       protected bool Init(string file)
       {
           VCRConfigTCP cf;
           VCReadState = ReadState.Stop;
           try
           {
               if(!File.Exists(file))
               {
                   logger.ErrorFormat("INIT Error,File is not exist[{0}]", file);
                   return false;
               }
               XmlSerializer xs = new XmlSerializer(typeof(VCRConfigTCP));
               using(Stream reader = new FileStream(file,FileMode.Open))
               {
                   cf =(VCRConfigTCP) xs.Deserialize(reader);
               }

               Config = cf;
               RemoteIp = cf.RemoteIp;
               TcpPort = cf.TcpPort;

               
           }catch(Exception e)
           {
               logger.ErrorFormat("INIT Error[{0}]", e.Message);
               return false;
           }

           return true;
       }


       public void ReadCode()
       {
           VCReadState = ReadState.Reading;
           
           lock(readLock)
           {
               try
               {
                   NetworkStream ns = tcp.GetStream();

                   for(;;)
                   {
                       if (VCReadState == ReadState.StopRequest)
                       {
                           VCReadState = ReadState.Stop;
                           break;
                       }
                   byte[] cmd = new byte[] { 103, 13 };
                   ns.Write(cmd, 0, cmd.Length);
                   ns.Flush();

                   logger.Debug("Send Read Command");
                   Thread.Sleep(10);
                   bool ack = false;
                   List<byte> lData = new List<byte>();
                   for (int i=0; ;i++ )
                   {
                       if(ns.DataAvailable)
                       {
                           ack = true;
                           byte[] bts = new byte[1024];

                         int count=  ns.Read(bts, 0, bts.Length);

                         byte[] bts2 = new byte[count];
                         Array.Copy(bts, bts2, count);
                         int iStrlenght = 0;
                           for(int j=0;;j++)
                           {
                               if(bts2[j]==Config.Terminator)
                               {
                                   iStrlenght = j ;
                                   break;
                               }

                           }

                           if(iStrlenght!=0)
                           {
                               byte[] bStrbytes = new byte[iStrlenght];
                               Array.Copy(bts2, bStrbytes, iStrlenght);

                               string sReveice = Encoding.ASCII.GetString(bStrbytes);
                               logger.DebugFormat("Received [{0}]", sReveice);
                               if(sReveice!=Config.NGString.ToUpper().Trim())
                               {
                                   if(ReadSuccess!=null)
                                   {
                                       ReadSuccess(this, new VCREeventArgs(sReveice));
                                       
                                   }
                                   //ewh.WaitOne();
                                   break;
                               }else
                               {
                                   if(ReadFail!=null)
                                   {
                                       ReadFail(this, new VCREeventArgs("Ack NG"));
                                       
                                   }
                                   break;
                               }

                           }

                          
                       }



                       if(i>Config.ReveiveTimeout/10)
                       {
                           logger.ErrorFormat("Reader Ack Timeout[{0}ms]", Config.ReveiveTimeout);
                           break;
                       }else
                       {
                           Thread.Sleep(10);
                       }

                   }//end for 2



                       if (VCReadState == ReadState.StopRequest)
                       {
                           VCReadState = ReadState.Stop;
                           break;
                       }
                       else
                       {
                           Thread.Sleep(Config.ReadInterval);
                           logger.Debug("Start Next Cycle");
                       }


                   }//end for 1



               }catch(Exception e)
               {
                   throw e;
               }

               if(ReadStopEvent!=null)
               {
                   ReadStopEvent(this, null);
               }

               logger.Debug("Reader Stop");
           }
           
       }




      /* public bool ReadCode(out string msg)
       {
           VCReadState = ReadState.Reading;
           bool Ack = false;
           for (; ; )
           {
               Ack = false;
               lock(readLock)
               {

                   byte[] readbytes = new byte[] { 103, 13 };

                   if(SendBytes(readbytes))
                   {

                       int timeout = Config.ReveiveTimeout;

                       List<byte> lData = new List<byte>();
                       
                       for(int i=0;;i++)
                       {
                           byte[] dt;

                           NetworkStream ns = tcp.GetStream();
                          
                           if(ns.DataAvailable)
                           {
                               
                           }


                           if(i>(timeout/10))
                           {

                               break;
                           }


                       }






                   }else
                   {
                       
                       break;
                   }




               }//end lock
               if(VCReadState==ReadState.StopRequest)
               {
                   break;
               }
               Thread.Sleep(300);
               
                  
           }

           VCReadState = ReadState.Stop;



            msg = null;
           return false;
       }*/






       public void StartRead()
       {
           if(VCReadState==ReadState.Reading||VCReadState==ReadState.ReadRequest)
           {
               return;
           }
           try
           {
               Thread t = new Thread(new ThreadStart(ReadCode));
               t.Priority = ThreadPriority.Lowest;
               t.Name = "VCR Read thread";
               //t.IsBackground = true;
               t.Start();

               if(ReadStartEvent!=null)
               {
                   ReadStartEvent(this, null);
               }

           }catch(Exception e)
           {
               logger.ErrorFormat("Read Start Error[{0}]", e.Message);
           }


       }

       public void StopRead()
       {
           VCReadState = ReadState.StopRequest;
           logger.Debug("Reader Stop Request");
       }
    }
}
