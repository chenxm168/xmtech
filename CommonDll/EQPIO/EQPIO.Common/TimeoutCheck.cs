using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class TimeoutCheck
	{
		[XmlElement]
		public CheckItem[] CheckItem
		{
			get;
			set;
		}
	}
}
