using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using WinSECS.global;
using WinSECS.callback;
using WinSECS.structure;
using WinSECS.timeout;


namespace WinSECS.driver
{

    [ComSourceInterfaces("WinSECS.callback.ISECSListener"), ClassInterface(ClassInterfaceType.None)]
    public class SinglePlugIn : ISingleplugIn
    {
        private ISECSConfig config;
        private WinSECS.manager.ManagerFactory managerFactory;

        public event driverIDDelegate onConnected;

        public event driverIDDelegate onDisconnected;

        public event transactionDelegate onIllegal;

        public event logDelegate onLog;

        public event transactionDelegate onReceived;

        public event transactionDelegate onSendComplete;

        public event timeoutDelegate onTimeout;

        public event transactionDelegate onUnknownReceived;

        public SinglePlugIn()
        {
            this.InitBlock();
        }

        public virtual bool AddSECSListener(ISECSListener secsListener)
        {
            this.managerFactory.CallbackManager.addSECSListener(secsListener);
            return true;
        }

        public virtual void close()
        {
            this.managerFactory = null;
        }

        public virtual IReturnObject GetDefinedMessage(int Stream, int Function, string MessageName)
        {
            return this.managerFactory.MessageFactory.GetDefinedMessageByMessageName(Stream, Function, MessageName);
        }

        private void getDriverInfoFromFile(string fileName, string driverId, string xPath, ReturnObject returnObject)
        {
            XmlDocument document = null;
            try
            {
                document = new XmlDocument();
                document.Load(fileName);
                XmlNode returnData = document.SelectSingleNode(xPath + driverId.Trim().ToUpper());
                if (returnData != null)
                {
                    returnObject.setReturnData(returnData);
                }
                else
                {
                    returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFO_FROM_FILE);
                }
            }
            catch (XmlException e)
            {
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_FILE);
            }
            catch (IOException)
            {
                returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFOFILE);
            }
        }

        private void getTypeFromFile(string fileName, string typeName, string xPath, ReturnObject returnObject)
        {
            XmlDocument document = null;
            try
            {
                document = new XmlDocument();
                document.Load(fileName);
                XmlNode node = document.SelectSingleNode(xPath + typeName + "/SMDPATH");
                if (node != null)
                {
                    returnObject.setReturnData(node.InnerText);
                }
                else
                {
                    returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFO_FROM_FILE);
                }
            }
            catch (XmlException)
            {
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_FILE);
            }
            catch (IOException)
            {
                returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFOFILE);
            }
        }

        private void InitBlock()
        {
            this.managerFactory = new WinSECS.manager.ManagerFactory();
        }

        public virtual IReturnObject Initialize()
        {
            ReturnObject obj2 = new ReturnObject();
            if (this.config == null)
            {
                obj2.setError(SEComError.SEComPlugIn.ERR_CANT_DRIVER_NULL);
            }
            else
            {
                return this.Initialize(this.config);
            }
            return obj2;
        }

        public virtual IReturnObject Initialize(ISECSConfig config)
        {
            ReturnObject returnObject = new ReturnObject();
            if (this.config == null)
            {
                this.config = config;
            }
            this.managerFactory.Initialize(this, config as SECSConfig, returnObject);
            return returnObject;
        }

        public virtual IReturnObject Initialize(string driverId)
        {
            ReturnObject returnObject = new ReturnObject();
            string path = driverId + ".xml";
            if (File.Exists(path))
            {
                this.getDriverInfoFromFile(path, driverId, "STATION/SECOMDRIVER/", returnObject);
                if (returnObject.isSuccess())
                {
                    SECSConfig config = SECSConfig.getSECSConfigFromXML(returnObject.getReturnData() as XmlNode);
                    if (config == null)
                    {
                        returnObject.setError("Unknown Error, Please contact Developer");
                    }
                    else
                    {
                        this.getTypeFromFile(path, config.EqpType, "STATION/TYPE/", returnObject);
                        if (returnObject.isSuccess())
                        {
                            config.ModelingInfoFromFile = returnObject.getReturnData() as string;
                            return this.Initialize(config);
                        }
                    }
                    return returnObject;
                }
                returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFO_FROM_FILE);
                return returnObject;
            }
            returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_RELALTED_DRIVERID_INFOFILE);
            return returnObject;
        }

        public virtual IReturnObject Initialize(string driverId, string SEComINIXMLFilePath)
        {
            ReturnObject returnObject = new ReturnObject();
            if (File.Exists(SEComINIXMLFilePath))
            {
                this.getDriverInfoFromFile(SEComINIXMLFilePath, driverId, "STATION/SECOMDRIVERLIST/", returnObject);
                if (returnObject.isSuccess())
                {
                    SECSConfig config = SECSConfig.getSECSConfigFromXML(returnObject.getReturnData() as XmlNode);
                    if (config == null)
                    {
                        returnObject.setError("Unknown Error, Please contact Developer");
                    }
                    else
                    {
                        this.getTypeFromFile(SEComINIXMLFilePath, config.EqpType, "STATION/TYPELIST/", returnObject);
                        if (returnObject.isSuccess())
                        {
                            config.ModelingInfoFromFile = returnObject.getReturnData() as string;
                            return this.Initialize(config);
                        }
                    }
                    return returnObject;
                }
                returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFO_FROM_FILE);
                return returnObject;
            }
            returnObject.setError(SEComError.SEComPlugIn.ERR_NOT_FOUND_DRIVERINFOFILE);
            return returnObject;
        }

        public void onConnectedEvent(string driverID)
        {
            if (this.onConnected != null)
            {
                this.onConnected(driverID);
            }
        }

        public void onDisconnectedEvent(string driverID)
        {
            if (this.onDisconnected != null)
            {
                this.onDisconnected(driverID);
            }
        }

        public void onIllegalEvent(string driverID, SECSTransaction transaction)
        {
            if (this.onIllegal != null)
            {
                this.onIllegal(driverID, transaction);
            }
        }

        public void onLogEvent(string driverID, string log)
        {
            if (this.onLog != null)
            {
                this.onLog(driverID, log);
            }
        }

        public void onReceivedEvent(string driverID, SECSTransaction transaction)
        {
            if (this.onReceived != null)
            {
                this.onReceived(driverID, transaction);
            }
        }

        public void onSendCompleteEvent(string driverID, SECSTransaction transaction)
        {
            if (this.onSendComplete != null)
            {
                this.onSendComplete(driverID, transaction);
            }
        }

        public void onTimeoutEvent(string driverID, SECSTimeout timeout)
        {
            if (this.onTimeout != null)
            {
                this.onTimeout(driverID, timeout);
            }
        }

        public void onUnknownReceivedEvent(string driverID, SECSTransaction transaction)
        {
            if (this.onUnknownReceived != null)
            {
                this.onUnknownReceived(driverID, transaction);
            }
        }

        public virtual void open()
        {
        }

        public virtual void reconnect()
        {
            long disconnectToken = this.managerFactory.ConnectManager.GetDisconnectToken();
            if (disconnectToken != 0L)
            {
                this.managerFactory.ConnectManager.releaseMutext();
                this.managerFactory.ConnectManager.ReleaseDisconnectToken(disconnectToken);
            }
        }

        public virtual IReturnObject ReloadConfiguration(ISECSConfig newConfig, bool enforceReconnect, bool reloadSMD)
        {
            ReturnObject returnObject = new ReturnObject();
            this.managerFactory.ReloadConfig(newConfig as SECSConfig, enforceReconnect, reloadSMD, returnObject);
            return returnObject;
        }

        public IReturnObject ReloadSMD()
        {
            ReturnObject returnObject = new ReturnObject();
            this.managerFactory.ReloadSMD(returnObject);
            return returnObject;
        }

        public IReturnObject ReloadSMD(ISECSConfig newConfig)
        {
            ReturnObject returnObject = new ReturnObject();
            this.managerFactory.ReloadSMD(newConfig as SECSConfig, returnObject);
            return returnObject;
        }

        public virtual IReturnObject reply(ISECSTransaction message)
        {
            return this.managerFactory.ConnectManager.reply(message as SECSTransaction);
        }

        public virtual IReturnObject request(ISECSTransaction message)
        {
            return this.managerFactory.ConnectManager.request(message as SECSTransaction);
        }

        public virtual IReturnObject Terminate()
        {
            ReturnObject returnObject = new ReturnObject();
            this.managerFactory.Terminate(returnObject);
            return returnObject;
        }

        public virtual ISECSConfig Config
        {
            get
            {
                return this.config;
            }
            set
            {
                this.config = value;
            }
        }

        internal WinSECS.manager.ManagerFactory ManagerFactory
        {
            get
            {
                return this.managerFactory;
            }
            set
            {
                this.managerFactory = value;
            }
        }
    }
}
