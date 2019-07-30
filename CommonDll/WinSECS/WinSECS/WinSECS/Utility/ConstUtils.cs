using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinSECS.Utility
{
    internal class ConstUtils
    {
        public static readonly string BASEDIR = Environment.CurrentDirectory;
        public static readonly string DIR = Path.DirectorySeparatorChar.ToString();
        public static readonly char DIRCHAR = Path.DirectorySeparatorChar;
        public const string ISO = "iso-8859-1";
        public const string KOREAN = "ks_c_5601-1987";
        public static readonly string NEWLINE = Environment.NewLine;
        public static readonly long ONE_CENTURY = (100L * ONE_YEAR);
        public static readonly long ONE_DAY = (0x18L * ONE_HOUR);
        public static readonly long ONE_HOUR = (60L * ONE_MINUTE);
        public static readonly long ONE_MINUTE = 0xea60L;
        public const long ONE_SECOND = 0x3e8L;
        public static readonly long ONE_WEEK = (7L * ONE_DAY);
        public static readonly long ONE_YEAR = ((long)(365.2425 * ONE_DAY));
        public const string QUOTE = "\"";
        public const string SINGLE_QUOTE = "'";
        public const string SPACE = " ";
        public const string TAB = "    ";
        public static readonly string USERHOME = Environment.GetEnvironmentVariable("userprofile");
        public const string VERSION = "1.01";
        public const string XML_DECL_END = "\"?>";
        public const string XML_DECL_START = "<?xml version=\"1.0\" encoding=\"";
    }
}
