
namespace HF.BC.Tool.EIPDriver.Utils
{
    using HF.BC.Tool.EIPDriver.Data;
    using System;

    public class BinaryUtils
    {
        public static void ConvertBinaryToBinary(int[] sendData, Item item)
        {
            int[] destinationArray = new int[item.WordPoints];
            Array.Copy(sendData, item.WordOffset, destinationArray, 0, item.WordPoints);
            for (int i = 0; i < item.WordPoints; i++)
            {
                char[] chArray2;
                char[] chArray = Convert.ToString(destinationArray[i], 2).PadLeft(0x10, '0').ToCharArray();
                int destinationIndex = 0;
                if (item.BitPoints > 0x10)
                {
                    chArray2 = item.Value.Substring(0x10 * i, 0x10).ToCharArray();
                }
                else
                {
                    chArray2 = item.Value.ToCharArray();
                    destinationIndex = (0x10 - item.BitOffset) - item.BitPoints;
                }
                Array.Copy(chArray2, 0, chArray, destinationIndex, chArray2.Length);
                destinationArray[i] = Convert.ToUInt16(new string(chArray), 2);
            }
            Array.Reverse(destinationArray);
            Array.Copy(destinationArray, 0, sendData, item.WordOffset, item.WordPoints);
        }

        public static void ConvertIntToBinary(int[] sendData, Item item)
        {
            int num = sendData[item.WordOffset];
            int destinationIndex = (0x10 - item.BitOffset) - item.BitPoints;
            char[] destinationArray = Convert.ToString(num, 2).PadLeft(0x10, '0').ToCharArray();
            Array.Copy(Convert.ToString(int.Parse(item.Value), 2).PadLeft(item.BitPoints, '0').ToCharArray(), 0, destinationArray, destinationIndex, item.BitPoints);
            sendData[item.WordOffset] = Convert.ToUInt16(new string(destinationArray), 2);
        }

        public static void ConvertItemToBit(int[] sendData, Item item)
        {
            int num = sendData[item.WordOffset];
            if (item.Value.Equals("1"))
            {
                num |= ((int)1) << item.BitOffset;
            }
            else
            {
                num &= (ushort)~(((int)1) << item.BitOffset);
            }
            sendData[item.WordOffset] = num;
        }
    }
}
