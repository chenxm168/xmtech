
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Utils;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class BlockCompiler : SubCompiler
    {
        public void CheckBlockAttributes()
        {
            foreach (XmlNode node in base.SearchBlockDefs())
            {
                this.CheckBlockAttributes(node);
            }
        }

        public void CheckBlockAttributes(List<XmlNode> blockDefs)
        {
            foreach (XmlNode node in blockDefs)
            {
                this.CheckBlockAttributes(node);
            }
        }

        public void CheckBlockAttributes(XmlNode blockDef)
        {
            if (blockDef.NodeType == XmlNodeType.Element)
            {
                base.ClearExceptErrors(blockDef, ErrorCode.ErrorUnique);
                XmlAttribute attribute = blockDef.Attributes[EIPConst.ATTRIBUTE_POINTS];
                if ((attribute == null) || (attribute.Value == ""))
                {
                    base.AppendCompileResult(blockDef, ErrorCode.ErrorPointNull);
                }
                else
                {
                    try
                    {
                        if (int.Parse(attribute.Value) < 1)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        base.AppendCompileResult(blockDef, ErrorCode.ErrorPointFormat);
                        return;
                    }
                    if (base.SearchItemDefsOfBlock(blockDef).Count == 0)
                    {
                        base.AppendCompileResult(blockDef, ErrorCode.WarnEmpty);
                    }
                }
            }
        }

        public void CheckBlockUnique()
        {
            this.ClearBlockUniqueErrors();
            base.CheckUnique(base.SearchBlockDefs());
        }

        public void CheckItemAttribute(XmlNode itemDef, XmlNode blockDef)
        {
            if ((itemDef.NodeType == XmlNodeType.Element) && (blockDef.NodeType == XmlNodeType.Element))
            {
                base.ClearExceptErrors(itemDef, ErrorCode.ErrorUnique);
                ErrorCode errorCode = base.CheckItemDefAttributes(itemDef);
                if (errorCode != ErrorCode.None)
                {
                    base.AppendCompileResult(itemDef, errorCode);
                }
                else
                {
                    int num = 0;
                    try
                    {
                        XmlAttribute attribute = blockDef.Attributes[EIPConst.ATTRIBUTE_POINTS];
                        if (((attribute == null) || (attribute.Value == "")) || ((num = int.Parse(attribute.Value)) < 1))
                        {
                            return;
                        }
                    }
                    catch
                    {
                        return;
                    }
                    string str = itemDef.Attributes[EIPConst.ATTRIBUTE_OFFSET].Value;
                    string str2 = itemDef.Attributes[EIPConst.ATTRIBUTE_POINTS].Value;
                    int num2 = int.Parse(str.Split(new char[] { ':' })[0]);
                    int num3 = int.Parse(str2.Split(new char[] { ':' })[0]);
                    if ((num2 >= num) || ((num2 + num3) > num))
                    {
                        base.AppendCompileResult(itemDef, ErrorCode.ErrorOutOfBounds);
                    }
                }
            }
        }

        public void CheckItemAttributes()
        {
            this.CheckItemAttributesOfBlockList(base.SearchBlockDefs());
        }

        public void CheckItemAttributesOfBlock(XmlNode blockDef)
        {
            foreach (XmlNode node in base.SearchItemDefsOfBlock(blockDef))
            {
                this.CheckItemAttribute(node, blockDef);
            }
        }

        public void CheckItemAttributesOfBlockList(List<XmlNode> blockDefs)
        {
            foreach (XmlNode node in blockDefs)
            {
                this.CheckItemAttributesOfBlock(node);
            }
        }

        public void CheckItemGroupUndefine()
        {
            this.ClearItemGroupUndefineErrors();
            base.CheckUndefine(base.SearchItemGroupRefs());
        }

        public void CheckItemGroupUndefineOfBlock(XmlNode blockNode)
        {
            this.ClearItemGroupUndefineErrorOfBlock(blockNode);
            List<XmlNode> xmlNodeList = XmlUtils.SearchChildNodes(blockNode, EIPConst.ELEMENT_ITEMGROUP);
            base.CheckUndefine(xmlNodeList);
        }

        public void CheckItemGroupUndefineOfBlockList(List<XmlNode> blockDefs)
        {
            foreach (XmlNode node in blockDefs)
            {
                this.CheckItemGroupUndefineOfBlock(node);
            }
        }

        public void CheckItemUnique()
        {
            foreach (XmlNode node in base.SearchBlockDefs())
            {
                this.CheckItemUniqueOfBlock(node);
            }
        }

        public void CheckItemUniqueOfBlock(XmlNode blockDef)
        {
            this.ClearItemUniqueErrorsOfBlock(blockDef);
            base.CheckUnique(base.SearchItemDefsOfBlock(blockDef));
        }

        public void CheckItemUniqueOfBlockList(List<XmlNode> blockDefs)
        {
            foreach (XmlNode node in blockDefs)
            {
                this.CheckItemUniqueOfBlock(node);
            }
        }

        public void ClearBlockUniqueErrors()
        {
            base.ClearUniqueErrors(EIPConst.ELEMENT_BLOCK);
        }

        public void ClearChildErrorsOfBlock(XmlNode blockNode)
        {
            base.ClearChildErrors(blockNode);
        }

        public void ClearChildErrorsOfBlockList(List<XmlNode> blockDefs)
        {
            foreach (XmlNode node in blockDefs)
            {
                this.ClearChildErrorsOfBlock(node);
            }
        }

        public void ClearItemGroupUndefineErrorOfBlock(XmlNode blockNode)
        {
            base.ClearUndefineErrors(blockNode, EIPConst.ELEMENT_ITEMGROUP);
        }

        public void ClearItemGroupUndefineErrors()
        {
            base.ClearUndefineErrors(EIPConst.ELEMENT_ITEMGROUP);
        }

        public void ClearItemUniqueErrorsOfBlock(XmlNode blockNode)
        {
            base.ClearUniqueErrors(blockNode, EIPConst.ELEMENT_ITEM);
        }
    }
}
