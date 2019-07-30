using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinSECS.global;
using WinSECS.structure;

namespace WinSECS.MessageHandler
{
    [ComVisible(false)]
    public class Composer_messagename_only
    {
        private Dictionary<string, SECSTransaction> messageFactory = new Dictionary<string, SECSTransaction>();

        public virtual bool AddModelingInfo(string MessageName, SECSTransaction trx)
        {
            this.messageFactory.Add(MessageName, trx);
            return true;
        }

        public virtual bool ClearModelingInfo()
        {
            this.messageFactory.Clear();
            return true;
        }

        public virtual ReturnObject GetSECSTransaction(string MessageName)
        {
            ReturnObject obj2 = new ReturnObject();
            SECSTransaction transaction = this.messageFactory[MessageName];
            if (transaction == null)
            {
                obj2.setError(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WITH_THIS_NAME);
                return obj2;
            }
            try
            {
                obj2.setReturnData(transaction.Clone());
            }
            catch (Exception)
            {
                obj2.setError(SEComError.SEComMessageHanlder.FAIL_DURING_SECSTRANSACTION_DUPLICATION);
            }
            return obj2;
        }

        public virtual void Terminate(ReturnObject returnObject)
        {
        }
    }
}
