using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml;
using WinSECS.structure;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class modelingFileParser
    {
        public modelingFileParser()
        {
            this.InitBlock();
        }

        private void InitBlock()
        {
        }

        private IFormat ParseDataItem(XmlElement element)
        {
            IFormat format = FormatFactory.newInstance(element.Name);
            if (format != null)
            {
                format.Name = element.Attributes["ItemName"].Value;
                format.Length = Convert.ToInt32(element.Attributes["Count"].Value);
                if ((element.FirstChild != null) && (element.FirstChild.NodeType == XmlNodeType.Text))
                {
                    format.Value = element.FirstChild.Value;
                }
                if (element.HasAttribute("Fixed"))
                {
                    format.Variable = !bool.Parse(element.Attributes["Fixed"].Value);
                }
                if (element.HasAttribute("ItemKey"))
                {
                    format.ItemKey = bool.Parse(element.Attributes["ItemKey"].Value);
                }
                if (format.Type != 0)
                {
                    return format;
                }
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].NodeType == XmlNodeType.Element)
                    {
                        XmlElement element2 = element.ChildNodes[i] as XmlElement;
                        IFormat format2 = this.ParseDataItem(element2);
                        if (format2 != null)
                        {
                            format.add(format2);
                        }
                    }
                }
            }
            return format;
        }

        public virtual SECSTransaction ParseSECSMessage(XmlNode element)
        {
            SECSTransaction transaction = new SECSTransaction
            {
                MessageName = element.SelectSingleNode("Header/MessageName").InnerText,
                Direction = element.SelectSingleNode("Header/Direction").InnerText
            };
            bool wbit = Convert.ToBoolean(element.SelectSingleNode("Header/Wait").InnerText);
            transaction.setStreamNWbit(Convert.ToUInt16(element.SelectSingleNode("Header/Stream").InnerText), wbit);
            transaction.Function = Convert.ToUInt16(element.SelectSingleNode("Header/Function").InnerText);
            transaction.Autoreply = Convert.ToBoolean(element.SelectSingleNode("Header/AutoReply").InnerText);
            transaction.IsLogging = !Convert.ToBoolean(element.SelectSingleNode("Header/NoLogging").InnerText);
            if (element.SelectSingleNode("Header/PairName") != null)
            {
                transaction.PairName = element.SelectSingleNode("Header/PairName").InnerText;
            }
            if (element.SelectSingleNode("Header/ItemKey") != null)
            {
                transaction.HasItemKey = Convert.ToBoolean(element.SelectSingleNode("Header/ItemKey").InnerText);
            }
            XmlNode node = null;
            node = element.SelectSingleNode("DataItem");
            if (node != null)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode node2 = node.ChildNodes[i];
                    if (node2.NodeType == XmlNodeType.Element)
                    {
                        IFormat format = this.ParseDataItem((XmlElement)node2);
                        if (format != null)
                        {
                            transaction.add(format);
                        }
                    }
                }
            }
            return transaction;
        }
    }
}
