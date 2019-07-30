using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Xml;
using log4net;
using WinSECS.Utility;
using WinSECS.manager;
using WinSECS.structure;
using WinSECS.global;
using WinSECS.driver;

namespace WinSECS.MessageHandler
{
    internal class ModelingMessageFactory : abstractManager
    {
        private Composer composer;
        private Dispatcher dispatcher;
        private LengthFilterFactory lengthFactory = new LengthFilterFactory();
        private ILog logger;
        private static int MULTIPLE_MAXLENGTH = 1;

        public ModelingMessageFactory()
        {
            this.InitBlock();
        }

        private void AppendSECSTransaction(SECSTransaction trx)
        {
            string sxFx = string.Format("S{0}F{1}", trx.Stream, trx.Function);
            string messageName = trx.MessageName;
            if (base.config.Host)
            {
                if (trx.Direction.Equals("H->E"))
                {
                    this.composer.AddModelingInfo(sxFx, messageName, trx);
                }
                else if (trx.Direction.Equals("H<->E"))
                {
                    this.composer.AddModelingInfo(sxFx, messageName, trx);
                    this.dispatcher.AddModelingInfo(sxFx, messageName, trx);
                }
                else
                {
                    this.dispatcher.AddModelingInfo(sxFx, messageName, trx);
                }
            }
            else if (trx.Direction.Equals("H<-E"))
            {
                this.composer.AddModelingInfo(sxFx, messageName, trx);
            }
            else if (trx.Direction.Equals("H<->E"))
            {
                this.composer.AddModelingInfo(sxFx, messageName, trx);
                this.dispatcher.AddModelingInfo(sxFx, messageName, trx);
            }
            else
            {
                this.dispatcher.AddModelingInfo(sxFx, messageName, trx);
            }
        }

        public virtual void CheckDefinedMessage(SECSTransaction trx, ReturnObject returnObject)
        {
            this.dispatcher.GetAdaptableMessage(trx, returnObject);
        }

        public virtual ReturnObject GetDefinedMessageByMessageName(int Stream, int Function, string MessageName)
        {
            return this.composer.GetDefinedMessageByMessageName(Stream, Function, MessageName);
        }

        public virtual ReturnObject GetDefinedMessageFirstItem(int Stream, int Function)
        {
            return this.composer.GetDefinedMessageFirstItem(Stream, Function);
        }

        public IDictionary<string, object> GetDefinedMessageSet(int Stream, int Function)
        {
            return this.composer.getMessageSet(string.Format("S{0}F{1}", Stream, Function));
        }

        private void GetModelingInfoFromFile(string fileName, ReturnObject returnObject)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(fileName);
            }
            catch (XmlException exception)
            {
                this.logger.Error("File Name=" + fileName + " Error:" + exception.Message);
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_FILE);
                return;
            }
            catch (IOException exception2)
            {
                this.logger.Error("File Name=" + fileName + " Error:" + exception2.Message);
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_FILE_NOT_FOUND);
                return;
            }
            this.InitalizeMessageFactory(doc, returnObject);
        }

        private void GetModelingInfoFromXMLString(string xmlString, ReturnObject returnObject)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml(xmlString);
            }
            catch (XmlException exception)
            {
                this.logger.Error("XML String Error:" + exception.Message);
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_XML_STRING);
                return;
            }
            catch (IOException exception2)
            {
                this.logger.Error("XML String Error:" + exception2.Message);
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_XML_STRING);
                return;
            }
            this.InitalizeMessageFactory(doc, returnObject);
        }

        private static bool getUserDefinedLengthFilter(XmlNode filterElement, LengthFilterFactory lengthFactory)
        {
            try
            {
                for (int i = 0; i < filterElement.ChildNodes.Count; i++)
                {
                    XmlNode node = filterElement.ChildNodes[i];
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        string name = node.Name;
                        int length = int.Parse(node.Value);
                        lengthFactory.add(name, length, true);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void InitalizeMessageFactory(XmlDocument doc, ReturnObject returnObject)
        {
            string str = DateTime.Now.ToString("HH:mm:ss fff");
            if (doc.DocumentElement.Name != "SECSMessage")
            {
                returnObject.setError(SEComError.SEComMessageHanlder.INVALID_MODELING_FILE_WITH_BAD_ROOT_NAME);
            }
            else
            {
                modelingFileParser parser = new modelingFileParser();
                for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                {
                    XmlNode filterElement = doc.DocumentElement.ChildNodes[i];
                    if (filterElement.NodeType == XmlNodeType.Element)
                    {
                        if (filterElement.Name == "FILTER")
                        {
                            getUserDefinedLengthFilter(filterElement, this.lengthFactory);
                        }
                        else
                        {
                            SECSTransaction trx = parser.ParseSECSMessage(filterElement);
                            if (trx != null)
                            {
                                this.AppendSECSTransaction(trx);
                                this.lengthFactory.add(trx.StreamFunctionString, trx.getMaxPossibleByteLength(), false);
                            }
                        }
                    }
                }
                this.logger.Debug("Message Initialize Time StartTime : " + str + " EndTime : " + DateTime.Now.ToString("HH:mm:ss fff"));
                this.logger.Debug(string.Concat(new object[] { "Composer Message Size : ", this.composer.Size(), " Dispatcher Message Size : ", this.dispatcher.Size() }));
                returnObject.setReturnData(doc);
            }
        }

        private void InitBlock()
        {
        }

        public override void Initialize(SinglePlugIn rootHandle, SECSConfig config, ReturnObject returnObject)
        {
            base.Initialize(rootHandle, config, returnObject);
            this.logger = rootHandle.ManagerFactory.LoggerManager.Logger;
            this.composer = new Composer();
            this.dispatcher = new Dispatcher(this.logger);
            if (config.ModelingInfoFromFile.Length > 0)
            {
                this.GetModelingInfoFromFile(config.ModelingInfoFromFile, returnObject);
            }
            else if (config.ModelingInfoFromXMLString.Length > 0)
            {
                this.GetModelingInfoFromXMLString(config.ModelingInfoFromXMLString, returnObject);
            }
            else if (!config.DispatchOn)
            {
                this.logger.Info("[ModelingMessageFactory][Initialize] Not Use Dispatcher(NO SMD Information");
            }
            else
            {
                returnObject.setError(SEComError.SEComMessageHanlder.NO_MODELING_INFO);
            }
        }

        public bool isExistLengthFilter(string SxFy, int messageLength)
        {
            LengthFilterInfo info = this.lengthFactory.getMaxLength(SxFy);
            if (info == null)
            {
                return false;
            }
            int length = info.Length;
            if (!(info.IsUserDefined || (length == 0x7fffffff)))
            {
                length *= MULTIPLE_MAXLENGTH;
            }
            if (length < messageLength)
            {
                return false;
            }
            return true;
        }

        public void ReloadSMD(ReturnObject returnObject)
        {
            this.logger = base.rootHandle.ManagerFactory.LoggerManager.Logger;
            if (this.composer != null)
            {
                this.composer.ClearModelingInfo();
            }
            if (this.dispatcher != null)
            {
                this.dispatcher.ClearModelingInfo();
                this.dispatcher.Logger = this.logger;
            }
            if (base.config.ModelingInfoFromFile.Length > 0)
            {
                this.GetModelingInfoFromFile(base.config.ModelingInfoFromFile, returnObject);
            }
            else if (base.config.ModelingInfoFromXMLString.Length > 0)
            {
                this.GetModelingInfoFromXMLString(base.config.ModelingInfoFromXMLString, returnObject);
            }
            else if (!base.config.DispatchOn)
            {
                this.logger.Info("[ModelingMessageFactory][Initialize] Not Use Dispatcher(NO SMD Information");
            }
            else
            {
                returnObject.setError(SEComError.SEComMessageHanlder.NO_MODELING_INFO);
            }
        }

        public void ReloadSMD(SECSConfig newConfig, ReturnObject returnObject)
        {
            this.logger = base.rootHandle.ManagerFactory.LoggerManager.Logger;
            if (this.composer != null)
            {
                this.composer.ClearModelingInfo();
            }
            if (this.dispatcher != null)
            {
                this.dispatcher.ClearModelingInfo();
                this.dispatcher.Logger = this.logger;
            }
            if (newConfig.ModelingInfoFromFile.Length > 0)
            {
                this.GetModelingInfoFromFile(newConfig.ModelingInfoFromFile, returnObject);
            }
            else if (newConfig.ModelingInfoFromXMLString.Length > 0)
            {
                this.GetModelingInfoFromXMLString(newConfig.ModelingInfoFromXMLString, returnObject);
            }
            else if (!base.config.DispatchOn)
            {
                this.logger.Info("[ModelingMessageFactory][Initialize] Not Use Dispatcher(NO SMD Information");
            }
            else
            {
                returnObject.setError(SEComError.SEComMessageHanlder.NO_MODELING_INFO);
            }
        }

        public override void Terminate(ReturnObject returnObject)
        {
        }
    }
}
