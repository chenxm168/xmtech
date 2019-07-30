using System.Xml.Serialization;

namespace EQPIO.Controller
{
	public class ConnectionInfo
	{
		[XmlAttribute]
		public string name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string path
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool use
		{
			get;
			set;
		}
	}
}
