using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class Globalproperties
	{
		private ILog logger = LogManager.GetLogger(typeof(Globalproperties));

		private static Globalproperties m_Instance;

		private Dictionary<string, string> m_dicLoggingFilterItem = new Dictionary<string, string>();

		private Dictionary<string, CheckItem> m_dicTimeoutCheckItem = new Dictionary<string, CheckItem>();

		private string m_strVirtualEQPPLCMap = string.Empty;

		private Dictionary<enumConnectionType, ScanStatusEachConnectionType> m_dicScanStatusMonitoring = new Dictionary<enumConnectionType, ScanStatusEachConnectionType>();

		private string m_strEquipmentName = string.Empty;

		private string m_strIPAddress = string.Empty;

		private bool m_bUseDirectAccess = false;

		private object m_objScanStatusMonitoringLock = new object();

		      public static Globalproperties Instance
        {
            get
            {
                return m_Instance;
            }
        }
                public Dictionary<string, string> DicLoggingFilterItem
        {
            get
            {
                return this.m_dicLoggingFilterItem;
            }
        }
        
        public Dictionary<string, CheckItem> DicTimeoutCheckItem
        {
            get
            {
                return this.m_dicTimeoutCheckItem;
            }
        }


		public string VirtaulEQPPLCMap
		{
			get
			{
				return m_strVirtualEQPPLCMap;
			}
			set
			{
				m_strVirtualEQPPLCMap = value;
			}
		}

	        public Dictionary<enumConnectionType, ScanStatusEachConnectionType> ScanStatusAll
        {
            get
            {
                return this.m_dicScanStatusMonitoring;
            }
        }

		public ScanStatusEachConnectionType ScanStatusPLCBoard
		{
			get
			{
				if (!m_dicScanStatusMonitoring.ContainsKey(enumConnectionType.PLCBoard))
				{
					return new ScanStatusEachConnectionType();
				}
				return m_dicScanStatusMonitoring[enumConnectionType.PLCBoard];
			}
		}

		public ScanStatusEachConnectionType ScanStatusPLCEhternet
		{
			get
			{
				if (!m_dicScanStatusMonitoring.ContainsKey(enumConnectionType.PLCEthernet))
				{
					return new ScanStatusEachConnectionType();
				}
				return m_dicScanStatusMonitoring[enumConnectionType.PLCEthernet];
			}
		}

		public ScanStatusEachConnectionType ScanStatusEIP
		{
			get
			{
				if (!m_dicScanStatusMonitoring.ContainsKey(enumConnectionType.EIP))
				{
					return new ScanStatusEachConnectionType();
				}
				return m_dicScanStatusMonitoring[enumConnectionType.EIP];
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
				m_strEquipmentName = value;
			}
		}

		public string IPAddress
		{
			get
			{
				return m_strIPAddress;
			}
			set
			{
				m_strIPAddress = value;
			}
		}

		public bool UseDirectAccess
		{
			get
			{
				return m_bUseDirectAccess;
			}
			set
			{
				m_bUseDirectAccess = value;
			}
		}

		public Globalproperties()
		{
			m_Instance = this;
		}

		public void EQPIOOptionInit()
		{
			DefaultLoggingFilterItemSet();
			string empty = string.Empty;
			empty = "..\\EQPIO\\Config\\EQPIOOption.xml";
			if (File.Exists(empty))
			{
				LoadOption(empty);
			}
		}

		private void DefaultLoggingFilterItemSet()
		{
			m_dicLoggingFilterItem.Clear();
			m_dicLoggingFilterItem.Add("MACHINEHEARTBEATSIGNAL", "MACHINEHEARTBEATSIGNAL");
			m_dicLoggingFilterItem.Add("EQUIPMENTALIVE", "EQUIPMENTALIVE");
		}

		private void LoadOption(string strFilePath)
		{
			try
			{
				EQPIOOption EQPIOOption = new EQPIOOption();
				XmlDocument xmlDocument = new XmlDocument();
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(EQPIOOption));
				XmlNode xmlNode = null;
				if (File.Exists(strFilePath))
				{
					xmlDocument.Load(strFilePath);
					xmlNode = xmlDocument.SelectSingleNode("EQPIOOption");
					EQPIOOption = (EQPIOOption)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
					if (EQPIOOption != null)
					{
						if (EQPIOOption.LoggingFilterText != null)
						{
							LoggingFilterOptionSet(EQPIOOption.LoggingFilterText);
						}
						if (EQPIOOption.LoggingFilter != null)
						{
							LoggingFilterOptionSet(EQPIOOption.LoggingFilter);
						}
						if (EQPIOOption.TimeoutCheck != null)
						{
							TimeoutCehckOptionSet(EQPIOOption.TimeoutCheck);
						}
					}
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		private void LoggingFilterOptionSet(LoggingFilterText filterText)
		{
			try
			{
				if (filterText.LogText != null)
				{
					LogText[] logText = filterText.LogText;
					foreach (LogText logText2 in logText)
					{
						if (!m_dicLoggingFilterItem.ContainsKey(logText2.Text))
						{
							m_dicLoggingFilterItem.Add(logText2.Text.ToUpper(), logText2.Text.ToUpper());
						}
					}
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		private void LoggingFilterOptionSet(LoggingFilter filter)
		{
			try
			{
				if (filter.FilterItem != null)
				{
					FilterItem[] filterItem = filter.FilterItem;
					foreach (FilterItem filterItem2 in filterItem)
					{
						if (!m_dicLoggingFilterItem.ContainsKey(filterItem2.Name))
						{
							m_dicLoggingFilterItem.Add(filterItem2.Name.ToUpper(), filterItem2.Name.ToUpper());
						}
					}
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		private void TimeoutCehckOptionSet(TimeoutCheck timeoutCheck)
		{
			try
			{
				if (timeoutCheck.CheckItem != null)
				{
					CheckItem[] checkItem = timeoutCheck.CheckItem;
					foreach (CheckItem checkItem2 in checkItem)
					{
						if (!m_dicTimeoutCheckItem.ContainsKey(checkItem2.Name))
						{
							m_dicTimeoutCheckItem.Add(checkItem2.Name.ToUpper(), checkItem2);
						}
					}
				}
			}
			catch (Exception message)
			{
				logger.Error(message);
			}
		}

		public bool UpdateScanStatus(enumConnectionType type, string localName, MultiBlock multiblock, bool onoff)
		{
			lock (m_objScanStatusMonitoringLock)
			{
				ScanStatusEachConnectionType scanStatusEachConnectionType = new ScanStatusEachConnectionType();
				Dictionary<MultiBlock, bool> dictionary = new Dictionary<MultiBlock, bool>();
				if (m_dicScanStatusMonitoring.ContainsKey(type))
				{
					scanStatusEachConnectionType = m_dicScanStatusMonitoring[type];
					if (scanStatusEachConnectionType.MultiBlockOnOff.ContainsKey(localName))
					{
						if (scanStatusEachConnectionType.MultiBlockOnOff[localName].ContainsKey(multiblock))
						{
							if (scanStatusEachConnectionType.MultiBlockOnOff[localName][multiblock] != onoff)
							{
								scanStatusEachConnectionType.MultiBlockOnOff[localName][multiblock] = onoff;
                                  logger.Error(string.Format("[{0}] [{1}] [{2}] {3} -> {4}", new object[] { type.ToString(), localName, multiblock.Name, !onoff, onoff }));
							
							}
						}
						else
						{
							scanStatusEachConnectionType.MultiBlockOnOff[localName].Add(multiblock, onoff);
						}
					}
					else
					{
						dictionary.Add(multiblock, onoff);
						scanStatusEachConnectionType.MultiBlockOnOff.Add(localName, dictionary);
					}
				}
				else
				{
					scanStatusEachConnectionType = new ScanStatusEachConnectionType(localName, multiblock, onoff);
					m_dicScanStatusMonitoring.Add(type, scanStatusEachConnectionType);
				}
			}
			return true;
		}

		public string GetFirstIPv4()
		{
			string text = string.Empty;
			int num = 0;
			Regex regex = new Regex("^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])string.Format(");
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (regex.IsMatch(iPAddress.ToString()))
				{
                     text= text + string.Format("[{0}] {1} ", num++,iPAddress.ToString());
				
				}
			}
			return text;
		}
	}
}
