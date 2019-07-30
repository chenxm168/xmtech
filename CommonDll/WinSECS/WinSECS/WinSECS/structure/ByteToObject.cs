using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.structure
{
    [ComVisible(false)]
    public class ByteToObject
    {
        protected internal const string SPACE = " ";

        public static string byte2Binary(byte[] aBytes)
        {
            switch (aBytes.Length)
            {
                case 0:
                    return "";

                case 1:
                    return byte2UnsignedByte(aBytes[0]).ToString();
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < aBytes.Length; i++)
            {
                builder.Append(byte2UnsignedByte(aBytes[i]) + " ");
            }
            return builder.ToString().Trim();
        }

        public static byte byte2byte(byte b1)
        {
            return b1;
        }

        public static string byte2byteString(byte b1)
        {
            return byte2byte(b1).ToString();
        }

        public static float byte2Float4(byte[] a4Bytes)
        {
            try
            {
                a4Bytes = FormatUtility.swapFloat(a4Bytes);
                return BitConverter.ToSingle(a4Bytes, 0);
            }
            catch (Exception)
            {
                return 0f;
            }
        }

        public static string byte2Float4String(byte[] a4Bytes)
        {
            try
            {
                switch (a4Bytes.Length)
                {
                    case 0:
                        return "";

                    case 4:
                        return byte2Float4(a4Bytes).ToString("G8");
                }
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < (a4Bytes.Length / 4); i++)
                {
                    byte[] buffer = new byte[] { a4Bytes[i * 4], a4Bytes[(i * 4) + 1], a4Bytes[(i * 4) + 2], a4Bytes[(i * 4) + 3] };
                    builder.Append(byte2Float4(buffer).ToString("G8") + " ");
                }
                return builder.ToString().Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static double byte2Float8(byte[] a8Bytes)
        {
            try
            {
                a8Bytes = FormatUtility.swapDouble(a8Bytes);
                return BitConverter.ToDouble(a8Bytes, 0);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public static string byte2Float8String(byte[] a8Bytes)
        {
            try
            {
                switch (a8Bytes.Length)
                {
                    case 0:
                        return "";

                    case 8:
                        return byte2Float8(a8Bytes).ToString("G16");
                }
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < (a8Bytes.Length / 8); i++)
                {
                    byte[] buffer = new byte[] { a8Bytes[i * 8], a8Bytes[(i * 8) + 1], a8Bytes[(i * 8) + 2], a8Bytes[(i * 8) + 3], a8Bytes[(i * 8) + 4], a8Bytes[(i * 8) + 5], a8Bytes[(i * 8) + 6], a8Bytes[(i * 8) + 7] };
                    builder.Append(byte2Float8(buffer).ToString("G16") + " ");
                }
                return builder.ToString().Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static int byte2Int(byte[] a4Bytes)
        {
            if (a4Bytes.Length != 4)
            {
                return 0;
            }
            return (((((a4Bytes[0] & 0xff) << 0x18) | ((a4Bytes[1] & 0xff) << 0x10)) | ((a4Bytes[2] & 0xff) << 8)) | (a4Bytes[3] & 0xff));
        }

        public static string byte2int1(byte[] bs1)
        {
            switch (bs1.Length)
            {
                case 0:
                    return "";

                case 1:
                    return byte2byte(bs1[0]).ToString();
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bs1.Length; i++)
            {
                builder.Append(byte2byte(bs1[i]).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2int2(byte[] bs2)
        {
            switch (bs2.Length)
            {
                case 0:
                    return "";

                case 2:
                    return byte2Short(bs2).ToString();
            }
            byte[] destinationArray = new byte[2];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (bs2.Length / 2); i++)
            {
                Array.Copy(bs2, i * 2, destinationArray, 0, 2);
                builder.Append(byte2Short(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2int4(byte[] aBytes)
        {
            switch (aBytes.Length)
            {
                case 0:
                    return "";

                case 4:
                    return byte2Int(aBytes).ToString();
            }
            byte[] destinationArray = new byte[4];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (aBytes.Length / 4); i++)
            {
                Array.Copy(aBytes, i * 4, destinationArray, 0, 4);
                builder.Append(byte2Int(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2int8(byte[] aBytes)
        {
            switch (aBytes.Length)
            {
                case 0:
                    return "";

                case 8:
                    return byte2Long(aBytes).ToString();
            }
            byte[] destinationArray = new byte[8];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (aBytes.Length / 8); i++)
            {
                Array.Copy(aBytes, i * 8, destinationArray, 0, 8);
                builder.Append(byte2Unsignedlong(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static long byte2Long(byte[] a8Bytes)
        {
            if (a8Bytes.Length != 8)
            {
                return 0L;
            }
            return (((((((((a8Bytes[0] & 0xff) << 0x38) | ((a8Bytes[1] & 0xff) << 0x30)) | ((a8Bytes[2] & 0xff) << 40)) | ((a8Bytes[3] & 0xff) << 0x20)) | ((a8Bytes[4] & 0xff) << 0x18)) | ((a8Bytes[5] & 0xff) << 0x10)) | ((a8Bytes[6] & 0xff) << 8)) | (a8Bytes[7] & 0xff));
        }

        public static int byte2Short(byte[] a2Bytes)
        {
            if (a2Bytes.Length != 2)
            {
                return 0;
            }
            return (((short)((a2Bytes[0] & 0xff) << 8)) | ((short)(a2Bytes[1] & 0xff)));
        }

        public static string byte2Uint1(byte[] bs1)
        {
            switch (bs1.Length)
            {
                case 0:
                    return "";

                case 1:
                    return byte2UnsignedByte(bs1[0]).ToString();
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bs1.Length; i++)
            {
                builder.Append(byte2UnsignedByte(bs1[i]).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2Uint2(byte[] bs2)
        {
            switch (bs2.Length)
            {
                case 0:
                    return "";

                case 2:
                    return byte2UnsignedShort(bs2).ToString();
            }
            byte[] destinationArray = new byte[2];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (bs2.Length / 2); i++)
            {
                Array.Copy(bs2, i * 2, destinationArray, 0, 2);
                builder.Append(byte2UnsignedShort(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2Uint4(byte[] aBytes)
        {
            switch (aBytes.Length)
            {
                case 0:
                    return "";

                case 4:
                    return byte2UnsignedInt(aBytes).ToString();
            }
            byte[] destinationArray = new byte[4];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (aBytes.Length / 4); i++)
            {
                Array.Copy(aBytes, i * 4, destinationArray, 0, 4);
                builder.Append(byte2UnsignedInt(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static string byte2Uint8(byte[] aBytes)
        {
            switch (aBytes.Length)
            {
                case 0:
                    return "";

                case 8:
                    return byte2Unsignedlong(aBytes).ToString();
            }
            byte[] destinationArray = new byte[8];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (aBytes.Length / 8); i++)
            {
                Array.Copy(aBytes, i * 8, destinationArray, 0, 8);
                builder.Append(byte2Unsignedlong(destinationArray).ToString() + " ");
            }
            return builder.ToString().Trim();
        }

        public static int byte2UnsignedByte(byte b1)
        {
            return (b1 & 0xff);
        }

        public static long byte2UnsignedInt(byte[] a4Bytes)
        {
            if (a4Bytes.Length != 4)
            {
                return 0L;
            }
            return (((((a4Bytes[0] & 0xff) << 0x18) | ((a4Bytes[1] & 0xff) << 0x10)) | ((a4Bytes[2] & 0xff) << 8)) | (a4Bytes[3] & 0xff));
        }

        public static ulong byte2Unsignedlong(byte[] a8Bytes)
        {
            return (ulong)((((((((a8Bytes[0] << 0x38) | (a8Bytes[1] << 0x30)) | (a8Bytes[2] << 40)) | (a8Bytes[3] << 0x20)) | (a8Bytes[4] << 0x18)) | (a8Bytes[5] << 0x10)) | (a8Bytes[6] << 8)) | a8Bytes[7]);
        }

        public static int byte2UnsignedShort(byte[] bs2)
        {
            return (((bs2[0] & 0xff) << 8) | (((ushort)(bs2[1] & 0xff)) & 0xffff));
        }
    }
}
