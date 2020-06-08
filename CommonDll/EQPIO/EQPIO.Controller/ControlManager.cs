using EQPIO.Common;
using EQPIO.Controller.Proxy;
//using EQPIO.LinkSignalTest;
using EQPIO.MessageData;
using EQPIO.MNetProtocol;
using EQPIO.RabbitMQInterface.Parser;
using EQPIO.RabbitMQInterface.Parser.Impl;
using log4net;
//using EQPIO.PLCsimControlLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
//using VirtualEQP;

namespace EQPIO.Controller
{
	public class ControlManager:IDisposable
	{
		public delegate void ConnectedEventHandler(object sender);

		public delegate void DisconnectedEventHandler(object sender);

		public delegate void OPCallMessage(string message);

        private ILog logger = LogManager.GetLogger(typeof(ControlManager));
        //private ILog logger = LogManager.GetLogger("EQPIO");

		private Globalproperties m_globalProperties = new Globalproperties();

		private int m_iTransactionNo = 1;

		private EQPIOConfig m_IOConConfig;

		private MQProxy mqProxy;

		private MProtocolProxy mProtocolProxy;

		private MNetProxy mNetProxy;

		private EIPProxy mEipProxy;

		private MessageData<PLCMessageBody> mNetConnectionInfo = new MessageData<PLCMessageBody>();

		private MessageData<EIPMessageBody> eipConnectionInfo = new MessageData<EIPMessageBody>();

		private Thread m_tEventProcessing = null;

		private Thread m_tTraceDataProcessing = null;

		private Thread m_tLicenseAlarmProcessing = null;

		private Thread m_tRGADataProcessing = null;

		private string m_strUsbKind = "ElecKey";

		private string m_strElecKeyModuleID = "0";

		private string m_strEquipmentName = "0";

        //private FormLinkSignalTestTool m_linkSignalTestTool = null;

        //private FormLinkSignalChartViewer m_linkSignalView = null;

        //private FormLinkSignalTypeModeler m_linkSignalTestToolModeler = null;

		private FormReadWriteRequestTest m_readWriteRequestTest = null;

        //private vEQP m_virtualEQP = null;

		private object m_objEthernetEvnetFuncLock = new object();

		private object m_objEthernetTraceFuncLock = new object();

		private object m_objEthernetRGADataFuncLock = new object();

		private object m_objBoardEvnetFuncLock = new object();

		private object m_objBoardTraceFuncLock = new object();

		private object m_objBoardRGAFuncLock = new object();

		private object eipObject = new object();

		private object m_objEIPEventFuncLock = new object();

		private object m_objEIPTraceDataFuncLock = new object();

		private string m_strInitFilePath = "./EQPIO.ini";

		private int m_iEventDequeueInterval = 10;

		private int m_iTraceDequeueInterval = 10;

		private int m_iLicenseAlarmDequeueInterval = 10;

		private int m_iRGADequeueInterval = 10;

		private Queue m_EventQ = new Queue();

		private Queue m_TraceDataQ = new Queue();

		private Queue m_LicenseAlarmQ = new Queue();

		private Queue m_RGADataQ = new Queue();

		private object m_objEventQLock = new object();

		private object m_objTraceDataQLock = new object();

		private object m_objLicenseAlarmQLock = new object();

		private object m_objRGADataQLock = new object();

        private IEPQEventHandler iEQPEventHandler;

        private bool resourceConfig = true;

        public IEPQEventHandler EQPEventHandler
        {
            get { return iEQPEventHandler; }
            set { iEQPEventHandler = value; }
        }

        public IEQPTraceDataHandler EQPTraceDataHandler
        {
            get;
            set;
        }

		public bool UseEthernet
		{
			get;
			set;
		}

		public bool UseBoard
		{
			get;
			set;
		}

		public bool UseEIP
		{
			get;
			set;
		}

		public bool UseMQ
		{
			get;
			set;
		}

        //public MProtocolProxy ProtocolProxy => mProtocolProxy;

        //public MNetProxy NetProxy => mNetProxy;
             public MNetProxy NetProxy
        {
            get
            {
                return this.mNetProxy;
            }
        }
        
        public MProtocolProxy ProtocolProxy
        {
            get
            {
                return this.mProtocolProxy;
            }
        }

        public EIPProxy EipProxy
        {
            get
            {
                return this.mEipProxy;
            }
        }

		public string USBKind
		{
			get
			{
				return m_strUsbKind;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					m_strUsbKind = "ElecKey";
				}
				else
				{
					m_strUsbKind = value;
				}
			}
		}

		public string ElecKeyModuleID
		{
			get
			{
				return m_strElecKeyModuleID;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					m_strElecKeyModuleID = "0";
				}
				else
				{
					m_strElecKeyModuleID = value;
				}
			}
		}

		public string EquipmentName
		{
			get
			{
				return m_strEquipmentName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					m_strEquipmentName = "0";
				}
				else
				{
					m_strEquipmentName = value;
				}
			}
		}

	        public Globalproperties GlobalProperties
        {
            get
            {
                return this.m_globalProperties;
            }
        }

        //public FormLinkSignalTestTool LinkSignalTestTool
        //{
        //    get
        //    {
        //        return m_linkSignalTestTool;
        //    }
        //    set
        //    {
        //        m_linkSignalTestTool = value;
        //    }
        //}

        //public FormLinkSignalChartViewer LinkSignalView
        //{
        //    get
        //    {
        //        return m_linkSignalView;
        //    }
        //    set
        //    {
        //        m_linkSignalView = value;
        //    }
        //}

        //public FormLinkSignalTypeModeler LinkSignalTestToolModeler
        //{
        //    get
        //    {
        //        return m_linkSignalTestToolModeler;
        //    }
        //    set
        //    {
        //        m_linkSignalTestToolModeler = value;
        //    }
        //}

		public FormReadWriteRequestTest FormReadWriteRequestTest
		{
			get
			{
				return m_readWriteRequestTest;
			}
			set
			{
				m_readWriteRequestTest = value;
			}
		}

		public event ConnectedEventHandler OnConnected;

		public event DisconnectedEventHandler OnDisconnected;

		public event OPCallMessage OnOPCall;

		private void mProtocolProxy_OnConnected(object sender)
		{
            MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
            messageData.MessageBody = new PLCMessageBody();
            //cxm 20191210 start
            string sType = sender.GetType().ToString();
            if(sType.ToLower().Contains("scan"))
            {
                messageData.MessageBody.EventName = "ScanUnitOnConnected";
                IMNetScanUnit u = sender as IMNetScanUnit;
                messageData.MachineName = u.ConnectionUnitName; //cxm 20191210
            }else
            {
                messageData.MessageBody.EventName = "UnitOnConnected";
                IMNetUnit u = sender as IMNetUnit;
                messageData.MachineName = u.Name; //cxm 2019121
            }

           //20191223 IMNetUnit iMNetUnit = sender as IMNetUnit; //cxm 20191012 end
			
            //messageData.MessageType = "Ethernet";
            messageData.MessageType = "MelsecEthernet";
			messageData.MessageName = "Connection";
			//20191210 messageData.MachineName = "L2";
          
			messageData.Transaction = MakeNewTransactionNo();
			
			messageData.ReturnCode = 0;
			messageData.ReturnMessage = string.Empty;
            if(mqProxy!=null)
            { 
			mqProxy.WriteSend(messageData, null);
            }
            if(OnConnected!=null)
            {
			this.OnConnected(this);
            }
            if(iEQPEventHandler!=null)
            {
                iEQPEventHandler.EQPEventProcess(messageData);
            }
               
			WritePLCLogData(messageData);
		}

		private void mProtocolProxy_OnDisconnected(object sender)
		{
            IMNetUnit iMNetUnit = sender as IMNetUnit; //cxm 20191012
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
            messageData.MessageBody = new PLCMessageBody();
            //messageData.MessageType = "Ethernet";
            //20191223 
               string sType = sender.GetType().ToString();
               if (sType.ToLower().Contains("scan"))
               {
                   messageData.MessageBody.EventName = "ScanUnitOnDisonnected";
                   IMNetScanUnit u = sender as IMNetScanUnit;
                   messageData.MachineName = u.ConnectionUnitName; //cxm 20191210
               }
               else
               {
                   messageData.MessageBody.EventName = "UnitOnDisconnected";
                   IMNetUnit u = sender as IMNetUnit;
                   messageData.MachineName = u.Name;
               }

            //20191223 end

            messageData.MessageType = "MelsecEthernet";
			messageData.MessageName = "Disconnection";
			//cxm 20191210 messageData.MachineName = "L2";
           // messageData.MachineName = iMNetUnit.Name; //cxm 20191210
			messageData.Transaction = MakeNewTransactionNo();
			//messageData.MessageBody = new PLCMessageBody();
			messageData.ReturnCode = 0;
			messageData.ReturnMessage = string.Empty;
            if(mqProxy!=null)
            {
                mqProxy.WriteSend(messageData, null);
            }
			if(OnDisconnected!=null)
            {
			this.OnDisconnected(this);
            }

            if(iEQPEventHandler!=null)
            {
                iEQPEventHandler.EQPEventProcess(messageData);
            }
			WritePLCLogData(messageData);
		}

		private void mqProxy_OnMelsecEthernetRequestReceived(object sender, MessageData<PLCMessageBody> message)
		{
			try
			{
				if (message == null)
				{
					logger.Error("message is NULL");
				}
				else if (mProtocolProxy != null)
				{
					if (!mProtocolProxy.Connection && message.MessageName != "ConnectionCheck")
					{
						message.MessageName = ((message.MessageName == "ReadRequest") ? "ReadReply" : "WriteReply");
						message.ReturnCode = 1;
						message.ReturnMessage = "Ethernet Not Connection";
						AddEvent(message);
						logger.Error(string.Format("OnMelsecEthernetRequestReceived : {0} - Ethernet Not Connection",message.MessageName));
					}
					else
					{
						WritePLCLogData(message);
						switch (message.MessageName)
						{
						case "ReadRequest":
							message.MessageName = "ReadReply";
							message = mProtocolProxy.ReadData(message);
							ExtraReadRequestBoard(message);
							if (message.MachineName != "Virtual")
							{
								AddEvent(message);
							}
							break;
						case "WriteRequest":
							message.MessageName = "WriteReply";
							if (!mProtocolProxy.WriteData(message))
							{
								if (!ExtraWriteRequestBoard(message))
								{
									message.ReturnCode = 1;
									message.ReturnMessage = "Write fail";
								}
								else
								{
									message.ReturnCode = 0;
									message.ReturnMessage = "Write Success";
								}
							}
							else
							{
								message.ReturnCode = 0;
								message.ReturnMessage = "Write Success";
							}
							if (message.MachineName != "Virtual")
							{
								AddEvent(message);
							}
							break;
						case "ConnectionCheck":
							if ((mProtocolProxy == null || !mProtocolProxy.Connection) && (mNetProxy == null || !mNetProxy.Connection))
							{
								message.MessageName = "Disconnection";
							}
							else
							{
								message.MessageName = "Connection";
							}
							AddEvent(message);
							break;
						default:
							logger.Error("MelsecEthernetRequestReceived : Invalid MessageName.. [ReadRequest, WriteRequest,ConnectionCheck]");
							return;
						case "WriteReply":
						case "ReadReply":
							break;
						}
						if (message.MachineName != "Virtual")
						{
							WritePLCLogData(message);
						}
					}
				}
			}
			catch (Exception arg)
			{
				logger.Error(string.Format("mqProxy_OnMelsecEthernetRequestReceived : {0}",arg));
			}
		}

		private void ExtraReadRequestBoard(MessageData<PLCMessageBody> message)
		{
			if (mNetProxy != null)
			{
				message = mNetProxy.ReadData(message);
			}
		}

		private bool ExtraWriteRequestBoard(MessageData<PLCMessageBody> message)
		{
			bool flag = false;
			if (mNetProxy != null)
			{
				if (!mNetProxy.WriteData(message))
				{
					message.ReturnCode = 1;
					message.ReturnMessage = "Write fail";
					flag = false;
				}
				else
				{
					message.ReturnCode = 0;
					flag = true;
				}
				return flag;
			}
			return false;
		}

		private void mProtocolProxy_OnEventReceived(object sender, MessageData<PLCMessageBody> message)
		{
			lock (m_objEthernetEvnetFuncLock)
			{
				bool flag = false;
				if (message.MessageType == "LoggingSkip")
				{
					flag = true;
				}
				message.MessageType = "MelsecEthernet";
				message.Transaction = MakeNewTransactionNo();
				AddEvent(message);
				if (!flag)
				{
					WritePLCLogData(message);
				}
			}
		}

		private void mProtocolProxy_OnTraceDataReceived(object sender, MessageData<PLCMessageBody> message)
		{
			lock (m_objEthernetTraceFuncLock)
			{
				try
				{
					message.MessageType = "MelsecEthernet";
					message.Transaction = MakeNewTransactionNo();
					AddTraceData(message);
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("OnSVEventReceived(Ethernet), Error Message : {1}", ex.Message));
				}
			}
		}

		private void mProtocolProxy_OnRGADataReceived(object sender, MessageData<PLCMessageBody> message)
		{
			lock (m_objEthernetRGADataFuncLock)
			{
				try
				{
					message.MessageType = "MelsecEthernet";
					message.Transaction = MakeNewTransactionNo();
					AddRGAData(message);
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("OnRGADAtaReceived(Ethernet), Error Message : {1}", ex.Message));
				}
			}
		}

        //private void mProtocolProxy_OnLinkSignalScanReceived(object sender, Dictionary<string, string> signal, Block block)
        //{
        //    if (m_linkSignalTestTool != null && m_linkSignalTestTool.LinkSignalChartViewer != null && !m_linkSignalTestTool.LinkSignalChartViewer.NowEditing)
        //    {
        //        m_linkSignalTestTool.LinkSignalChartViewer.DrawLinkSignal(block, signal);
        //    }
        //}

		private void mProtocolProxy_OnVirtualEQPEventReceived(object sender, MessageData<PLCMessageBody> message)
		{
			throw new NotImplementedException();
		}

		private void mqProxy_OnMNetRequestReceived(object sender, MessageData<PLCMessageBody> message)
		{
			try
			{
				if (message == null)
				{
					logger.Error("OnMNetRequestReceived : Data is null");
				}
				else if (mProtocolProxy == null)
				{
					if (mProtocolProxy != null || mNetProxy != null)
					{
						if (mProtocolProxy != null || mNetProxy == null)
						{
							goto IL_008e;
						}
						goto IL_008e;
					}
					message.ReturnCode = 1;
					message.ReturnMessage = "MNet Not Connection";
					AddEvent(message);
				}
				goto end_IL_0001;
				IL_008e:
				if (!mNetProxy.Connection && message.MessageName != "ConnectionCheck")
				{
					message.MessageName = ((message.MessageName == "ReadRequest") ? "ReadReply" : "WriteReply");
					message.ReturnCode = 1;
					message.ReturnMessage = "MNet Not Connection";
					AddEvent(message);
					logger.Error(string.Format("OnMNetRequestReceived : {0} - MNet Not Connection",message.MessageName));
				}
				else
				{
					switch (message.MessageName)
					{
					case "ReadRequest":
						WritePLCLogData(message);
						message.MessageName = "ReadReply";
						message = mNetProxy.ReadData(message);
						AddEvent(message);
						WritePLCLogData(message);
						break;
					case "WriteRequest":
						WritePLCLogData(message);
						WriteRequest(message);
						break;
					case "ConnectionCheck":
						WritePLCLogData(message);
						if (mNetProxy == null)
						{
							message.MessageName = "Disconnection";
						}
						else if (!mNetProxy.Connection)
						{
							message.MessageName = "Disconnection";
						}
						else
						{
							message.MessageName = "Connection";
						}
						message.ReturnCode = 0;
						AddEvent(message);
						WritePLCLogData(message);
						break;
					default:
						logger.Error("OnMNetRequestReceived : invalid MessageName..");
						return;
					case "WriteReply":
					case "ReadReply":
						break;
					}
					logger.Info(string.Format("OnMNetRequestReceived : {0} - {1}",message.MessageName,message.MachineName));
				}
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				this.OnOPCall(ex.Message);
			}
		}

		private void mqProxy_OnNetworkEvent(MessageData<PLCMessageBody> message)
		{
			WritePLCLogData(message);
			WriteRequest(message);
		}

		private void WriteRequest(MessageData<PLCMessageBody> message)
		{
			message.MessageName = "WriteReply";
			if (mNetProxy == null)
			{
				message.ReturnCode = 1;
				message.ReturnMessage = "MNet Not Connection";
				AddEvent(message);
			}
			else
			{
				if (!mNetProxy.WriteData(message))
				{
					message.ReturnCode = 1;
					message.ReturnMessage = "Write fail";
				}
				else
				{
					message.ReturnCode = 0;
				}
				AddEvent(message);
				WritePLCLogData(message);
			}
		}

		private void mNetProxy_OnEventReceived(object sender, MessageData<PLCMessageBody> message, string blockName)
		{
			lock (m_objBoardEvnetFuncLock)
			{
				try
				{
					bool flag = false;
					if (message.MessageType == "LoggingSkip")
					{
						flag = true;
					}
					message.MessageType = "MNet";
					message.Transaction = MakeNewTransactionNo();
					AddEvent(message);
					if (((!(blockName.Substring(3).ToUpper() == "MACHINEHEARTBEATSIGNAL") && !(blockName.Substring(4).ToUpper() == "MACHINEHEARTBEATSIGNAL") && !(blockName.Substring(3).ToUpper() == "EQUIPMENTALIVE")) || blockName.Substring(4).ToUpper() == "EQUIPMENTALIVE") && !flag)
					{
						WritePLCLogData(message);
					}
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("OnEventReceived Block Nmae : {0} , Error Message : {1}",blockName,ex.Message));
				}
			}
		}

		private void mNetProxy_OnTraceDataReceived(object sender, MessageData<PLCMessageBody> message)
		{
			lock (m_objBoardTraceFuncLock)
			{
				try
				{
					AddTraceData(message);
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("OnSVEventReceived, Error Message : {1}", ex.Message));
				}
			}
		}

		private void mNetProxy_OnRGADataReceived(object sender, MessageData<PLCMessageBody> message)
		{
			lock (m_objBoardRGAFuncLock)
			{
				try
				{
					AddRGAData(message);
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("OnRGADataReceived, Error Message : {1}", ex.Message));
				}
			}
		}

		private void mNetProxy_OnErrorMessage(string message)
		{
			this.OnOPCall(message);
		}

		private void mNetProxy_OnTimeOutReceived(object sender, MessageData<PLCMessageBody> message)
		{
			try
			{
				message.MessageType = "MNet";
				message.Transaction = MakeNewTransactionNo();
				AddEvent(message);
				WritePLCLogData(message);
			}
			catch (Exception message2)
			{
				logger.Error(message2);
			}
		}

        //private void mNetProxy_OnLinkSignalScanReceived(object sender, Dictionary<string, string> signal, Block block)
        //{
        //    if (m_linkSignalTestTool != null && m_linkSignalTestTool.LinkSignalChartViewer != null)
        //    {
        //        m_linkSignalTestTool.LinkSignalChartViewer.DrawLinkSignal(block, signal);
        //    }
        //}

		private void mqProxy_OnEIPRequestReceived(object sender, MessageData<EIPMessageBody> message)
		{
			lock (eipObject)
			{
				try
				{
					if (message == null)
					{
						logger.Error("OnEIPRequestReceived : Data is null");
					}
					else
					{
						if (mEipProxy == null)
						{
							message.ReturnCode = 1;
							message.ReturnMessage = "EIP Not Connection";
							AddEvent(message);
						}
						if (!mEipProxy.Connection && message.MessageName != "ConnectionCheck")
						{
							message.MessageName = ((message.MessageName == "ReadRequest") ? "ReadReply" : "WriteReply");
							message.ReturnCode = 1;
							message.ReturnMessage = "EIP Not Connection";
							AddEvent(message);
							logger.Error(string.Format("OnEIPRequestReceived : {0} - EIP Not Connection",message.MessageName));
						}
						else
						{
							WritePLCLogData(message);
							switch (message.MessageName)
							{
							case "ReadRequest":
								message.MessageName = "ReadReply";
								mEipProxy.ReadData(message.MessageBody);
								AddEvent(message);
								WritePLCLogData(message);
								break;
							case "WriteRequest":
								message.MessageName = "WriteReply";
								if (!mEipProxy.WriteData(message.MessageBody))
								{
									message.ReturnCode = 1;
									message.ReturnMessage = "Write fail";
								}
								else
								{
									message.ReturnCode = 0;
								}
								AddEvent(message);
								WritePLCLogData(message);
								break;
							case "ConnectionCheck":
								if (!mEipProxy.Connection)
								{
									message.MessageName = "Disconnection";
								}
								else
								{
									message.MessageName = "Connection";
								}
								message.ReturnCode = 0;
								AddEvent(message);
								WritePLCLogData(message);
								break;
							default:
								logger.Error("OnEIPRequestReceived : invalid MessageName..");
								return;
							}
							logger.Info(string.Format("OnEIPRequestReceived : {0} - {1}",message.MessageName,message.MachineName));
						}
					}
				}
				catch (Exception message2)
				{
					logger.Error(message2);
				}
			}
		}

		private void eipProxy_OnEventReceived(object sender, MessageData<EIPMessageBody> message)
		{
			lock (m_objEIPEventFuncLock)
			{
				try
				{
					message.MessageType = "EIP";
					message.Transaction = MakeNewTransactionNo();
					AddEvent(message);
					WritePLCLogData(message);
					//logger.Info("OnEventReceived");
                    logger.Info("OnEventReceived");
				}
				catch (Exception message2)
				{
					logger.Error(message2);
				}
			}
		}

		private void eipProxy_OnSVEventReceived(object sender, MessageData<EIPMessageBody> message)
		{
			lock (m_objEIPTraceDataFuncLock)
			{
				try
				{
					AddTraceData(message);
				}
				catch (Exception message2)
				{
					logger.Error(message2);
				}
			}
		}

		private string MakeNewTransactionNo()
		{
			if (m_iTransactionNo > 10000)
			{
				m_iTransactionNo = 1;
			}
			return string.Format("{0}_{1}", DateTime.Now.ToString("yyy-MM-dd HH:mm-ss"), m_iTransactionNo++);
		}

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		private void InitFileCheck()
		{
			if (!File.Exists(m_strInitFilePath))
			{
				DefailtInitFileMake();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(255);
				int privateProfileString = GetPrivateProfileString("LicenseCheck", "USB Kind", "ElecKey", stringBuilder, 255, m_strInitFilePath);
				USBKind = stringBuilder.ToString();
				privateProfileString = GetPrivateProfileString("LicenseCheck", "ElecKey ModuleID", "0", stringBuilder, 255, m_strInitFilePath);
				ElecKeyModuleID = stringBuilder.ToString();
				privateProfileString = GetPrivateProfileString("EQUIPMENTINFO", "Name", "", stringBuilder, 255, m_strInitFilePath);
				Globalproperties.Instance.EquipmentName = stringBuilder.ToString();
				privateProfileString = GetPrivateProfileString("EQUIPMENTINFO", "IPAddress", "", stringBuilder, 255, m_strInitFilePath);
				Globalproperties.Instance.IPAddress = stringBuilder.ToString();
			}
		}

		private void DefailtInitFileMake()
		{
			WritePrivateProfileString("LicenseCheck", "USB Kind", "ElecKey", m_strInitFilePath);
			WritePrivateProfileString("LicenseCheck", "ElecKey ModuleID", "0", m_strInitFilePath);
		}

		public void Init(string filePath)
		{
            resourceConfig = false;
			XmlDocument xmlDocument = new XmlDocument();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(EQPIOConfig));

            UseMQ = false;
            UseBoard = false;
            UseEIP = false;
            UseEthernet = false;
			try
			{
                //xmlDocument.Load("../EQPIO/Config/IOConConfig.xml");
                if(!File.Exists(filePath))
                {
                    logger.ErrorFormat("Not file driver config file! [{0}]",filePath);
                }
                xmlDocument.Load(filePath);
				//XmlNode xmlNode = xmlDocument.SelectSingleNode("IOConConfig");
                XmlNode xmlNode = xmlDocument.SelectSingleNode("EQPIOConfig");
			    var	m_EQPIOConfig = (EQPIOConfig)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                m_IOConConfig = m_EQPIOConfig;
				Driver[] driver = m_IOConConfig.Driver;
				foreach (Driver driver2 in driver)
				{
					switch (driver2.name)
					{
					case "MQ":
						UseMQ = driver2.ConnectionInfo.use;
						break;
					case "MelsecBoard":
						UseBoard = driver2.ConnectionInfo.use;
						break;
					case "MelsecEthernet":
						UseEthernet = driver2.ConnectionInfo.use;
						break;
					case "EIP":
						UseEIP = driver2.ConnectionInfo.use;
						break;
					}
				}
				m_tEventProcessing = new Thread(EventProcessingProc);
				m_tEventProcessing.Name = "EventProcessingProc";
				m_tEventProcessing.IsBackground = true;
				m_tEventProcessing.Start();
				m_tTraceDataProcessing = new Thread(TraceDataProcessingProc);
				m_tTraceDataProcessing.Name = "TraceDataProcessingProc";
				m_tTraceDataProcessing.IsBackground = true;
				m_tTraceDataProcessing.Start();
                /*
				m_tLicenseAlarmProcessing = new Thread(LicenseAlarmProc);
				m_tLicenseAlarmProcessing.Name = "LicenseAlarmProc";
				m_tLicenseAlarmProcessing.IsBackground = true;
				m_tLicenseAlarmProcessing.Start(); */
				m_tRGADataProcessing = new Thread(RGADataProcessingProc);
				m_tRGADataProcessing.Name = "RGADataProcessingProc";
				m_tRGADataProcessing.IsBackground = true;
				m_tRGADataProcessing.Start();
				InitFileCheck();
				logger.Info("Init ControlManager");
			}
			catch (Exception ex)
			{
				logger.Error("Init Error ControlManager : " + ex.StackTrace);
			}
		}

        public void Init()
        {
            /*
             * 改为使用嵌入式资源
             */ 
            string embeddedResource = GetResource.GetEmbeddedResource("EQPConfig.xml") as string;

            XmlDataDocument doc = new XmlDataDocument();
            doc.LoadXml(embeddedResource);
            Init(doc);

           // Init(@"../IOCon/Config/IOConConfig.xml");
        }


        public void Init(XmlDataDocument doc)
        {

            UseMQ = false;
            UseBoard = false;
            UseEIP = false;
            UseEthernet = false;

            XmlDocument xmlDocument = doc;
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EQPIOConfig));
            try
            {
                //xmlDocument.Load("../EQPIO/Config/IOConConfig.xml");
                //XmlNode xmlNode = xmlDocument.SelectSingleNode("IOConConfig");
                XmlNode xmlNode = xmlDocument.SelectSingleNode("EQPIOConfig");
                var m_EQPIOConfig = (EQPIOConfig)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                m_IOConConfig = m_EQPIOConfig;
                Driver[] driver = m_IOConConfig.Driver;
                foreach (Driver driver2 in driver)
                {
                    switch (driver2.name)
                    {
                        case "MQ":
                            UseMQ = driver2.ConnectionInfo.use;
                            break;
                        case "MelsecBoard":
                            UseBoard = driver2.ConnectionInfo.use;
                            break;
                        case "MelsecEthernet":
                            UseEthernet = driver2.ConnectionInfo.use;
                            break;
                        case "EIP":
                            UseEIP = driver2.ConnectionInfo.use;
                            break;
                    }
                }
                m_tEventProcessing = new Thread(EventProcessingProc);
                m_tEventProcessing.Name = "EventProcessingProc";
                m_tEventProcessing.IsBackground = true;
                m_tEventProcessing.Start();
                m_tTraceDataProcessing = new Thread(TraceDataProcessingProc);
                m_tTraceDataProcessing.Name = "TraceDataProcessingProc";
                m_tTraceDataProcessing.IsBackground = true;
                m_tTraceDataProcessing.Start();
                /*
                m_tLicenseAlarmProcessing = new Thread(LicenseAlarmProc);
                m_tLicenseAlarmProcessing.Name = "LicenseAlarmProc";
                m_tLicenseAlarmProcessing.IsBackground = true;
                m_tLicenseAlarmProcessing.Start();*/
                m_tRGADataProcessing = new Thread(RGADataProcessingProc);
                m_tRGADataProcessing.Name = "RGADataProcessingProc";
                m_tRGADataProcessing.IsBackground = true;
                m_tRGADataProcessing.Start();
                InitFileCheck();
                logger.Info("Init ControlManager");
            }
            catch (Exception ex)
            {
                logger.Error("Init Error ControlManager : " + ex.StackTrace);
            }

        }

		public void Close()
		{
			if (mNetProxy != null)
			{
				mNetProxy.Close();
				mNetConnectionInfo.MessageType = "MNet";
				mNetConnectionInfo.MessageName = "Disconnection";
				mNetConnectionInfo.MachineName = "MNet";
				mNetConnectionInfo.Transaction = MakeNewTransactionNo();
				mNetConnectionInfo.MessageBody = new PLCMessageBody();
				mNetConnectionInfo.ReturnCode = 0;
                if (mqProxy != null)
                {
                    mqProxy.WriteSend(mNetConnectionInfo, null);
                }
                if(iEQPEventHandler!=null)
                {
                    iEQPEventHandler.EQPEventProcess(mNetConnectionInfo);
                }
			}
			if (mProtocolProxy != null)
			{
				mProtocolProxy.Close();
                //mNetConnectionInfo.MessageType = "mProtocol";
                mNetConnectionInfo.MessageType = "MelsecEthernet";
				mNetConnectionInfo.MessageName = "Disconnection";
				mNetConnectionInfo.MachineName = "mProtocol";
				mNetConnectionInfo.Transaction = MakeNewTransactionNo();
				mNetConnectionInfo.MessageBody = new PLCMessageBody();
				mNetConnectionInfo.ReturnCode = 0;
                if(mqProxy!=null)
                { 
				 mqProxy.WriteSend(mNetConnectionInfo, null);
                }
                if (iEQPEventHandler != null)
                {
                    iEQPEventHandler.EQPEventProcess(mNetConnectionInfo);
                }
			}
			if (mEipProxy != null)
			{
				mEipProxy.Close();
				eipConnectionInfo.MessageType = "EIP";
				eipConnectionInfo.MessageName = "Disconnection";
				eipConnectionInfo.MachineName = "EIP";
				eipConnectionInfo.Transaction = MakeNewTransactionNo();
				eipConnectionInfo.MessageBody = new EIPMessageBody();
				eipConnectionInfo.ReturnCode = 0;
				mqProxy.WriteSend(eipConnectionInfo, null);
			}
			if (mqProxy != null)
			{
				mqProxy.Close();
			}
			if (m_tEventProcessing != null && m_tEventProcessing.IsAlive)
			{
				m_tEventProcessing.Abort();
			}
			if (m_tTraceDataProcessing != null && m_tTraceDataProcessing.IsAlive)
			{
				m_tTraceDataProcessing.Abort();
			}
			if (m_tLicenseAlarmProcessing != null && m_tLicenseAlarmProcessing.IsAlive)
			{
				m_tLicenseAlarmProcessing.Abort();
			}
			if (m_tRGADataProcessing != null && m_tRGADataProcessing.IsAlive)
			{
				m_tRGADataProcessing.Abort();
			}
		}

		public void InitMQ()
		{
			mqProxy = new MQProxy();
			mqProxy.OnMelsecEthernetRequestReceived += mqProxy_OnMelsecEthernetRequestReceived;
			mqProxy.OnMelsecBoardRequestReceived += mqProxy_OnMNetRequestReceived;
			mqProxy.OnEIPRequestReceived += mqProxy_OnEIPRequestReceived;
			mqProxy.OnNetworkEvent += mqProxy_OnNetworkEvent;
			try
			{
				Driver[] driver = m_IOConConfig.Driver;
				int num = 0;
				Driver driver2;
				while (true)
				{
					if (num >= driver.Length)
					{
						return;
					}
					driver2 = driver[num];
					if (driver2.name == "MQ")
					{
						break;
					}
					num++;
				}
				mqProxy.InitMQServer(driver2);
				mqProxy.Open();
				logger.Info("Init MQ Proxy");
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("InitMQ Error : {0}",ex.Message));
			}
		}

		public bool InitMNet(ref string errorMsg)
		{
			mNetProxy = new MNetProxy();
			try
			{
				Driver[] driver = m_IOConConfig.Driver;
				foreach (Driver driver2 in driver)
				{
					if (driver2.name == "MelsecBoard")
					{
						//if (mNetProxy.InitXml(driver2, ref errorMsg))
                        //改为使用内部资源
                        if(resourceConfig)
                        {
                            if (mNetProxy.InitByInnerResource(ref errorMsg))
                            {
                                break;
                            }
                            return false;
                        }
                        else
                        {
                            if (mNetProxy.InitXml(driver2, ref errorMsg))
                            {
                                break;
                            }
                            return false;
                        }
                       
					}
				}
				if (!mNetProxy.InitMNet())
				{
					mNetProxy = null;
					errorMsg = "Open fail : Need MelsecCard Check, MapConfig Check";
					logger.Error(errorMsg);
					return false;
				}
				mNetConnectionInfo.MessageName = "Connection";
				mNetConnectionInfo.MachineName = "MNet";
				mNetConnectionInfo.MessageType = "MNet";
				mNetConnectionInfo.Transaction = MakeNewTransactionNo();
				mNetConnectionInfo.MessageBody = new PLCMessageBody();
				mNetConnectionInfo.ReturnCode = 0;
				mNetConnectionInfo.ReturnMessage = string.Empty;
                if(mqProxy!=null)
                {
                    mqProxy.WriteSend(mNetConnectionInfo, null);
                }
				
				logger.Info("==MNet Connection==");
                if(this.OnConnected!=null)
                {
                    this.OnConnected(this);
                }
                logger.Info("MNet connected!");
				mNetProxy.OnEventReceived += mNetProxy_OnEventReceived;
				mNetProxy.OnTimeOutReceived += mNetProxy_OnTimeOutReceived;
				mNetProxy.OnSVEventReceived += mNetProxy_OnTraceDataReceived;
				mNetProxy.OnRGADataReceived += mNetProxy_OnRGADataReceived;
				mNetProxy.OnErrorMessage += mNetProxy_OnErrorMessage;
                //mNetProxy.OnLinkSignalScanReceived += mNetProxy_OnLinkSignalScanReceived;
				mNetProxy.InitMNetEvent();
				mNetProxy.MNetStart();
				return true;
			}
			catch (Exception ex)
			{
                logger.Error(ex.StackTrace);
				errorMsg = "InitMNet Error";
				logger.Error(string.Format("InitMNet Error : {0}",ex.Message));
				return false;
			}
		}

		public bool InitMEthernet(ref string errorMsg)
		{
			if (mProtocolProxy == null)
			{
				mProtocolProxy = new MProtocolProxy();
				mProtocolProxy.OnConnected += mProtocolProxy_OnConnected;
				mProtocolProxy.OnDisconnected += mProtocolProxy_OnDisconnected;
				mProtocolProxy.OnEventReceived += mProtocolProxy_OnEventReceived;
				mProtocolProxy.OnFDCEventReceived += mProtocolProxy_OnTraceDataReceived;
				mProtocolProxy.OnRGADataReceived += mProtocolProxy_OnRGADataReceived;
                //mProtocolProxy.OnLinkSignalScanReceived += mProtocolProxy_OnLinkSignalScanReceived;
				mProtocolProxy.OnVirtualEQPEventReceived += mProtocolProxy_OnVirtualEQPEventReceived;
				try
				{
					Driver[] driver = m_IOConConfig.Driver;
					foreach (Driver driver2 in driver)
					{
						if (driver2.name == "MelsecEthernet")
						{
                            /* 改为内嵌入资源
                             * 
							if (mProtocolProxy.InitXml(driver2))
							{
								break;
							} */

                            if(resourceConfig)
                            {
                                if (mProtocolProxy.InitByInnerResource())
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (mProtocolProxy.InitXml(driver2))
                                {
                                    break;
                                }
                            }
                            
							mProtocolProxy = null;
							errorMsg = "Xml Init Error";
							logger.Error(errorMsg);
							return false;
						}
					}
					if (mProtocolProxy.InitMEthernet())
					{
						mProtocolProxy.Connect();
						return true;
					}
					mProtocolProxy = null;
					errorMsg = "Ethernet Init Error";
					logger.Error("errorMsg");
					return false;
				}
				catch (Exception ex)
				{
					errorMsg = "InitMEthernet Error";
					logger.Error(string.Format("InitMEthernet Error : {0}",ex.Message));
					return false;
				}
			}
			errorMsg = "Already connected";
			return false;
		}

		public bool InitEIP(ref string errorMsg)
		{
			mEipProxy = new EIPProxy();
			bool result = false;
			try
			{
				if (m_IOConConfig.Driver != null)
				{
					Driver[] driver = m_IOConConfig.Driver;
					foreach (Driver driver2 in driver)
					{
						if (driver2.name == "EIP")
						{
							if (mEipProxy.Init(driver2.ConnectionInfo.path))
							{
								eipConnectionInfo.MessageType = "EIP";
								eipConnectionInfo.MessageName = "Connection";
								eipConnectionInfo.MachineName = "EIP";
								eipConnectionInfo.Transaction = MakeNewTransactionNo();
								eipConnectionInfo.MessageBody = new EIPMessageBody();
								eipConnectionInfo.ReturnCode = 0;
								eipConnectionInfo.ReturnMessage = string.Empty;
								mqProxy.WriteSend(eipConnectionInfo, null);
								result = true;
								logger.Info("==EIP Connection==");
								this.OnConnected(this);
								mEipProxy.OnEventReceived += eipProxy_OnEventReceived;
								mEipProxy.OnSVEventReceived += eipProxy_OnSVEventReceived;
								mEipProxy.Start();
							}
							else
							{
								errorMsg = "Open fail : EIP Check";
								result = false;
							}
						}
					}
					return result;
				}
				logger.Error("IOConConfig.Driver is null");
				return false;
			}
			catch (Exception ex)
			{
				errorMsg = "InitEIP Error";
				logger.Error(string.Format("InitEIP Error : {0}",ex.Message));
				return false;
			}
		}

        //public void LinkSignalChangeRequest(string block, string item, bool bOn)
        //{
        //    MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
        //    PLCMessageBody pLCMessageBody = new PLCMessageBody();
        //    Dictionary<string, string> dictionary = new Dictionary<string, string>();
        //    pLCMessageBody.WriteDataList.Add(block, new Dictionary<string, string>());
        //    dictionary.Add(item, bOn ? "1" : "0");
        //    pLCMessageBody.WriteDataList[block] = dictionary;
        //    pLCMessageBody.EventName = "LinkSignalChangeRequest";
        //    messageData.MessageBody = pLCMessageBody;
        //    messageData.MessageType = "MNet";
        //    messageData.MachineName = "L01";
        //    messageData.Transaction = "11";
        //    if (mProtocolProxy != null)
        //    {
        //        mProtocolProxy.WriteData(messageData);
        //    }
        //    else if (mNetProxy != null)
        //    {
        //        mNetProxy.WriteData(messageData);
        //    }
        //}
        public void LinkSignalChangeRequest(string block, string item, bool bOn)
        {
            EQPIO.MessageData.MessageData<PLCMessageBody> data = new EQPIO.MessageData.MessageData<PLCMessageBody>();
            PLCMessageBody body = new PLCMessageBody();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            body.WriteDataList.Add(block, new Dictionary<string, string>());
            dictionary.Add(item, bOn ? "1" : "0");
            body.WriteDataList[block] = dictionary;
            body.EventName = "LinkSignalChangeRequest";
            data.MessageBody = body;
            data.MessageType = "MNet";
            data.MachineName = "L01";
            data.Transaction = "11";
            if (this.mProtocolProxy != null)
            {
                this.mProtocolProxy.WriteData(data);
            }
            else if (this.mNetProxy != null)
            {
                this.mNetProxy.WriteData(data);
            }
        }
        
		public void VirtualEQPStart(string plcmapPath)
		{
			if (ProtocolProxy != null)
			{
                //m_virtualEQP = new vEQP(plcmapPath);
                //m_virtualEQP.OnVirtualEQPRequest += m_virtualEQP_OnVirtualEQPRequest;
                //PLCsimControlLibrary.DeviceMemory aMemory = new PLCsimControlLibrary.DeviceMemory();
                //m_virtualEQP.ShowEQP(aMemory);
			}
		}

		private void m_virtualEQP_OnVirtualEQPRequest(object sender, MessageData<PLCMessageBody> message)
		{
			message.MachineName = "Virtual";
			mqProxy_OnMelsecEthernetRequestReceived(this, message);
			if (!(message.MessageName == ""))
			{
				return;
			}
		}

		public MessageData<PLCMessageBody> VirtualMQRequestReceived(MessageData<PLCMessageBody> data)
		{
			mqProxy_OnMelsecEthernetRequestReceived(this, data);
			return data;
		}

		private void WritePLCLogData(object message)
		{
			MessageData<PLCMessageBody> messageData = null;
			MessageData<EIPMessageBody> messageData2 = null;
			if (message is MessageData<PLCMessageBody>)
			{
                
				messageData = (message as MessageData<PLCMessageBody>);
                if (messageData.MessageBody.EventName!=null&&messageData.MessageBody.EventName.ToUpper().Contains("EQUIPMENTALIVE"))
                {
                    return;
                }
				PLCWriteLog(messageData);
			}
			else if (message is MessageData<EIPMessageBody>)
			{
				messageData2 = (message as MessageData<EIPMessageBody>);
				EIPWriteLog(messageData2);
			}
			else
			{
				logger.Error("message is UnKnown");
			}
		}

		private void PLCWriteLog(MessageData<PLCMessageBody> data)
		{
            //StringCollection stringCollection = new StringCollection();
            //stringCollection.Add("\n");
            //stringCollection.Add(string.Format("==========================================\n"));
            //stringCollection.Add(string.Format("Message Name     : {data.MessageName} \n");
            //stringCollection.Add(string.Format("Transaction Name : {data.Transaction} \n");
            //stringCollection.Add(string.Format("MessageType      : {data.MessageType} \n");
            //stringCollection.Add(string.Format("MachineName      : {data.MachineName} \n");
            //stringCollection.Add(string.Format("Event Time       : {data.EventTime} \n");
            //stringCollection.Add(string.Format("Event Name       : {data.MessageBody.EventName} \n");
            //stringCollection.Add(string.Format("Event Value      : {data.MessageBody.EventValue} \n");
            //stringCollection.Add(string.Format("Return Code      : {data.ReturnCode} \n");
            //stringCollection.Add(string.Format("Return Message   : {data.ReturnMessage} \n");
            StringCollection stringCollection = new StringCollection();
            stringCollection.Add("\n");
            stringCollection.Add(string.Format("==========================================\n", new object[0]));
            stringCollection.Add(string.Format("Message Name     : {0} \n", data.MessageName));
            stringCollection.Add(string.Format("Transaction Name : {0} \n", data.Transaction));
            stringCollection.Add(string.Format("MessageType      : {0} \n", data.MessageType));
            stringCollection.Add(string.Format("MachineName      : {0} \n", data.MachineName));
            stringCollection.Add(string.Format("Event Time       : {0} \n", data.EventTime));
            stringCollection.Add(string.Format("Event Name       : {0} \n", data.MessageBody.EventName));
            stringCollection.Add(string.Format("Event Value      : {0} \n", data.MessageBody.EventValue));
            stringCollection.Add(string.Format("Return Code      : {0} \n", data.ReturnCode));
            stringCollection.Add(string.Format("Return Message   : {0} \n", data.ReturnMessage));
			Dictionary<string, Dictionary<string, string>>.KeyCollection.Enumerator enumerator;
			Dictionary<string, string>.KeyCollection.Enumerator enumerator2;
			if (data.MessageBody.ReadDataList.Count > 0)
			{
				enumerator = data.MessageBody.ReadDataList.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						stringCollection.Add(string.Format("Read BlockName : {0} \n",current));
						stringCollection.Add("==Read Item List== \n");
						if (data.MessageBody.ReadDataList[current] != null)
						{
							enumerator2 = data.MessageBody.ReadDataList[current].Keys.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									string current2 = enumerator2.Current;
									stringCollection.Add(string.Format("Read ItemName : {0}, Value : {1} \n",current2,data.MessageBody.ReadDataList[current][current2]));
								}
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (data.MessageBody.WriteDataList.Count > 0)
			{
				enumerator = data.MessageBody.WriteDataList.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current3 = enumerator.Current;
						stringCollection.Add(string.Format("Write BlockName : {0} \n",current3));
						stringCollection.Add("==Write Item List== \n");
						if (data.MessageBody.WriteDataList[current3] != null)
						{
							enumerator2 = data.MessageBody.WriteDataList[current3].Keys.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									string current2 = enumerator2.Current;
									stringCollection.Add(string.Format("Write ItemName : {0}, Value : {1} \n",current2,data.MessageBody.WriteDataList[current3][current2]));
								}
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			logger.Info(stringCollection);
		}

		private void PLCWriteLogJSON(object message)
		{
			IMessageParser messageParser = new JsonMessageParser<MessageData<PLCMessageBody>>();
			byte[] bytes = messageParser.ObjectToByteArray(message);
			logger.Info(Encoding.UTF8.GetString(bytes));
		}

        //private void EIPWriteLog(MessageData<EIPMessageBody> data)
        //{
        //    StringCollection stringCollection = new StringCollection();
        //    stringCollection.Add("\n");
        //    stringCollection.Add(string.Format("========================================== \n"));
        //    stringCollection.Add(string.Format("Message Name     : {data.MessageName} \n");
        //    stringCollection.Add(string.Format("Transaction Name : {data.Transaction} \n");
        //    stringCollection.Add(string.Format("MessageType      : {data.MessageType} \n");
        //    stringCollection.Add(string.Format("MachineName      : {data.MachineName} \n");
        //    stringCollection.Add(string.Format("Event Time       : {data.EventTime} \n");
        //    stringCollection.Add(string.Format("Event Name       : {data.MessageBody.EventName} \n");
        //    stringCollection.Add(string.Format("Event Value      : {data.MessageBody.EventValue} \n");
        //    stringCollection.Add(string.Format("Return Code      : {data.ReturnCode} \n");
        //    stringCollection.Add(string.Format("Return Message   : {data.ReturnMessage} \n");
         private void EIPWriteLog(EQPIO.MessageData.MessageData<EIPMessageBody> data)
        {
            
            StringCollection stringCollection = new StringCollection();
            stringCollection.Add("\n");
            stringCollection.Add(string.Format("========================================== \n", new object[0]));
            stringCollection.Add(string.Format("Message Name     : {0} \n", data.MessageName));
            stringCollection.Add(string.Format("Transaction Name : {0} \n", data.Transaction));
            stringCollection.Add(string.Format("MessageType      : {0} \n", data.MessageType));
            stringCollection.Add(string.Format("MachineName      : {0} \n", data.MachineName));
            stringCollection.Add(string.Format("Event Time       : {0} \n", data.EventTime));
            stringCollection.Add(string.Format("Event Name       : {0} \n", data.MessageBody.EventName));
            stringCollection.Add(string.Format("Event Value      : {0} \n", data.MessageBody.EventValue));
            stringCollection.Add(string.Format("Return Code      : {0} \n", data.ReturnCode));
            stringCollection.Add(string.Format("Return Message   : {0} \n", data.ReturnMessage));
			Dictionary<string, Dictionary<string, string>>.KeyCollection.Enumerator enumerator;
			Dictionary<string, string>.KeyCollection.Enumerator enumerator2;
			if (data.MessageBody.ReadDataList.Count > 0)
			{
				enumerator = data.MessageBody.ReadDataList.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						stringCollection.Add(string.Format("Read BlockName : {0} \n",current));
						stringCollection.Add("==Read Item List== \n");
						if (data.MessageBody.ReadDataList[current] != null)
						{
							enumerator2 = data.MessageBody.ReadDataList[current].Keys.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									string current2 = enumerator2.Current;
									stringCollection.Add(string.Format("Read ItemName : {0}, Value : {1} \n",current2,data.MessageBody.ReadDataList[current][current2]));
								}
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (data.MessageBody.WriteDataList.Count > 0)
			{
				enumerator = data.MessageBody.WriteDataList.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current3 = enumerator.Current;
						stringCollection.Add(string.Format("Write BlockName : {0} \n",current3));
						stringCollection.Add("==Write Item List== \n");
						if (data.MessageBody.WriteDataList[current3] != null)
						{
							enumerator2 = data.MessageBody.WriteDataList[current3].Keys.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									string current2 = enumerator2.Current;
									stringCollection.Add(string.Format("Write ItemName : {0}, Value : {1} \n",current2,data.MessageBody.WriteDataList[current3][current2]));
								}
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			logger.Info(stringCollection);
		}

		private void AddEvent(object evt)
		{
			lock (m_objEventQLock)
			{
				if (m_EventQ.Count < 6000)
				{
					m_EventQ.Enqueue(evt);
				}
				else
				{
					logger.Error("Too many events waiting for MQ Send.");
				}
			}
		}

		private void AddTraceData(object data)
		{
			lock (m_objTraceDataQLock)
			{
				if (m_TraceDataQ.Count < 6000)
				{
					m_TraceDataQ.Enqueue(data);
				}
				else
				{
					logger.Error("Too many datas[TRACE] waiting for MQ Send.");
				}
			}
		}

		private void AddRGAData(object data)
		{
			lock (m_objRGADataQLock)
			{
				if (m_RGADataQ.Count < 6000)
				{
					m_RGADataQ.Enqueue(data);
				}
				else
				{
					logger.Error("Too many datas[RGA] waiting for MQ Send.");
				}
			}
		}

		private void AddAlarm(object evt)
		{
			lock (m_objLicenseAlarmQLock)
			{
				if (m_LicenseAlarmQ.Count < 6000)
				{
					m_LicenseAlarmQ.Enqueue(evt);
				}
				else
				{
					logger.Error("Too many Alarm waiting for MQ Send.");
				}
			}
		}

		public void SendAlarm(object alarm)
		{
			try
			{
				AddAlarm(alarm);
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		private void EventProcessingProc()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					lock (m_objEventQLock)
					{
						if (m_EventQ.Count > 0)
						{
							logger.Info(string.Format("EventQ.Dequeue! As soon as mqProxy will be Sending, Remind EventQ Size : {0}",m_EventQ.Count));
							object obj = m_EventQ.Dequeue();
							if (obj != null && mqProxy != null)
							{
								mqProxy.SendEvent(obj);
							}
                            if(iEQPEventHandler!=null&&obj!=null) //cxm
                            {
                                iEQPEventHandler.EQPEventProcess(obj);
                            }
						}
					}
					Thread.Sleep(m_iEventDequeueInterval);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("EventProcessingProc Error : {0}",ex.Message));
				Thread.ResetAbort();
			}
		}

		private void TraceDataProcessingProc()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					lock (m_objTraceDataQLock)
					{
						if (m_TraceDataQ.Count > 0)
						{
							object obj = m_TraceDataQ.Dequeue();
							if (obj != null)
							{
								mqProxy.SendTraceData(obj);
							}
                            if (EQPTraceDataHandler != null && obj != null) //cxm
                            {
                                EQPTraceDataHandler.EQPTraceDateProcess(obj);
                            }
						}
					}
					Thread.Sleep(m_iTraceDequeueInterval);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("TraceDataProcessingProc Error : {0}",ex.Message));
				Thread.ResetAbort();
			}
		}

		private void LicenseAlarmProc()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					lock (m_objLicenseAlarmQLock)
					{
						if (m_LicenseAlarmQ.Count > 0)
						{
							object obj = m_LicenseAlarmQ.Dequeue();
							if (obj != null)
							{
								mqProxy.SendLicenseAlarm(obj);
							}
						}
					}
					Thread.Sleep(m_iLicenseAlarmDequeueInterval);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("LicenseAlarmProc Error : {0}",ex.Message));
				Thread.ResetAbort();
			}
		}

		private void RGADataProcessingProc()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					lock (m_objRGADataQLock)
					{
						if (m_RGADataQ.Count > 0)
						{
							object obj = m_RGADataQ.Dequeue();
							if (obj != null)
							{
								mqProxy.SendRGAData(obj);
							}
						}
					}
					Thread.Sleep(m_iRGADequeueInterval);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("TraceDataProcessingProc Error : {0}",ex.Message));
				Thread.ResetAbort();
			}
		}

		public void TestWrite()
		{
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody pLCMessageBody = new PLCMessageBody();
			pLCMessageBody.WriteDataList.Add("L2_W_JobDataforDownstreamBlock1", new Dictionary<string, string>());
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("GlassSizeCode", "5");
			dictionary.Add("FirstLotGlassCode", "1");
			dictionary.Add("LastLotGlass", "0");
			dictionary.Add("DummyGlass", "1");
			pLCMessageBody.WriteDataList["L2_W_JobDataforDownstreamBlock1"] = dictionary;
			messageData.MessageName = "WriteRequest";
			messageData.MessageBody = pLCMessageBody;
			messageData.MessageType = "MNet";
			messageData.MachineName = "1,2";
			messageData.Transaction = "11";
			if (mProtocolProxy != null)
			{
				mProtocolProxy.WriteData(messageData);
			}
		}

        public Transaction getTransation(string netType,string local)
        {
            if (netType.Trim() == "MelsecEthernet" )
            {
                if (UseEthernet)
                {
                    return mProtocolProxy.getTrx(local);
                }
            }
            if (netType.Trim() == "MNet" )
            {
                if (UseBoard)
                {
                    return mNetProxy.getTrx(local);
                }
            }

            return null;
        }

        public BlockMap getBlockMap(string netType, string local)
        {
            if (netType.Trim() == "MelsecEthernet")
            {
                if (UseEthernet)
                {
                    return mProtocolProxy.getBlockMap(local);
                }
            }
            if (netType.Trim() == "MNet")
            {
                if (UseBoard)
                {
                    return mNetProxy.getBlockMap(local);
                }
            }

            return null;
        }

        public MNetProxy getMNetProxy()
        {
            return mNetProxy;
        }

        public MProtocolProxy getMProtocolProxy()
        {
            return mProtocolProxy;
        }

        public EIPProxy getEIPProxy()
        {
            return mEipProxy;
        }

        public MQProxy getMQProxy()
        {
            return mqProxy;
        }

        public void Dispose()
        {
            Close();
        }


        public void RequestForServer(MessageData<PLCMessageBody> msg)
        {
            if(msg.MessageType.ToUpper().Contains("MNET")||msg.MessageType.ToUpper().Contains("BOARD"))
            {
                mqProxy_OnMNetRequestReceived(this, msg);
            }
            if(msg.MessageType.ToUpper().Contains("ETHERNET"))
            {
                mqProxy_OnMelsecEthernetRequestReceived(this, msg);
            }
        }
    }
}
