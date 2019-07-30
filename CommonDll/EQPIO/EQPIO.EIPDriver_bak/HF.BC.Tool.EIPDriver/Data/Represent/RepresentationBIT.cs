
namespace HF.BC.Tool.EIPDriver.Data.Represent
{
    using System;
    using System.Text.RegularExpressions;

    [Serializable]
    public class RepresentationBIT : Representation
    {
        internal RepresentationBIT()
        {
            base.expression = "BIT";
            base.regex = new Regex("^[01]*string.Format(");
            base.regexErrorMessage = "Item value is not valid format. It must be composed of 0 to 1.";
        }

        public override int[] GetData(string name, string strValue, int point)
        {
            return new int[] { Convert.ToInt32(strValue) };
        }

        public override string Parse(int[] ints, int point)
        {
            return Convert.ToString(ints[0], 2).PadLeft(0x10, '0');
        }
    }
}
