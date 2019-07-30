using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class Receive
	{
		[XmlElement]
		public Trx[] Trx
		{
			get;
			set;
		}
	}
}
