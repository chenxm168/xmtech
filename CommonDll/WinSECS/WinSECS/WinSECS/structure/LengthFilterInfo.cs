using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.structure
{
    public class LengthFilterInfo
    {
        private int length;
        private bool userDefined;

        public bool IsUserDefined
        {
            get
            {
                return this.userDefined;
            }
            set
            {
                this.userDefined = value;
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }
    }
}
