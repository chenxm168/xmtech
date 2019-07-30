using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinSECS.global;
using WinSECS.structure;
using WinSECS.MessageHandler;


namespace WinSECS.MessageHandler
{
    [ComVisible(false)]
    public class Composer
    {
        private HashFactory modelingFactory = new HashFactory();

        public bool AddModelingInfo(string SxFx, string MessageName, SECSTransaction trx)
        {
            return this.modelingFactory.Add(SxFx, MessageName, trx);
        }

        public bool ClearModelingInfo()
        {
            return this.modelingFactory.clear();
        }

        public virtual ReturnObject GetDefinedMessageByMessageName(int Stream, int Function, string MessageName)
        {
            ReturnObject obj2 = new ReturnObject();
            SECSTransaction transaction = (SECSTransaction)this.modelingFactory.getSamllCategory(string.Format("S{0}F{1}", Stream, Function), MessageName);
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

        public virtual ReturnObject GetDefinedMessageFirstItem(int Stream, int Function)
        {
            string bigCategory = string.Format("S{0}F{1}", Stream, Function);
            ReturnObject obj2 = new ReturnObject();
            Dictionary<string, object> dictionary = (Dictionary<string, object>)this.modelingFactory.getBigCategory(bigCategory);
            if (dictionary == null)
            {
                obj2.setError(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_STREAM_FUNCTION_SET);
            }
            else
            {
                foreach (string str2 in dictionary.Keys)
                {
                    SECSTransaction transaction = (SECSTransaction)dictionary[str2];
                    try
                    {
                        obj2.setReturnData(transaction.Clone());
                        return obj2;
                    }
                    catch (Exception)
                    {
                        obj2.setError(SEComError.SEComMessageHanlder.FAIL_DURING_SECSTRANSACTION_DUPLICATION);
                    }
                }
                obj2.setError(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WITH_THIS_NAME);
            }
            return obj2;
        }

        public IDictionary<string, object> getMessageSet(string SxFx)
        {
            return this.modelingFactory.getBigCategory(SxFx);
        }

        public int Size()
        {
            return this.modelingFactory.size();
        }
    }
}
