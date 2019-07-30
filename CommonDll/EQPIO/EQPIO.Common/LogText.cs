using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class LogText
	{
		[XmlAttribute]
		public string PartialMatching
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Text
		{
			get;
			set;
		}
	}
}
