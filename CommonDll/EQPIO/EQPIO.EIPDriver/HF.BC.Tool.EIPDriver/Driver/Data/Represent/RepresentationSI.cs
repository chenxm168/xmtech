using HF.BC.Tool.EIPDriver.Data.Represent;
using System;
using System.Text.RegularExpressions;

namespace HF.BC.Tool.EIPDriver.Driver.Data.Represent
{
	[Serializable]
	public class RepresentationSI : Representation
	{
        private readonly int standardValue = 0x10000;

		internal RepresentationSI()
		{
			expression = "SI";
			regex = new Regex("^[-]?\\d+string.Format(");
			regexErrorMessage = "Item value is not valid format. It must be composed of signed decimal number.";
		}

		public override string Parse(int[] ints, int point)
		{
			int num = 0;
			if (point > 1)
			{
				num = ints[0];
				num += ints[1] * standardValue;
			}
			else
			{
				num = ints[0];
			}
			return num.ToString();
		}

		public override int[] GetData(string name, string strValue, int point)
		{
			if (point < 1 || point > 2)
			{
				throw new Exception("Point is must be 1 or 2.");
			}
			CheckPattern(name, strValue);
			int[] array = new int[point];
			uint num = (uint)int.Parse(strValue);
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
