using System.Xml.Serialization;

namespace EQPIO.Controller
{
	public class DriverInfo
	{
		[XmlAttribute]
		public string NetworkNo
		{
			get;
			set;
		}

		[XmlAttribute]
		public string PCNo
		{
			get;
			set;
		}

		[XmlAttribute]
		public string LocalName
		{
			get;
			set;
		}

		[XmlAttribute]
		public string IpAddress
		{
			get;
			set;
		}

		[XmlAttribute]
		public string MelsecPort
		{
			get;
			set;
		}

		[XmlAttribute]
		public string FixedBufferPort
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool IsMelsecEnabled
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool isFixedBufferEnabled
		{
			get;
			set;
		}

		[XmlAttribute]
		public string CodeType
		{
			get;
			set;
		}
	}
}
