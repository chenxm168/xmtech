using System.Xml.Serialization;

namespace EQPIO.Controller
{
	public class EQPIOConfig
	{
		[XmlElement]
		public Driver[] Driver
		{
			get;
			set;
		}
	}
}
