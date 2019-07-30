
namespace HF.BC.Tool.EIPDriver.Data
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Driver.Compiler;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Enums;
    using HF.BC.Tool.EIPDriver.Utils;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class TagFactory
    {
        private Dictionary<string, Block> blockMap = null;
        private XmlDocument doc = null;
        private Dictionary<string, List<Item>> itemGroupMap = null;
        private ILog logger = LogManager.GetLogger(typeof(TagFactory));
        private Dictionary<string, XmlNode> recvTrxElementMap = null;
        private Dictionary<string, MultiTag> scanMap = null;
        private Dictionary<string, object> syncRootTagMap = null;
        private Dictionary<string, Tag> tagMap = null;

        private void CompileEIPMap()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n\r\n========== Start Compile : Device Memory Map ==========\r\n");
            EIPMapCompiler compiler = new EIPMapCompiler();
            compiler.Load(this.doc);
            compiler.CheckAll();
            foreach (CompileResult result in compiler.CompileResultCollection)
            {
                builder.Append(result.ToLogString());
                builder.Append("\r\n");
            }
            builder.AppendFormat("========== Finish Compile : WARN {0}, ERROR {1} ==========\r\n", compiler.WarnCount, compiler.ErrorCount);
            this.logger.Info(builder.ToString());
            if (compiler.ErrorCount > 0)
            {
                throw new Exception("Device Memory Compile Error Detected. Check log file for detail description.");
            }
        }

        public void Configure(string mapFile, bool compile)
        {
            try
            {
                this.doc = new XmlDocument();
                this.doc.Load(mapFile);
                this.RetrievePLCDataMap(compile);
            }
            catch (Exception exception)
            {
                this.doc = null;
                throw exception;
            }
        }

        public bool ContainsTag(string tagName)
        {
            return this.tagMap.ContainsKey(tagName);
        }

        public Block CreateBlock(string blockName)
        {
            if (!this.blockMap.ContainsKey(blockName))
            {
                throw new Exception(string.Format("Could not found block [{0}]", blockName));
            }
            Block block = this.blockMap[blockName];
            return (Block)block.Clone();
        }

        public Trx CreateReceiveTrx(Item item)
        {
            XmlNode node;
            Tag tag3;
            Block block = this.CreateBlock(item.ParentName);
            Tag tag = this.CreateTag(block.ParentName);
            string key = string.Format("{0}.{1}.{2}", tag.Name, block.Name, item.Name);
            if (!this.recvTrxElementMap.ContainsKey(key))
            {
                key = string.Format("{0}.{1}.*", tag.Name, block.Name);
                if (!this.recvTrxElementMap.ContainsKey(key))
                {
                    return null;
                }
                node = this.recvTrxElementMap[key];
            }
            else
            {
                node = this.recvTrxElementMap[key];
            }
            Trx trx = (Trx)XmlUtils.AttributesToObject(node, typeof(Trx));
            Tag tag2 = tag.CloneWithOutBlock();
            Block block2 = block.CloneWithOutItem();
            Item item2 = (Item)item.Clone();
            block2.AddItem(item2);
            tag2.AddBlock(block2);
            if (item.Value == "0")
            {
                if (trx.BitOffEvent)
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if ((node2.NodeType == XmlNodeType.Element) && (node2.Name == EIPConst.ELEMENT_TAG))
                        {
                            if (node2.Attributes[EIPConst.ATTRIBUTE_ACTION].Value == ActionEnum.R.ToString())
                            {
                                if (trx.BitOffReadAction)
                                {
                                    tag3 = this.Element2TrxTag(node2);
                                    trx.AddTag(tag3);
                                }
                            }
                            else
                            {
                                tag3 = this.Element2TrxTag(node2);
                                trx.AddTag(tag3);
                            }
                        }
                    }
                    trx.EventBit = false;
                }
            }
            else
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if ((node2.NodeType == XmlNodeType.Element) && !(node2.Name != EIPConst.ELEMENT_TAG))
                    {
                        tag3 = this.Element2TrxTag(node2);
                        trx.AddTag(tag3);
                    }
                }
                trx.EventBit = true;
            }
            trx.BitSyncValue = item.Value;
            trx.InitializeItemValue();
            return trx;
        }

        public List<MultiTag> CreateScans()
        {
            List<MultiTag> list = new List<MultiTag>();
            foreach (MultiTag tag in this.scanMap.Values)
            {
                list.Add((MultiTag)tag.Clone());
            }
            return list;
        }

        public Trx CreateSendTrx(string trxName)
        {
            XmlNode node = XmlUtils.SearchChildNode(this.doc.SelectSingleNode(EIPConst.XPATH_TRX_SEND), EIPConst.ELEMENT_TRX, EIPConst.ATTRIBUTE_NAME, trxName);
            if (node != null)
            {
                Trx trx = (Trx)XmlUtils.AttributesToObject(node, typeof(Trx));
                foreach (XmlNode node3 in node.ChildNodes)
                {
                    if ((node3.NodeType == XmlNodeType.Element) && !(node3.Name != EIPConst.ELEMENT_TAG))
                    {
                        Tag tag = this.Element2TrxTag(node3);
                        trx.AddTag(tag);
                    }
                }
                trx.InitializeItemValue();
                return trx;
            }
            return null;
        }

        public Tag CreateTag(string tagName)
        {
            if (!this.tagMap.ContainsKey(tagName))
            {
                throw new Exception(string.Format("Could not found tag [{0}]", tagName));
            }
            Tag tag = this.tagMap[tagName];
            return (Tag)tag.Clone();
        }

        private Block Element2Block(XmlNode node)
        {
            Block block = (Block)XmlUtils.AttributesToObject(node, typeof(Block));
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == EIPConst.ELEMENT_ITEM)
                {
                    this.ExistAttribute(node2, new string[] { EIPConst.ATTRIBUTE_NAME, EIPConst.ATTRIBUTE_OFFSET, EIPConst.ATTRIBUTE_POINTS, EIPConst.ATTRIBUTE_REPRESENTATION });
                    Item item = (Item)XmlUtils.AttributesToObject(node2, typeof(Item));
                    block.AddItem(item);
                }
                else if (node2.Name == EIPConst.ELEMENT_ITEMGROUP)
                {
                    string key = node2.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                    if (!this.itemGroupMap.ContainsKey(key))
                    {
                        throw new Exception("Could not found Item in ItemGroup. " + key);
                    }
                    List<Item> list = this.itemGroupMap[key];
                    foreach (Item item in list)
                    {
                        block.AddItem((Item)item.Clone());
                    }
                }
            }
            return block;
        }

        private MultiTag Element2MultiTag(XmlNode node)
        {
            MultiTag tag = (MultiTag)XmlUtils.AttributesToObject(node, typeof(MultiTag));
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.NodeType == XmlNodeType.Element)
                {
                    Tag tag2 = this.CreateTag(node2.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                    tag.AddTag(tag2);
                }
            }
            return tag;
        }

        private Tag Element2Tag(XmlNode node)
        {
            Tag tag = (Tag)XmlUtils.AttributesToObject(node, typeof(Tag));
            if (tag.Name.Substring(0, 2).Equals(EIPConst.WRITE_TAG_KEY_WORD))
            {
                tag.Action = ActionEnum.W;
            }
            else
            {
                tag.Action = ActionEnum.R;
            }
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == EIPConst.ELEMENT_BLOCK)
                {
                    string key = node2.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                    if (!this.blockMap.ContainsKey(key))
                    {
                        throw new Exception("Could not found Item in Block. " + key);
                    }
                    Block block = this.blockMap[key];
                    block.ParentName = tag.Name;
                    block.Offset = int.Parse(node2.Attributes[EIPConst.ATTRIBUTE_OFFSET].Value);
                    block.Points = int.Parse(node2.Attributes[EIPConst.ATTRIBUTE_POINTS].Value);
                    tag.AddBlock((Block)block.Clone());
                }
            }
            return tag;
        }

        private Tag Element2TrxTag(XmlNode node)
        {
            Tag tag = (Tag)XmlUtils.AttributesToObject(node, typeof(Tag));
            if (tag.Name.Substring(0, 2).Equals(EIPConst.WRITE_TAG_KEY_WORD))
            {
                tag.Action = ActionEnum.W;
            }
            else
            {
                tag.Action = ActionEnum.R;
            }
            Tag tag2 = this.CreateTag(tag.Name);
            tag.Points = tag2.Points;
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == EIPConst.ELEMENT_BLOCK)
                {
                    string key = node2.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                    if (!this.blockMap.ContainsKey(key))
                    {
                        throw new Exception("Could not found Item in Block. " + key);
                    }
                    Block block = null;
                    if (node2.ChildNodes.Count > 0)
                    {
                        block = (Block)XmlUtils.AttributesToObject(node2, typeof(Block));
                        Block block2 = this.CreateBlock(key);
                        block.Offset = block2.Offset;
                        block.Points = block2.Points;
                        block.LogMode = block2.LogMode;
                        foreach (XmlNode node3 in node2.ChildNodes)
                        {
                            if (node3.Name == EIPConst.ELEMENT_ITEM)
                            {
                                string str2 = node3.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                                Item item = (Item)block2.ItemCollection[str2].Clone();
                                XmlAttribute attribute = node3.Attributes[EIPConst.ATTRIBUTE_VALUE];
                                if (!((attribute == null) || string.IsNullOrEmpty(attribute.Value)))
                                {
                                    item.Value = attribute.Value;
                                }
                                attribute = node3.Attributes[EIPConst.ATTRIBUTE_SYNCVALUE];
                                if (!((attribute == null) || string.IsNullOrEmpty(attribute.Value)))
                                {
                                    item.SyncValue = Convert.ToBoolean(attribute.Value);
                                }
                                block.AddItem(item);
                            }
                        }
                    }
                    else
                    {
                        block = this.CreateBlock(key);
                    }
                    tag.AddBlock(block);
                }
            }
            return tag;
        }

        private void ExistAttribute(XmlNode node, string[] attributeNames)
        {
            if ((attributeNames.Length != 0) && (node.NodeType == XmlNodeType.Element))
            {
                if (node.Attributes == null)
                {
                    throw new Exception("Could not found any attribute in this Element\r\n" + node.ToString());
                }
                foreach (string str in attributeNames)
                {
                    if (node.Attributes[str] == null)
                    {
                        throw new Exception("Could not found " + str + " in this Element\r\n" + node.ToString());
                    }
                }
            }
        }

        public object getSyncTagObject(string tagName)
        {
            return this.syncRootTagMap[tagName];
        }

        public List<string> GetTagNames()
        {
            return this.tagMap.Keys.ToList<string>();
        }

        public string GetTagNames(string blockName)
        {
            foreach (Tag tag in this.tagMap.Values)
            {
                foreach (Block block in tag.BlockCollection.Values)
                {
                    if (block.Name.Equals(blockName))
                    {
                        return tag.Name;
                    }
                }
            }
            return string.Empty;
        }

        public List<Tag> GetTags()
        {
            List<Tag> list = new List<Tag>();
            foreach (Tag tag in this.tagMap.Values)
            {
                list.Add((Tag)tag.Clone());
            }
            return list;
        }

        private void RetrieveBlockMap(XmlNode parentNode)
        {
            try
            {
                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        if ((node.Name == EIPConst.ELEMENT_GROUP) || (node.Name == EIPConst.ELEMENT_ENTITYREF))
                        {
                            this.RetrieveBlockMap(node);
                        }
                        else if (node.Name == EIPConst.ELEMENT_BLOCK)
                        {
                            this.ExistAttribute(node, new string[] { EIPConst.ATTRIBUTE_NAME });
                            Block block = this.Element2Block(node);
                            block.InitializeValue();
                            this.blockMap.Add(block.Name, block);
                        }
                    }
                    else if (node.NodeType == XmlNodeType.EntityReference)
                    {
                        this.RetrieveBlockMap(node);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }

        private void RetrieveItemGroupMap(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if ((node.Name == EIPConst.ELEMENT_GROUP) || (node.Name == EIPConst.ELEMENT_ENTITYREF))
                    {
                        this.RetrieveItemGroupMap(node);
                    }
                    else if (node.Name == EIPConst.ELEMENT_ITEMGROUP)
                    {
                        this.ExistAttribute(node, new string[] { EIPConst.ATTRIBUTE_NAME });
                        List<Item> list = new List<Item>();
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if (node2.NodeType == XmlNodeType.Element)
                            {
                                this.ExistAttribute(node2, new string[] { EIPConst.ATTRIBUTE_NAME, EIPConst.ATTRIBUTE_OFFSET, EIPConst.ATTRIBUTE_POINTS, EIPConst.ATTRIBUTE_REPRESENTATION });
                                Item item = (Item)XmlUtils.AttributesToObject(node2, typeof(Item));
                                list.Add(item);
                            }
                        }
                        this.itemGroupMap.Add(node.Attributes[EIPConst.ATTRIBUTE_NAME].Value, list);
                    }
                }
                else if (node.NodeType == XmlNodeType.EntityReference)
                {
                    this.RetrieveItemGroupMap(node);
                }
            }
        }

        private void RetrievePLCDataMap(bool compile)
        {
            try
            {
                if (compile)
                {
                    this.CompileEIPMap();
                }
                XmlNode parentNode = this.doc.SelectSingleNode(EIPConst.XPATH_ITEMGROUPCOLLECTION);
                if (parentNode == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_ITEMGROUPCOLLECTION);
                }
                this.itemGroupMap = new Dictionary<string, List<Item>>();
                this.RetrieveItemGroupMap(parentNode);
                parentNode = this.doc.SelectSingleNode(EIPConst.XPATH_BLOCKMAP);
                if (parentNode == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_BLOCKMAP);
                }
                this.blockMap = new Dictionary<string, Block>();
                this.RetrieveBlockMap(parentNode);
                parentNode = this.doc.SelectSingleNode(EIPConst.XPATH_TAGMAP);
                if (parentNode == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_TAGMAP);
                }
                this.tagMap = new Dictionary<string, Tag>();
                this.syncRootTagMap = new Dictionary<string, object>();
                this.RetrieveTagMap(parentNode);
                parentNode = this.doc.SelectSingleNode(EIPConst.XPATH_DATAGATHERING_SCAN);
                if (parentNode == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_DATAGATHERING_SCAN);
                }
                this.scanMap = new Dictionary<string, MultiTag>();
                this.RetrieveScanMap(parentNode);
                parentNode = this.doc.SelectSingleNode(EIPConst.XPATH_TRX_RECEIVE);
                if (parentNode == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_TRX_RECEIVE);
                }
                this.recvTrxElementMap = new Dictionary<string, XmlNode>();
                this.RetrieveRecvTrxElementMap(parentNode);
                if (this.doc.SelectSingleNode(EIPConst.XPATH_TRX_SEND) == null)
                {
                    throw new Exception("Could not found XPath. " + EIPConst.XPATH_TRX_SEND);
                }
            }
            catch (Exception exception)
            {
                this.blockMap = null;
                this.tagMap = null;
                this.scanMap = null;
                this.recvTrxElementMap = null;
                throw exception;
            }
        }

        private void RetrieveRecvTrxElementMap(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if ((node.Name == EIPConst.ELEMENT_GROUP) || (node.Name == EIPConst.ELEMENT_ENTITYREF))
                    {
                        this.RetrieveRecvTrxElementMap(node);
                    }
                    else if (node.Name == EIPConst.ELEMENT_TRX)
                    {
                        string key = node.Attributes[EIPConst.ATTRIBUTE_KEY].Value;
                        this.recvTrxElementMap.Add(key, node);
                    }
                }
                else if (node.NodeType == XmlNodeType.EntityReference)
                {
                    this.RetrieveRecvTrxElementMap(node);
                }
            }
        }

        private void RetrieveScanMap(XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if ((node.Name == EIPConst.ELEMENT_GROUP) || (node.Name == EIPConst.ELEMENT_ENTITYREF))
                    {
                        this.RetrieveScanMap(node);
                    }
                    else if (node.Name == EIPConst.ELEMENT_MULTITAG)
                    {
                        this.ExistAttribute(node, new string[] { EIPConst.ATTRIBUTE_NAME, EIPConst.ATTRIBUTE_INTERVAL });
                        MultiTag tag = this.Element2MultiTag(node);
                        tag.Action = ActionEnum.R;
                        this.scanMap.Add(tag.Name, tag);
                    }
                }
                else if (node.NodeType == XmlNodeType.EntityReference)
                {
                    this.RetrieveScanMap(node);
                }
            }
        }

        private void RetrieveTagMap(XmlNode parentNode)
        {
            try
            {
                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        if ((node.Name == EIPConst.ELEMENT_GROUP) || (node.Name == EIPConst.ELEMENT_ENTITYREF))
                        {
                            this.RetrieveTagMap(node);
                        }
                        else if (node.Name == EIPConst.ELEMENT_TAG)
                        {
                            this.ExistAttribute(node, new string[] { EIPConst.ATTRIBUTE_NAME, EIPConst.ATTRIBUTE_POINTS });
                            Tag tag = this.Element2Tag(node);
                            tag.InitializeItemValue();
                            this.tagMap.Add(tag.Name, tag);
                            this.syncRootTagMap.Add(tag.Name, new object());
                        }
                    }
                    else if (node.NodeType == XmlNodeType.EntityReference)
                    {
                        this.RetrieveTagMap(node);
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                throw exception;
            }
        }
    }
}
