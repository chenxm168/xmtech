using HF.BC.Tool.EIPDriver;
using HF.BC.Tool.EIPDriver.Config;
using HF.BC.Tool.EIPDriver.Data;
using HF.BC.Tool.EIPDriver.Driver.Data;
using HF.BC.Tool.EIPDriver.Enums;
using EQPIO.MessageData;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EQPIO.Controller.Proxy
{
	public class EIPProxy
	{
		public delegate void EventHandler(object sender, MessageData<EIPMessageBody> message);

		public delegate void SVEventHandler(object sender, MessageData<EIPMessageBody> message);

		private readonly string _logFilePath = "../config/log4Net.xml";

		private bool running = true;

		private EIPClient _EIPClient;

		private ILog logger = LogManager.GetLogger(typeof(EIPProxy));

		private Thread thread;

		private Queue<Trx> queue = new Queue<Trx>();

		private readonly object SyncRoot = new object();

		public bool Connection
		{
			get;
			set;
		}

		public event EventHandler OnEventReceived;

		public event SVEventHandler OnSVEventReceived;

		public bool Init(string filePath)
		{
			try
			{
				EIPConfig eIPConfig = new EIPConfig();
				_EIPClient = new EIPClient();
				eIPConfig.EIPMapFile = filePath;
				eIPConfig.Log4NetPath = _logFilePath;
				if (!string.IsNullOrEmpty(eIPConfig.EIPMapFile))
				{
					if (_EIPClient.Configure(eIPConfig) == ConfigureError.None)
					{
						if (_EIPClient.Open())
						{
							Connection = true;
							_EIPClient.OnReceived += _EIPClient_OnReceived;
							_EIPClient.OnSVData += _EIPClient_OnSVData;
							logger.Info(string.Format("EIP Init..."));
							thread = new Thread(Run);
							thread.IsBackground = true;
							return true;
						}
						return false;
					}
					logger.Error("Map File이 정상적이지 않습니다.");
					return false;
				}
				logger.Error("Map File을 선택하지 않았습니다.");
				return false;
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("EIP Init Error : {0}", ex.Message));

				return false;
			}
		}

		public void ReadData(EIPMessageBody data)
		{
			foreach (string key in data.ReadDataList.Keys)
			{
				Block block = _EIPClient.CreateBlock(key);
				_EIPClient.ReadBlock(block);
				foreach (Item value in block.ItemCollection.Values)
				{
					data.ReadDataList[key].Add(value.Name, value.Value.Trim());
				}
			}
		}

		public bool WriteData(EIPMessageBody data)
		{
			if (string.IsNullOrEmpty(data.EventName.Trim()))
			{
				if (!WriteBlock(data))
				{
					return false;
				}
			}
			else if (!WriteTrx(data))
			{
				return false;
			}
			return true;
		}

		public bool WriteBlock(EIPMessageBody data)
		{
			try
			{
				foreach (string key in data.WriteDataList.Keys)
				{
					Block block = _EIPClient.CreateBlock(key);
					foreach (string key2 in data.WriteDataList[key].Keys)
					{
						block.ItemCollection[key2].Value = data.WriteDataList[key][key2];
					}
					_EIPClient.WriteBlock(block);
				}
				return true;
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("Write Block Error : {0}", ex.Message));
				return false;
			}
		}

		public bool WriteTrx(EIPMessageBody data)
		{
			try
			{
				Trx trx = _EIPClient.CreateSendTransaction(data.EventName);
				foreach (Tag value in trx.TagCollection.Values)
				{
					foreach (Block value2 in value.BlockCollection.Values)
					{
						foreach (string key in data.WriteDataList.Keys)
						{
							if (value2.Name == key)
							{
								foreach (string key2 in value2.ItemCollection.Keys)
								{
									if (data.WriteDataList[key].ContainsKey(key2))
									{
										value2.ItemCollection[key2].Value = data.WriteDataList[key][key2];
									}
								}
							}
						}
					}
				}
				_EIPClient.WriteTrx(trx);
				return true;
			}
			catch (Exception ex)
			{
                logger.Error(string.Format("Write Trx Error : {0}", ex.Message));
				return false;
			}
		}

		public void Close()
		{
			running = false;
			if (_EIPClient.IsOpen)
			{
				_EIPClient.Close();
			}
			if (thread != null)
			{
				thread.Abort();
			}
		}

		public void Start()
		{
			if (thread != null)
			{
				thread.Start();
			}
		}

		private void _EIPClient_OnReceived(object sender, Trx trx)
		{
			lock (SyncRoot)
			{
				queue.Enqueue(trx);
			}
		}

		private void _EIPClient_OnSVData(object sender, Trx trx)
		{
			MessageData<EIPMessageBody> messageData = new MessageData<EIPMessageBody>();
			EIPMessageBody eIPMessageBody2 = messageData.MessageBody = new EIPMessageBody();
			messageData.MessageName = "Event";
			messageData.MachineName = "EIP";
			messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			messageData.ReturnCode = 0;
			messageData.ReturnMessage = string.Empty;
			eIPMessageBody2.EventName = trx.Name;
			foreach (Tag value in trx.TagCollection.Values)
			{
				foreach (Block value2 in value.BlockCollection.Values)
				{
					messageData.MessageBody.ReadDataList.Add(value2.Name, new Dictionary<string, string>());
					foreach (Item value3 in value2.ItemCollection.Values)
					{
						messageData.MessageBody.ReadDataList[value2.Name].Add(value3.Name, value3.Value.Trim());
					}
				}
			}
			if (this.OnSVEventReceived != null)
			{
				this.OnSVEventReceived(this, messageData);
			}
		}

		private void Run()
		{
			while (running)
			{
				try
				{
					Trx trx = null;
					lock (SyncRoot)
					{
						if (queue.Count >= 1)
						{
							trx = queue.Dequeue();
							goto IL_004f;
						}
					}
					goto end_IL_0007;
					IL_004f:
					MessageData<EIPMessageBody> messageData = new MessageData<EIPMessageBody>();
					EIPMessageBody eIPMessageBody2 = messageData.MessageBody = new EIPMessageBody();
					messageData.MessageName = "Event";
					messageData.MachineName = "EIP";
					messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
					messageData.ReturnCode = 0;
					messageData.ReturnMessage = string.Empty;
					eIPMessageBody2.EventName = trx.Name;
					foreach (Tag value in trx.TagCollection.Values)
					{
						foreach (Block value2 in value.BlockCollection.Values)
						{
							messageData.MessageBody.ReadDataList.Add(value2.Name, new Dictionary<string, string>());
							foreach (Item value3 in value2.ItemCollection.Values)
							{
								messageData.MessageBody.ReadDataList[value2.Name].Add(value3.Name, value3.Value.Trim());
							}
						}
					}
					this.OnEventReceived(this, messageData);
					end_IL_0007:;
				}
				catch (Exception message)
				{
					logger.Error(message);
				}
				finally
				{
					Thread.Sleep(5);
				}
			}
		}
	}
}
