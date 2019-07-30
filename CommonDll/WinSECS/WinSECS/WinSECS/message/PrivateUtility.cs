using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    class CPrivateUtility
    {
        public static List<String> getStringListItems(ListFormat listformat)
        {
            List<String> items = new List<String>();
            for (int i = 0; i < listformat.Length; i++)
            {
                items.Add(listformat.Children[i].Value);
            }
            return items;
        }

        public static ListFormat getMessage(bool isNoPadding, List<String> itemList, byte formatType, String itemName, int fixedChildrenLength)
        {
            ListFormat listFormat = new ListFormat();
		 
            if (itemList != null)
            {
                foreach (String item in itemList)
                {
                    if (formatType == FormatType.ASCII)
                    {
                        if (isNoPadding)
                            listFormat.add(formatType, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(item).Length, itemName, item);
                        else
                            listFormat.add(formatType, fixedChildrenLength, itemName, item);
				    }
                    else
                    {
                        string[] sArray = item.Split(new char[] {' '});
                        if (isNoPadding)
                            listFormat.add(formatType, sArray.Length, itemName, item);
                        else
                            listFormat.add(formatType, fixedChildrenLength, itemName, item);
				    }
			    }
		    }
            return listFormat;
        }

        public static ListFormat getMessage(bool isNoPadding, List<String> itemList, byte formatType, String itemName, bool isVariableItems, int fixedChildrenLength)
        {
            return getMessage(isNoPadding, itemList, formatType, itemName, fixedChildrenLength);
        }
    }
}
