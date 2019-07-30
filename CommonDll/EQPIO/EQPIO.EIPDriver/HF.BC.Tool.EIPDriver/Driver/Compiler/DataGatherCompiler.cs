using HF.BC.Tool.EIPDriver.Utils;
using System;
using System.Xml;

namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
	internal class DataGatherCompiler : SubCompiler
	{
		public void ClearMultiTagUniqueErrors()
		{
			ClearUniqueErrors(EIPConst.ELEMENT_MULTITAG);
		}

		public void ClearTagUndefineErrorOfMultiTag(XmlNode multiTagNode)
		{
			ClearUndefineErrors(multiTagNode, EIPConst.ELEMENT_TAG);
		}

		public void ClearTagUndefineErrors()
		{
			ClearUndefineErrors(EIPConst.ELEMENT_TAG);
		}

		public void CheckMultiTagUnique()
		{
			ClearMultiTagUniqueErrors();
			CheckUnique(SearchMultiTagsInDataGather());
		}

		public void CheckTagUndefine()
		{
			ClearTagUndefineErrors();
			CheckUndefine(SearchTagRefsInDataGather());
		}

		public void CheckTagUndefineOfMultiTag(XmlNode multiTagNode)
		{
			ClearTagUndefineErrorOfMultiTag(multiTagNode);
			CheckUndefine(XmlUtils.SearchChildNodes(multiTagNode, EIPConst.ELEMENT_TAG));
		}

		public void CheckMultiTagAttribute()
		{
			foreach (XmlNode item in SearchMultiTagsInDataGather())
			{
				CheckMultiTagAttribute(item);
			}
		}

		public void CheckMultiTagAttribute(XmlNode multiTagkNode)
		{
			ClearExceptErrors(multiTagkNode, ErrorCode.ErrorUnique);
			XmlAttribute xmlAttribute = multiTagkNode.Attributes[EIPConst.ATTRIBUTE_INTERVAL];
			if (xmlAttribute != null && xmlAttribute.Value != "")
			{
				try
				{
					if (int.Parse(xmlAttribute.Value) < 0)
					{
						throw new Exception();
					}
				}
				catch
				{
					AppendCompileResult(multiTagkNode, ErrorCode.ErrorIntervalFormat);
					return;
				}
			}
			XmlAttribute xmlAttribute2 = multiTagkNode.Attributes[EIPConst.ATTRIBUTE_STARTUP];
			if (xmlAttribute2 != null && xmlAttribute2.Value != "")
			{
				try
				{
					Convert.ToBoolean(xmlAttribute2.Value);
				}
				catch
				{
					AppendCompileResult(multiTagkNode, ErrorCode.ErrorStartUpFormat);
					return;
				}
			}
			if (XmlUtils.SearchChildNodes(multiTagkNode, EIPConst.ELEMENT_TAG).Count == 0)
			{
				AppendCompileResult(multiTagkNode, ErrorCode.ErrorEmpty);
			}
		}
	}
}
