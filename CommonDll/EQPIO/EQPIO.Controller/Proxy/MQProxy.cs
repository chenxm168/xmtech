using EQPIO.Common;
using EQPIO.MessageData;
using EQPIO.RabbitMQInterface;
using EQPIO.RabbitMQInterface.Parser;
using EQPIO.RabbitMQInterface.Parser.Impl;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Controller.Proxy
{
	public class MQProxy
	{
		public delegate void MelsecBoardRequestReceivedEventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void MelsecEthernetRequestReceivedEventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void MelsecEIPRequestReceivedEventHandler(object sender, MessageData<EIPMessageBody> message);

		public delegate void MelsecNetworkEventHandler(MessageData<PLCMessageBody> message);

		private ILog logger = LogManager.GetLogger(typeof(MQProxy));

		private Objects mqObj;

		private Dictionary<string, MQInterface> m_dicEventMQInterfaceList = new Dictionary<string, MQInterface>();

		private Dictionary<string, MQInterface> m_dicTraceMQInterfaceList = new Dictionary<string, MQInterface>();

		private Dictionary<string, MQInterface> m_dicLicenseMQInterfaceList = new Dictionary<string, MQInterface>();

		private Dictionary<string, MQInterface> m_dicRGAMQInterfaceList = new Dictionary<string, MQInterface>();

		private Thread networkThread;

		private object mqWriteSendLock = new object();

		private object m_objSendEventLock = new object();

		private object m_objTraceDataLock = new object();

		private object m_objLicenseLock = new object();

		private object m_objRGADataLock = new object();

		private Queue<MessageData<PLCMessageBody>> m_qEventQ = new Queue<MessageData<PLCMessageBody>>();

		public event MelsecBoardRequestReceivedEventHandler OnMelsecBoardRequestReceived;

		public event MelsecEthernetRequestReceivedEventHandler OnMelsecEthernetRequestReceived;

		public event MelsecEIPRequestReceivedEventHandler OnEIPRequestReceived;

		public event MelsecNetworkEventHandler OnNetworkEvent;

		public void InitMQServer(Driver driver)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(driver.ConnectionInfo.path);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Objects));
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Objects");
			mqObj = (Objects)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
			Property[] property = mqObj.Property;
			foreach (Property property2 in property)
			{
				if (property2.Value)
				{
					switch (property2.MessageType.ToUpper())
					{
					case "PLC":
					{
						IMessageParser messageParser = new JsonMessageParser<MessageData<PLCMessageBody>>();
						if (!m_dicEventMQInterfaceList.ContainsKey(property2.HostName))
						{
							m_dicEventMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
							m_dicEventMQInterfaceList[property2.HostName].OnMessageReceived += MQProxy_OnMessageReceived;
							logger.Info(string.Format("InitMQ PLC Server : {0} , {1}",property2.HostName,property2.HostIP));
						}
						break;
					}
					case "PLCFDC":
						if (!m_dicTraceMQInterfaceList.ContainsKey(property2.HostName))
						{
							IMessageParser messageParser = new JsonMessageParser<MessageData<PLCMessageBody>>();
							m_dicTraceMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
                            logger.Info(string.Format("InitMQ PLCFDC Server : {0} , {1}", property2.HostName, property2.HostIP));
						}
						break;
					case "EIP":
						if (!m_dicEventMQInterfaceList.ContainsKey(property2.HostName))
						{
							IMessageParser messageParser = new JsonMessageParser<MessageData<EIPMessageBody>>();
							m_dicEventMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
							m_dicEventMQInterfaceList[property2.HostName].OnMessageReceived += MQProxy_OnEIPMessageReceived;
							logger.Info(string.Format("InitMQ PLCEIP Server : {0} , {1}",property2.HostName,property2.HostIP));
						}
						break;
					case "EIPFDC":
						if (!m_dicTraceMQInterfaceList.ContainsKey(property2.HostName))
						{
							IMessageParser messageParser = new JsonMessageParser<MessageData<EIPMessageBody>>();
							m_dicTraceMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
							logger.Info(string.Format("InitMQ FDC Server : {0} , {1}",property2.HostName,property2.HostIP));
						}
						break;
					case "LIC":
						if (!m_dicLicenseMQInterfaceList.ContainsKey(property2.HostName))
						{
							IMessageParser messageParser = new JsonMessageParser<MessageData<PLCMessageBody>>();
							m_dicLicenseMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
							logger.Info(string.Format("InitMQ LIC Server : {0} , {1}",property2.HostName,property2.HostIP));
						}
						break;
					case "RGA":
						if (!m_dicRGAMQInterfaceList.ContainsKey(property2.HostName))
						{
							IMessageParser messageParser = new JsonMessageParser<MessageData<PLCMessageBody>>();
							m_dicRGAMQInterfaceList.Add(property2.HostName, new MQInterface(property2.HostIP, property2.ProducerExchange, property2.ProducerRoutingKey, property2.ConsumerExchange, property2.ConsumerRoutingKey, property2.ConsumerQueue, messageParser));
							logger.Info(string.Format("InitMQ RGA Server : {0} , {1}",property2.HostName,property2.HostIP));
						}
						break;
					}
				}
			}
		}

		private void MQProxy_OnEIPMessageReceived(object sender, object message)
		{
			MessageData<EIPMessageBody> messageData = message as MessageData<EIPMessageBody>;
			if (messageData == null)
			{
				logger.Error("RequestReceived : Data is null");
			}
			else
			{
				try
				{
					this.OnEIPRequestReceived(sender, messageData);
                    logger.Info(string.Format("[OnEIPMQMessageReceived] MessageName : {0}, Transaction : {1}", messageData.MessageName, messageData.Transaction));
				}
				catch (Exception message2)
				{
					logger.Error(message2);
				}
			}
		}

		private void MQProxy_OnMessageReceived(object sender, object message)
		{
			MessageData<PLCMessageBody> messageData = message as MessageData<PLCMessageBody>;
			if (messageData == null)
			{
				logger.Error("RequestReceived : Data is null");
			}
			else
			{
				try
				{
					this.OnMelsecEthernetRequestReceived(sender, messageData);
					this.OnMelsecBoardRequestReceived(sender, messageData);
                    logger.Info(string.Format("[OnMessageReceived] MessageName : {0}, Transaction : {1}", messageData.MessageName, messageData.Transaction));
				}
				catch (Exception message2)
				{
					logger.Error(message2);
				}
			}
		}

		public void Open()
		{
			Dictionary<string, MQInterface>.ValueCollection.Enumerator enumerator = m_dicEventMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Open();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicTraceMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Open();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicLicenseMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Open("eosuser", "eosuser");
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicRGAMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Open();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			networkThread = new Thread(NetworkThreadProc);
			networkThread.Name = "NetworkThreadProc";
			networkThread.IsBackground = true;
			networkThread.Start();
			logger.Info("MQ Open Completed");
		}

		public void Close()
		{
			Dictionary<string, MQInterface>.ValueCollection.Enumerator enumerator = m_dicEventMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicTraceMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicLicenseMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_dicRGAMQInterfaceList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MQInterface current = enumerator.Current;
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (networkThread != null && networkThread.IsAlive && networkThread != null)
			{
				networkThread.Abort();
			}
			logger.Info("MQ Close");
		}

		public void WriteSend(object messgae, object sender)
		{
			lock (mqWriteSendLock)
			{
				MQInterface mQInterface = sender as MQInterface;
				if (mQInterface != null)
				{
					mQInterface.Send(messgae);
				}
				else
				{
					foreach (MQInterface value in m_dicEventMQInterfaceList.Values)
					{
						value.Send(messgae);
					}
				}
			}
			logger.Info("MQ WriteSend Message");
		}

		public void SendEvent(object message)
		{
			lock (m_objSendEventLock)
			{
				foreach (MQInterface value in m_dicEventMQInterfaceList.Values)
				{
					value.Send(message);
				}
			}
		}

		public void SendTraceData(object message)
		{
			lock (m_objTraceDataLock)
			{
				foreach (MQInterface value in m_dicTraceMQInterfaceList.Values)
				{
					value.Send(message);
				}
			}
		}

		public void SendLicenseAlarm(object message)
		{
			lock (m_objLicenseLock)
			{
				foreach (MQInterface value in m_dicLicenseMQInterfaceList.Values)
				{
					value.Send(message);
				}
			}
		}

		public void SendRGAData(object message)
		{
			lock (m_objRGADataLock)
			{
				foreach (MQInterface value in m_dicRGAMQInterfaceList.Values)
				{
					value.Send(message);
				}
			}
		}

		private void AddEvent(MessageData<PLCMessageBody> ev)
		{
			m_qEventQ.Enqueue(ev);
		}

		private void NetworkThreadProc()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					if (m_qEventQ.Count > 0)
					{
						MessageData<PLCMessageBody> messageData = m_qEventQ.Dequeue();
						if (messageData != null)
						{
							this.OnNetworkEvent(messageData);
						}
					}
					Thread.Sleep(10);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("EventPlcThreadProc Error : {0}",ex.Message));
				Thread.ResetAbort();
			}
		}
	}
}
