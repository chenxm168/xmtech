using EQPIO.Common;
using EQPIO.MessageData;
using EQPIO.MNetProtocol;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Controller.Proxy
{
	public class MProtocolProxy:IMapInfo
	{
		public delegate void ConnectedEventHandler(object sender);

		public delegate void DisconnectedEventHandler(object sender);

		public delegate void EventHandler(object sender, MessageData<PLCMessageBody> message);

		public delegate void LinkSignalScanEventHandler(object sender, Dictionary<string, string> signal, Block block);

		private ILog logger = LogManager.GetLogger(typeof(MProtocolProxy));

		private Configuration ethernetConfig;

		private bool m_bConnection = false;
        private bool mpUnitConected = false;  //cxm 20191210
        private bool mpScanUnitConected = false; //cxm 20191210

		private Dictionary<string, PLCMap> mMapList = new Dictionary<string, PLCMap>();

		private BlockMap m_BlockMap = new BlockMap();

		private DataGathering m_DataGathering;

		private Transaction m_Transaction;

		private Dictionary<string, IMNetUnit> mPUnitList = new Dictionary<string, IMNetUnit>();

		private Dictionary<string, IMNetScanUnit> mPScanUnitList = new Dictionary<string, IMNetScanUnit>();

		private Dictionary<string, MNetOnDemand> mOnDemandList = new Dictionary<string, MNetOnDemand>();

		private Dictionary<string, IMNetScanUnit> mPCacheUnitList = new Dictionary<string, IMNetScanUnit>();

		private Dictionary<string, IMNetUnit> m_dicVirtualEQPRequestUnit = new Dictionary<string, IMNetUnit>();

		private Dictionary<string, IMNetScanUnit> m_dicVirtualEQPScanUnit = new Dictionary<string, IMNetScanUnit>();

		private object m_objEventScanRecivedLock = new object();

		private object m_objTraceDataReportLock = new object();

		private object m_objRGADataReportLock = new object();

		private object m_objLinkSignalLock = new object();

		private object writeTransactionObj = new object();

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


        public bool MpUnitConnected
        {
            get
            {
                return mpUnitConected;
            }
            set
            {
                mpUnitConected = value;
            }
        }

        public bool MpScanConnected
        {
            get
            {
                return mpScanUnitConected;
            }
            set
            {
                mpScanUnitConected = value;
            }
        }

		public Dictionary<string, PLCMap> MapList
        {
            get
            {
                return mMapList;
            }
        }

		public Dictionary<string, IMNetScanUnit> ScanUnitList ;

		public Dictionary<string, IMNetScanUnit> CacheUnitList ;

		public event ConnectedEventHandler OnConnected;

		public event DisconnectedEventHandler OnDisconnected;

		public event EventHandler OnEventReceived;

		public event EventHandler OnFDCEventReceived;

		public event EventHandler OnRGADataReceived;

		public event EventHandler OnVirtualEQPEventReceived;

		public event LinkSignalScanEventHandler OnLinkSignalScanReceived;

		private void unit_EventScanReceived(object sender, ScanReceivedEventArgs e)
		{
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody messageBody = new PLCMessageBody();
			lock (m_objEventScanRecivedLock)
			{
				messageData.MessageBody = messageBody;
				messageData.MessageName = "Event";
				messageData.MachineName = e.UnitName;
				messageData.ReturnCode = 0;
				messageData.ReturnMessage = string.Empty;
				messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				try
				{
					if (mMapList[e.UnitName].transaction != null && mMapList[e.UnitName].transaction.Receive != null && mMapList[e.UnitName].transaction.Receive.Trx != null)
					{
						bool flag = true;
						Trx[] trx = mMapList[e.UnitName].transaction.Receive.Trx;
						foreach (Trx trx2 in trx)
						{
							if (trx2.Key == e.BlockName + "." + e.ItemName || trx2.Key == e.MultiBlock + "." + e.BlockName + "." + e.ItemName)
							{
								messageData = DataCollect(messageData, trx2, e.Flag);
								messageData = EventTransaction(messageData, trx2, e.Flag);
								string[] array = trx2.Name.Split('_');
								messageData.MachineName = array[0];
								if (!e.Flag && !trx2.BitOffEventReport)
								{
									logger.Info(string.Format("BitOffEventReport is false, Trx : {0}",trx2.Name));
									return;
								}
								messageData.MessageBody.EventName = trx2.Name;
								messageData.MessageBody.EventValue = (e.Flag ? "1" : "0");
								flag = trx2.BitOffReadAction;
							}
						}
						if (!flag || messageData.MessageBody.ReadDataList.Count > 0 || messageData.MessageBody.WriteDataList.Count > 0)
						{
							if (Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(e.ItemName.ToUpper()))
							{
								messageData.MessageType = "LoggingSkip";
							}
							this.OnEventReceived(this, messageData);
						}
						else if (!Globalproperties.Instance.DicLoggingFilterItem.ContainsKey(e.ItemName.ToUpper()))
						{
							logger.Error(string.Format("Can't Find Trx - MultiBlockName:{0}, blockName:{1}, itemName:{2}, flag:{3}",e.MultiBlock,e.BlockName,e.ItemName,e.Flag));
						}
					}
				}
				catch (Exception ex)
				{
					logger.Error(string.Format("unit_OnScanReceived Error : {0}",ex.Message));
				}
			}
		}

		private void unit_TraceDataReport(object sender, FDCScanReceivedEventArgs e)
		{
			string machineName = string.Empty;
			if (e != null && e.BlockName != null)
			{
				machineName = e.BlockName.Substring(0, e.BlockName.IndexOf('_'));
			}
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody messageBody = new PLCMessageBody();
			lock (m_objTraceDataReportLock)
			{
				messageData.MessageBody = messageBody;
				messageData.MessageName = "TRACEDATA";
				messageData.MessageType = "Ethernet";
				messageData.MachineName = machineName;
				messageData.ReturnCode = 0;
				messageData.ReturnMessage = string.Empty;
				messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				messageData.MessageBody.ReadDataList.Add(e.BlockName, e.ItemList);
				this.OnFDCEventReceived(this, messageData);
			}
		}

		private void unit_RGADataReport(object sender, FDCScanReceivedEventArgs e)
		{
			string machineName = string.Empty;
			if (e != null && e.BlockName != null)
			{
				machineName = e.BlockName.Substring(0, e.BlockName.IndexOf('_'));
			}
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody messageBody = new PLCMessageBody();
			lock (m_objRGADataReportLock)
			{
				messageData.MessageBody = messageBody;
				messageData.MessageName = "RGADATA";
				messageData.MessageType = "Ethernet";
				messageData.MachineName = machineName;
				messageData.ReturnCode = 0;
				messageData.ReturnMessage = string.Empty;
				messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				messageData.MessageBody.ReadDataList.Add(e.BlockName, e.ItemList);
				this.OnRGADataReceived(this, messageData);
			}
		}

		private void unit_LinkSignalScanReceived(object sender, LinkSignalScanReceivedEventArgs e)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int num = 0;
			string readBitString = e.ReadBitString;
			int overCount = e.OverCount;
			Block plcScanBlock = e.PlcScanBlock;
			lock (m_objLinkSignalLock)
			{
				Item[] item = plcScanBlock.Item;
				foreach (Item item2 in item)
				{
					num = ((overCount <= 0) ? int.Parse(item2.Offset) : (int.Parse(item2.Offset) + overCount));
					char c;
					if (!dictionary.ContainsKey(item2.Name))
					{
						Dictionary<string, string> dictionary2 = dictionary;
						string name = item2.Name;
						c = readBitString[num];
						dictionary2.Add(name, c.ToString());
					}
					else
					{
						Dictionary<string, string> dictionary3 = dictionary;
						string name2 = item2.Name;
						c = readBitString[num];
						dictionary3[name2] = c.ToString();
					}
				}
				this.OnLinkSignalScanReceived(this, dictionary, e.PlcScanBlock);
			}
		}

		private void unit_VirtaulEQPScanReceived(object sender, ScanReceivedEventArgs e)
		{
			MessageData<PLCMessageBody> messageData = new MessageData<PLCMessageBody>();
			PLCMessageBody pLCMessageBody2 = messageData.MessageBody = new PLCMessageBody();
			messageData.MessageName = "VirtualEQPEvent";
			messageData.MachineName = e.UnitName;
			messageData.ReturnCode = 0;
			messageData.ReturnMessage = string.Empty;
			messageData.EventTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			try
			{
				if (mMapList[e.UnitName].vEQPtransaction != null && mMapList[e.UnitName].vEQPtransaction.Receive != null && mMapList[e.UnitName].vEQPtransaction.Receive.Trx != null)
				{
					Trx[] trx = mMapList[e.UnitName].vEQPtransaction.Receive.Trx;
					foreach (Trx trx2 in trx)
					{
						if (trx2.Key == e.BlockName + "." + e.ItemName || trx2.Key == e.MultiBlock + "." + e.BlockName + "." + e.ItemName)
						{
							messageData = DataCollect(messageData, trx2, e.Flag);
							messageData = EventTransaction(messageData, trx2, e.Flag);
							string[] array = trx2.Name.Split('_');
							messageData.MachineName = array[0];
							if (!e.Flag && !trx2.BitOffEventReport)
							{
								logger.Info(string.Format("Transaction BitOffReport false , Trx : {0}",trx2.Name));
								return;
							}
							messageData.MessageBody.EventName = trx2.Name;
							messageData.MessageBody.EventValue = (e.Flag ? "1" : "0");
						}
					}
					this.OnVirtualEQPEventReceived(this, messageData);
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		private void Unit_OnConnected(object sender, ConnectedEventArgs e)
		{
			IMNetUnit iMNetUnit = sender as IMNetUnit;
			iMNetUnit.ThreadStart();
			iMNetUnit.StopConnect();
            /* cxm 20191210
			if (m_bConnection)
			{
				//cxm 20191210 this.OnConnected(this);
                this.OnConnected(sender);//cxm 20191210
			}
			m_bConnection = true;
             * */

            this.OnConnected(sender);
            mpUnitConected = true;
            m_bConnection = mpUnitConected & mpScanUnitConected;   //cxm 20191210

			logger.Info(string.Format("MelsecEthernet OnConnected : {0} ",iMNetUnit.Name));
		}

		private void Unit_OnDisconnected(object sender, DisconnectedEventArgs e)
		{
			IMNetUnit iMNetUnit = sender as IMNetUnit;
            /* cxm 20191210
			if (!m_bConnection)
			{
				this.OnDisconnected(this);
			}
			m_bConnection = false;
             */
            this.OnDisconnected(sender);
            mpUnitConected = false;
            m_bConnection = mpUnitConected & mpScanUnitConected;  //cxm 20191210

			iMNetUnit.ThreadStop();
			iMNetUnit.Close();
			iMNetUnit.ReConnect();
			logger.Info(string.Format("MelsecEthernet OnDisconnected : {0} ",iMNetUnit.Name));
		}

		private void mPScanUnit_OnConnected(object sender, ConnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			iMNetScanUnit.ScanReceived += unit_EventScanReceived;
			iMNetScanUnit.FDCScanReceived += unit_TraceDataReport;
			iMNetScanUnit.LinkSignalScanReceived += unit_LinkSignalScanReceived;
			iMNetScanUnit.RGAScanReceived += unit_RGADataReport;
			iMNetScanUnit.ThreadStart();
			iMNetScanUnit.StopReconnect();
            /* cxm 20191210
			if (m_bConnection)
			{
				//this.OnConnected(this);
                this.OnConnected(sender); //cxm 20191210
			}
			m_bConnection = true;
             */
            this.OnConnected(sender);
            mpScanUnitConected = true;
            m_bConnection = mpUnitConected & mpScanUnitConected;  //cxm 20191210
            

			logger.Info(string.Format("Scan MelsecEthernet OnConnected : {0} ",iMNetScanUnit.ConnectionUnitName));
		}

		private void mPScanUnit_OnDisconnected(object sender, DisconnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			iMNetScanUnit.ScanReceived -= unit_EventScanReceived;
			iMNetScanUnit.FDCScanReceived -= unit_TraceDataReport;
			iMNetScanUnit.RGAScanReceived -= unit_RGADataReport;
			if (iMNetScanUnit.IsConnection)
			{
                /* cxm 20191210
				iMNetScanUnit.IsConnection = false;
				m_bConnection = false;
				if (!m_bConnection)
				{
					this.OnDisconnected(this);
				}
              */
                this.OnDisconnected(sender); //cxm 20191210
                mpScanUnitConected = false; //cxm 20191210
                m_bConnection = mpUnitConected & mpScanUnitConected; //cxm 20191210


				iMNetScanUnit.ThreadStop();
				iMNetScanUnit.Close();
				iMNetScanUnit.StartReconnect();
				logger.Info(string.Format("Scan MelsecEthernet OnDisconnected : {0} ",iMNetScanUnit.ConnectionUnitName));
			}
		}

		private void mOnDemandUnit_OnConnected(object sender)
		{
			MNetOnDemand mNetOnDemand = sender as MNetOnDemand;
			mNetOnDemand.ScanReceived += unit_EventScanReceived;
			mNetOnDemand.ThreadStart();
			mNetOnDemand.StopConnect();
			if (m_bConnection)
			{
				this.OnConnected(this);
			}
			m_bConnection = true;
			logger.Info(string.Format("Scan MelsecEthernet OnConnected : {0} ",mNetOnDemand.Name));
		}

		private void mOnDemandUnit_OnDisconnected(object sender)
		{
			MNetOnDemand mNetOnDemand = sender as MNetOnDemand;
			mNetOnDemand.ScanReceived -= unit_EventScanReceived;
			if (mNetOnDemand.IsConnection)
			{
				mNetOnDemand.IsConnection = false;
				if (!m_bConnection)
				{
					this.OnDisconnected(this);
				}
				m_bConnection = false;
				mNetOnDemand.ThreadStop();
				mNetOnDemand.Close();
				mNetOnDemand.ReConnect();
				if (mPUnitList.ContainsKey(mNetOnDemand.Name))
				{
					Unit_OnDisconnected(mPUnitList[mNetOnDemand.Name], null);
				}
				if (mPScanUnitList.ContainsKey(mNetOnDemand.Name))
				{
					mPScanUnit_OnDisconnected(mPScanUnitList[mNetOnDemand.Name], null);
				}
				logger.Info(string.Format("Scan MelsecEthernet OnDisconnected : {0} ",mNetOnDemand.Name));
			}
		}

		private void vEQPScanner_Connected(object sender, ConnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			iMNetScanUnit.VirtaulEQPScanReceived += unit_VirtaulEQPScanReceived;
			iMNetScanUnit.ThreadStart();
			iMNetScanUnit.StopReconnect();
			logger.Info(string.Format("vEQPScanner OnConnected : {0} ",iMNetScanUnit.ConnectionUnitName));
		}

		private void vEQPScanner_Disconnected(object sender, DisconnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			iMNetScanUnit.VirtaulEQPScanReceived -= unit_VirtaulEQPScanReceived;
			if (iMNetScanUnit.IsConnection)
			{
				iMNetScanUnit.IsConnection = false;
				iMNetScanUnit.ThreadStop();
				iMNetScanUnit.Close();
				iMNetScanUnit.StartReconnect();
				logger.Info(string.Format("vEQPScanner OnDisconnected : {0} ",iMNetScanUnit.ConnectionUnitName));
			}
		}

		private void mPCacheUnit_OnConnected(object sender, ConnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			iMNetScanUnit.CacheThreadStart();
			iMNetScanUnit.StopReconnect();
			if (m_bConnection)
			{
				this.OnConnected(this);
			}
			m_bConnection = true;
			logger.Info(string.Format("CacheUnit OnConnected : {0} ",iMNetScanUnit.ConnectionUnitName));
		}

		private void mPCacheUnit_OnDisconnected(object sender, DisconnectedEventArgs e)
		{
			IMNetScanUnit iMNetScanUnit = sender as IMNetScanUnit;
			if (iMNetScanUnit.IsConnection)
			{
				iMNetScanUnit.IsConnection = false;
				if (!m_bConnection)
				{
					this.OnDisconnected(this);
				}
				m_bConnection = false;
				iMNetScanUnit.CacheThreadStop();
				iMNetScanUnit.Close();
				iMNetScanUnit.StartReconnect();
				logger.Info(string.Format("CacheUnit OnDisconnected : {} ",iMNetScanUnit.ConnectionUnitName));
			}
		}

		private BlockMap SetPlcMap(XmlSerializer xmlSerialize, ItemGroupCollection itemGroupCollection, XmlNode xml)
		{
			try
			{
				ItemGroup itemGroup = new ItemGroup();
				ItemGroup itemGroup2 = new ItemGroup();
				Block block = new Block();
				Item[] array = null;
				foreach (XmlNode childNode in xml.ChildNodes)
				{
					block = new Block();
					if (childNode.NodeType == XmlNodeType.Element)
					{
						block = (Block)xmlSerialize.Deserialize(new StringReader(childNode.OuterXml));
						itemGroup = new ItemGroup();
						string[] array2 = block.HeadDevice.Split('x');
						block.HeadDevice = ((array2.Length > 1) ? array2[1] : array2[0]);
						if (block.ItemGroup != null)
						{
							itemGroup = (from groupName in itemGroupCollection.ItemGroup
							where groupName.Name == block.ItemGroup.Name
							select groupName).FirstOrDefault();
							itemGroup2 = new ItemGroup();
							itemGroup2 = (ItemGroup)itemGroup.Clone();
							if (block.Item != null)
							{
								array = null;
								array = new Item[block.Item.Length + itemGroup2.Item.Length];
								int num = 0;
								Item[] item = itemGroup2.Item;
								foreach (Item value in item)
								{
									array.SetValue(value, num);
									num++;
								}
								item = block.Item;
								foreach (Item value in item)
								{
									array.SetValue(value, num);
									num++;
								}
								block.Item = array;
							}
							else
							{
								block.Item = itemGroup2.Item;
							}
						}
						if (block.Item != null)
						{
							Item[] item = block.Item;
							foreach (Item item2 in item)
							{
								string[] array3 = item2.Offset.Split(':');
								item2.ItemAddress = Convert.ToString(Convert.ToInt32(block.HeadDevice, 16) + Convert.ToInt32(array3[0]), 16).PadLeft(4, '0').ToUpper();
							}
							m_BlockMap.Add(block);
						}
					}
					else if (childNode.NodeType == XmlNodeType.EntityReference)
					{
						SetPlcMap(xmlSerialize, itemGroupCollection, childNode);
					}
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
			return m_BlockMap;
		}

		private DataGathering SetDataGathering(XmlSerializer xmlSerialize, BlockMap blockMap, XmlNode xml)
		{
			DataGathering dataGathering = (DataGathering)xmlSerialize.Deserialize(new StringReader(xml.OuterXml));
			try
			{
				if (dataGathering.Scan == null)
				{
					return dataGathering;
				}
				if (dataGathering.Scan.MultiBlock == null)
				{
					return dataGathering;
				}
				if (dataGathering.Scan.MultiBlock.Length <= 0)
				{
					return dataGathering;
				}
				MultiBlock[] array = new MultiBlock[dataGathering.Scan.MultiBlock.Length];
				int num = 0;
				MultiBlock[] multiBlock = dataGathering.Scan.MultiBlock;
				foreach (MultiBlock multiBlock2 in multiBlock)
				{
					if (multiBlock2.Block.Length > 0)
					{
						MultiBlock multiBlock3 = new MultiBlock();
						Block[] array2 = new Block[multiBlock2.Block.Length];
						int num2 = 0;
						Block[] block2 = multiBlock2.Block;
						Block b;
						for (int j = 0; j < block2.Length; j++)
						{
							b = block2[j];
							Block block3 = (from block in blockMap.Block
							where block.Name == b.Name
							select block).FirstOrDefault();
							if (block3 != null)
							{
								array2.SetValue(block3, num2);
								num2++;
							}
						}
						multiBlock3.Block = array2;
						array.SetValue(multiBlock3, num);
						num++;
					}
				}
				for (int k = 0; k < dataGathering.Scan.MultiBlock.Length; k++)
				{
					dataGathering.Scan.MultiBlock[k].Block = array[k].Block;
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
			return dataGathering;
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
				}
			}
			return mdata;
		}

		private bool WriteNormalData(MessageData<PLCMessageBody> data)
		{
			int num = 0;
			if (data.MachineName == null)
			{
				if (mPUnitList.Count != 1)
				{
					logger.Error("MachineName is null, EQPIO can not Understand");
					return false;
				}
				foreach (KeyValuePair<string, IMNetUnit> mPUnit in mPUnitList)
				{
					data.MachineName = mPUnit.Key;
				}
			}
			try
			{
				foreach (KeyValuePair<string, PLCMap> mMap in mMapList)
				{
					if (!mMap.Value.bDirectAccess)
					{
						foreach (Block item in mMap.Value.blockMap.Block)
						{
							if (data.MessageBody.WriteDataList.ContainsKey(item.Name))
							{
								num++;
								switch (item.DeviceCode)
								{
								case "B":
									if (!mPUnitList[mMap.Key].WriteBit(item, data.MessageBody.WriteDataList[item.Name]))
									{
										return false;
									}
									break;
								case "W":
									if (!mPUnitList[mMap.Key].WriteWord(item, data.MessageBody.WriteDataList[item.Name]))
									{
										return false;
									}
									break;
								case "R":
								{
									string[] array2 = data.MachineName.Split(',');
									if (!mPUnitList[mMap.Key].WriteRWord(item, data.MessageBody.WriteDataList[item.Name], int.Parse(array2[0]), int.Parse(array2[1])))
									{
										return false;
									}
									break;
								}
								case "ZR":
								{
									string[] array5 = data.MachineName.Split(',');
									if (!mPUnitList[mMap.Key].WriteWord(item, data.MessageBody.WriteDataList[item.Name]))
									{
										return false;
									}
									break;
								}
								case "X":
								{
									string[] array4 = data.MachineName.Split(',');
									if (!mPUnitList[mMap.Key].WriteRBit(item, data.MessageBody.WriteDataList[item.Name], int.Parse(array4[0]), int.Parse(array4[1])))
									{
										return false;
									}
									break;
								}
								case "Y":
								{
									string[] array6 = data.MachineName.Split(',');
									if (!mPUnitList[mMap.Key].WriteRBit(item, data.MessageBody.WriteDataList[item.Name], int.Parse(array6[0]), int.Parse(array6[1])))
									{
										return false;
									}
									break;
								}
								case "D":
								{
									string[] array3 = data.MachineName.Split(',');
									if (array3.Length > 1)
									{
										if (!mPUnitList[mMap.Key].WriteRWord(item, data.MessageBody.WriteDataList[item.Name], int.Parse(array3[0]), int.Parse(array3[1])))
										{
											return false;
										}
									}
									else if (!mPUnitList[mMap.Key].WriteWord(item, data.MessageBody.WriteDataList[item.Name]))
									{
										return false;
									}
									break;
								}
								case "M":
								{
									string[] array = data.MachineName.Split(',');
									if (!mPUnitList[mMap.Key].WriteRBit(item, data.MessageBody.WriteDataList[item.Name], int.Parse(array[0]), int.Parse(array[1])))
									{
										return false;
									}
									break;
								}
								}
							}
						}
					}
				}
				logger.Info("Write NormalData");
				return num > 0;
			}
			catch (Exception arg)
			{
				logger.Error(string.Format("WriteData Error : {0}",arg));
				return false;
			}
		}

		private MessageData<PLCMessageBody> DataCollect(MessageData<PLCMessageBody> mdata, Trx trx, bool flag)
		{
			MultiBlock[] multiBlock = trx.MultiBlock;
            if(multiBlock!=null)
            {
			foreach (MultiBlock multiBlock2 in multiBlock)
			{
				int num;
				switch (flag)
				{
				case false:
					if (!trx.BitOffReadAction)
					{
						num = 1;
						break;
					}
					goto default;
				default:
					num = ((!(multiBlock2.Action == "R")) ? 1 : 0);
					break;
				}
				if (num == 0)
				{
					Block[] block = multiBlock2.Block;
					foreach (Block block2 in block)
					{
						mdata.MessageBody.ReadDataList.Add(block2.Name, null);
					}
				}
				else
				{
					int num2;
					switch (flag)
					{
					case false:
						if (!trx.BitOffEvent)
						{
							num2 = 1;
							break;
						}
						goto default;
					default:
						num2 = ((!(multiBlock2.Action == "W")) ? 1 : 0);
						break;
					}
					if (num2 == 0)
					{
						Block[] block = multiBlock2.Block;
						foreach (Block block2 in block)
						{
							Dictionary<string, string> dictionary = new Dictionary<string, string>();
							if (block2.Item == null)
							{
                                logger.Error(string.Format("Block Item is Null, Trx Name {0}, Block Name : {1}", trx.Name, block2.Name));
							}
							else
							{
								Item[] item = block2.Item;
								foreach (Item item2 in item)
								{
									if (item2.SyncValue)
									{
										dictionary.Add(item2.Name, flag ? "1" : "0");
									}
									else
									{
										dictionary.Add(item2.Name, item2.Value);
									}
								}
								mdata.MessageBody.WriteDataList.Add(block2.Name, dictionary);
							}
						}
					}
				}
			}//end foreach
        }
			return mdata;
		}

		private MessageData<PLCMessageBody> EventTransaction(MessageData<PLCMessageBody> mData, Trx trx, bool flag)
		{
			if (mData.MessageBody.ReadDataList.Count > 0)
			{
				mData = TransactionReadData(mData, trx);
			}
			if (mData.MessageBody.WriteDataList.Count > 0)
			{
				WriteTransactionData(mData, trx);
			}
			return mData;
		}

		private MessageData<PLCMessageBody> TransactionReadData(MessageData<PLCMessageBody> data, Trx trx)
		{
			using (List<Block>.Enumerator enumerator = mMapList[data.MachineName].blockMap.Block.GetEnumerator())
			{
				Block block;
				while (enumerator.MoveNext())
				{
					block = enumerator.Current;
					string text = (from keyName in data.MessageBody.ReadDataList.Keys
					where keyName == block.Name
					select keyName).FirstOrDefault();
					if (text != null)
					{
						switch (block.DeviceCode)
						{
						case "B":
							data.MessageBody.ReadDataList[text] = mPUnitList[data.MachineName].ReadBit(block, true);
							break;
						case "W":
							data.MessageBody.ReadDataList[text] = mPUnitList[data.MachineName].ReadWordOnce(block, true);
							break;
						case "R":
						case "ZR":
							data.MessageBody.ReadDataList[text] = mPUnitList[data.MachineName].ReadWordOnce(block, false);
							break;
						}
					}
				}
			}
			return data;
		}

		private bool WriteTransactionData(MessageData<PLCMessageBody> data, Trx trx)
		{
			lock (writeTransactionObj)
			{
				int num = 0;
				int networkNo = 0;
				int pcNo = 0;
				MultiBlock[] multiBlock = trx.MultiBlock;
				foreach (MultiBlock multiBlock2 in multiBlock)
				{
					if (!string.IsNullOrEmpty(multiBlock2.NetworkNo))
					{
						networkNo = Convert.ToInt32(multiBlock2.NetworkNo);
					}
					if (!string.IsNullOrEmpty(multiBlock2.NetworkNo))
					{
						pcNo = Convert.ToInt32(multiBlock2.PCNo);
					}
					Block[] block2 = multiBlock2.Block;
					Block sendBlock;
					for (int j = 0; j < block2.Length; j++)
					{
						sendBlock = block2[j];
						Block block3 = null;
						string text = string.Empty;
						foreach (KeyValuePair<string, PLCMap> mMap in mMapList)
						{
							block3 = (from block in mMap.Value.blockMap.Block
							where block.Name == sendBlock.Name
							select block).FirstOrDefault();
							if (block3 != null)
							{
								text = mMap.Key;
								break;
							}
						}
						if (block3 != null && !string.IsNullOrEmpty(text) && data.MessageBody.WriteDataList.ContainsKey(block3.Name))
						{
							num++;
							switch (block3.DeviceCode)
							{
							case "B":
								if (!mPUnitList[text].WriteBit(block3, data.MessageBody.WriteDataList[block3.Name]))
								{
									return false;
								}
								break;
							case "W":
								if (!mPUnitList[text].WriteWord(block3, data.MessageBody.WriteDataList[block3.Name]))
								{
									return false;
								}
								break;
							case "R":
								if (!mPUnitList[text].WriteRWord(block3, data.MessageBody.WriteDataList[block3.Name], networkNo, pcNo))
								{
									return false;
								}
								break;
							case "ZR":
								if (!mPUnitList[text].WriteRWord(block3, data.MessageBody.WriteDataList[block3.Name], networkNo, pcNo))
								{
									return false;
								}
								break;
							case "D":
								if (!mPUnitList[text].WriteRWord(block3, data.MessageBody.WriteDataList[block3.Name], networkNo, pcNo))
								{
									return false;
								}
								break;
							}
						}
					}
				}
				logger.Info("Write TransactionData");
				return num > 0;
			}
		}


        public bool InitByInnerResource()
        {

            string mnConfigString = GetResource.GetEmbeddedResource("MNetConfig.xml") as string;


            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                XmlDocument xmlDocument2 = new XmlDocument();
               // xmlDocument2.Load(driver.ConnectionInfo.path);
                xmlDocument2.LoadXml(mnConfigString);
                XmlNode xmlNode = xmlDocument2.SelectSingleNode("Configuration");
                ethernetConfig = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                EQPIO.Common.ConnectionInfo[] connectionInfo = ethernetConfig.ConnectionInfo;
                foreach (EQPIO.Common.ConnectionInfo connectionInfo2 in connectionInfo)
                {
                    m_DataGathering = new DataGathering();
                    m_Transaction = new Transaction();
                    m_BlockMap = new BlockMap();
                    //xmlDocument.Load(connectionInfo2.PLCMapFile);
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
                    m_DataGathering = SetDataGathering(xmlSerializer, m_BlockMap, xmlNode);
                    xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
                    xmlSerializer = new XmlSerializer(typeof(Transaction));
                    m_Transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                    if (!string.IsNullOrEmpty(connectionInfo2.IsVirtualEQPUsed) && !string.IsNullOrEmpty(connectionInfo2.VirtualEQPPLCMapFile) && connectionInfo2.IsVirtualEQPUsed.ToLower() == "true" && File.Exists(connectionInfo2.VirtualEQPPLCMapFile))
                    {
                        DataGathering dataGathering = new DataGathering();
                        Transaction transaction = new Transaction();
                        Globalproperties.Instance.VirtaulEQPPLCMap = connectionInfo2.VirtualEQPPLCMapFile;
                        xmlDocument.Load(connectionInfo2.VirtualEQPPLCMapFile);
                        xmlNode = xmlDocument.SelectSingleNode("PLCDriver/DataGathering");
                        xmlSerializer = new XmlSerializer(typeof(DataGathering));
                        dataGathering = SetDataGathering(xmlSerializer, m_BlockMap, xmlNode);
                        xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
                        xmlSerializer = new XmlSerializer(typeof(Transaction));
                        transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                        if (!mMapList.ContainsKey(connectionInfo2.LocalName))
                        {
                            mMapList.Add(connectionInfo2.LocalName, new PLCMap(connectionInfo2, m_BlockMap, m_DataGathering, m_Transaction, dataGathering, transaction, false));
                        }
                    }
                    else if (!mMapList.ContainsKey(connectionInfo2.LocalName))
                    {
                        mMapList.Add(connectionInfo2.LocalName, new PLCMap(connectionInfo2, m_BlockMap, m_DataGathering, m_Transaction, (connectionInfo2.DirectAccess.ToLower() == "true") ? true : false));
                    }
                }
                logger.Info("MProtocolProxy : InitXxml");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("MProtocolProxy InitXml Error : {0}", ex.Message));
                return false;
            }




        }

		public bool InitXml(Driver driver)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
				XmlDocument xmlDocument2 = new XmlDocument();
				xmlDocument2.Load(driver.ConnectionInfo.path);
				XmlNode xmlNode = xmlDocument2.SelectSingleNode("Configuration");
				ethernetConfig = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
				EQPIO.Common.ConnectionInfo[] connectionInfo = ethernetConfig.ConnectionInfo;
				foreach (EQPIO.Common.ConnectionInfo connectionInfo2 in connectionInfo)
				{
					m_DataGathering = new DataGathering();
					m_Transaction = new Transaction();
					m_BlockMap = new BlockMap();
					xmlDocument.Load(connectionInfo2.PLCMapFile);
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/ItemGroupCollection");
					xmlSerializer = new XmlSerializer(typeof(ItemGroupCollection));
					ItemGroupCollection itemGroupCollection = (ItemGroupCollection)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/BlockMap");
					xmlSerializer = new XmlSerializer(typeof(Block));
					SetPlcMap(xmlSerializer, itemGroupCollection, xmlNode);
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/DataGathering");
					xmlSerializer = new XmlSerializer(typeof(DataGathering));
					m_DataGathering = SetDataGathering(xmlSerializer, m_BlockMap, xmlNode);
					xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
					xmlSerializer = new XmlSerializer(typeof(Transaction));
					m_Transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					if (!string.IsNullOrEmpty(connectionInfo2.IsVirtualEQPUsed) && !string.IsNullOrEmpty(connectionInfo2.VirtualEQPPLCMapFile) && connectionInfo2.IsVirtualEQPUsed.ToLower() == "true" && File.Exists(connectionInfo2.VirtualEQPPLCMapFile))
					{
						DataGathering dataGathering = new DataGathering();
						Transaction transaction = new Transaction();
						Globalproperties.Instance.VirtaulEQPPLCMap = connectionInfo2.VirtualEQPPLCMapFile;
						xmlDocument.Load(connectionInfo2.VirtualEQPPLCMapFile);
						xmlNode = xmlDocument.SelectSingleNode("PLCDriver/DataGathering");
						xmlSerializer = new XmlSerializer(typeof(DataGathering));
						dataGathering = SetDataGathering(xmlSerializer, m_BlockMap, xmlNode);
						xmlNode = xmlDocument.SelectSingleNode("PLCDriver/Transaction");
						xmlSerializer = new XmlSerializer(typeof(Transaction));
						transaction = (Transaction)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
						if (!mMapList.ContainsKey(connectionInfo2.LocalName))
						{
							mMapList.Add(connectionInfo2.LocalName, new PLCMap(connectionInfo2, m_BlockMap, m_DataGathering, m_Transaction, dataGathering, transaction, false));
						}
					}
					else if (!mMapList.ContainsKey(connectionInfo2.LocalName))
					{
						mMapList.Add(connectionInfo2.LocalName, new PLCMap(connectionInfo2, m_BlockMap, m_DataGathering, m_Transaction, (connectionInfo2.DirectAccess.ToLower() == "true") ? true : false));
					}
				}
				logger.Info("MProtocolProxy : InitXxml");
				return true;
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("MProtocolProxy InitXml Error : {0}",ex.Message));
				return false;
			}
		}

		public bool InitMEthernet()
		{
			foreach (string key in mMapList.Keys)
			{
				string[] array = mMapList[key].connectionInfo.MelsecPort.Split(',');
				string localName = mMapList[key].connectionInfo.LocalName;
				string[] array2 = null;
				if (!string.IsNullOrEmpty(mMapList[key].connectionInfo.VirtualEQPPort))
				{
					array2 = mMapList[key].connectionInfo.VirtualEQPPort.Split(',');
				}
				switch (mMapList[key].connectionInfo.CPUType)
				{
				case "A":
					mPUnitList.Add(localName, new MNet1EUnit(mMapList[key].connectionInfo, mMapList[key].dataGathering, mMapList[key].blockMap, mMapList[key].transaction, int.Parse(array[0])));
					if (mMapList[key].connectionInfo.IsMelsecEnabled.ToLower() == "true")
					{
						mPScanUnitList.Add(localName, new MNet1EScanUnit(mMapList[key].connectionInfo, mMapList[key].dataGathering, mMapList[key].blockMap, int.Parse(array[1])));
					}
					break;
				case "Q":
					mPUnitList.Add(localName, new MNet3EUnit(mMapList[key].connectionInfo, mMapList[key].dataGathering, mMapList[key].blockMap, mMapList[key].transaction, int.Parse(array[0])));
					if (mMapList[key].connectionInfo.IsMelsecEnabled.ToLower() == "true")
					{
						mPScanUnitList.Add(localName, new MNet3EScanUnit(mMapList[key].connectionInfo, mMapList[key].dataGathering, mMapList[key].blockMap, int.Parse(array[1])));
					}
					if (!string.IsNullOrEmpty(mMapList[key].connectionInfo.IsVirtualEQPUsed) && mMapList[key].connectionInfo.IsVirtualEQPUsed.ToLower() == "true")
					{
						m_dicVirtualEQPRequestUnit.Add(localName, new MNet3EUnit(mMapList[key].connectionInfo, mMapList[key].vEQPdataGathering, mMapList[key].blockMap, mMapList[key].vEQPtransaction, int.Parse(array2[0])));
						MNet3EScanUnit mNet3EScanUnit = new MNet3EScanUnit(mMapList[key].connectionInfo, mMapList[key].vEQPdataGathering, mMapList[key].blockMap, int.Parse(array2[1]));
						mNet3EScanUnit.RunVirtualEQPScan = true;
						mNet3EScanUnit.Connected += vEQPScanner_Connected;
						mNet3EScanUnit.Disconnected += vEQPScanner_Disconnected;
						m_dicVirtualEQPScanUnit.Add(localName, mNet3EScanUnit);
					}
					break;
				}
				if (mPUnitList.ContainsKey(localName))
				{
					mPUnitList[localName].Connected += Unit_OnConnected;
					mPUnitList[localName].Disconnected += Unit_OnDisconnected;
					if (mMapList[key].connectionInfo.IsFixedBufferEnabled.ToLower() == "true")
					{
						mOnDemandList.Add(localName, new MNetOnDemand(mMapList[key].connectionInfo, mMapList[key].dataGathering, mMapList[key].blockMap));
						mOnDemandList[localName].OnConnected += mOnDemandUnit_OnConnected;
						mOnDemandList[localName].OnDisconnected += mOnDemandUnit_OnDisconnected;
					}
					if (mMapList[key].connectionInfo.IsMelsecEnabled.ToLower() == "true")
					{
						mPScanUnitList[localName].Connected += mPScanUnit_OnConnected;
						mPScanUnitList[localName].Disconnected += mPScanUnit_OnDisconnected;
					}
				}
			}
			logger.Info("MProtocolProxy : InitMEthernet");
			return true;
		}

		public void Connect()
		{
			Dictionary<string, IMNetUnit>.ValueCollection.Enumerator enumerator = mPUnitList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMNetUnit current = enumerator.Current;
					current.Init();
				}
			}
			finally
			{
				
                var d = (IDisposable)enumerator;
                ((IDisposable)enumerator).Dispose();
			}
			Dictionary<string, IMNetScanUnit>.ValueCollection.Enumerator enumerator2 = mPScanUnitList.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					IMNetScanUnit current2 = enumerator2.Current;
					current2.Init();
				}
			}
			finally
			{
               
				((IDisposable)enumerator2).Dispose();
			}
            if (mOnDemandList!=null&&mOnDemandList.Count>0)
            {
			foreach (MNetOnDemand value in mOnDemandList.Values)
			{
				value.Init();
			}
			enumerator = m_dicVirtualEQPRequestUnit.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
                {
					IMNetUnit current = enumerator.Current;
					current.Init();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
            }
			enumerator2 = m_dicVirtualEQPScanUnit.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					IMNetScanUnit current2 = enumerator2.Current;
					current2.Init();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		public void Close()
		{
			Dictionary<string, IMNetUnit>.ValueCollection.Enumerator enumerator = mPUnitList.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMNetUnit current = enumerator.Current;
					current.ThreadClose();
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Dictionary<string, IMNetScanUnit>.ValueCollection.Enumerator enumerator2 = mPScanUnitList.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					IMNetScanUnit current2 = enumerator2.Current;
					current2.ThreadClose();
					current2.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			foreach (MNetOnDemand value in mOnDemandList.Values)
			{
				value.ThreadClose();
				value.Close();
			}
			enumerator = m_dicVirtualEQPRequestUnit.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMNetUnit current = enumerator.Current;
					current.ThreadClose();
					current.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator2 = m_dicVirtualEQPScanUnit.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					IMNetScanUnit current2 = enumerator2.Current;
					current2.ThreadClose();
					current2.Close();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		public MessageData<PLCMessageBody> ReadData(MessageData<PLCMessageBody> data)
		{
			if (data.MachineName == null)
			{
				if (mPUnitList.Count != 1)
				{
					return data;
				}
				foreach (KeyValuePair<string, IMNetUnit> mPUnit in mPUnitList)
				{
					data.MachineName = mPUnit.Key;
				}
			}
			try
			{
				foreach (KeyValuePair<string, PLCMap> mMap in mMapList)
				{
					if (!mMap.Value.bDirectAccess)
					{
						foreach (Block item in mMap.Value.blockMap.Block)
						{
							if (data.MessageBody.ReadDataList.ContainsKey(item.Name))
							{
								switch (item.DeviceCode)
								{
								case "B":
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadBit(item, true);
									break;
								case "W":
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadWordOnce(item, true);
									break;
								case "R":
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadWordOnce(item, false);
									break;
								case "ZR":
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadWordOnce(item, false);
									break;
								case "M":
								{
									string[] array4 = data.MachineName.Split(',');
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadRBit(item, false, int.Parse(array4[0]), int.Parse(array4[1]));
									break;
								}
								case "X":
								{
									string[] array3 = data.MachineName.Split(',');
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadRBit(item, true, int.Parse(array3[0]), int.Parse(array3[1]));
									break;
								}
								case "Y":
								{
									string[] array2 = data.MachineName.Split(',');
									data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadRBit(item, true, int.Parse(array2[0]), int.Parse(array2[1]));
									break;
								}
								case "D":
								{
									string[] array = data.MachineName.Split(',');
									if (array.Length > 1)
									{
										data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadRWord(item, false, int.Parse(array[0]), int.Parse(array[1]));
									}
									else
									{
										data.MessageBody.ReadDataList[item.Name] = mPUnitList[mMap.Key].ReadWord(item, false);
									}
									break;
								}
								default:
									logger.Error("MProtocolProxy ReadData : Invalid device");
									break;
								}
								data.ReturnCode = ((data.MessageBody.ReadDataList[item.Name] == null) ? 1 : 0);
							}
						}
					}
				}
				data.ReturnCode = 0;
				logger.Info("ReadData");
			}
			catch (Exception arg)
			{
				data.ReturnCode = 1;
				logger.Error(string.Format("ReadData Error : {0}",arg));
			}
			return data;
		}

		public bool WriteData(MessageData<PLCMessageBody> data)
		{
			if (!string.IsNullOrEmpty(data.MessageBody.EventName))
			{
				Trx trx = null;
				foreach (KeyValuePair<string, PLCMap> mMap in mMapList)
				{
					if (!mMap.Value.bDirectAccess)
					{
						if (mMap.Value.transaction == null || mMap.Value.transaction.Send == null || mMap.Value.transaction.Send.Trx == null)
						{
							break;
						}
						trx = (from trxName in mMap.Value.transaction.Send.Trx
						where trxName.Name == data.MessageBody.EventName
						select trxName).FirstOrDefault();
						if (trx != null)
						{
							break;
						}
					}
				}
				if (trx != null)
				{
					data = WriteTransactionDataCollect(data, trx);
					return WriteTransactionData(data, trx);
				}
				logger.Error(string.Format("transaction invalid - transactionName : {}",data.MessageBody.EventName));
				return false;
			}
			return WriteNormalData(data);
		}

        public Transaction getTrx(string local)
        {
            return MapList[local].transaction;
        }

        public BlockMap getBlockMap(string local)
        {
            return MapList[local].blockMap;
        }
    }
}
