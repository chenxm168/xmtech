using System.Xml.Serialization;

namespace EQPIO.Controller
{
	public class Driver
	{
		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlElement]
		public ConnectionInfo ConnectionInfo
		{
			get;
			set;
		}

		[XmlElement]
		public DriverInfo[] DriverInfo
		{
			get;
			set;
		}
	}
}
