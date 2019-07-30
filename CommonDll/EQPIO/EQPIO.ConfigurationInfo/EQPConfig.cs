using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using EQPIO.Controller;
using EQPIO.Controller.Proxy;
using EQPIO.Common;

namespace EQPIO.ConfigurationInfo
{
    public class EQPConfig
    {

        private ILog logger = LogManager.GetLogger(typeof(EQPConfig));

        private Dictionary<string, EQPIO.Controller.ConnectionInfo> connectionInfoDict = new Dictionary<string, EQPIO.Controller.ConnectionInfo>();

        public Dictionary<string, EQPIO.Controller.ConnectionInfo> ConnectionInfoDict
        {
            get { return ConnectionInfoDict; }
            set { connectionInfoDict = value; }
        }

        private Dictionary<string,PLCMap> mNetPlcMapDict=new Dictionary<string,PLCMap>();

        private bool useMQ;

        public bool UseMQ
        {
            get { return useMQ; }
            set { useMQ = value; }
        }

        private bool useBoard;

        public bool UseBoard
        {
            get { return useBoard; }
            set { useBoard = value; }
        }

        private bool useEthernet;

        public bool UseEthernet
        {
            get { return useEthernet; }
            set { useEthernet = value; }
        }

        private bool useEIP;

        public bool UseEIP
        {
            get { return useEIP; }
            set { useEIP = value; }
        }



        public EQPConfig()
        {
            InitDriverXml("../EQPIO/Config/IOConConfig.xml");
        }

        public EQPConfig(string configFile)
        {
            InitDriverXml(configFile);
        }


        public bool InitDriverXml(string file)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EQPIOConfig));
            try
            {
                //xmlDocument.Load("../EQPIO/Config/IOConConfig.xml");
                xmlDocument.Load(file);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("IOConConfig");
                var m_IOConConfig = (EQPIOConfig)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
                Driver[] driver = m_IOConConfig.Driver;
                foreach (Driver driver2 in driver)
                {
                    switch (driver2.name)
                    {
                        case "MQ":
                            UseMQ = driver2.ConnectionInfo.use;
                            connectionInfoDict.Add("MQ", driver2.ConnectionInfo);
                            break;
                        case "MelsecBoard":
                            UseBoard = driver2.ConnectionInfo.use;
                            connectionInfoDict.Add("MelsecBoard", driver2.ConnectionInfo);
                            if(UseBoard)
                            {

                                InitMNetXml(driver2);
                            }

                            break;
                        case "MelsecEthernet":
                            UseEthernet = driver2.ConnectionInfo.use;
                            connectionInfoDict.Add("MelsecEthernet", driver2.ConnectionInfo);
                            break;
                        case "EIP":
                            UseEIP = driver2.ConnectionInfo.use;
                            connectionInfoDict.Add("EIP", driver2.ConnectionInfo);
                            break;
                    }
                }

                if(useBoard)
                {
                   // InitMNetXml()
                }

            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                return false;
            }

            return true;


        }

        private bool InitMNetXml(Driver driver)
        {
            string msg = "Error message: {0}";
          if(  InitMNetXml(driver, ref msg))
          {
              return true;
          }
          logger.Error(msg);
          return false;
        }

        public bool InitMNetXml(Driver driver, ref string msg)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                XmlDocument xmlDocument2 = new XmlDocument();
                xmlDocument2.Load(driver.ConnectionInfo.path);
                XmlNode xmlNode = xmlDocument2.SelectSingleNode("Configuration");
                Configuration mNetconfig = (Configuration)xmlSerializer.Deserialize(new StringReader(xmlNode.OuterXml));
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
                logger.Error(string.Format("MNetProxy InitXml Error : {0}", ex.Message));
                return false;
            }
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
                            logger.Error(string.Format("Item Offset or Point Error , BlockName : {0}, BlockPoint {1}, ItemName : {2}, ItemOffset : {3}, ItemPoint : {4}", item2.Name, item2.Points, item2.Name, item2.Offset, item2.Points));
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
        
    }
}
