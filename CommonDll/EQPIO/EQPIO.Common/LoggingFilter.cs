using System.Xml.Serialization;

namespace EQPIO.Common
{
	public class LoggingFilter
	{
		[XmlElement]
        public EQPIO.Common.FilterItem[] FilterItem { get; set; }
	}
}
