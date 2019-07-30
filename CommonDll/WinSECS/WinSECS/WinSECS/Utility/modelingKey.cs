using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinSECS.Utility
{
    [ComVisible(false)]
    public class modelingKey
    {
        public const string DATA = "DataItem";
        public const string DIRECTION_BOTH = "H<->E";
        public const string DIRECTION_FROMEQUIPMENT = "H<-E";
        public const string DIRECTION_FROMHOST = "H->E";
        public const string FORMAT_FIXED = "Fixed";
        public const string FORMAT_ITEMKEY = "ItemKey";
        public const string FORMAT_ITEMNAME = "ItemName";
        public const string FORMAT_LENGTH = "Count";
        public const string HEADER = "Header";
        public const string HEADER_AUTOREPLY = "Header/AutoReply";
        public const string HEADER_DESCRIPTION = "Header/Description";
        public const string HEADER_DIRECTION = "Header/Direction";
        public const string HEADER_FUNCTION = "Header/Function";
        public const string HEADER_ITEMKEY = "Header/ItemKey";
        public const string HEADER_MESSAGENAME = "Header/MessageName";
        public const string HEADER_NOLOGGING = "Header/NoLogging";
        public const string HEADER_PAIRNAME = "Header/PairName";
        public const string HEADER_STREAM = "Header/Stream";
        public const string HEADER_WAIT = "Header/Wait";
        public const string ROOTNAME = "SECSMessage";
    }
}
