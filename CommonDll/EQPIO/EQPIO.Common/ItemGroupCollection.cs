using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class ItemGroupCollection
	{
		[XmlElement]
		public ItemGroup[] ItemGroup
		{
			get;
			set;
		}
	}
}
