using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class Transaction
	{
		[XmlElement]
		public Receive Receive
		{
			get;
			set;
		}

		[XmlElement]
		public Send Send
		{
			get;
			set;
		}
	}
}
