using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinSECS.global
{
    [ComVisible(false)]
    public class SECSException : SystemException
    {
        private bool error;
        private const long serialVersionUID = 1L;

        public SECSException()
        {
        }

        public SECSException(string message)
            : base(message)
        {
            this.Error = true;
        }

        public virtual bool Error
        {
            get
            {
                return this.error;
            }
            set
            {
                this.error = value;
            }
        }
    }
}
