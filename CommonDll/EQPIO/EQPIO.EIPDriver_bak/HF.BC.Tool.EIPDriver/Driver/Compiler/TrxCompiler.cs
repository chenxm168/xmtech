
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Enums;
    using HF.BC.Tool.EIPDriver.Utils;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class TrxCompiler : SubCompiler
    {
        public void CheckBlockUndefine()
        {
            this.ClearBlockUndefineErrors();
            base.CheckUndefine(base.SearchBlockRefsInTrxMap());
        }

        public void CheckBlockUndefine(XmlNode xmlNode)
        {
            this.ClearBlockUndefineError(xmlNode);
            base.CheckUndefine(XmlUtils.SearchChildNodes(xmlNode, EIPConst.ELEMENT_BLOCK));
        }

        public void CheckItemAttributes(XmlNode itemRef)
        {
            string blockName = itemRef.ParentNode.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
            XmlNode itemDef = XmlUtils.SearchChildNode(base.SearchItemDefsOfBlock(blockName), itemRef.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
            if (itemDef != null)
            {
                this.CheckItemAttributes(itemRef, itemDef);
            }
        }

        private void CheckItemAttributes(XmlNode itemRef, XmlNode itemDef)
        {
            base.ClearExceptErrors(itemRef, ErrorCode.None);
            ErrorCode errorCode = base.CheckItemRefValue(itemDef, itemRef.Attributes[EIPConst.ATTRIBUTE_VALUE]);
            if (errorCode != ErrorCode.None)
            {
                base.AppendCompileResult(itemRef, errorCode);
            }
        }

        public void CheckItemAttributesOfBlockList(List<XmlNode> blockRefs)
        {
            foreach (XmlNode node in blockRefs)
            {
                this.CheckItemAttributesOfBlockRef(node);
            }
        }

        public void CheckItemAttributesOfBlockRef(XmlNode blockRef)
        {
            string blockName = blockRef.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
            List<XmlNode> xmlNodeList = base.SearchItemDefsOfBlock(blockName);
            foreach (XmlNode node in XmlUtils.SearchChildNodes(blockRef, EIPConst.ELEMENT_ITEM))
            {
                XmlNode itemDef = XmlUtils.SearchChildNode(xmlNodeList, node.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                if (itemDef != null)
                {
                    this.CheckItemAttributes(node, itemDef);
                }
            }
        }

        public void CheckTagAttribute(XmlNode tagNode)
        {
            base.ClearExceptErrors(tagNode, ErrorCode.ErrorUnique);
            List<XmlNode> blockRefs = XmlUtils.SearchChildNodes(tagNode, EIPConst.ELEMENT_BLOCK);
            if (blockRefs.Count == 0)
            {
                base.AppendCompileResult(tagNode, ErrorCode.ErrorEmpty);
            }
            XmlAttribute attribute = tagNode.Attributes[EIPConst.ATTRIBUTE_ACTION];
            if ((attribute == null) || (attribute.Value == string.Empty))
            {
                base.AppendCompileResult(tagNode, ErrorCode.ErrorActionNull);
            }
            else if ((attribute.Value != ActionEnum.R.ToString()) && (attribute.Value != ActionEnum.W.ToString()))
            {
                base.AppendCompileResult(tagNode, ErrorCode.ErrorActionFormat);
            }
            this.CheckItemAttributesOfBlockList(blockRefs);
        }

        public void CheckTagUnique()
        {
            foreach (XmlNode node in base.SearchTrxList())
            {
                this.CheckTagUnique(node);
            }
        }

        public void CheckTagUnique(XmlNode trx)
        {
            this.ClearTagUniqueErrors(trx);
            base.CheckUnique(XmlUtils.SearchChildNodes(trx, EIPConst.ELEMENT_TAG));
        }

        public void CheckTrxAttributes()
        {
            foreach (XmlNode node in base.SearchTrxList())
            {
                this.CheckTrxAttributes(node);
            }
        }

        public void CheckTrxAttributes(XmlNode trxNode)
        {
            base.ClearExceptErrors(trxNode, ErrorCode.ErrorUnique);
            this.CheckTrxKey(trxNode);
            List<XmlNode> list = XmlUtils.SearchChildNodes(trxNode, EIPConst.ELEMENT_TAG);
            if (list.Count == 0)
            {
                base.AppendCompileResult(trxNode, ErrorCode.ErrorEmpty);
            }
            else
            {
                foreach (XmlNode node in list)
                {
                    this.CheckTagAttribute(node);
                }
            }
        }

        public void CheckTrxKey()
        {
            foreach (XmlNode node in XmlUtils.SearchChildNodes(base.ReceiveTransactionNode, EIPConst.ELEMENT_TRX))
            {
                this.CheckTrxKey(node);
            }
        }

        public void CheckTrxKey(XmlNode trxNode)
        {
            if (XmlUtils.IsChildOf(trxNode, base.ReceiveTransactionNode))
            {
                base.ClearErrors(trxNode, new ErrorCode[] { ErrorCode.ErrorKeyNull, ErrorCode.ErrorKeyInvalid });
                XmlAttribute attribute = trxNode.Attributes[EIPConst.ATTRIBUTE_KEY];
                if ((attribute == null) || (attribute.Value == string.Empty))
                {
                    base.AppendCompileResult(trxNode, ErrorCode.ErrorKeyNull);
                }
                else
                {
                    try
                    {
                        string[] strArray = attribute.Value.Split(new char[] { '.' });
                        if (strArray.Length != 3)
                        {
                            throw new Exception();
                        }
                        XmlNode node = XmlUtils.SearchChildNode(base.TagMapNode, EIPConst.ELEMENT_TAG, EIPConst.ATTRIBUTE_NAME, strArray[0]);
                        if (node == null)
                        {
                            throw new Exception();
                        }
                        XmlNode tagDef = base.SearchTagDef(node.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                        if (tagDef == null)
                        {
                            throw new Exception();
                        }
                        XmlNode node3 = base.SearchBlockDefsOfTag(tagDef, strArray[1]);
                        if (node3 == null)
                        {
                            throw new Exception();
                        }
                        XmlNode blockDef = base.SearchBlockDef(node3.Attributes[EIPConst.ATTRIBUTE_NAME].Value);
                        if (blockDef == null)
                        {
                            throw new Exception();
                        }
                        if (strArray[2] != "*")
                        {
                            bool flag = false;
                            foreach (XmlNode node5 in base.SearchItemDefsOfBlock(blockDef))
                            {
                                if (node5.Attributes[EIPConst.ATTRIBUTE_NAME].Value == strArray[2])
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    catch
                    {
                        base.AppendCompileResult(trxNode, ErrorCode.ErrorKeyInvalid);
                    }
                }
            }
        }

        public void CheckTrxUnique()
        {
            this.ClearTrxUniqueErrors();
            base.CheckUnique(base.SearchTrxList());
        }

        public void CheckTrxUniqueKey()
        {
            base.ClearErrors(ErrorCode.ErrorKeyUnique);
            Dictionary<string, List<XmlNode>> dictionary = new Dictionary<string, List<XmlNode>>();
            foreach (XmlNode node in XmlUtils.SearchChildNodes(base.ReceiveTransactionNode, EIPConst.ELEMENT_TRX))
            {
                string str = node.Attributes[EIPConst.ATTRIBUTE_KEY].Value;
                if (!string.IsNullOrEmpty(str))
                {
                    if (dictionary.ContainsKey(str))
                    {
                        dictionary[str].Add(node);
                    }
                    else
                    {
                        List<XmlNode> list = new List<XmlNode> {
                            node
                        };
                        dictionary[str] = list;
                    }
                }
            }
            foreach (List<XmlNode> list in dictionary.Values)
            {
                if (list.Count > 1)
                {
                    foreach (XmlNode node in list)
                    {
                        base.AppendCompileResult(node, ErrorCode.ErrorKeyUnique);
                    }
                }
            }
        }

        public void ClearBlockUndefineError(XmlNode element)
        {
            base.ClearUndefineErrors(element, EIPConst.ELEMENT_BLOCK);
        }

        public void ClearBlockUndefineErrors()
        {
            base.ClearUndefineErrors(EIPConst.ELEMENT_BLOCK);
        }

        public void ClearTagUniqueErrors(XmlNode trx)
        {
            base.ClearUniqueErrors(trx, EIPConst.ELEMENT_TAG);
        }

        public void ClearTrxUniqueErrors()
        {
            base.ClearUniqueErrors(EIPConst.ELEMENT_TRX);
        }
    }
}
