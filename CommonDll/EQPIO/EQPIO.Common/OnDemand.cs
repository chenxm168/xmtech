using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class OnDemand
	{
		[XmlElement]
		public Block[] Block
		{
			get;
			set;
		}
	}
}
