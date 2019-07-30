using System;
using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class ItemGroup : ICloneable
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlElement]
		public Item[] Item
		{
			get;
			set;
		}

		public object Clone()
		{
			ItemGroup itemGroup = new ItemGroup();
			itemGroup.Name = Name;
			if (Item != null)
			{
				itemGroup.Item = new Item[Item.Length];
				for (int i = 0; i < Item.Length; i++)
				{
					itemGroup.Item[i] = (Item)Item[i].Clone();
				}
			}
			return itemGroup;
		}
	}
}
