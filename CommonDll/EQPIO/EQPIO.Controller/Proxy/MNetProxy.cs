using EQPIO.Common;
using EQPIO.MessageData;
using EQPIO.MNetDriver;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;


namespace EQPIO.Controller.Proxy
{
	public class MNetProxy:IMapInfo
	{
		public delegate void EventHandler(object sender, MessageData<PLCMessageBody> message, string blockName);

		public delegate void SVEventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void RGADataEventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void LinkSignalScanEventHandler(object sender, Dictionary<string, string> signal, Block block);

		public delegate void TimeOutEventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void ErrorEventHandler(string message);

		private ILog logger = LogManager.GetLogger(typeof(MNetProxy));

		private Configuration mNetconfig;

		private MNetUnit mUnit;

		private Dictionary<string, MNetUnit> mUnitList = new Dictionary<string, MNetUnit>();

		private Dictionary<string, MNetScanUnit> mScanUnitList = new Dictionary<string, MNetScanUnit>();

		private Dictionary<string, MNetSVScanUnit> mSVScanUnitList = new Dictionary<string, MNetSVScanUnit>();

		private Dictionary<string, MNetLinkSignalScanUnit> mLinkSignalScanUnitList = new Dictionary<string, MNetLinkSignalScanUnit>();

		private bool m_bConnection = false;

		private BlockMap m_BlockMap = new BlockMap();


		private DataGathering m_DataGathering;

		private Transaction m_Transaction;

		private object m_objFDCLock = new object();

		private object m_objRGALock = new object();

		public bool Connection
		{
			get
			{
				return m_bConnection;
			}
			set
			{
				m_bConnection = value;
			}
		}

		public BlockMap BlockMap ;

		public Transaction Transaction ;

		public event EventHandler OnEventReceived;

		public event SVEventHandler OnSVEventReceived;

		public event RGADataEventHandler OnRGADataReceived;

		public event LinkSignalScanEventHandler OnLinkSignalScanReceived;

		public event TimeOutEventHandler OnTimeOutReceived;

		public event ErrorEventHandler OnErrorMessage;

        public BlockMap M_BlockMap
        {
            get { return m_BlockMap; }
            set { m_BlockMap = value; }
        }

        public Transaction M_Transaction
        {
            get { return m_Transaction; }
            set { m_Transaction = value; }
        }

        public PLCMap EQPPlcMap
        {
            get;
            set;
        }


		private void unit_OnScanReceived(object sender, string blockName, string itemName, bool flag)
		{
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody messageBody = new PLCMessageBody();
			MNetScanUnit mNetScanUnit = sender as MNetScanUnit;
			messageData.MessageBody = messageBody;
			messageData.MessageName = "Event";
			messageData.MachineName = "MNet";
			messageData.ReturnCode = 0;
			messageData.ReturnMessage = string.Empty;
			messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			try
			{
				Trx[] trx = m_Transaction.Receive.Trx;
				foreach (Trx trx2 in trx)
				{
					if (trx2.Key == blockName + "." + itemName || trx2.Key == mNetScanUnit.MultiBlockName + "." + blockName + "." + itemName)
					{
						messageData = mUnitList[mNetScanUnit.Name].DataCollect(messageData, trx2, flag);
						messageData = mUnitList[mNetScanUnit.Name].EventTransaction(messageData, trx2, flag);
						if (!flag && !trx2.BitOffEventReport)
						{
							 //logger.Info(string.Format("Transaction BitOffReport flase , Trx : {0}", trx2.Name));
                            logger.Info(string.Format("Transaction BitOffReport flase , Trx : {0}", trx2.Name));
							return;
						}
						messageData.MessageBody.EventName = trx2.Name;
						messageData.MessageBody.EventValue = (flag ? "1" : "0");
					}
				}
				if (messageData.MessageBody.EventName == null && itemName.ToUpper() != "EQUIPMENTALIVE" && !Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(itemName.ToUpper()))
				{
                     logger.Error(string.Format("Can't Find Trx - MultiBlockName: : {0},BlockName:{1}, ItemName:{2},Flag:{3}", mNetScanUnit.MultiBlockName,blockName,itemName,flag));
				
				}
				if (!string.IsNullOrEmpty(messageData.MessageBody.EventName))
				{
					if (!(messageData.MessageBody.EventName.Substring(3).ToUpper() == "MACHINEHEARTBEATSIGNAL") && !(messageData.MessageBody.EventName.Substring(4).ToUpper() == "MACHINEHEARTBEATSIGNAL") && !(messageData.MessageBody.EventName.Substring(3).ToUpper() == "EQUIPMENTALIVE"))
					{
						if (!Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(itemName.ToUpper()))
						{
                             logger.Info(string.Format("OnScanReceived Complete EventName : {0}",messageData.MessageBody.EventName));
						
						}
						else
						{
							messageData.MessageType = "LoggingSkip";
						}
					}
					this.OnEventReceived(this, messageData, messageData.MessageBody.EventName);
				}
			}
			catch (Exception ex)
			{
                 logger.Error(string.Format("unit_OnScanReceived Error {0}", ex.Message));
			}
		}

		private void unit_OnScanReceived(object sender, string blockName, Dictionary<string, string> list)
		{
			lock (m_objFDCLock)
			{
				MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
				PLCMessageBody pLCMessageBody2 = messageData.MessageBody = new PLCMessageBody();
				messageData.MessageName = "TRACEDATA";
				messageData.MachineName = "MNet";
				messageData.MessageType = "TRACE";
				messageData.ReturnCode = 0;
				messageData.ReturnMessage = string.Empty;
				messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				messageData.MessageBody.ReadDataList.Add(blockName, list);
				this.OnSVEventReceived(this, messageData);
			}
		}

		private void unit_OnRGAScanReceived(object sender, string blockName, Dictionary<string, string> list)
		{
			lock (m_objRGALock)
			{
				MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
				PLCMessageBody pLCMessageBody2 = messageData.MessageBody = new PLCMessageBody();
				messageData.MessageName = "RGADATA";
				messageData.MachineName = "MNet";
				messageData.MessageType = "RGA";
				messageData.ReturnCode = 0;
				messageData.ReturnMessage = string.Empty;
				messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				messageData.MessageBody.ReadDataList.Add(blockName, list);
				this.OnRGADataReceived(this, messageData);
			}
		}

		private void unit_OnLinkSignalScanReceived(object sender, string bitString, int overCount, Block plcScanBlock)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int num = 0;
			Item[] item = plcScanBlock.Item;
			foreach (Item item2 in item)
			{
				num = ((overCount <= 0) ? int.Parse(item2.Offset) : (int.Parse(item2.Offset) + overCount));
				char c;
				if (dictionary.ContainsKey(item2.Name))
				{
					Dictionary<string, string> dictionary2 = dictionary;
					string name = item2.Name;
					c = bitString[num];
					dictionary2.Add(name, c.ToString());
				}
				else
				{
					Dictionary<string, string> dictionary3 = dictionary;
					string name2 = item2.Name;
					c = bitString[num];
					dictionary3[name2] = c.ToString();
				}
			}
			this.OnLinkSignalScanReceived(this, dictionary, plcScanBlock);
		}

		private void mUnit_OnTimeoutEvent(object sender, string localName)
		{
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody pLCMessageBody2 = messageData.MessageBody = new PLCMessageBody();
			messageData.MessageName = "Event";
			messageData.MachineName = localName;
			messageData.ReturnCode = 1;
			messageData.ReturnMessage = "WriteTransaction TimeOut";
			messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			messageData.MessageBody.EventName = "";
			messageData.MessageBody.EventValue = "";
			this.OnTimeOutReceived(this, messageData);
		}

		private void unit_OnErrorMessage(string message)
		{
			this.OnErrorMessage(message);
		}

		private void SetPlcMap(XmlSerializer xmlSerialize, ItemGroupCollection itemGroupCollection, XmlNode xml)
		{
			foreach (XmlNode childNode in xml.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					Block block = (Block)xmlSerialize.Deserialize(new StringReader(childNode.OuterXml));
					string[] array = block.HeadDevice.Split('x');
					block.address = new MNetDev(block.DeviceCode + ((array.Length > 1) ? array[1] : array[0]));
					if (block.ItemGroup != null)
					{
						ItemGroup itemGroup = (from groupName in itemGroupCollection.ItemGroup
						where groupName.Name == block.ItemGroup.Name
						select groupName).FirstOrDefault();
						if (block.Item != null)
						{
							Item[] array2 = new Item[block.Item.Length + itemGroup.Item.Length];
							int num = 0;
							Item[] item = itemGroup.Item;
							foreach (Item value in item)
							{
								array2.SetValue(value, num);
								num++;
							}
							item = block.Item;
							foreach (Item value in item)
							{
								array2.SetValue(value, num);
								num++;
							}
							block.Item = array2;
						}
						else
						{
							block.Item = itemGroup.Item;
						}
					}
					if (block.Item != null)
					{
						m_BlockMap.Add(block);
					}
				}
				else if (childNode.NodeType == XmlNodeType.EntityReference)
				{
					SetPlcMap(xmlSerialize, itemGroupCollection, childNode);
				}
			}
		}

		private bool XmlVerification(Transaction transaction)
		{
			if (transaction.Receive.Trx != null)
			{
				Trx[] trx = transaction.Receive.Trx;
				foreach (Trx trx2 in trx)
				{
					if (!TransactionVerification(trx2))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		private bool XmlVerification(BlockMap map)
		{
			int num = 0;
			foreach (Block item3 in map.Block)
			{
				if (item3.Item != null)
				{
					Item[] item = item3.Item;
					foreach (Item item2 in item)
					{
						string[] array = item2.Offset.Split(':');
						string[] array2 = item2.Points.Split(':');
						if (item3.Points < int.Parse(array[0]) + int.Parse(array2[0]) - 1)
						{
							num++;
                            logger.Error(string.Format("Item Offset or Point Error , BlockName : {0}, BlockPoint {1}, ItemName : {2}, ItemOffset : {3}, ItemPoint : {4}", item2.Name, item2.Points, item2.Name, item2.Offset, item2.Points ));
						}
					}
				}
			}
			if (num <= 0)
			{
				return true;
			}
			return false;
		}

		private bool TransactionVerification(Trx trx)
		{
			if (trx.MultiBlock != null)
			{
				MultiBlock[] multiBlock = trx.MultiBlock;
				foreach (MultiBlock multiBlock2 in multiBlock)
				{
					if (multiBlock2.Block == null)
					{
						logger.Error(string.Format("Transaction Block is null, multiBlockName : {0}",multiBlock2.Name));
						return false;
					}
					Block[] block = multiBlock2.Block;
					foreach (Block block2 in block)
					{
						if (multiBlock2.Action == "W" && block2.Item == null)
						{
							logger.Error(string.Format("Transaction Item is null, BlockName : {0}",block2.Name));
							return false;
						}
					}
				}
				return true;
			}
			logger.Error(string.Format("Transaction MultiBlock is null, TransactionName : {0}",trx.Name));
			return false;
		}

		private MessageData<PLCMessageBody> WriteTransactionDataCollect(MessageData<PLCMessageBody> mdata, Trx trx)
		{
			MultiBlock[] multiBlock = trx.MultiBlock;
			foreach (MultiBlock multiBlock2 in multiBlock)
			{
				Block[] block = multiBlock2.Block;
				foreach (Block block2 in block)
				{
					if (!mdata.MessageBody.WriteDataList.ContainsKey(block2.Name) && block2.Item != null)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						Item[] item = block2.Item;
						foreach (Item item2 in item)
						{
							dictionary.Add(item2.Name, item2.Value);
						}
						mdata.MessageBody.WriteDataList.Add(block2.Name, dictionary);
					}
					else if (mdata.MessageBody.WriteDataList.ContainsKey(block2.Name) || block2.Item != null)
					{
						continue;
					}
				}
			}
			return mdata;
		}

		private bool WriteTransactionData(MessageData<PLCMessageBody> data, Trx trx)
		{
			int num = 0;
			ushort netwrokNo = 0;
			ushort stationNo = 0;
			MultiBlock[] multiBlock = trx.MultiBlock;
			foreach (MultiBlock multiBlock2 in multiBlock)
			{
				if (!string.IsNullOrEmpty(multiBlock2.NetworkNo))
				{
					netwrokNo = (ushort)Convert.ToInt32(multiBlock2.NetworkNo);
				}
				if (!string.IsNullOrEmpty(multiBlock2.PCNo))
				{
					stationNo = (ushort)Convert.ToInt32(multiBlock2.PCNo);
				}
				Block[] block2 = multiBlock2.Block;
				Block sendBlock;
				for (int j = 0; j < block2.Length; j++)
				{
					sendBlock = block2[j];
					Block block3 = (from block in m_BlockMap.Block
					where block.Name == sendBlock.Name
					select block).FirstOrDefault();
					if (block3 != null && data.MessageBody.WriteDataList.ContainsKey(block3.Name))
					{
						num++;
						switch (block3.DeviceCode)
						{
						case "B":
							if (!mUnit.WriteBit(block3, data.MessageBody.WriteDataList[block3.Name]))
							{
								return false;
							}
							break;
						case "W":
							if (!mUnit.WriteWord(block3, data.MessageBody.WriteDataList[block3.Name], 0, 0, false))
							{
								return false;
							}
							break;
						case "R":
							if (!mUnit.WriteWord(block3, data.MessageBody.WriteDataList[block3.Name], netwrokNo, stationNo, true))
							{
								return false;
							}
							break;
						case "ZR":
							if (!mUnit.WriteWord(block3, data.MessageBody.WriteDataList[block3.Name], netwrokNo, stationNo, true))
							{
								return false;
							}
							break;
						}
					}
				}
			}
			//logger.Info("Write TransactionData");
            logger.Info("Write TransactionData");
			return num > 0;
		}

		private bool WriteNormalData(MessageData<PLCMessageBody> data)
		{
			int num = 0;
			foreach (Block item in m_BlockMap.Block)
			{
				if (data.MessageBody.WriteDataList.ContainsKey(item.Name))
				{
					num++;
					switch (item.DeviceCode)
					{
					case "B":
						if (!mUnit.WriteBit(item, data.MessageBody.WriteDataList[item.Name]))
						{
							return false;
						}
						break;
					case "W":
						if (!mUnit.WriteWord(item, data.MessageBody.WriteDataList[item.Name], 0, 0, false))
						{
							return false;
						}
						break;
					case "R":
					{
						string[] array2 = data.MachineName.Split(',');
						if (array2.Length < 2)
						{
							logger.Error("To request with LocalPLC, you need to send data of type (NetworkNo,PcNo) to MachineName.");
							return false;
						}
						ushort netwrokNo2 = ushort.Parse(array2[0]);
						ushort stationNo2 = ushort.Parse(array2[1]);
						if (!mUnit.WriteWord(item, data.MessageBody.WriteDataList[item.Name], netwrokNo2, stationNo2, true))
						{
							return false;
						}
						break;
					}
					case "ZR":
					{
						string[] array = data.MachineName.Split(',');
						if (array.Length < 2)
						{
							logger.Error("To request with LocalPLC, you need to send data of type (NetworkNo,PcNo) to MachineName.");
							return false;
						}
						ushort netwrokNo = ushort.Parse(array[0]);
						ushort stationNo = ushort.Parse(array[1]);
						if (!mUnit.WriteWord(item, data.MessageBody.WriteDataList[item.Name], netwrokNo, stationNo, true))
						{
							return false;
						}
						break;
					}
					}
				}
			}
			//logger.Info("Write NormalData");
            logger.Info("Write NormalData");
			return num > 0;
		}

        public bool InitByInnerResource( ref string msg)
        {
            string mnConfigString = GetResource.GetEmbeddedResource("MNetConfig.xml") as string;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                XmlDocument xmlDocument2 = new XmlDocument();
                xmlDocument2.LoadXml(mnConfigString);
                XmlNode xmlNode = xmlDocument2.SelectSingleNode("Configuration");
                mNetconfig = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                EQPIO.Common.ConnectionInfo[] connectionInfo = mNetconfig.ConnectionInfo;
                // xmlDocument2.Load(driver.ConnectionInfo.path);
               // string mapString = GetResource.GetEmbeddedResource(connectionInfo2.PLCMapFile) as string;
                

                
                
               
                foreach (EQPIO.Common.ConnectionInfo connectionInfo2 in connectionInfo)
                {
                    // xmlDocument.Load(connectionInfo2.PLCMapFile);
                    string mapString = GetResource.GetEmbeddedResource(connectionInfo2.PLCMapFile) as string;
                    xmlDocument.LoadXml(mapString);
                    xmlNode = xmlDocument.SelectSingleNode("PLCDriver/ItemGroupCollection");
                    xmlSerializer = new XmlSerializer(typeof(ItemGroupCollection));
                    ItemGroupCollection itemGroupCollection = (ItemGroupCollection)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                    xmlNode = xmlDocument.SelectSingleNode("PLCDriver/BlockMap");
                    xmlSerializer = new XmlSerializer(typeof(Block));
                    SetPlcMap(xmlSerializer, itemGroupCollection, xmlNode);
                    xmlNode = xmlDocument.SelectSingleNode("PLCDriver/DataGathering");
                    xmlSerializer = new XmlSerializer(typeof(DataGathering));
                    m_DataGathering = (DataGathering)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                    xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
                    xmlSerializer = new XmlSerializer(typeof(Transaction));
                    m_Transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));

                    this.EQPPlcMap = new PLCMap(connectionInfo2, m_BlockMap, m_DataGathering, m_Transaction, m_DataGathering, m_Transaction, false);

                    if (!XmlVerification(m_Transaction))
                    {
                        msg = string.Format("Map Config Transaction Error \n Check Error Log");
                        return false;
                    }
                    if (!XmlVerification(m_BlockMap))
                    {
                        msg = string.Format("Map Config BlcokMap or ItemGroup Error \n Check Error Log");
                        return false;
                    }
                }
                logger.Info("MNetProxy InitXml");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("MNetProxy InitXml Error : {0}", ex.Message));
                return false;
            }


        }

		public bool InitXml(Driver driver, ref string msg)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
				XmlDocument xmlDocument2 = new XmlDocument();
				xmlDocument2.Load(driver.ConnectionInfo.path);
				XmlNode xmlNode = xmlDocument2.SelectSingleNode("Configuration");
				mNetconfig = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
				EQPIO.Common.ConnectionInfo[] connectionInfo = mNetconfig.ConnectionInfo;
				foreach (EQPIO.Common.ConnectionInfo connectionInfo2 in connectionInfo)
				{
					xmlDocument.Load(connectionInfo2.PLCMapFile);
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/ItemGroupCollection");
					xmlSerializer = new XmlSerializer(typeof(ItemGroupCollection));
					ItemGroupCollection itemGroupCollection = (ItemGroupCollection)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/BlockMap");
					xmlSerializer = new XmlSerializer(typeof(Block));
					SetPlcMap(xmlSerializer, itemGroupCollection, xmlNode);
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/DataGathering");
					xmlSerializer = new XmlSerializer(typeof(DataGathering));
					m_DataGathering = (DataGathering)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
					xmlSerializer = new XmlSerializer(typeof(Transaction));
					m_Transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					if (!XmlVerification(m_Transaction))
					{
						msg = string.Format("Map Config Transaction Error \n Check Error Log");
						return false;
					}
					if (!XmlVerification(m_BlockMap))
					{
						msg = string.Format("Map Config BlcokMap or ItemGroup Error \n Check Error Log");
						return false;
					}
				}
				logger.Info("MNetProxy InitXml");
				return true;
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("MNetProxy InitXml Error : {0}",ex.Message));
				return false;
			}
		}

		public bool InitMNet()
		{
			try
			{
				mUnit = new MNetUnit(mNetconfig.ConnectionInfo[0].Channel, m_BlockMap, m_DataGathering, m_Transaction, Convert.ToInt32(mNetconfig.ConnectionInfo[0].TimeoutCheckLimit));
				MultiBlock[] multiBlock = m_DataGathering.Scan.MultiBlock;
				Block block;
				Block block2;
				Block block3;
				foreach (MultiBlock multiBlock2 in multiBlock)
				{
					if (multiBlock2.StartUp.ToUpper() == "TRUE")
					{
						switch (multiBlock2.Name.ToUpper())
						{
						case "FDC":
						case "TRACE":
						case "RGA":
							if (!multiBlock2.DirectAccess)
							{
								Block[] block4 = multiBlock2.Block;
								for (int j = 0; j < block4.Length; j++)
								{
									block = block4[j];
									Block block6 = (from blockName in m_BlockMap.Block
									where blockName.Name == block.Name
									select blockName).FirstOrDefault();
									if (block6 != null)
									{
										MNetSVScanUnit mNetSVScanUnit = new MNetSVScanUnit(mNetconfig.ConnectionInfo[0].Channel, m_BlockMap, block6, multiBlock2.Interval, block.Name, multiBlock2.Name);
										if (multiBlock2.Name.ToUpper() == "RGA")
										{
											mNetSVScanUnit.IsRGA = true;
											logger.Info(string.Format("Create RGAScanUnit : {0}",block.Name));
										}
										else
										{
											logger.Info(string.Format("Create SVScanUnit : {0}",block.Name));
										}
										mSVScanUnitList.Add(block.Name, mNetSVScanUnit);
									}
								}
							}
							break;
						case "LINKSIGNAL":
						{
							Block[] block4 = multiBlock2.Block;
							for (int j = 0; j < block4.Length; j++)
							{
								block2 = block4[j];
								Block block7 = (from blockName in m_BlockMap.Block
								where blockName.Name == block2.Name
								select blockName).FirstOrDefault();
								if (block7 != null)
								{
									mLinkSignalScanUnitList.Add(block2.Name, new MNetLinkSignalScanUnit(mNetconfig.ConnectionInfo[0].Channel, m_BlockMap, block7, multiBlock2.Interval, block2.Name, multiBlock2.Name));
									logger.Info(string.Format("Create LinkSignalScanUnit : {}",block2.Name));
								}
							}
							break;
						}
						default:
						{
							Block[] block4 = multiBlock2.Block;
							for (int j = 0; j < block4.Length; j++)
							{
								block3 = block4[j];
								Block block5 = (from blockName in m_BlockMap.Block
								where blockName.Name == block3.Name
								select blockName).FirstOrDefault();
								if (block5 != null)
								{
									mScanUnitList.Add(block3.Name, new MNetScanUnit(mNetconfig.ConnectionInfo[0].Channel, m_BlockMap, block5, multiBlock2));
									mUnitList.Add(block3.Name, new MNetUnit(mNetconfig.ConnectionInfo[0].Channel, m_BlockMap, m_DataGathering, m_Transaction, Convert.ToInt32(mNetconfig.ConnectionInfo[0].TimeoutCheckLimit)));
									logger.Info(string.Format("Create Unit ,ScanUnit : {0}",block3.Name));
								}
							}
							break;
						}
						}
					}
					else
					{
						Globalproperties.Instance.UpdateScanStatus(enumConnectionType.PLCBoard, mNetconfig.ConnectionInfo[0].DriverName, multiBlock2, false);
					}
				}
				if (mUnit.InitMNet())
				{
					logger.Info("MNetProxy InitMNet mUnit");
					foreach (MNetScanUnit value in mScanUnitList.Values)
					{
						if (!value.Open())
						{
							logger.Error("InitMNet : ScanUnit Open Fail");
							return false;
						}
					}
					logger.Info("MNetProxy InitMNet ScanUnitList");
					foreach (MNetUnit value2 in mUnitList.Values)
					{
						if (!value2.InitMNet())
						{
							logger.Error("InitMNet : MNetUnitList Open Fail");
							return false;
						}
					}
					logger.Info("MNetProxy InitMNet mUnitList");
					foreach (MNetSVScanUnit value3 in mSVScanUnitList.Values)
					{
						if (!value3.Open())
						{
							logger.Error("InitMNet : SVScanUnit Open Fail");
							return false;
						}
					}
					logger.Info("MNetProxy InitMNet mSVScanUnitList");
					m_bConnection = true;
					mUnit.SetChekTimeOut();
					logger.Info("MNetProxy InitMNet Compeleted");
					return m_bConnection;
				}
				logger.Error("InitMNet : mUnit Open Fail");
				return false;
			}
			catch (Exception ex)
			{
				m_bConnection = false;
				logger.Error(string.Format("MNetProxy : InitMNet Error : {0}",ex.Message));
				return m_bConnection;
			}
		}

		public void InitMNetEvent()
		{
			foreach (MNetSVScanUnit value in mSVScanUnitList.Values)
			{
				value.OnScanReceived += unit_OnScanReceived;
				value.OnRGAScanReceived += unit_OnRGAScanReceived;
				value.OnErrorMessage += unit_OnErrorMessage;
			}
			foreach (MNetScanUnit value2 in mScanUnitList.Values)
			{
				value2.OnScanReceived += unit_OnScanReceived;
				value2.OnErrorMessage += unit_OnErrorMessage;
			}
			foreach (MNetUnit value3 in mUnitList.Values)
			{
				value3.OnErrorMessage += unit_OnErrorMessage;
			}
			foreach (MNetLinkSignalScanUnit value4 in mLinkSignalScanUnitList.Values)
			{
				value4.OnLinkSignalScanReceived += unit_OnLinkSignalScanReceived;
			}
			mUnit.OnTimeoutEvent += mUnit_OnTimeoutEvent;
			mUnit.OnErrorMessage += unit_OnErrorMessage;
			logger.Info("InitMNetEvent");
		}

		public void MNetStart()
		{
			mUnit.TimeoutCheckThreadStart();
			foreach (MNetScanUnit value in mScanUnitList.Values)
			{
				value.Start();
			}
			foreach (MNetSVScanUnit value2 in mSVScanUnitList.Values)
			{
				value2.Start();
			}
			logger.Info("MNetStart");
		}

		public void Close()
		{
			if (mUnit != null)
			{
				mUnit.TimeoutCheckThreadStop();
			}
			if (mScanUnitList != null)
			{
				foreach (MNetScanUnit value in mScanUnitList.Values)
				{
					value.Stop();
				}
				logger.Info("MNetProxy : Close mScanUnitList");
			}
			if (mSVScanUnitList != null)
			{
				foreach (MNetSVScanUnit value2 in mSVScanUnitList.Values)
				{
					value2.Stop();
				}
				logger.Info("MNetProxy : Close mSVScanUnitList");
			}
			if (mUnitList != null)
			{
				foreach (MNetUnit value3 in mUnitList.Values)
				{
					value3.TimeoutCheckThreadStop();
				}
				logger.Info("MNetProxy : Close mUnitList");
			}
			m_bConnection = false;
		}

		public MessageData<PLCMessageBody> ReadData(MessageData<PLCMessageBody> data)
		{
			try
			{
				foreach (Block item in m_BlockMap.Block)
				{
					if (data.MessageBody.ReadDataList.ContainsKey(item.Name))
					{
						switch (item.DeviceCode)
						{
						case "B":
							data.MessageBody.ReadDataList[item.Name] = mUnit.ReadBit(item);
							break;
						case "W":
							data.MessageBody.ReadDataList[item.Name] = mUnit.ReadWord(item);
							break;
						case "R":
						{
							string[] array2 = data.MachineName.Split(',');
							if (array2.Length < 2)
							{
								logger.Error("To request with LocalPLC, you need to send data of type (NetworkNo,PcNo) to MachineName.");
							}
							else
							{
								ushort networkNo2 = ushort.Parse(array2[0]);
								ushort stationNo2 = ushort.Parse(array2[1]);
								data.MessageBody.ReadDataList[item.Name] = mUnit.ReadNetworkWord(item, networkNo2, stationNo2);
							}
							break;
						}
						case "ZR":
						{
							string[] array = data.MachineName.Split(',');
							if (array.Length < 2)
							{
								logger.Error("To request with LocalPLC, you need to send data of type (NetworkNo,PcNo) to MachineName.");
							}
							else
							{
								ushort networkNo = ushort.Parse(array[0]);
								ushort stationNo = ushort.Parse(array[1]);
								data.MessageBody.ReadDataList[item.Name] = mUnit.ReadNetworkWord(item, networkNo, stationNo);
							}
							break;
						}
						default:
							logger.Error("ReadData : Invalid DeviceCode");
							break;
						}
					}
				}
				logger.Info("ReadData");
				return data;
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("ReadData Error : {}",ex.Message));
				return data;
			}
		}

		public bool WriteData(MessageData<PLCMessageBody> data)
		{
			if (!string.IsNullOrEmpty(data.MessageBody.EventName))
			{
				Trx trx = (from trxName in m_Transaction.Send.Trx
				where trxName.Name == data.MessageBody.EventName
				select trxName).FirstOrDefault();
				if (trx != null)
				{
					data = WriteTransactionDataCollect(data, trx);
					logger.Info("Write Transaction Start");
					return WriteTransactionData(data, trx);
				}
				logger.Error(string.Format("transaction invalid - transactionName : {}",data.MessageBody.EventName));
				return false;
			}
			logger.Info("Write Normal Start");
			return WriteNormalData(data);
		}

		public bool RuntimeScanOnOff(bool requestOn, MultiBlock multiblock)
		{
			if (multiblock != null)
			{
				if (requestOn)
				{
					Block[] block = multiblock.Block;
					foreach (Block block2 in block)
					{
						if (!string.IsNullOrEmpty(block2.Name) && mScanUnitList.ContainsKey(block2.Name))
						{
							continue;
						}
					}
				}
				else
				{
					Block[] block = multiblock.Block;
					foreach (Block block2 in block)
					{
						if (!string.IsNullOrEmpty(block2.Name) && !mScanUnitList.ContainsKey(block2.Name))
						{
							continue;
						}
					}
				}
				return true;
			}
			return false;
		}

        public Transaction getTrx(string local)
        {
            return m_Transaction;
        }

        public BlockMap getBlockMap(string local)
        {
            return m_BlockMap;
        }
    }
}
