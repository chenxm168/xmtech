using System;
using System.Text.RegularExpressions;

namespace HF.BC.Tool.EIPDriver.Data.Represent
{
	[Serializable]
	public class RepresentationI : Representation
	{
        private readonly int standardValue = 0x10000;

		internal RepresentationI()
		{
			expression = "I";
			regex = new Regex("^\\d+string.Format(");
			regexErrorMessage = "Item value is not valid format. It must be composed of decimal number.";
		}

		public override string Parse(int[] ints, int point)
		{
			uint num = 0u;
			if (point > 1)
			{
				num = (uint)ints[0];
				num = (uint)((int)num + ints[1] * standardValue);
			}
			else
			{
				num = (uint)ints[0];
			}
			return num.ToString();
		}

		public override int[] GetData(string name, string strValue, int point)
		{
			CheckPattern(name, strValue);
			int[] array = new int[point];
			uint num = uint.Parse(strValue);
			if (point > 1)
			{
				array[0] = (int)((long)num % (long)standardValue);
				array[1] = (int)((long)num / (long)standardValue);
			}
			else
			{
				array[0] = (int)num;
			}
			return array;
		}
	}
}
