using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class FilterItem
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}
	}
}
