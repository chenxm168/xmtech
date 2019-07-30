using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
   public class FormatType
    {
        // Fields
        public const byte ANY_FORMAT = 0x7f;
        public const byte ASCII = 0x10;
        public const byte BINARY = 8;
        public const byte BOOLEAN = 9;
        public const byte FLOAT_4_BYTE = 0x24;
        public const byte FLOAT_8_BYTE = 0x20;
        public const byte INTEGER_1_BYTE = 0x19;
        public const byte INTEGER_2_BYTE = 0x1a;
        public const byte INTEGER_4_BYTE = 0x1c;
        public const byte INTEGER_8_BYTE = 0x18;
        public const byte JIS_8 = 0x11;
        public const byte LIST = 0;
        public const byte SECS_TRANSACTION = 0x3f;
        public const byte UNSIGNED_INTEGER_1_BYTE = 0x29;
        public const byte UNSIGNED_INTEGER_2_BYTE = 0x2a;
        public const byte UNSIGNED_INTEGER_4_BYTE = 0x2c;
        public const byte UNSIGNED_INTEGER_8_BYTE = 40;


    }
}
