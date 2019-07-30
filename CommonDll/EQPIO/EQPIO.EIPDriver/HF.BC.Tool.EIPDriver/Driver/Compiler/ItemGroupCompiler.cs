
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class ItemGroupCompiler : SubCompiler
    {
        public void CheckAllItemAttributes()
        {
            this.CheckItemAttributesOfItemGroupList(base.SearchItemGroupDefs());
        }

        public void CheckItemAttributes(XmlNode itemNode)
        {
            base.ClearExceptErrors(itemNode, ErrorCode.ErrorUnique);
            ErrorCode errorCode = base.CheckItemDefAttributes(itemNode);
            if (errorCode != ErrorCode.None)
            {
                base.AppendCompileResult(itemNode, errorCode);
            }
        }

        public void CheckItemAttributesOfItemGroup(XmlNode itemGroupNode)
        {
            foreach (XmlNode node in itemGroupNode)
            {
                this.CheckItemAttributes(node);
            }
        }

        public void CheckItemAttributesOfItemGroupList(List<XmlNode> itemGroupList)
        {
            foreach (XmlNode node in itemGroupList)
            {
                this.CheckItemAttributesOfItemGroup(node);
            }
        }

        public void CheckItemGroupUnique()
        {
            this.ClearItemGroupUniqueErrors();
            base.CheckUnique(base.SearchItemGroupDefs());
        }

        public void CheckItemUnique()
        {
            foreach (XmlNode node in base.SearchItemGroupDefs())
            {
                this.CheckItemUniqueOfItemGroup(node);
            }
        }

        public void CheckItemUniqueOfItemGroup(XmlNode itemGroupNode)
        {
            this.ClearItemUniqueErrorsOfItemGroup(itemGroupNode);
            base.CheckUnique(itemGroupNode.ChildNodes);
        }

        public void ClearChildErrorsOfItemGroup(XmlNode itemGroupNode)
        {
            base.ClearChildErrors(itemGroupNode);
        }

        public void ClearItemGroupUniqueErrors()
        {
            base.ClearUniqueErrors(EIPConst.ELEMENT_ITEMGROUP);
        }

        public void ClearItemUniqueErrorsOfItemGroup(XmlNode itemGroupNode)
        {
            base.ClearUniqueErrors(itemGroupNode, EIPConst.ELEMENT_ITEM);
        }
    }
}
