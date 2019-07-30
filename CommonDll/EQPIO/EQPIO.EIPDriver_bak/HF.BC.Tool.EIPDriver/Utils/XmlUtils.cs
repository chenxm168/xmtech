
namespace HF.BC.Tool.EIPDriver.Utils
{
    using HF.BC.Tool.EIPDriver;
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;

    public class XmlUtils
    {
        public static void AppendAttributes(XmlNode xmlNode, string[] attributeNames)
        {
            foreach (string str in attributeNames)
            {
                XmlAttribute node = xmlNode.Attributes[str];
                if (node == null)
                {
                    node = xmlNode.OwnerDocument.CreateAttribute(str);
                    xmlNode.Attributes.Append(node);
                }
            }
        }

        public static object AttributesToObject(XmlNode node, Type type)
        {
            object obj2 = Activator.CreateInstance(type);
            foreach (XmlAttribute attribute in node.Attributes)
            {
                PropertyInfo property = type.GetProperty(attribute.Name);
                if (property != null)
                {
                    SetProperty(property, obj2, attribute.Value);
                }
                else
                {
                    property = type.GetProperty("UserFields");
                    if (property != null)
                    {
                        ((Dictionary<string, string>)property.GetValue(obj2, null)).Add(attribute.Name, attribute.Value);
                    }
                }
            }
            return obj2;
        }

        public static object ElementToObject(XmlNode node, Type type)
        {
            object obj2 = Activator.CreateInstance(type);
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.NodeType == XmlNodeType.Element)
                {
                    PropertyInfo property = type.GetProperty(node2.Name);
                    if (property != null)
                    {
                        SetProperty(property, obj2, node2.InnerText);
                    }
                }
            }
            return obj2;
        }

        public static void EntityReferenceToEntityRefElement(XmlDocument doc)
        {
            List<XmlEntityReference> list = SearchEntityReference(doc.DocumentElement);
            foreach (XmlEntityReference reference in list)
            {
                XmlTextReader reader = new XmlTextReader(string.Format("<{0} Name=\"{1}\">{2}</{0}>", EIPConst.ELEMENT_ENTITYREF, reference.Name, reference.InnerXml), XmlNodeType.Element, null);
                XmlNode newChild = doc.ReadNode(reader);
                XmlNode parentNode = reference.ParentNode;
                parentNode.InsertAfter(newChild, reference);
                parentNode.RemoveChild(reference);
            }
        }

        public static bool ExistChildNode(XmlNode parentNode, string nodeName, string attributeName, string attributeValue)
        {
            return (SearchChildNode(parentNode, nodeName, attributeName, attributeValue) != null);
        }

        public static bool ExistItemGroup(XmlNode parentNode, string itemGroupName)
        {
            return ExistChildNode(parentNode, EIPConst.ELEMENT_ITEMGROUP, EIPConst.ATTRIBUTE_NAME, itemGroupName);
        }

        public static bool ExistTag(XmlNode parentNode, string tagName)
        {
            return ExistChildNode(parentNode, EIPConst.ELEMENT_TAG, EIPConst.ATTRIBUTE_NAME, tagName);
        }

        public static string GetCategoryName(XmlNode xmlNode)
        {
            XmlNode parentNode = GetParentNode(xmlNode, new string[] { EIPConst.ELEMENT_ITEMGROUPCOLLECTION, EIPConst.ELEMENT_TAGMAP, EIPConst.ELEMENT_SCAN, EIPConst.ELEMENT_RECEIVE, EIPConst.ELEMENT_SEND });
            if (parentNode != null)
            {
                return parentNode.Name;
            }
            return "";
        }

        public static XmlNode GetChildNode(XmlNode paretnNode, string nodeName, string attributeName, string attributeValue)
        {
            foreach (XmlNode node in paretnNode.ChildNodes)
            {
                if (((node.NodeType == XmlNodeType.Element) && (node.Name == nodeName)) && (node.Attributes[attributeName].Value == attributeValue))
                {
                    return node;
                }
            }
            return null;
        }

        public static XmlNode GetParentNode(XmlNode xmlNode, string[] parentNodeNames)
        {
            if (Array.IndexOf<string>(parentNodeNames, xmlNode.Name) > -1)
            {
                return xmlNode;
            }
            if (xmlNode.ParentNode == null)
            {
                return null;
            }
            return GetParentNode(xmlNode.ParentNode, parentNodeNames);
        }

        public static bool HaveSameContents(XmlNode node1, XmlNode node2)
        {
            if (node1 != node2)
            {
                int num;
                if (node1.NodeType != node2.NodeType)
                {
                    return false;
                }
                if (node2.Name != node2.Name)
                {
                    return false;
                }
                if (node1.Attributes.Count != node2.Attributes.Count)
                {
                    return false;
                }
                for (num = 0; num < node1.Attributes.Count; num++)
                {
                    XmlAttribute attribute = node1.Attributes[num];
                    XmlAttribute attribute2 = node2.Attributes[num];
                    if (attribute.Name != attribute2.Name)
                    {
                        return false;
                    }
                    if (attribute.Value != attribute2.Value)
                    {
                        return false;
                    }
                }
                if (node1.ChildNodes.Count != node2.ChildNodes.Count)
                {
                    return false;
                }
                for (num = 0; num < node1.ChildNodes.Count; num++)
                {
                    XmlNode node = node1.ChildNodes[num];
                    XmlNode node3 = node2.ChildNodes[num];
                    if (!HaveSameContents(node, node3))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int IndexOf(XmlNode parentNode, XmlNode childNode)
        {
            int num = 0;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (node == childNode)
                    {
                        return num;
                    }
                    num++;
                }
            }
            return -1;
        }

        public static bool IsChildOf(XmlNode xmlNode, string parentNodeName)
        {
            if (xmlNode.Name == parentNodeName)
            {
                return true;
            }
            if (xmlNode.ParentNode == null)
            {
                return false;
            }
            return IsChildOf(xmlNode.ParentNode, parentNodeName);
        }

        public static bool IsChildOf(XmlNode xmlNode, XmlNode parentNode)
        {
            if (xmlNode == parentNode)
            {
                return true;
            }
            if (xmlNode.ParentNode == null)
            {
                return false;
            }
            return IsChildOf(xmlNode.ParentNode, parentNode);
        }

        public static bool IsChildOfItemGroupMap(XmlNode xmlNode)
        {
            return IsChildOf(xmlNode, EIPConst.ELEMENT_ITEMGROUPCOLLECTION);
        }

        public static bool IsChildOfRecvTrxMap(XmlNode xmlNode)
        {
            return IsChildOf(xmlNode, EIPConst.ELEMENT_RECEIVE);
        }

        public static bool IsChildOfScanMap(XmlNode xmlNode)
        {
            return IsChildOf(xmlNode, EIPConst.ELEMENT_SCAN);
        }

        public static bool IsChildOfSendTrxMap(XmlNode xmlNode)
        {
            return IsChildOf(xmlNode, EIPConst.ELEMENT_SEND);
        }

        public static bool IsChildOfTagMap(XmlNode xmlNode)
        {
            return IsChildOf(xmlNode, EIPConst.ELEMENT_TAGMAP);
        }

        public static void ObjectToElement(object obj, XmlNode node)
        {
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                XmlNode node2 = node.SelectSingleNode(info.Name);
                if ((node2 != null) && (info.GetValue(obj, null) != null))
                {
                    node2.InnerText = info.GetValue(obj, null).ToString();
                }
            }
        }

        public static void RemoveAttributes(XmlNode xmlNode, string[] attributes)
        {
            foreach (string str in attributes)
            {
                XmlAttribute node = xmlNode.Attributes[str];
                if (node != null)
                {
                    xmlNode.Attributes.Remove(node);
                }
            }
        }

        public static List<XmlNode> RemoveChildren(XmlNode xmlNode)
        {
            List<XmlNode> list = new List<XmlNode>();
            if (xmlNode.ChildNodes.Count != 0)
            {
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    list.Add(node);
                }
                foreach (XmlNode node in list)
                {
                    xmlNode.RemoveChild(node);
                }
            }
            return list;
        }

        public static void RemoveExceptAttributes(XmlNode xmlNode, string[] exceptAttributes)
        {
            List<XmlAttribute> list = new List<XmlAttribute>();
            foreach (XmlAttribute attribute in xmlNode.Attributes)
            {
                if (Array.IndexOf<string>(exceptAttributes, attribute.Name) < 0)
                {
                    list.Add(attribute);
                }
            }
            foreach (XmlAttribute attribute in list)
            {
                xmlNode.Attributes.Remove(attribute);
            }
        }

        public static XmlNode SearchChildNode(List<XmlNode> xmlNodeList, string nameValue)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Attributes[EIPConst.ATTRIBUTE_NAME].Value == nameValue)
                {
                    return node;
                }
            }
            return null;
        }

        public static XmlNode SearchChildNode(XmlNode parentNode, string nodeName, string attributeName, string attributeValue)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                XmlNode node2;
                if (node.NodeType == XmlNodeType.Element)
                {
                    if ((node.Name == nodeName) && (node.Attributes != null))
                    {
                        XmlAttribute attribute = node.Attributes[attributeName];
                        if ((attribute != null) && (attributeValue == attribute.Value))
                        {
                            return node;
                        }
                    }
                    node2 = SearchChildNode(node, nodeName, attributeName, attributeValue);
                    if (node2 != null)
                    {
                        return node2;
                    }
                }
                else if (node.NodeType == XmlNodeType.EntityReference)
                {
                    node2 = SearchChildNode(node, nodeName, attributeName, attributeValue);
                    if (node2 != null)
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        public static List<XmlNode> SearchChildNodes(XmlNode parentNode, string nodeName)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == nodeName)
                {
                    list.Add(node);
                }
                else
                {
                    list.AddRange(SearchChildNodes(node, nodeName));
                }
            }
            return list;
        }

        public static List<XmlNode> SearchChildNodes(XmlNode parentNode, string nodeName, string attributeName, string attributeValue)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if ((node.Name == nodeName) && (node.Attributes != null))
                {
                    XmlAttribute attribute = node.Attributes[attributeName];
                    if ((attribute != null) && (attributeValue == attribute.Value))
                    {
                        list.Add(node);
                    }
                }
                else
                {
                    List<XmlNode> collection = SearchChildNodes(node, nodeName, attributeName, attributeValue);
                    list.AddRange(collection);
                }
            }
            return list;
        }

        public static List<XmlEntityReference> SearchEntityReference(XmlNode parentNode)
        {
            List<XmlEntityReference> list = new List<XmlEntityReference>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.EntityReference)
                {
                    list.Add(node as XmlEntityReference);
                }
                else
                {
                    list.AddRange(SearchEntityReference(node));
                }
            }
            return list;
        }

        public static void SetProperty(PropertyInfo property, object obj, string value)
        {
            if (value != "")
            {
                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(obj, Convert.ToInt32(value), null);
                }
                else if (property.PropertyType == typeof(short))
                {
                    property.SetValue(obj, Convert.ToInt16(value), null);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(obj, Convert.ToBoolean(value), null);
                }
                else if (property.PropertyType == typeof(long))
                {
                    property.SetValue(obj, Convert.ToInt64(value), null);
                }
                else if (property.PropertyType == typeof(Representation))
                {
                    property.SetValue(obj, Representation.Parse(value), null);
                }
                else if (property.PropertyType.IsEnum)
                {
                    object obj2 = Enum.Parse(property.PropertyType, value, true);
                    property.SetValue(obj, obj2, null);
                }
                else
                {
                    property.SetValue(obj, value, null);
                }
            }
        }

        public static void SyncronizeXmlAttribute(XmlAttribute sourceAttribute, XmlAttribute targetAttribute)
        {
            if ((((sourceAttribute != null) && (targetAttribute != null)) && !(sourceAttribute.Name != targetAttribute.Name)) && (sourceAttribute.Value != targetAttribute.Value))
            {
                targetAttribute.Value = sourceAttribute.Value;
            }
        }
    }
}
