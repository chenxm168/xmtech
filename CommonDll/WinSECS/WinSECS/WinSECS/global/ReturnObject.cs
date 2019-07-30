using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSECS.global
{
    public class ReturnObject : IReturnObject
    {
        private ErrorObject errorObject;
        private object ReturnData;
        private bool Success;

        public ReturnObject()
        {
            this.Success = true;
        }

        public ReturnObject(int ErrorCode)
        {
            this.Success = true;
            this.setError(ErrorCode);
        }

        public ReturnObject(object ReturnData)
        {
            this.Success = true;
            this.setReturnData(ReturnData);
        }

        public virtual string getErrorDescription()
        {
            if (this.errorObject == null)
            {
                this.errorObject = new ErrorObject(0);
            }
            return this.errorObject.getErrorDiscription();
        }

        public virtual ErrorObject getErrorObject()
        {
            if (this.errorObject == null)
            {
                this.errorObject = new ErrorObject(0);
            }
            return this.errorObject;
        }

        public virtual object getReturnData()
        {
            return this.ReturnData;
        }

        public virtual bool isSuccess()
        {
            return this.Success;
        }

        public virtual void setError(int errorCode)
        {
            if (errorCode != 0)
            {
                this.Success = false;
                this.errorObject = new ErrorObject(errorCode);
            }
            else
            {
                this.Success = true;
            }
        }

        public virtual void setError(string ErrorMessage)
        {
            this.errorObject = new ErrorObject(ErrorMessage);
            this.Success = false;
        }

        public virtual void setReturnData(object returnData)
        {
            this.ReturnData = returnData;
            this.Success = true;
        }
    }
}
