
namespace HF.BC.Tool.EIPDriver.Data.Represent
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class RepresentationA : Representation
    {
        internal RepresentationA()
        {
            base.expression = "A";
            base.regex = new Regex(".*");
            base.regexErrorMessage = "Item value is not valid format. It must be composed of A to Z.";
        }

        private bool check(byte b)
        {
            if (b == 0)
            {
                return false;
            }
            if ((b < 0x20) || (b >= 0x7f))
            {
                return false;
            }
            return true;
        }

        public override int[] GetData(string name, string strValue, int point)
        {
            base.CheckPattern(name, strValue);
            if ((strValue.Length % 2) == 1)
            {
                strValue = strValue + " ";
            }
            int num = strValue.Length / 2;
            string[] strArray = new string[num];
            for (int i = 0; i < num; i++)
            {
                strArray[i] = strValue.Substring(i * 2, 2);
            }
            int[] numArray = new int[point];
            int index = 0;
            foreach (string str in strArray)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                numArray[index] = BitConverter.ToInt16(bytes, 0);
                index++;
            }
            return numArray;
        }

        public override string Parse(int[] ints, int point)
        {
            StringBuilder builder = new StringBuilder(point * 2);
            foreach (int num in ints)
            {
                byte[] bytes = BitConverter.GetBytes(num);
                if (this.check(bytes[0]) && this.check(bytes[1]))
                {
                    builder.Append(Encoding.ASCII.GetString(bytes, 0, 2));
                }
                else if (!(!this.check(bytes[0]) || this.check(bytes[1])))
                {
                    builder.Append(Encoding.ASCII.GetString(bytes, 0, 1)).Append(" ");
                }
                else if (!(this.check(bytes[0]) || !this.check(bytes[1])))
                {
                    builder.Append(" ").Append(Encoding.ASCII.GetString(bytes, 1, 1));
                }
                else
                {
                    builder.Append("  ");
                }
            }
            return builder.ToString();
        }
    }
}
