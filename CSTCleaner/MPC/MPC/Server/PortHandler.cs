using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService;
using TIBMessageIo.MessageSet;

namespace MPC.Server
{
   public  class PortHandler
    {
       private static object ob = new object();

       public static void PortLoadRequestReport(string portid)
       {
           var portSvr = ServiceManager.GetPortService();
           if (portSvr == null)
           {
               return;
           }


           var ports = portSvr.FindAll();
           foreach (var port in ports)
           {
               //if(port.PortStatus=="LoadRequest")
               if(port.PortName==portid)
               {
                   var TibSend = ObjectManager.getObject("TibSender") as TIBMessageIo.ISendable;

                   var portInfo = new PortBaseInfo();
                   if(port.PortStatus.ToUpper().Trim()=="LOADREQUEST")
                   {
                       portInfo.PORTTRANSFERSTATENAME= "LR";
                   }
                   
                   portInfo.PORTNAME=port.PortName;
                   portInfo.PORTTYPE = port.PortType;
                   portInfo.CARRIERTYPE =port.CassetteType;

                   var mSvr = ServiceManager.GetEquipmentService();

                   var eq = mSvr.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
                  if(eq !=null)
                  {
                      portInfo.MACHINESTATENAME = eq.EquipmentStatus;
                      portInfo.MACHINECONTROLSTATENAME = eq.OnlineControlStatus;
                  }
                   PortBaseInfo[] ps = new PortBaseInfo[]{portInfo};
                  var sendMsg=  PortDataMessage.getPortTransferStateChangedMessage(ps);
                   
                   TibSend.Send(sendMsg);

                  
               }
           }//end foreach
           }//end lock


       public static void PortUnloadRequestReport(string portid,string cstid)
       {
           var portSvr = ServiceManager.GetPortService();
           if (portSvr == null)
           {
               return;
           }


           var ports = portSvr.FindAll();
           foreach (var port in ports)
           {
               //if(port.PortStatus=="LoadRequest")
               if (port.PortName == portid)
               {
                   var TibSend = ObjectManager.getObject("TibSender") as TIBMessageIo.ISendable;

                   var portInfo = new PortBaseInfo();
                   if (port.PortStatus.ToUpper().Trim() == "UNLOADREQUEST")
                   {
                       portInfo.PORTTRANSFERSTATENAME = "UR";
                   }

                   portInfo.PORTNAME = port.PortName;
                   portInfo.PORTTYPE = port.PortType;
                   portInfo.CARRIERTYPE = port.CassetteType;
                   portInfo.CARRIERNAME = cstid;

                   var mSvr = ServiceManager.GetEquipmentService();

                   var eq = mSvr.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
                   if (eq != null)
                   {
                       portInfo.MACHINESTATENAME = eq.EquipmentStatus;
                       portInfo.MACHINECONTROLSTATENAME = eq.OnlineControlStatus;
                   }
                   PortBaseInfo[] ps = new PortBaseInfo[] { portInfo };
                   var sendMsg = PortDataMessage.getPortTransferStateChangedMessage(ps);

                   TibSend.Send(sendMsg);


               }
           }//end foreach
       }//end lock

       
       public static void PortStatusReport()
       {
           lock(ob)
           { 
           if (EquipmentIsOffline())
           {
               return;
           }
           var portSvr = ServiceManager.GetPortService();
           if(portSvr==null)
           {
               return;
           }

           var ports = portSvr.FindAll();
           foreach (var port in ports)
           {
               if(port.PortStatus=="LoadRequest"||port.PortStatus=="UnloadRequest")
               {
                   var TibSend = ObjectManager.getObject("TibSender") as TIBMessageIo.ISendable;

                   var portInfo = new PortBaseInfo();
                   if(port.PortStatus.ToUpper().Trim()=="LOADREQUEST")
                   {
                       portInfo.PORTTRANSFERSTATENAME= "LR";
                   }else
                   {
                       portInfo.PORTTRANSFERSTATENAME="UR";
                       portInfo.CARRIERNAME = "S517A9999";
                   }
                   
                   portInfo.PORTNAME=port.PortName;
                   portInfo.PORTTYPE = port.PortType;
                   portInfo.CARRIERTYPE =port.CassetteType;

                   var mSvr = ServiceManager.GetEquipmentService();

                   var eq = mSvr.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
                  if(eq !=null)
                  {
                      portInfo.MACHINESTATENAME = eq.EquipmentStatus;
                      portInfo.MACHINECONTROLSTATENAME = eq.OnlineControlStatus;
                  }
                   PortBaseInfo[] ps = new PortBaseInfo[]{portInfo};
                  var sendMsg=  PortDataMessage.getPortTransferStateChangedMessage(ps);
                   

                   port.PortStatus = "Reserved";
                  if( portSvr.UpdatePort(port, "Reserved")>0)
                  {
                      TibSend.Send(sendMsg);
                  }
               }
           }//end foreach
           }//end lock

       }

       public static bool EquipmentIsOffline()
       {
           bool IsOffline = true;

           var eqSvr = ServiceManager.GetEquipmentService();
           var eq = eqSvr.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
           if(eq.OnlineControlStatus.Trim().ToUpper()!="OFFLINE")
           {
               IsOffline = false;
           }
           //((IDisposable)eqSvr).Dispose();
           return IsOffline;
       }
    }
}
