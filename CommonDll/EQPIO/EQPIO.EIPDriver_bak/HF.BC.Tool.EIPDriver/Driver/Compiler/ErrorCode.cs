
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using System;

    public enum ErrorCode
    {
        ErrorActionFormat = 0x1024,
        ErrorActionNull = 0x1023,
        ErrorBegin = 0x3e9,
        ErrorDeviceCodeFormat = 0xc1e,
        ErrorDeviceCodeNull = 0xc1d,
        ErrorEmpty = 0x1069,
        ErrorFormatA = 0x85d,
        ErrorFormatB = 0x859,
        ErrorFormatBit = 0x853,
        ErrorFormatH = 0x85b,
        ErrorFormatI = 0x855,
        ErrorFormatSI = 0x857,
        ErrorHeadDeviceDecFormat = 0xc28,
        ErrorHeadDeviceHexFormat = 0xc29,
        ErrorHeadDeviceNull = 0xc27,
        ErrorIntervalFormat = 0x1005,
        ErrorKeyInvalid = 0x13ee,
        ErrorKeyNull = 0x13ed,
        ErrorKeyUnique = 0x13ef,
        ErrorLogModeFormat = 0x1019,
        ErrorOffsetBitFormat = 0x837,
        ErrorOffsetFormat = 0x836,
        ErrorOffsetNull = 0x835,
        ErrorOutOfBounds = 0x867,
        ErrorPointBitFormat = 0x841,
        ErrorPointFormat = 0x840,
        ErrorPointNull = 0x83f,
        ErrorRepresentationBit = 0x84b,
        ErrorRepresentationFormat = 0x84a,
        ErrorRepresentationNull = 0x849,
        ErrorSizeA = 0x85e,
        ErrorSizeB = 0x85a,
        ErrorSizeBit = 0x854,
        ErrorSizeH = 0x85c,
        ErrorSizeI = 0x856,
        ErrorSizeSI = 0x858,
        ErrorStartUpFormat = 0x100f,
        ErrorTriggerFormat = 0x871,
        ErrorUndefine = 0x3fd,
        ErrorUnique = 0x3f3,
        None = 0,
        WarnEmpty = 1
    }
}
