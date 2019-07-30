
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using HF.BC.Tool.EIPDriver.Utils;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal abstract class SubCompiler
    {
        private List<CompileResult> compileResultCollection = new List<CompileResult>();
        private XmlNode rootNode;
        private XmlNode searchRoot;

        protected SubCompiler()
        {
        }

        public void AddCompileResult(CompileResult result)
        {
            if (this.compileResultCollection.IndexOf(result) < 0)
            {
                this.compileResultCollection.Add(result);
            }
        }

        public void AddCompileResultRange(List<CompileResult> results)
        {
            foreach (CompileResult result in results)
            {
                this.AddCompileResult(result);
            }
        }

        protected CompileResult AppendCompileResult(XmlNode resource, ErrorCode errorCode)
        {
            CompileResult result = new CompileResult
            {
                Resource = resource,
                Error = errorCode,
                Root = this.rootNode
            };
            this.AddCompileResult(result);
            return result;
        }

        protected ErrorCode CheckItemDefAttributes(XmlNode itemDef)
        {
            if (itemDef.NodeType != XmlNodeType.Element)
            {
                return ErrorCode.None;
            }
            ErrorCode code = this.CheckItemRepresent(itemDef);
            if (code != ErrorCode.None)
            {
                return code;
            }
            code = this.CheckItemOffsetPointsFormat(itemDef);
            if (code != ErrorCode.None)
            {
                return code;
            }
            return this.CheckItemValue(itemDef, itemDef.Attributes[EIPConst.ATTRIBUTE_VALUE]);
        }

        private ErrorCode CheckItemOffset(XmlNode itemNode)
        {
            if (itemNode.NodeType == XmlNodeType.Element)
            {
                XmlAttribute attribute = itemNode.Attributes[EIPConst.ATTRIBUTE_OFFSET];
                if ((attribute == null) || (attribute.Value == ""))
                {
                    return ErrorCode.ErrorOffsetNull;
                }
            }
            return ErrorCode.None;
        }

        protected ErrorCode CheckItemOffsetPointsFormat(XmlNode itemNode)
        {
            if (itemNode.NodeType == XmlNodeType.Element)
            {
                ErrorCode code = this.CheckItemOffset(itemNode);
                if (code != ErrorCode.None)
                {
                    return code;
                }
                code = this.CheckItemPoints(itemNode);
                if (code != ErrorCode.None)
                {
                    return code;
                }
                XmlAttribute attribute = itemNode.Attributes[EIPConst.ATTRIBUTE_OFFSET];
                XmlAttribute attribute2 = itemNode.Attributes[EIPConst.ATTRIBUTE_POINTS];
                string[] strArray = attribute.Value.Split(new char[] { ':' });
                string[] strArray2 = attribute2.Value.Split(new char[] { ':' });
                if ((strArray.Length == 1) && (strArray2.Length == 1))
                {
                    try
                    {
                        if (int.Parse(attribute.Value) < 0)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        return ErrorCode.ErrorOffsetFormat;
                    }
                    try
                    {
                        if (int.Parse(attribute2.Value) < 1)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        return ErrorCode.ErrorPointFormat;
                    }
                }
                else
                {
                    if (strArray.Length != 2)
                    {
                        return ErrorCode.ErrorOffsetBitFormat;
                    }
                    if (strArray2.Length != 2)
                    {
                        return ErrorCode.ErrorPointBitFormat;
                    }
                    try
                    {
                        if (int.Parse(strArray[0]) < 0)
                        {
                            throw new Exception();
                        }
                        if ((int.Parse(strArray[1]) < 0) && (int.Parse(strArray[1]) > 15))
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        return ErrorCode.ErrorOffsetBitFormat;
                    }
                    try
                    {
                        if (int.Parse(strArray2[0]) < 0)
                        {
                            throw new Exception();
                        }
                        if ((int.Parse(strArray2[1]) < 1) && (int.Parse(strArray2[1]) > 0x10))
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        return ErrorCode.ErrorPointBitFormat;
                    }
                }
            }
            return ErrorCode.None;
        }

        private ErrorCode CheckItemPoints(XmlNode itemNode)
        {
            if (itemNode.NodeType == XmlNodeType.Element)
            {
                XmlAttribute attribute = itemNode.Attributes[EIPConst.ATTRIBUTE_POINTS];
                if ((attribute == null) || (attribute.Value == ""))
                {
                    return ErrorCode.ErrorPointNull;
                }
            }
            return ErrorCode.None;
        }

        protected ErrorCode CheckItemRefValue(XmlNode itemDef, XmlAttribute valueAttribute)
        {
            if (itemDef.NodeType != XmlNodeType.Element)
            {
                return ErrorCode.None;
            }
            if (this.CheckItemRepresent(itemDef) != ErrorCode.None)
            {
                return ErrorCode.None;
            }
            if (this.CheckItemPoints(itemDef) != ErrorCode.None)
            {
                return ErrorCode.None;
            }
            return this.CheckItemValue(itemDef, valueAttribute);
        }

        protected ErrorCode CheckItemRepresent(XmlNode itemNode)
        {
            if (itemNode.NodeType == XmlNodeType.Element)
            {
                XmlAttribute attribute = itemNode.Attributes[EIPConst.ATTRIBUTE_REPRESENTATION];
                if ((attribute == null) || (attribute.Value == ""))
                {
                    return ErrorCode.ErrorRepresentationNull;
                }
                try
                {
                    Representation.Parse(attribute.Value);
                }
                catch
                {
                    return ErrorCode.ErrorRepresentationFormat;
                }
            }
            return ErrorCode.None;
        }

        private ErrorCode CheckItemValue(XmlNode itemDef, XmlAttribute valueAttribute)
        {
            if (itemDef.NodeType == XmlNodeType.Element)
            {
                if ((valueAttribute == null) || (valueAttribute.Value == ""))
                {
                    return ErrorCode.None;
                }
                XmlAttribute attribute = itemDef.Attributes[EIPConst.ATTRIBUTE_POINTS];
                int points = int.Parse(attribute.Value.Split(new char[] { ':' })[0]);
                XmlAttribute attribute2 = itemDef.Attributes[EIPConst.ATTRIBUTE_REPRESENTATION];
                Representation representation = Representation.Parse(attribute2.Value);
                if (representation == Representation.I)
                {
                    if (points > 2)
                    {
                        return this.GetItemSizeError(representation);
                    }
                }
                else
                {
                    if (valueAttribute.Value.Length > this.GetItemValidSize(representation, points))
                    {
                        return this.GetItemSizeError(representation);
                    }
                    if (!representation.Pattern.IsMatch(valueAttribute.Value))
                    {
                        return this.GetItemFormatError(representation);
                    }
                }
            }
            return ErrorCode.None;
        }

        protected void CheckUndefine(List<XmlNode> xmlNodeList)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (node.Name == EIPConst.ELEMENT_TAG)
                    {
                        string tagName = node.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                        if (this.SearchTagDef(tagName) == null)
                        {
                            this.AppendCompileResult(node, ErrorCode.ErrorUndefine);
                        }
                    }
                    else if (node.Name == EIPConst.ELEMENT_BLOCK)
                    {
                        string blockName = node.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                        if (this.SearchBlockDef(blockName) == null)
                        {
                            this.AppendCompileResult(node, ErrorCode.ErrorUndefine);
                        }
                    }
                    else if (node.Name == EIPConst.ELEMENT_ITEMGROUP)
                    {
                        string itemGroupName = node.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                        if (this.SearchItemGroupDef(itemGroupName) == null)
                        {
                            this.AppendCompileResult(node, ErrorCode.ErrorUndefine);
                        }
                    }
                }
            }
        }

        protected void CheckUnique(List<XmlNode> xmlNodeList)
        {
            Dictionary<string, List<XmlNode>> dictionary = new Dictionary<string, List<XmlNode>>();
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    string key = node.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key].Add(node);
                    }
                    else
                    {
                        List<XmlNode> list = new List<XmlNode> {
                            node
                        };
                        dictionary[key] = list;
                    }
                }
            }
            foreach (List<XmlNode> list in dictionary.Values)
            {
                if (list.Count > 1)
                {
                    foreach (XmlNode node in list)
                    {
                        this.AppendCompileResult(node, ErrorCode.ErrorUnique);
                    }
                }
            }
        }

        protected void CheckUnique(XmlNodeList xmlNodeList)
        {
            Dictionary<string, List<XmlNode>> dictionary = new Dictionary<string, List<XmlNode>>();
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    string key = node.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key].Add(node);
                    }
                    else
                    {
                        List<XmlNode> list = new List<XmlNode> {
                            node
                        };
                        dictionary[key] = list;
                    }
                }
            }
            foreach (List<XmlNode> list in dictionary.Values)
            {
                if (list.Count > 1)
                {
                    foreach (XmlNode node in list)
                    {
                        this.AppendCompileResult(node, ErrorCode.ErrorUnique);
                    }
                }
            }
        }

        public void Clean()
        {
            this.compileResultCollection.Clear();
        }

        protected void ClearChildErrors(XmlNode xmlNode)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((xmlNode != result.Resource) && XmlUtils.IsChildOf(result.Resource, xmlNode))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearChildErrors(XmlNode parentNode, ErrorCode error)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (((error == result.Error) && (parentNode != result.Resource)) && XmlUtils.IsChildOf(result.Resource, parentNode))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearChildErrors(XmlNode xmlNode, List<ErrorCode> errors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (((xmlNode != result.Resource) && XmlUtils.IsChildOf(result.Resource, xmlNode)) && (errors.IndexOf(result.Error) > -1))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearChildErrors(XmlNode parentNode, string childXmlNodeName, ErrorCode error)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((((error == result.Error) && (parentNode != result.Resource)) && (result.Resource.Name == childXmlNodeName)) && XmlUtils.IsChildOf(result.Resource, parentNode))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearChildExceptErrors(XmlNode xmlNode, List<ErrorCode> exceptErrors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (((xmlNode != result.Resource) && XmlUtils.IsChildOf(result.Resource, xmlNode)) && (exceptErrors.IndexOf(result.Error) < 0))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearErrors(ErrorCode error)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (result.Error == error)
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearErrors(List<ErrorCode> errors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (errors.IndexOf(result.Error) > -1)
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearErrors(string xmlNodeName, ErrorCode error)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((result.Resource.Name == xmlNodeName) && (result.Error == error))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearErrors(XmlNode resource, ErrorCode[] errors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((result.Resource == resource) && (Array.IndexOf<ErrorCode>(errors, result.Error) > -1))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearExceptErrors(List<ErrorCode> exceptErrors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (exceptErrors.IndexOf(result.Error) < 0)
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearExceptErrors(XmlNode resource, ErrorCode exceptError)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((result.Resource == resource) && (exceptError != result.Error))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearExceptErrors(XmlNode resource, List<ErrorCode> exceptErrors)
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if ((result.Resource == resource) && (exceptErrors.IndexOf(result.Error) < 0))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        public void ClearRemovedResourceErrors()
        {
            List<CompileResult> list = new List<CompileResult>();
            foreach (CompileResult result in this.compileResultCollection)
            {
                if (!XmlUtils.IsChildOf(result.Resource, this.rootNode))
                {
                    list.Add(result);
                }
            }
            foreach (CompileResult result in list)
            {
                this.compileResultCollection.Remove(result);
            }
        }

        protected void ClearUndefineErrors(string xmlNodeName)
        {
            this.ClearErrors(xmlNodeName, ErrorCode.ErrorUndefine);
        }

        protected void ClearUndefineErrors(XmlNode parent)
        {
            this.ClearChildErrors(parent, ErrorCode.ErrorUndefine);
        }

        protected void ClearUndefineErrors(XmlNode parent, string xmlNodeName)
        {
            this.ClearChildErrors(parent, xmlNodeName, ErrorCode.ErrorUndefine);
        }

        protected void ClearUniqueErrors(string xmlNodeName)
        {
            this.ClearErrors(xmlNodeName, ErrorCode.ErrorUnique);
        }

        protected void ClearUniqueErrors(XmlNode parent)
        {
            this.ClearChildErrors(parent, ErrorCode.ErrorUnique);
        }

        protected void ClearUniqueErrors(XmlNode parent, string xmlNodeName)
        {
            this.ClearChildErrors(parent, xmlNodeName, ErrorCode.ErrorUnique);
        }

        public void Configure(XmlNode searchRoot, XmlNode rootNode)
        {
            this.searchRoot = searchRoot;
            this.rootNode = rootNode;
        }

        protected ErrorCode GetItemFormatError(Representation representation)
        {
            if (representation == Representation.I)
            {
                return ErrorCode.ErrorFormatI;
            }
            if (representation == Representation.A)
            {
                return ErrorCode.ErrorFormatA;
            }
            if (representation == Representation.B)
            {
                return ErrorCode.ErrorFormatB;
            }
            if (representation == Representation.BIT)
            {
                return ErrorCode.ErrorFormatBit;
            }
            return ErrorCode.None;
        }

        protected ErrorCode GetItemSizeError(Representation representation)
        {
            if (representation == Representation.I)
            {
                return ErrorCode.ErrorSizeI;
            }
            if (representation == Representation.A)
            {
                return ErrorCode.ErrorSizeA;
            }
            if (representation == Representation.B)
            {
                return ErrorCode.ErrorSizeB;
            }
            if (representation == Representation.BIT)
            {
                return ErrorCode.ErrorSizeBit;
            }
            return ErrorCode.None;
        }

        protected int GetItemValidSize(Representation representation, int points)
        {
            if (representation == Representation.A)
            {
                return (points * 2);
            }
            if (representation == Representation.B)
            {
                return (points * 0x10);
            }
            if (representation == Representation.BIT)
            {
                return points;
            }
            return 0;
        }

        public XmlNode SearchBlockDef(string blockName)
        {
            return XmlUtils.SearchChildNode(this.BlockMapNode, EIPConst.ELEMENT_BLOCK, EIPConst.ATTRIBUTE_NAME, blockName);
        }

        public List<XmlNode> SearchBlockDefs()
        {
            return XmlUtils.SearchChildNodes(this.BlockMapNode, EIPConst.ELEMENT_BLOCK);
        }

        public List<XmlNode> SearchBlockDefsOfTag(XmlNode tagDef)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in tagDef.ChildNodes)
            {
                if ((node.NodeType == XmlNodeType.Element) && (node.Name == EIPConst.ELEMENT_BLOCK))
                {
                    list.Add(node);
                }
            }
            return list;
        }

        public XmlNode SearchBlockDefsOfTag(XmlNode tagDef, string blockName)
        {
            foreach (XmlNode node in tagDef.ChildNodes)
            {
                if ((node.NodeType == XmlNodeType.Element) && ((node.Name == EIPConst.ELEMENT_BLOCK) && node.Attributes[EIPConst.ATTRIBUTE_NAME].Value.Equals(blockName)))
                {
                    return node;
                }
            }
            return null;
        }

        public List<XmlNode> SearchBlockRefs()
        {
            return XmlUtils.SearchChildNodes(this.TagMapNode, EIPConst.ELEMENT_BLOCK);
        }

        public List<XmlNode> SearchBlockRefsInTrxMap()
        {
            return XmlUtils.SearchChildNodes(this.TransactionNode, EIPConst.ELEMENT_BLOCK);
        }

        public List<XmlNode> SearchItemDefsOfBlock(string blockName)
        {
            List<XmlNode> list = new List<XmlNode>();
            XmlNode node = this.SearchBlockDef(blockName);
            if (node != null)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (node2.NodeType == XmlNodeType.Element)
                    {
                        if (node2.Name == EIPConst.ELEMENT_ITEMGROUP)
                        {
                            XmlNode node3 = this.SearchItemGroupDef(node2.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                            if (node3 != null)
                            {
                                foreach (XmlNode node4 in node3.ChildNodes)
                                {
                                    if (node2.NodeType == XmlNodeType.Element)
                                    {
                                        list.Add(node4);
                                    }
                                }
                            }
                        }
                        else if (node2.Name == EIPConst.ELEMENT_ITEM)
                        {
                            list.Add(node2);
                        }
                    }
                }
            }
            return list;
        }

        public List<XmlNode> SearchItemDefsOfBlock(XmlNode blockDef)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in blockDef.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (node.Name == EIPConst.ELEMENT_ITEMGROUP)
                    {
                        XmlNode node2 = this.SearchItemGroupDef(node.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                        if (node2 != null)
                        {
                            foreach (XmlNode node3 in node2.ChildNodes)
                            {
                                if (node.NodeType == XmlNodeType.Element)
                                {
                                    list.Add(node3);
                                }
                            }
                        }
                    }
                    else if (node.Name == EIPConst.ELEMENT_ITEM)
                    {
                        list.Add(node);
                    }
                }
            }
            return list;
        }

        public XmlNode SearchItemGroupDef(string itemGroupName)
        {
            return XmlUtils.SearchChildNode(this.ItemGroupCollectionNode, EIPConst.ELEMENT_ITEMGROUP, EIPConst.ATTRIBUTE_NAME, itemGroupName);
        }

        public List<XmlNode> SearchItemGroupDefs()
        {
            return XmlUtils.SearchChildNodes(this.ItemGroupCollectionNode, EIPConst.ELEMENT_ITEMGROUP);
        }

        public List<XmlNode> SearchItemGroupRefs()
        {
            return XmlUtils.SearchChildNodes(this.BlockMapNode, EIPConst.ELEMENT_ITEMGROUP);
        }

        public List<XmlNode> SearchMultiTagsInDataGather()
        {
            return XmlUtils.SearchChildNodes(this.DataGatherNode, EIPConst.ELEMENT_MULTITAG);
        }

        public XmlNode SearchTagDef(string tagName)
        {
            return XmlUtils.SearchChildNode(this.TagMapNode, EIPConst.ELEMENT_TAG, EIPConst.ATTRIBUTE_NAME, tagName);
        }

        public List<XmlNode> SearchTagDefs()
        {
            return XmlUtils.SearchChildNodes(this.TagMapNode, EIPConst.ELEMENT_TAG);
        }

        public List<XmlNode> SearchTagRefsInDataGather()
        {
            return XmlUtils.SearchChildNodes(this.DataGatherNode, EIPConst.ELEMENT_TAG);
        }

        public List<XmlNode> SearchTrxList()
        {
            return XmlUtils.SearchChildNodes(this.TransactionNode, EIPConst.ELEMENT_TRX);
        }

        public XmlNode BlockMapNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_BLOCKMAP);
            }
        }

        public IEnumerable<CompileResult> CompileResultCollection
        {
            get
            {
                return this.compileResultCollection;
            }
        }

        public XmlNode DataGatherNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_DATAGATHERING);
            }
        }

        public int ErrorCount
        {
            get
            {
                int num = 0;
                foreach (CompileResult result in this.compileResultCollection)
                {
                    if (result.Severity == Severity.Error)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public XmlNode ItemGroupCollectionNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_ITEMGROUPCOLLECTION);
            }
        }

        public XmlNode ReceiveTransactionNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_TRX_RECEIVE);
            }
        }

        public XmlNode ScanNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_DATAGATHERING_SCAN);
            }
        }

        public XmlNode SendTransactionNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_TRX_SEND);
            }
        }

        public XmlNode TagMapNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_TAGMAP);
            }
        }

        public XmlNode TransactionNode
        {
            get
            {
                return this.searchRoot.SelectSingleNode(EIPConst.XPATH_TRX);
            }
        }

        public int WarnCount
        {
            get
            {
                int num = 0;
                foreach (CompileResult result in this.compileResultCollection)
                {
                    if (result.Severity == Severity.Warn)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
    }
}
