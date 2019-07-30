
namespace HF.BC.Tool.EIPDriver.Data.Represent
{
    using HF.BC.Tool.EIPDriver.Driver.Data.Represent;
    using System;
    using System.Text.RegularExpressions;

    [Serializable]
    public abstract class Representation
    {
        public static Representation A = new RepresentationA();
        public static Representation B = new RepresentationB();
        public static Representation BIT = new RepresentationBIT();
        protected string expression = string.Empty;
        public static Representation I = new RepresentationI();
        protected Regex regex = null;
        protected string regexErrorMessage;
        public static Representation SI = new RepresentationSI();

        protected Representation()
        {
        }

        public void CheckPattern(string name, string strValue)
        {
            if (!this.regex.IsMatch(strValue))
            {
                throw new Exception(string.Format("{0} Name={1}, Value={2}", this.regexErrorMessage, name, strValue));
            }
        }

        public abstract int[] GetData(string name, string strValue, int point);
        public static Representation Parse(string represent)
        {
            switch (represent)
            {
                case "A":
                    return A;

                case "I":
                    return I;

                case "B":
                    return B;

                case "BIT":
                    return BIT;

                case "SI":
                    return SI;
            }
            throw new Exception("Invalid Representation Type !!! " + represent);
        }

        public abstract string Parse(int[] ints, int point);
        public override string ToString()
        {
            return this.expression;
        }

        public string Expression
        {
            get
            {
                return this.expression;
            }
        }

        public Regex Pattern
        {
            get
            {
                return this.regex;
            }
        }
    }
}
