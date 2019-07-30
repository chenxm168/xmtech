
namespace EQPIO.MNetProtocol
{
    using System;
    using System.Text;

    internal class Make1EProtocol
    {
        private const string ACPU = "000A";
        private const string PLCNo = "FF";
        private const string ReadBit = "00";
        private const string ReadR = "17";
        private const string ReadWord = "01";
        private const string WriteBit = "02";
        private const string WriteR = "18";
        private const string WriteWord = "03";

        public byte[] GetRead(string deviceCode, string address, int length, bool isHex, BlockType blockType)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(deviceCode);
            string s = "00";
            if (blockType == BlockType.Bit)
            {
                s = "00";
            }
            else if (blockType == BlockType.Word)
            {
                s = "01";
            }
            else if (blockType == BlockType.Register)
            {
                s = "17";
            }
            s = (((s + "FF000A") + Convert.ToString(bytes[0], 0x10) + "20") + (isHex ? address.PadLeft(8, '0').ToUpper() : Convert.ToString(int.Parse(address), 0x10).PadLeft(8, '0').ToUpper())) + Convert.ToString(length, 0x10).PadLeft(2, '0').ToUpper() + "00";
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetRRead(string deviceCode, string address, int length, bool isHex, BlockType blockType, int pcNo)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(deviceCode);
            string s = "17";
            if (blockType == BlockType.Bit)
            {
                s = "00";
            }
            else if (blockType == BlockType.Word)
            {
                s = "01";
            }
            else if (blockType == BlockType.Register)
            {
                s = "17";
            }
            s = (((s + pcNo.ToString().PadLeft(2, '0') + "000A") + Convert.ToString(bytes[0], 0x10) + "20") + (isHex ? address.PadLeft(8, '0').ToUpper() : Convert.ToString(int.Parse(address), 0x10).PadLeft(8, '0').ToUpper())) + Convert.ToString(length, 0x10).PadLeft(2, '0').ToUpper() + "00";
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetRWrite(string deviceCode, string address, string writeData, int length, bool isHex, BlockType blockType, int pcNo)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(deviceCode);
            string s = "18";
            if (blockType == BlockType.Bit)
            {
                s = "02";
            }
            else if (blockType == BlockType.Word)
            {
                s = "03";
            }
            else if (blockType == BlockType.Register)
            {
                s = "18";
            }
            s = (((s + pcNo.ToString().PadLeft(2, '0') + "000A") + Convert.ToString(bytes[0], 0x10) + "20") + (isHex ? address.PadLeft(8, '0').ToUpper() : Convert.ToString(int.Parse(address), 0x10).PadLeft(8, '0').ToUpper()) + Convert.ToString(length, 0x10).PadLeft(2, '0').ToUpper()) + "00" + (((writeData.Length % 2) == 0) ? writeData : (writeData + "0"));
            return Encoding.ASCII.GetBytes(s);
        }

        public byte[] GetWrite(string deviceCode, string address, string writeData, int length, bool isHex, BlockType blockType)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(deviceCode);
            string s = "02";
            if (blockType == BlockType.Bit)
            {
                s = "02";
            }
            else if (blockType == BlockType.Word)
            {
                s = "03";
            }
            else if (blockType == BlockType.Register)
            {
                s = "18";
            }
            s = (((s + "FF000A") + Convert.ToString(bytes[0], 0x10) + "20") + (isHex ? address.PadLeft(8, '0').ToUpper() : Convert.ToString(int.Parse(address), 0x10).PadLeft(8, '0').ToUpper()) + Convert.ToString(length, 0x10).PadLeft(2, '0').ToUpper()) + "00" + (((writeData.Length % 2) == 0) ? writeData : (writeData + "0"));
            return Encoding.ASCII.GetBytes(s);
        }

        public enum BlockType
        {
            Bit,
            Word,
            Register
        }
    }
}
