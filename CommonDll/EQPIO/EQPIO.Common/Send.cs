using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class Send
	{
		[XmlElement]
		public Trx[] Trx
		{
			get;
			set;
		}
	}
}
