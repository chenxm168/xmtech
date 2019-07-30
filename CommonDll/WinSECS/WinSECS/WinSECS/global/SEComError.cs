using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.global
{
    [ComVisible(false)]
    public sealed class SEComError
    {
        public const int ERR_NONE = 0;
        public const int ERR_NOT_FIXED = 2;
        public const int ERR_UNKNOWN = 1;
        public const string sERR_NONE = "ERR_NONE";
        public const string sERR_NOT_FIXED = "ERR_NOT_FIXED";
        public const string sERR_UNKNOWN = "ERR_UNKNOWN";

        private SEComError()
        {
        }

        public static string getErrDescription(int aErrorCode)
        {
            if (aErrorCode == 0)
            {
                return "ERR_NONE";
            }
            if (aErrorCode == 1)
            {
                return "ERR_UNKNOWN";
            }
            if (aErrorCode == 2)
            {
                return "ERR_NOT_FIXED";
            }
            if ((aErrorCode >= 100) && (aErrorCode < 200))
            {
                return SEComDriver.getErrDescription(aErrorCode);
            }
            if ((aErrorCode >= 200) && (aErrorCode < 300))
            {
                return SEComPlugIn.getErrDescription(aErrorCode);
            }
            if ((aErrorCode >= 300) && (aErrorCode < 400))
            {
                return SEComMessageHanlder.getErrDescription(aErrorCode);
            }
            if ((aErrorCode > 400) && (aErrorCode < 500))
            {
                return SEComConnection.getErrDescription(aErrorCode);
            }
            if (aErrorCode > 0x3e8)
            {
                return SEComETC.getErrDescription(aErrorCode);
            }
            return "Unknown Error !!";
        }

        [ComVisible(false)]
        public sealed class SEComConnection
        {
            public static readonly int ERR_NOT_SENDABLE_STATUS = 0x19b;
            public static readonly int ERR_TIMEOUT_T1 = 0x191;
            public static readonly int ERR_TIMEOUT_T2 = 0x192;
            public static readonly int ERR_TIMEOUT_T3 = 0x193;
            public static readonly int ERR_TIMEOUT_T4 = 0x194;
            public static readonly int ERR_TIMEOUT_T5 = 0x195;
            public static readonly int ERR_TIMEOUT_T6 = 0x196;
            public static readonly int ERR_TIMEOUT_T7 = 0x197;
            public static readonly int ERR_TIMEOUT_T8 = 0x198;
            public const string sERR_NOT_SENDABLE_STATUS = "Not Sendable Satus, disconnected or not selected";
            private const int StartCount = 400;

            public static string getErrDescription(int aErrorCode)
            {
                if ((aErrorCode >= ERR_TIMEOUT_T1) && (aErrorCode <= ERR_TIMEOUT_T8))
                {
                    return ("T" + (aErrorCode - 400) + " TIMEOUT");
                }
                if (aErrorCode == ERR_NOT_SENDABLE_STATUS)
                {
                    return "Not Sendable Satus, disconnected or not selected";
                }
                return "ERR_UNKNOWN";
            }
        }

        [ComVisible(false)]
        public sealed class SEComDriver
        {
            public const int ERR_ALREADY_INITIALIZED = 0x67;
            public const int ERR_CRITICAL_VALUE_CHANGED = 0x83;
            public const int ERR_DUPLICATE_SYSTEMBYTE = 0x72;
            public const int ERR_HSMS_NOT_SELECTED = 0x79;
            public const int ERR_INITIALIZE_FAIL = 0x65;
            public const int ERR_INVALID_FUNCTION_NUMBER = 0x71;
            public const int ERR_INVALID_PROFILE = 0x66;
            public const int ERR_INVALID_STREAM_NUMBER = 0x70;
            public const int ERR_NONE = 0;
            public const int ERR_NULL_SECSMESSAGE = 0x6f;
            private const string sERR_ALREADY_INITIALIZED = "ERR_ALREADY_INITIALIZED";
            private const string sERR_CRITICAL_VALUE_CHANGED = "ERR_CRITICAL_VALUE_CHANGED";
            private const string sERR_DUPLICATE_SYSTEMBYTE = "ERR_DUPLICATE_SYSTEMBYTE";
            private const string sERR_HSMS_NOT_SELECTED = "ERR_HSMS_NOT_SELECTED";
            private const string sERR_INITIALIZE_FAIL = "ERR_INITIALIZE_FAIL";
            private const string sERR_INVALID_FUNCTION_NUMBER = "ERR_INVALID_FUNCTION_NUMBER";
            private const string sERR_INVALID_PROFILE = "ERR_INVALID_PROFILE";
            private const string sERR_INVALID_STREAM_NUMBER = "ERR_INVALID_STREAM_NUMBER";
            private const string sERR_NONE = "ERR_NONE";
            private const string sERR_NULL_SECSMESSAGE = "ERR_NULL_SECSMESSAGE";

            private SEComDriver()
            {
            }

            public static string getErrDescription(int aErrorCode)
            {
                if (aErrorCode == 0)
                {
                    return "ERR_NONE";
                }
                if (aErrorCode == 0x65)
                {
                    return "ERR_INITIALIZE_FAIL";
                }
                if (aErrorCode == 0x66)
                {
                    return "ERR_INVALID_PROFILE";
                }
                if (aErrorCode == 0x67)
                {
                    return "ERR_ALREADY_INITIALIZED";
                }
                if (aErrorCode == 0x6f)
                {
                    return "ERR_NULL_SECSMESSAGE";
                }
                if (aErrorCode == 0x70)
                {
                    return "ERR_INVALID_STREAM_NUMBER";
                }
                if (aErrorCode == 0x71)
                {
                    return "ERR_INVALID_FUNCTION_NUMBER";
                }
                if (aErrorCode == 0x72)
                {
                    return "ERR_DUPLICATE_SYSTEMBYTE";
                }
                if (aErrorCode == 0x79)
                {
                    return "ERR_HSMS_NOT_SELECTED";
                }
                if (aErrorCode == 0x83)
                {
                    return "ERR_CRITICAL_VALUE_CHANGED";
                }
                return "ERR_UNKNOWN";
            }
        }

        [ComVisible(false)]
        public sealed class SEComETC
        {
            public static readonly int ERR_FAIL_TO_GET_SYNC_REPLY = 0x3e9;
            public const string sERR_FAIL_TO_GET_SYNC_REPLY = "Fail to Get appropriate Reply in time, please check whether reply reachs in time from log";
            private const int StartCount = 0x3e8;

            public static string getErrDescription(int aErrorCode)
            {
                if (aErrorCode == ERR_FAIL_TO_GET_SYNC_REPLY)
                {
                    return "Fail to Get appropriate Reply in time, please check whether reply reachs in time from log";
                }
                return "ERR_UNKNOWN";
            }
        }

        [ComVisible(false)]
        public sealed class SEComGlobal
        {
            private const int StartCount = 100;

            private SEComGlobal()
            {
            }

            public static string getErrDescription(int aErrorCode)
            {
                return "";
            }
        }

        [ComVisible(false)]
        public sealed class SEComMessageHanlder
        {
            public static readonly int FAIL_DURING_MESSAGE_ENCODING = 310;
            public static readonly int FAIL_DURING_SECSTRANSACTION_DUPLICATION = 0x137;
            public static readonly int INVALID_MODELING_FILE = 0x130;
            public static readonly int INVALID_MODELING_FILE_ELEMENT = 0x132;
            public static readonly int INVALID_MODELING_FILE_NOT_FOUND = 0x13b;
            public static readonly int INVALID_MODELING_FILE_WITH_BAD_ROOT_NAME = 0x131;
            public static readonly int INVALID_MODELING_XML_STRING = 0x133;
            public static readonly int NO_MATCH_MODELING_MESSAGE_BIG_CATEGORY = 0x12d;
            public static readonly int NO_MATCH_MODELING_MESSAGE_SMALL_CATEGORY = 0x12e;
            public static readonly int NO_MATCH_MODELING_MESSAGE_WITH_THIS_NAME = 0x12f;
            public static readonly int NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY = 0x13a;
            public static readonly int NO_MATCH_MODELING_STREAM_FUNCTION_SET = 0x138;
            public static readonly int NO_MODELING_INFO = 0x134;
            public static readonly int NOT_HAVE_VALID_XML_NODE = 0x135;
            public const string sFAIL_DURING_MESSAGE_ENCODING = "Fail during Message Encoding for sending Message, please check input data at first, if continuing, ask developer";
            public const string sFAIL_DURING_SECSTRANSACTION_DUPLICATION = "Fail to duplicate SECSTransaction, may be not support clonable Property, please ask to developer";
            public const string sINVALID_MODELING_FILE = "Wrong XML Format file, validate Format of file";
            public const string sINVALID_MODELING_FILE_ELEMENT = "";
            public const string sINVALID_MODELING_FILE_NOT_FOUND = "maybe Fail to find SMD File, Check file path or refer Error LOG";
            public const string sINVALID_MODELING_FILE_WITH_BAD_ROOT_NAME = "The root element is not <SECSMessage>";
            public const string sINVALID_MODELING_XML_STRING = "";
            public const string sNO_MATCH_MODELING_MESSAGE_BIG_CATEGORY = "Fail to find Maching Message(SxFx) from modeling information, check modeling information or identity(Host/Equipment)";
            public const string sNO_MATCH_MODELING_MESSAGE_SMALL_CATEGORY = "Fail to find Maching Message(SxFxs) from modeling information, check modeling information or identity(Host/Equipment)";
            public const string sNO_MATCH_MODELING_MESSAGE_WITH_THIS_NAME = "Fail to find Maching Message by name from modeling information, check modeling information or identity(Host/Equipment)";
            public const string sNO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY = "Fail to find Maching Message(SxFx) from modeling information, Please Check Use of ItemKey";
            public const string sNO_MATCH_MODELING_STREAM_FUNCTION_SET = "Fail to find Maching Message Set with this Stream+Function from modeling information, check modeling information or identity(Host/Equipment)";
            public const string sNO_MODELING_INFO = "No Modeling Info, if not indended, please input modeling Infomation";
            public const string sNOT_HAVE_VALID_XML_NODE = "Not have valid XML NODE information, please check modeling File";
            private const int StartCount = 300;
            public const string sWARN_RANDOM_AUTOREPLY = "Fail to find exact reply, please check Whether reply is correct and modify modeling information";
            public static readonly int WARN_RANDOM_AUTOREPLY = 0x139;

            private SEComMessageHanlder()
            {
            }

            public static string getErrDescription(int aErrorCode)
            {
                if (aErrorCode == NO_MATCH_MODELING_MESSAGE_BIG_CATEGORY)
                {
                    return "Fail to find Maching Message(SxFx) from modeling information, check modeling information or identity(Host/Equipment)";
                }
                if (aErrorCode == NO_MATCH_MODELING_MESSAGE_SMALL_CATEGORY)
                {
                    return "Fail to find Maching Message(SxFxs) from modeling information, check modeling information or identity(Host/Equipment)";
                }
                if (aErrorCode == NO_MATCH_MODELING_MESSAGE_WITH_THIS_NAME)
                {
                    return "Fail to find Maching Message by name from modeling information, check modeling information or identity(Host/Equipment)";
                }
                if (aErrorCode == INVALID_MODELING_FILE)
                {
                    return "Wrong XML Format file, validate Format of file";
                }
                if (aErrorCode == INVALID_MODELING_FILE_WITH_BAD_ROOT_NAME)
                {
                    return "The root element is not <SECSMessage>";
                }
                if (aErrorCode == INVALID_MODELING_FILE_ELEMENT)
                {
                    return "";
                }
                if (aErrorCode == INVALID_MODELING_XML_STRING)
                {
                    return "";
                }
                if (aErrorCode == NO_MODELING_INFO)
                {
                    return "No Modeling Info, if not indended, please input modeling Infomation";
                }
                if (aErrorCode == NOT_HAVE_VALID_XML_NODE)
                {
                    return "Not have valid XML NODE information, please check modeling File";
                }
                if (aErrorCode == FAIL_DURING_MESSAGE_ENCODING)
                {
                    return "Fail during Message Encoding for sending Message, please check input data at first, if continuing, ask developer";
                }
                if (aErrorCode == FAIL_DURING_SECSTRANSACTION_DUPLICATION)
                {
                    return "Fail to duplicate SECSTransaction, may be not support clonable Property, please ask to developer";
                }
                if (aErrorCode == NO_MATCH_MODELING_STREAM_FUNCTION_SET)
                {
                    return "Fail to find Maching Message Set with this Stream+Function from modeling information, check modeling information or identity(Host/Equipment)";
                }
                if (aErrorCode == WARN_RANDOM_AUTOREPLY)
                {
                    return "Fail to find exact reply, please check Whether reply is correct and modify modeling information";
                }
                if (aErrorCode == NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY)
                {
                    return "Fail to find Maching Message(SxFx) from modeling information, Please Check Use of ItemKey";
                }
                if (aErrorCode == INVALID_MODELING_FILE_NOT_FOUND)
                {
                    return "maybe Fail to find SMD File, Check file path or refer Error LOG";
                }
                return "ERR_UNKNOWN";
            }
        }

        [ComVisible(false)]
        public sealed class SEComPlugIn
        {
            public static readonly int ERR_CANT_DRIVER_NULL = 0xcd;
            public static readonly int ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE = 0xc9;
            public static readonly int ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE_UnsupportedEncodingException = 0xca;
            public static readonly int ERR_NOT_FOUND_DRIVERINFO_FROM_FILE = 0xce;
            public static readonly int ERR_NOT_FOUND_DRIVERINFOFILE = 0xcb;
            public static readonly int ERR_NOT_FOUND_RELALTED_DRIVERID_INFOFILE = 0xcc;
            public static readonly int ERR_UNKNOWN = 0xcf;
            public const string sERR_CANT_DRIVER_NULL = "SECSConfig Can't be NULL, Make SECSConfig before Call Initiailze, or Call Other Parameterized Initialize(...)";
            public const string sERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE = "Fail to Convert SECSTransaction to Bytes";
            public const string sERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE_UnsupportedEncodingException = "UnsupportedEncodingException occures during Converting SECSTransaction to Bytes, ask to developer ";
            public const string sERR_NOT_FOUND_DRIVERINFO_FROM_FILE = "Validate File Information from INI File";
            public const string sERR_NOT_FOUND_DRIVERINFOFILE = "Validate File Path";
            public const string sERR_NOT_FOUND_RELALTED_DRIVERID_INFOFILE = "Validate File Existence (ex: driverid.xml) in same directory with Executable File";
            public const string sERR_UNKNOWN = "Unknown Error, Please contact Developer";
            private const int StartCount = 200;

            private SEComPlugIn()
            {
            }

            public static string getErrDescription(int aErrorCode)
            {
                if (aErrorCode == ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE)
                {
                    return "Fail to Convert SECSTransaction to Bytes";
                }
                if (aErrorCode == ERR_FAIL_TO_CONVERT_SECSTRANSACTION_TO_BYTE_UnsupportedEncodingException)
                {
                    return "UnsupportedEncodingException occures during Converting SECSTransaction to Bytes, ask to developer ";
                }
                if (aErrorCode == ERR_NOT_FOUND_DRIVERINFOFILE)
                {
                    return "Validate File Path";
                }
                if (aErrorCode == ERR_NOT_FOUND_RELALTED_DRIVERID_INFOFILE)
                {
                    return "Validate File Existence (ex: driverid.xml) in same directory with Executable File";
                }
                if (aErrorCode == ERR_CANT_DRIVER_NULL)
                {
                    return "SECSConfig Can't be NULL, Make SECSConfig before Call Initiailze, or Call Other Parameterized Initialize(...)";
                }
                if (aErrorCode == ERR_NOT_FOUND_DRIVERINFO_FROM_FILE)
                {
                    return "Validate File Information from INI File";
                }
                if (aErrorCode == ERR_UNKNOWN)
                {
                    return "Unknown Error, Please contact Developer";
                }
                return "ERR_UNKNOWN";
            }
        }

        [ComVisible(false)]
        public sealed class WinSockErrorCode
        {
            public static string getErrDescription(int aErrorCode)
            {
                switch (aErrorCode)
                {
                    case 0x2714:
                        return "Interrupted Function call";

                    case 0x271d:
                        return "Permission denied";

                    case 0x271e:
                        return "Bad Address";

                    case 0x2726:
                        return "Invalid Argument";

                    case 0x2728:
                        return "Too many open files";

                    case 0x2733:
                        return "Resource temporarily unavailable";

                    case 0x2734:
                        return "Operation now in progress";

                    case 0x2735:
                        return "opration already in progress";

                    case 0x2736:
                        return "Socket operation on nonsocket";

                    case 0x2737:
                        return "Destination address required";

                    case 0x2738:
                        return "Message too long";

                    case 0x2739:
                        return "Protocol wrong type for socket";

                    case 0x273a:
                        return "Bad protocol Option";

                    case 0x273b:
                        return "Protocol not supported";

                    case 0x273c:
                        return "Socket Type not supported";

                    case 0x273d:
                        return "Operation not Supported";

                    case 0x273e:
                        return "Protocol family not supported";

                    case 0x273f:
                        return "Address family not supported by protocol family";

                    case 0x2740:
                        return "Address already in use";

                    case 0x2741:
                        return "Cannot assign requested address";

                    case 0x2742:
                        return "Network is down. indicate a serious failure of the network system (that is, the protocol stack that the Windows Sockets DLL runs over:, the network interface, or the local network itself";

                    case 0x2743:
                        return "Network is unreachable.  local software knows no route to reach the remote host";

                    case 0x2744:
                        return "Network dropped connection on reset. The connection has been broken due to keep-alive activity detecting a failure while the operation was in progress.";

                    case 0x2745:
                        return "Software caused connection abort. due to a data transmission time-out or protocol error";

                    case 0x2746:
                        return "Connection reset by peer. An existing connection was forcibly closed by the remote host. This normally results if the peer application on the remote host is suddenly stopped, the host is rebooted, the host or remote network interface is disabled, or the remote host uses a hard close";

                    case 0x2747:
                        return "No buffer space available.An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full";

                    case 0x2748:
                        return "Socket is already connected";

                    case 0x2749:
                        return "Socket is not connected";

                    case 0x274a:
                        return "Cannot send after socket shutdown";

                    case 0x274c:
                        return "Connection timed out.A connection attempt failed because the connected party did not properly respond after a period of time, or the established connection failed because the connected host has failed to respond";

                    case 0x274d:
                        return "Connection refused. No connection could be made because the target computer actively refused it. This usually results from trying to connect to a service that is inactive on the foreign host";

                    case 0x2750:
                        return "Host is down.A socket operation failed because the destination host is down. A socket operation encountered a dead host. ";

                    case 0x2751:
                        return "No route to host. A socket operation was attempted to an unreachable host";

                    case 0x2753:
                        return "Too many processes. A Windows Sockets implementation may have a limit on the number of applications that can use it simultaneously";

                    case 0x276b:
                        return "Network subsystem is unavailable.";

                    case 0x276c:
                        return "";

                    case 0x276d:
                        return "";

                    case 0x2775:
                        return "";

                    case 0x2af9:
                        return "";

                    case 0x2afa:
                        return "";

                    case 0x2afb:
                        return "";

                    case 0x2afc:
                        return "";

                    case 0x277d:
                        return "";
                }
                return "unknown or it depends on OS";
            }
        }
    }
}
