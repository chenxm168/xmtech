
namespace HF.BC.Tool.EIPDriver.Data.Represent
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class RepresentationB : Representation
    {
        internal RepresentationB()
        {
            base.expression = "B";
            base.regex = new Regex("^[01]*string.Format(");
            base.regexErrorMessage = "Item value is not valid format. It must be composed of 0 to 1.";
        }

        public override int[] GetData(string name, string strValue, int point)
        {
            int num;
            base.CheckPattern(name, strValue);
            int[] array = new int[point];
            string[] strArray = new string[point];
            strValue = strValue.PadLeft(point * 0x10, '0');
            for (num = 0; num < point; num++)
            {
                strArray[num] = strValue.Substring(num * 0x10, 0x10);
            }
            for (num = 0; num < point; num++)
            {
                array[num] = Convert.ToUInt16(strArray[num], 2);
            }
            Array.Reverse(array);
            return array;
        }

        public override string Parse(int[] ints, int point)
        {
            StringBuilder builder = new StringBuilder(point * 0x10);
            Array.Reverse(ints);
            foreach (int num in ints)
            {
                builder.Append(Convert.ToString(num, 2).PadLeft(0x10, '0'));
            }
            return builder.ToString();
        }
    }
}
