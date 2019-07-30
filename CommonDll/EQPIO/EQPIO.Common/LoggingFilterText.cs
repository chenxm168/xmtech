using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class LoggingFilterText
	{
		[XmlElement]
		public LogText[] LogText
		{
			get;
			set;
		}
	}
}
