
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Utils;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class TagCompiler : SubCompiler
    {
        public void CheckBlockAttribute(XmlNode blockDef, XmlNode tagDef)
        {
            if ((blockDef.NodeType == XmlNodeType.Element) && (tagDef.NodeType == XmlNodeType.Element))
            {
                base.ClearExceptErrors(blockDef, ErrorCode.ErrorUnique);
                XmlAttribute attribute = blockDef.Attributes[EIPConst.ATTRIBUTE_OFFSET];
                if ((attribute == null) || (attribute.Value == string.Empty))
                {
                    base.AppendCompileResult(blockDef, ErrorCode.ErrorOffsetNull);
                }
                else
                {
                    XmlAttribute attribute2 = blockDef.Attributes[EIPConst.ATTRIBUTE_POINTS];
                    if ((attribute2 == null) || (attribute2.Value == string.Empty))
                    {
                        base.AppendCompileResult(blockDef, ErrorCode.ErrorPointNull);
                    }
                }
            }
        }

        public void CheckBlockAttributes()
        {
            this.CheckBlockAttributesOfTagList(base.SearchTagDefs());
        }

        public void CheckBlockAttributesOfTag(XmlNode tagDef)
        {
            foreach (XmlNode node in base.SearchBlockDefsOfTag(tagDef))
            {
                this.CheckBlockAttribute(node, tagDef);
            }
        }

        public void CheckBlockAttributesOfTagList(List<XmlNode> tagDefs)
        {
            foreach (XmlNode node in tagDefs)
            {
                this.CheckBlockAttributesOfTag(node);
            }
        }

        public void CheckBlockUndefine()
        {
            this.ClearBlockUndefineErrors();
            base.CheckUndefine(base.SearchBlockRefs());
        }

        public void CheckBlockUndefineOfTag(XmlNode tagNode)
        {
            this.ClearBlockUndefineErrorOfTag(tagNode);
            List<XmlNode> xmlNodeList = XmlUtils.SearchChildNodes(tagNode, EIPConst.ELEMENT_BLOCK);
            base.CheckUndefine(xmlNodeList);
        }

        public void CheckBlockUndefineOfTagList(List<XmlNode> tagDefs)
        {
            foreach (XmlNode node in tagDefs)
            {
                this.CheckBlockUndefineOfTag(node);
            }
        }

        public void CheckBlockUnique()
        {
            foreach (XmlNode node in base.SearchTagDefs())
            {
                this.CheckBlockUniqueOfTag(node);
            }
        }

        public void CheckBlockUniqueOfTag(XmlNode tagDef)
        {
            this.ClearBlockUniqueErrorsOfTag(tagDef);
            base.CheckUnique(base.SearchBlockDefsOfTag(tagDef));
        }

        public void CheckBlockUniqueOfTagList(List<XmlNode> tagDefs)
        {
            foreach (XmlNode node in tagDefs)
            {
                this.CheckBlockUniqueOfTag(node);
            }
        }

        public void CheckTagUnique()
        {
            this.ClearTagUniqueErrors();
            base.CheckUnique(base.SearchTagDefs());
        }

        public void ClearBlockUndefineErrorOfTag(XmlNode tagNode)
        {
            base.ClearUndefineErrors(tagNode, EIPConst.ELEMENT_BLOCK);
        }

        public void ClearBlockUndefineErrors()
        {
            base.ClearUndefineErrors(EIPConst.ELEMENT_BLOCK);
        }

        public void ClearBlockUniqueErrorsOfTag(XmlNode tagNode)
        {
            base.ClearUniqueErrors(tagNode, EIPConst.ELEMENT_BLOCK);
        }

        public void ClearChildErrorsOfTag(XmlNode tagNode)
        {
            base.ClearChildErrors(tagNode);
        }

        public void ClearChildErrorsOfTagList(List<XmlNode> tagDefs)
        {
            foreach (XmlNode node in tagDefs)
            {
                this.ClearChildErrorsOfTag(node);
            }
        }

        public void ClearTagUniqueErrors()
        {
            base.ClearUniqueErrors(EIPConst.ELEMENT_TAG);
        }
    }
}
