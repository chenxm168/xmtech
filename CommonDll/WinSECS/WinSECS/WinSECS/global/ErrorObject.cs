using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.global
{
    public class ErrorObject
    {
        private int ErrorCode;
        private string ErrorDiscription;

        public ErrorObject(int ErrorCode)
        {
            this.setErrorCode(ErrorCode);
        }

        public ErrorObject(string ErrorMessage)
        {
            this.ErrorCode = 2;
            this.ErrorDiscription = ErrorMessage;
        }

        public virtual int getErrorCode()
        {
            return this.ErrorCode;
        }

        public virtual string getErrorDiscription()
        {
            return this.ErrorDiscription;
        }

        internal virtual void setErrorCode(int errorCode)
        {
            this.ErrorCode = errorCode;
            this.ErrorDiscription = SEComError.getErrDescription(errorCode);
        }
    }
}
