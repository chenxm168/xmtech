using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class Scan
	{
		[XmlElement]
		public MultiBlock[] MultiBlock
		{
			get;
			set;
		}
	}
}
