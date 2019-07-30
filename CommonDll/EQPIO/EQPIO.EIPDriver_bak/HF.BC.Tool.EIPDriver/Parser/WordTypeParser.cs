
namespace HF.BC.Tool.EIPDriver.Parser
{
    using HF.BC.Tool.EIPDriver.Data;
    using HF.BC.Tool.EIPDriver.Data.Represent;
    using HF.BC.Tool.EIPDriver.Driver.Data;
    using HF.BC.Tool.EIPDriver.Utils;
    using System;

    public class WordTypeParser
    {
        public static int[] ConvertBlockToWriteData(Block block)
        {
            block.RawData = new int[block.Points];
            foreach (Item item in block.ItemCollection.Values)
            {
                if (item.Representation == Representation.BIT)
                {
                    BinaryUtils.ConvertItemToBit(block.RawData, item);
                }
                else if (item.IsLikeBitMode)
                {
                    if (item.Representation == Representation.I)
                    {
                        BinaryUtils.ConvertIntToBinary(block.RawData, item);
                    }
                    else if (item.Representation == Representation.B)
                    {
                        BinaryUtils.ConvertBinaryToBinary(block.RawData, item);
                    }
                }
                else
                {
                    Array.Copy(item.GetData(), 0, block.RawData, item.WordOffset, item.WordPoints);
                }
            }
            return block.RawData;
        }

        public static void ConvertBlockToWriteData(int[] sourceData, Block block)
        {
            block.RawData = new int[block.Points];
            Array.Copy(sourceData, block.Offset, block.RawData, 0, block.Points);
            foreach (Item item in block.ItemCollection.Values)
            {
                if (item.Representation == Representation.BIT)
                {
                    BinaryUtils.ConvertItemToBit(block.RawData, item);
                }
                else if (item.IsLikeBitMode)
                {
                    if (item.Representation == Representation.I)
                    {
                        BinaryUtils.ConvertIntToBinary(block.RawData, item);
                    }
                    else if (item.Representation == Representation.B)
                    {
                        BinaryUtils.ConvertBinaryToBinary(block.RawData, item);
                    }
                }
                else
                {
                    Array.Copy(item.GetData(), 0, block.RawData, item.WordOffset, item.WordPoints);
                }
            }
            Array.Copy(block.RawData, 0, sourceData, block.Offset, block.Points);
        }

        public static void ConvertObjectToBlock(Block block, bool isLazy)
        {
            int[] rawData = block.RawData;
            if (!isLazy)
            {
                foreach (Item item in block.ItemCollection.Values)
                {
                    int[] ints = ArrayUtils<int>.GetSubInt(rawData, item.WordOffset, item.WordPoints);
                    item.Parse(ints);
                }
                block.ParseCompleted = true;
            }
            else
            {
                block.ParseCompleted = false;
            }
        }

        public static void ConvertObjectToTag(Tag tag, bool isLazy)
        {
            int[] rawData = tag.RawData;
            foreach (Block block in tag.BlockCollection.Values)
            {
                int[] numArray2 = ArrayUtils<int>.GetSubInt(rawData, block.Offset, block.Points);
                block.RawData = numArray2;
                if (!isLazy)
                {
                    ConvertObjectToBlock(block, isLazy);
                }
            }
        }

        public static void ConvertRawDataToItem(int[] rawData, Item item)
        {
            int[] ints = ArrayUtils<int>.GetSubInt(rawData, item.WordOffset, item.WordPoints);
            item.Parse(ints);
        }

        public static void ConvertTagToWriteData(Tag tag)
        {
            tag.RawData = new int[tag.Points];
            foreach (Block block in tag.BlockCollection.Values)
            {
                int[] sourceArray = ConvertBlockToWriteData(block);
                Array.Copy(sourceArray, 0, tag.RawData, block.Offset, sourceArray.Length);
            }
        }
    }
}
